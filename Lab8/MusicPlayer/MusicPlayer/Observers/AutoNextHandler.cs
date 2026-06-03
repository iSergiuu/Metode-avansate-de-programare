namespace MusicPlayer.Observers;

using System;
using MusicPlayer.Audio;
using MusicPlayer.Models;
using MusicPlayer.Strategies;

public class AutoNextHandler
{
    private readonly AudioPlayer _player;
    private readonly Playlist _playlist;

    // Folosim un delegat (Func) pentru a obtine mereu strategia curenta setata în interfata
    private readonly Func<IPlaybackStrategy> _getCurrentStrategy;

    public AutoNextHandler(AudioPlayer player, Playlist playlist, Func<IPlaybackStrategy> getCurrentStrategy)
    {
        _player = player;
        _playlist = playlist;
        _getCurrentStrategy = getCurrentStrategy;

        // La TrackEnded cere automat next track de la strategie
        _player.TrackEnded += OnTrackEnded;
    }

    private void OnTrackEnded(object? sender, EventArgs e)
    {
        var strategy = _getCurrentStrategy();
        var nextTrack = strategy.GetNextTrack(_playlist, _player.CurrentTrack);

        if (nextTrack != null)
        {
            _player.Load(nextTrack);
            // ... si porneste redarea
            _player.Play();
        }
    }
}