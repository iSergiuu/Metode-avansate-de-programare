namespace MusicPlayer.Strategies;

using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class ShuffleStrategy : IPlaybackStrategy
{
    public string Name => "Shuffle";
    private Queue<Track> _shuffledQueue = new();
    private readonly Random _random = new();

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        if (!playlist.Tracks.Any()) return null;
        if (!_shuffledQueue.Any()) Reset(playlist);

        return _shuffledQueue.Any() ? _shuffledQueue.Dequeue() : null;
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        return GetNextTrack(playlist, currentTrack);
    }

    public void Reset(Playlist playlist)
    {
        var shuffled = playlist.Tracks.OrderBy(x => _random.Next()).ToList();
        _shuffledQueue = new Queue<Track>(shuffled);
    }
}