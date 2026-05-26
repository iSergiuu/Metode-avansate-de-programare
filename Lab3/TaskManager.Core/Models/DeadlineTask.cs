using TaskStatus = TaskManager.Core.Enums.TaskStatus;
using TaskType = TaskManager.Core.Enums.TaskType;

namespace TaskManager.Core.Models;

public class DeadlineTask : TaskItem
{
    public override TaskType TaskType => TaskType.Deadline;

    public DeadlineTask()
    {
    }

    public DeadlineTask(DateTime dueDate)
    {
        DueDate = dueDate;
    }

    public void SetDueDate(DateTime dueDate)
    {
        DueDate = dueDate;
    }

    protected override void CompleteCore()
    {
        Status = TaskStatus.Done;
    }
}