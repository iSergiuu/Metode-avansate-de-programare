using NUnit.Framework;
using TaskManager.Core.Models;
using TaskStatus = TaskManager.Core.Enums.TaskStatus;

namespace TaskManager.Tests;

public class TaskHierarchyTests
{
    [TestCase("standard")]
    [TestCase("deadline")]
    [TestCase("recurring")]
    public void Complete_ShouldSetStatusToDone_ForAllTaskTypes(string type)
    {
        TaskItem task = type switch
        {
            "deadline" => new DeadlineTask(DateTime.UtcNow.AddDays(1)),
            "recurring" => new RecurringTask(DateTime.UtcNow.AddDays(1), 2),
            _ => new TaskItem()
        };

        task.Complete();

        Assert.That(task.Status, Is.EqualTo(TaskStatus.Done));
    }

    [Test]
    public void Complete_ShouldThrow_WhenAlreadyDone()
    {
        var task = new TaskItem();
        task.Complete();

        Assert.Throws<InvalidOperationException>(() => task.Complete());
    }
}