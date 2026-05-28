using TaskManager.Core.Enums;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Notifications;
using TaskManager.Core.Services;
using TaskManager.Data.Repositories;

namespace TaskManager.UI;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        ITaskRepository repository = new SqliteTaskRepository();
        var validator = new TaskValidator();

        IReadOnlyDictionary<NotificationType, ITaskNotifier> notifiers =
            new Dictionary<NotificationType, ITaskNotifier>
            {
        { NotificationType.Console, new ConsoleNotifier() },
        { NotificationType.Email, new EmailNotifier() },
        { NotificationType.FileLog, new FileLogNotifier() },
        { NotificationType.Slack, new SlackNotifier() }
            };

        var taskService = new TaskService(repository, validator, notifiers);
        var reportService = new ReportService(repository);

        Application.Run(new Form1(taskService, reportService));
    }
}