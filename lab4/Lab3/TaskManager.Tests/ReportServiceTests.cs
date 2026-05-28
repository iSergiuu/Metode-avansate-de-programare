using NUnit.Framework;
using TaskManager.Core.Models;
using TaskManager.Core.Services;
using TaskManager.Data.Repositories;

namespace TaskManager.Tests;

public class ReportServiceTests
{
    [Test]
    public void GenerateSummary_ShouldReturnTotalAndDoneCount()
    {
        var repo = new InMemoryTaskRepository();

        var task1 = new TaskItem { Title = "Task 1" };
        var task2 = new TaskItem { Title = "Task 2" };

        repo.Add(task1);
        repo.Add(task2);

        task1.Complete();
        repo.Update(task1);

        var reportService = new ReportService(repo);

        var summary = reportService.GenerateSummary();

        Assert.That(summary, Is.EqualTo("Total: 2, Done: 1"));
    }
}