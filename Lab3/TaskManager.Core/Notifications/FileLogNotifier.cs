using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Notifications;

public class FileLogNotifier : ITaskNotifier
{
    private readonly string _filePath;

    public FileLogNotifier(string filePath = "tasks.log")
    {
        _filePath = filePath;
    }

    public void Notify(TaskItem task)
    {
        var message = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Task finalizat: {task.Title}{Environment.NewLine}";
        File.AppendAllText(_filePath, message);
    }
}