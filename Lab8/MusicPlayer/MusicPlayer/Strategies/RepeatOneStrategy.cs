namespace MusicPlayer.Strategies;

using MusicPlayer.Models;
using System.Linq;

public class RepeatOneStrategy : IPlaybackStrategy
{
    public string Name => "Repeat One";

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        return currentTrack ?? playlist.Tracks.FirstOrDefault();
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        return currentTrack ?? playlist.Tracks.FirstOrDefault();
    }

    public void Reset(Playlist playlist) { }
}