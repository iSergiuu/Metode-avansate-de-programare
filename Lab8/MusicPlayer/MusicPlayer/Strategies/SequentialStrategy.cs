namespace MusicPlayer.Strategies;

using MusicPlayer.Models;
using System.Linq;

public class SequentialStrategy : IPlaybackStrategy
{
    public string Name => "Sequential";

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        if (!playlist.Tracks.Any()) return null;
        if (currentTrack == null) return playlist.Tracks.First();

        var index = playlist.Tracks.ToList().IndexOf(currentTrack);
        if (index >= 0 && index < playlist.Tracks.Count - 1)
            return playlist.Tracks[index + 1];

        return null; // Ne oprim la capăt
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        if (!playlist.Tracks.Any()) return null;
        if (currentTrack == null) return playlist.Tracks.First();

        var index = playlist.Tracks.ToList().IndexOf(currentTrack);
        if (index > 0)
            return playlist.Tracks[index - 1];

        return playlist.Tracks.First();
    }

    public void Reset(Playlist playlist) { }
}