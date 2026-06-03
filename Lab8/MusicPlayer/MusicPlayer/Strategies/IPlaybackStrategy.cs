namespace MusicPlayer.Strategies;

using MusicPlayer.Models;

public interface IPlaybackStrategy
{
    string Name { get; }
    Track? GetNextTrack(Playlist playlist, Track? currentTrack);
    Track? GetPreviousTrack(Playlist playlist, Track? currentTrack);
    void Reset(Playlist playlist);
}