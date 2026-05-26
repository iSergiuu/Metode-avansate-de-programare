using TaskManager.Core.Models;

namespace TaskManager.Core.Services;

public class TaskValidator
{
    public void Validate(TaskItem task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentException("Titlul nu poate fi gol.");

        if (task.Title.Length > 200)
            throw new ArgumentException("Titlul nu poate avea mai mult de 200 de caractere.");

        if (task is DeadlineTask deadlineTask)
        {
            if (!deadlineTask.DueDate.HasValue)
                throw new ArgumentException("DeadlineTask trebuie să aibă DueDate.");

            if (deadlineTask.DueDate.Value <= DateTime.UtcNow)
                throw new ArgumentException("DueDate trebuie să fie în viitor pentru DeadlineTask.");
        }

        if (task is RecurringTask recurringTask)
        {
            if (!recurringTask.DueDate.HasValue)
                throw new ArgumentException("RecurringTask trebuie să aibă DueDate.");

            if (!recurringTask.RecurrenceInterval.HasValue || recurringTask.RecurrenceInterval.Value <= 0)
                throw new ArgumentException("RecurringTask trebuie să aibă un RecurrenceInterval mai mare decât 0.");
        }
    }
}