using NotificationType = TaskManager.Core.Enums.NotificationType;
using TaskPriority = TaskManager.Core.Enums.TaskPriority;
using TaskStatus = TaskManager.Core.Enums.TaskStatus;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Services;

public class TaskService
{
    private readonly ITaskRepository _repository;
    private readonly TaskValidator _validator;
    private readonly IReadOnlyDictionary<NotificationType, ITaskNotifier> _notifiers;

    public TaskService(
        ITaskRepository repository,
        TaskValidator validator,
        IReadOnlyDictionary<NotificationType, ITaskNotifier> notifiers)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _notifiers = notifiers ?? throw new ArgumentNullException(nameof(notifiers));
    }

    public IReadOnlyList<TaskItem> GetAllTasks()
    {
        return _repository.GetAll();
    }

    public TaskItem? GetTaskById(int id)
    {
        return _repository.GetById(id);
    }

    public void AddTask(TaskItem task)
    {
        _validator.Validate(task);
        _repository.Add(task);
    }

    public void UpdateTask(TaskItem task)
    {
        _validator.Validate(task);

        var existingTask = _repository.GetById(task.Id);
        if (existingTask == null)
            throw new InvalidOperationException("Task-ul nu există.");

        _repository.Update(task);
    }

    public void DeleteTask(int id)
    {
        var existingTask = _repository.GetById(id);
        if (existingTask == null)
            throw new InvalidOperationException("Task-ul nu există.");

        _repository.Delete(id);
    }

    public void CompleteTask(int id)
    {
        var task = _repository.GetById(id);
        if (task == null)
            throw new InvalidOperationException("Task-ul nu există.");

        task.Complete();
        _repository.Update(task);

        if (!_notifiers.TryGetValue(task.NotificationType, out var notifier))
            throw new InvalidOperationException($"Nu există notifier pentru tipul {task.NotificationType}.");

        notifier.Notify(task);
    }

    public IReadOnlyList<TaskItem> FilterByStatus(TaskStatus status)
    {
        return _repository
            .GetAll()
            .Where(t => t.Status == status)
            .ToList();
    }

    public IReadOnlyList<TaskItem> FilterByPriority(TaskPriority priority)
    {
        return _repository
            .GetAll()
            .Where(t => t.Priority == priority)
            .ToList();
    }
}