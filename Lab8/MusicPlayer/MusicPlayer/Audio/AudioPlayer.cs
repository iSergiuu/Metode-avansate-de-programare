using System;
using System.ComponentModel;
using System.Windows.Threading;
using NAudio.Wave;
using MusicPlayer.Models;

namespace MusicPlayer.Audio
{
    // Clasa incapsuleaza IWavePlayer și AudioFileReader din NAudio
    public class AudioPlayer : INotifyPropertyChanged, IDisposable
    {
        private IWavePlayer? _waveOut;
        private AudioFileReader? _reader;
        private readonly DispatcherTimer _timer;

        public event PropertyChangedEventHandler? PropertyChanged;
        
        // Eveniment ridicat doar cand track-ul s-a terminat natural
        public event EventHandler? TrackEnded;

        private PlayerState _state = PlayerState.Stopped;
        public PlayerState State
        {
            get => _state;
            private set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        private Track? _currentTrack;
        public Track? CurrentTrack
        {
            get => _currentTrack;
            private set
            {
                _currentTrack = value;
                OnPropertyChanged(nameof(CurrentTrack));
            }
        }

        public TimeSpan Duration => _reader?.TotalTime ?? TimeSpan.Zero;

        // Position se actualizeaza printr-un DispatcherTimer
        public TimeSpan Position
        {
            get => _reader?.CurrentTime ?? TimeSpan.Zero;
            private set { /* Setter privat doar pentru a anunta UI-ul */ } 
        }

        private double _volume = 1.0; // 0.0 - 1.0
        public double Volume
        {
            get => _volume;
            set
            {
                _volume = Math.Clamp(value, 0.0, 1.0);
                if (_waveOut != null) _waveOut.Volume = (float)_volume;
                OnPropertyChanged(nameof(Volume));
            }
        }

        public AudioPlayer()
        {
            // Timer setat la fiecare 200 ms
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            _timer.Tick += (s, e) =>
            {
                OnPropertyChanged(nameof(Position));
                
                // Distinctia se face verificând dacă Position >= Duration
                if (State == PlayerState.Playing && _reader != null && _reader.CurrentTime >= _reader.TotalTime)
                {
                    Stop();
                    TrackEnded?.Invoke(this, EventArgs.Empty);
                }
            };
        }

        public void Load(Track track)
        {
            CleanUp(); // Curatam fisierul vechi
            CurrentTrack = track;
            
            _reader = new AudioFileReader(track.FilePath);
            _waveOut = new WaveOutEvent { Volume = (float)Volume };
            _waveOut.Init(_reader);
            
            OnPropertyChanged(nameof(Duration));
            OnPropertyChanged(nameof(Position));
        }

        public void Play()
        {
            if (_waveOut != null && State != PlayerState.Playing)
            {
                _waveOut.Play();
                State = PlayerState.Playing;
                _timer.Start();
            }
        }

        public void Pause()
        {
            if (_waveOut != null && State == PlayerState.Playing)
            {
                _waveOut.Pause();
                State = PlayerState.Paused;
                _timer.Stop();
            }
        }

        public void Stop()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
                _reader!.Position = 0; // Resetam la început
                State = PlayerState.Stopped;
                _timer.Stop();
                OnPropertyChanged(nameof(Position));
            }
        }

        public void Seek(TimeSpan position)
        {
            if (_reader != null)
            {
                _reader.CurrentTime = position;
                OnPropertyChanged(nameof(Position));
            }
        }

        private void CleanUp()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
        }

        public void Dispose()
        {
            _timer.Stop();
            CleanUp();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}