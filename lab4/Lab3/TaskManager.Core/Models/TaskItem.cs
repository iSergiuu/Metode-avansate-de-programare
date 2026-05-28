using TaskPriority = TaskManager.Core.Enums.TaskPriority;
using TaskType = TaskManager.Core.Enums.TaskType;
using NotificationType = TaskManager.Core.Enums.NotificationType;
using TaskStatus = TaskManager.Core.Enums.TaskStatus;

namespace TaskManager.Core.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; protected set; } = TaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public NotificationType NotificationType { get; set; } = NotificationType.Console;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual TaskType TaskType => TaskType.Standard;
    public virtual DateTime? DueDate { get; protected set; }
    public virtual int? RecurrenceInterval { get; protected set; }

    public bool IsOverdue =>
        DueDate.HasValue &&
        DueDate.Value < DateTime.UtcNow &&
        Status != TaskStatus.Done;

    public void Complete()
    {
        if (Status == TaskStatus.Done)
            throw new InvalidOperationException("Task-ul este deja finalizat.");

        CompleteCore();

        if (Status != TaskStatus.Done)
            throw new InvalidOperationException("Postcondiția a fost încălcată: Status trebuie să fie Done.");

        if (Status == TaskStatus.Done && IsOverdue)
            throw new InvalidOperationException("Invarianta a fost încălcată: un task Done nu poate fi Overdue.");
    }

    protected virtual void CompleteCore()
    {
        Status = TaskStatus.Done;
    }

    public void SetStatus(TaskStatus status)
    {
        Status = status;
    }
}