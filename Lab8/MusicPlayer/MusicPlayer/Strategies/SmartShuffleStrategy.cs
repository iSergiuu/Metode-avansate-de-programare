namespace MusicPlayer.Strategies;

using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class SmartShuffleStrategy : IPlaybackStrategy
{
    public string Name => "Smart Shuffle";
    private readonly Queue<Track> _history = new();
    private readonly Random _random = new();
    private const int MaxHistorySize = 5;

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        if (!playlist.Tracks.Any()) return null;

        // Fereastra dinamica: dacă ai 3 piese în playlist, history size e 2
        int currentLimit = Math.Min(MaxHistorySize, playlist.Tracks.Count - 1);

        if (currentTrack != null && !_history.Contains(currentTrack))
        {
            _history.Enqueue(currentTrack);
            while (_history.Count > currentLimit) _history.Dequeue();
        }

        var availableTracks = playlist.Tracks.Where(t => !_history.Contains(t)).ToList();

        // Cand se termina piesele disponibile, alegem din tot playlistul
        if (!availableTracks.Any()) availableTracks = playlist.Tracks.ToList();

        return availableTracks[_random.Next(availableTracks.Count)];
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        return GetNextTrack(playlist, currentTrack);
    }

    public void Reset(Playlist playlist)
    {
        _history.Clear();
    }
}