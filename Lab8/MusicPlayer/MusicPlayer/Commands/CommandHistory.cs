namespace MusicPlayer.Commands;

using System.Collections.Generic;
using System.Linq;

public class CommandHistory
{
    private readonly Stack<IPlayerCommand> _undoStack = new();
    private readonly Stack<IPlayerCommand> _redoStack = new();
    private const int MaxCapacity = 50; // Capacitate maxima 50 actiuni

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public void Execute(IPlayerCommand cmd)
    {
        cmd.Execute();

        if (cmd.CanUndo)
        {
            _undoStack.Push(cmd);
            _redoStack.Clear(); // Orice Execute() nou golește redo stack-ul

            // Limităm istoricul la 50 de elemente
            if (_undoStack.Count > MaxCapacity)
            {
                var arr = _undoStack.ToArray();
                _undoStack.Clear();
                for (int i = arr.Length - 2; i >= 0; i--) _undoStack.Push(arr[i]);
            }
        }
    }

    public void Undo()
    {
        if (!CanUndo) return;
        var cmd = _undoStack.Pop();
        cmd.Undo();
        _redoStack.Push(cmd);
    }

    public void Redo()
    {
        if (!CanRedo) return;
        var cmd = _redoStack.Pop();
        cmd.Execute();
        _undoStack.Push(cmd);
    }

    // Pentru interfata grafica (sa afisam istoricul)
    public IEnumerable<IPlayerCommand> GetHistory() => _undoStack.ToList();
}