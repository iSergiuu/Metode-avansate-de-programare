using NUnit.Framework;
using TaskManager.Core.Models;
using TaskManager.Core.Services;

namespace TaskManager.Tests;

public class TaskValidatorTests
{
    private TaskValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TaskValidator();
    }

    [Test]
    public void Validate_ShouldThrow_WhenTitleIsEmpty()
    {
        var task = new TaskItem { Title = "" };

        Assert.Throws<ArgumentException>(() => _validator.Validate(task));
    }

    [Test]
    public void Validate_ShouldThrow_WhenTitleTooLong()
    {
        var task = new TaskItem { Title = new string('a', 201) };

        Assert.Throws<ArgumentException>(() => _validator.Validate(task));
    }

    [Test]
    public void Validate_ShouldPass_ForValidTask()
    {
        var task = new TaskItem { Title = "Task valid" };

        Assert.DoesNotThrow(() => _validator.Validate(task));
    }
}