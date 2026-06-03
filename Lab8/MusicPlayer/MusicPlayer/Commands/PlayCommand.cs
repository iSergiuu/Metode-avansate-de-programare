namespace MusicPlayer.Commands;

using MusicPlayer.Audio;

public class PlayCommand : IPlayerCommand
{
    private readonly AudioPlayer _player;
    public PlayCommand(AudioPlayer player) => _player = player;

    public bool CanUndo => false; // Comenzile de control NU sunt undoable
    public string Description => "Play";

    public void Execute() => _player.Play();
    public void Undo() { } // Nu face nimic
}