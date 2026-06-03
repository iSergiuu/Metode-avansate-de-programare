namespace MusicPlayer.Observers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MusicPlayer.Audio;
using MusicPlayer.Models;

// Snapshot imutabil pentru afisare în UI
public record StatisticsSnapshot(string TotalPlayed, string TopArtist, int Skips);

public class StatisticsTracker
{
    private TimeSpan _totalPlayed = TimeSpan.Zero;
    private int _skips = 0;
    private readonly Dictionary<string, TimeSpan> _artistTimes = new();

    private Track? _lastTrack;
    private DateTime _trackStartTime;

    public StatisticsSnapshot Snapshot => new(
        $"{(int)_totalPlayed.TotalHours}h {_totalPlayed.Minutes}m",
        _artistTimes.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key ?? "N/A",
        _skips
    );

    public StatisticsTracker(AudioPlayer player)
    {
        player.PropertyChanged += HandlePlayerPropertyChanged;
    }

    private void HandlePlayerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AudioPlayer.CurrentTrack) && sender is AudioPlayer player)
        {
            // Daca a existat o piesă anterioara, calculam timpul ascultat
            if (_lastTrack != null)
            {
                var playedTime = DateTime.Now - _trackStartTime;

                // Numar de skip-uri (track schimbat sub 30s)
                if (playedTime.TotalSeconds < 30) _skips++;

                _totalPlayed += playedTime;

                if (!_artistTimes.ContainsKey(_lastTrack.Artist))
                    _artistTimes[_lastTrack.Artist] = TimeSpan.Zero;

                _artistTimes[_lastTrack.Artist] += playedTime;
            }

            _lastTrack = player.CurrentTrack;
            _trackStartTime = DateTime.Now;
        }
    }
}