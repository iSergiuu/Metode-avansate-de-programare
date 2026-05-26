using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Notifications;

public class EmailNotifier : ITaskNotifier
{
    public void Notify(TaskItem task)
    {
        Console.WriteLine($"[Email] Email sent to user for task: {task.Title}");
    }
}