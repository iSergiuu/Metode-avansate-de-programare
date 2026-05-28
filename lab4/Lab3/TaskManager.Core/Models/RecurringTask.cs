using TaskStatus = TaskManager.Core.Enums.TaskStatus;
using TaskType = TaskManager.Core.Enums.TaskType;
namespace TaskManager.Core.Models;

public class RecurringTask : TaskItem
{
    public override TaskType TaskType => TaskType.Recurring;

    public RecurringTask()
    {
    }

    public RecurringTask(DateTime dueDate, int recurrenceInterval)
    {
        DueDate = dueDate;
        RecurrenceInterval = recurrenceInterval;
    }

    public void Configure(DateTime dueDate, int recurrenceInterval)
    {
        DueDate = dueDate;
        RecurrenceInterval = recurrenceInterval;
    }

    protected override void CompleteCore()
    {
        Status = TaskStatus.Done;

        if (DueDate.HasValue && RecurrenceInterval.HasValue)
        {
            DueDate = DueDate.Value.AddDays(RecurrenceInterval.Value);
        }
    }
}