namespace MusicPlayer.Commands;

using MusicPlayer.Models;
using System.Linq;

public class RemoveTrackCommand : IPlayerCommand
{
    private readonly Playlist _playlist;
    private readonly Track _track;
    private int _originalIndex; // Memento pentru index

    public RemoveTrackCommand(Playlist playlist, Track track)
    {
        _playlist = playlist;
        _track = track;
    }

    public bool CanUndo => true; // Este undoable
    public string Description => $"Remove \"{_track.Title}\"";

    public void Execute()
    {
        // Retinem unde era piesa iainte să o stergem
        _originalIndex = _playlist.Tracks.ToList().IndexOf(_track);
        if (_originalIndex != -1)
        {
            _playlist.Remove(_track);
        }
    }

    public void Undo()
    {
        // Reinsereaza track-ul la pozitia originala
        if (_originalIndex != -1)
        {
            _playlist.Insert(_originalIndex, _track);
        }
    }
}