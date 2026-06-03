using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MusicPlayer.Models;

public class Playlist
{
    private readonly ObservableCollection<Track> _tracks = new();

    public IReadOnlyList<Track> Tracks => _tracks;

    public void Add(Track track) => _tracks.Add(track);
    public void Remove(Track track) => _tracks.Remove(track);
    public void Move(int oldIndex, int newIndex) => _tracks.Move(oldIndex, newIndex);
    public void Clear() => _tracks.Clear();
    public void Insert(int index, Track track) => _tracks.Insert(index, track);
}