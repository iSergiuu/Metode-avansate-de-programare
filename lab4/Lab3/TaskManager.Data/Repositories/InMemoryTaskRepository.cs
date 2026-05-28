using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Data.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public IReadOnlyList<TaskItem> GetAll()
    {
        return _tasks.ToList();
    }

    public TaskItem? GetById(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }

    public void Add(TaskItem task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        task.Id = _nextId++;
        _tasks.Add(task);
    }

    public void Update(TaskItem task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        var existing = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existing == null)
            throw new InvalidOperationException("Task-ul nu există.");

        var index = _tasks.IndexOf(existing);
        _tasks[index] = task;
    }

    public void Delete(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
            throw new InvalidOperationException("Task-ul nu există.");

        _tasks.Remove(task);
    }
}