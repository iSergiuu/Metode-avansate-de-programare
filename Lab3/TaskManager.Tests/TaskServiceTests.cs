using NUnit.Framework;
using TaskManager.Core.Models;
using TaskManager.Core.Services;
using TaskManager.Data.Repositories;
using TaskManager.Core.Enums;
using TaskManager.Core.Interfaces;

namespace TaskManager.Tests;

public class TaskServiceTests
{
    private TaskService _service;
    private InMemoryTaskRepository _repository;
    private TaskValidator _validator;
    private Dictionary<NotificationType, ITaskNotifier> _notifiers;

    private class MockNotifier : ITaskNotifier
    {
        public bool WasCalled { get; private set; }

        public void Notify(TaskItem task)
        {
            WasCalled = true;
        }
    }

    [SetUp]
    public void Setup()
    {
        _repository = new InMemoryTaskRepository();
        _validator = new TaskValidator();

        _notifiers = new Dictionary<NotificationType, ITaskNotifier>
        {
            { NotificationType.Console, new MockNotifier() },
            { NotificationType.Email, new MockNotifier() },
            { NotificationType.FileLog, new MockNotifier() },
            { NotificationType.Slack, new MockNotifier() }
        };

        _service = new TaskService(_repository, _validator, _notifiers);
    }

    [Test]
    public void AddTask_ShouldAddTask()
    {
        var task = new TaskItem { Title = "Test task" };

        _service.AddTask(task);

        Assert.That(_repository.GetAll().Count, Is.EqualTo(1));
    }

    [Test]
    public void CompleteTask_ShouldCallNotifier()
    {
        var notifier = new MockNotifier();

        _notifiers[NotificationType.Console] = notifier;

        var task = new TaskItem
        {
            Title = "Test",
            NotificationType = NotificationType.Console
        };

        _service.AddTask(task);

        var addedTask = _repository.GetAll().First();

        _service.CompleteTask(addedTask.Id);

        Assert.That(notifier.WasCalled, Is.True);
    }

    [Test]
    public void DeleteTask_ShouldRemoveTask()
    {
        var task = new TaskItem { Title = "Test" };

        _service.AddTask(task);
        var addedTask = _repository.GetAll().First();

        _service.DeleteTask(addedTask.Id);

        Assert.That(_repository.GetAll().Count, Is.EqualTo(0));
    }
}