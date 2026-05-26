using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Notifications;

public class SlackNotifier : ITaskNotifier
{
    public void Notify(TaskItem task)
    {
        Console.WriteLine($"[Slack] Message sent for task: {task.Title}");
    }
}