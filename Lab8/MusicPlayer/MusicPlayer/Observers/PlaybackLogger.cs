namespace MusicPlayer.Observers;

using System;
using System.IO;
using System.ComponentModel;
using MusicPlayer.Audio;

public class PlaybackLogger
{
    public PlaybackLogger(AudioPlayer player)
    {
        // Se aboneaza la TrackEnded (emis doar la finalizarea naturala)
        player.TrackEnded += (s, e) => Log("TrackEnded (Natural)");

        // Se aboneaza la PropertyChanged pentru a prinde schimbarea piesei
        player.PropertyChanged += OnPlayerPropertyChanged;
    }

    private void OnPlayerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AudioPlayer.CurrentTrack) && sender is AudioPlayer player)
        {
            if (player.CurrentTrack != null)
            {
                Log($"TrackStarted: {player.CurrentTrack.Title}");
            }
        }
    }

    private void Log(string message)
    {
        // Scrie în fisierul playback_log.txt cu timestamp
        File.AppendAllText("playback_log.txt", $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n");
    }
}