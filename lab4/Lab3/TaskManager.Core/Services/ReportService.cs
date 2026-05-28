using TaskManager.Core.Interfaces;
using TaskStatus = TaskManager.Core.Enums.TaskStatus;

namespace TaskManager.Core.Services;

public class ReportService
{
    private readonly ITaskReader _reader;

    public ReportService(ITaskReader reader)
    {
        _reader = reader;
    }

    public string GenerateSummary()
    {
        var tasks = _reader.GetAll();
        var doneCount = tasks.Count(t => t.Status == TaskStatus.Done);

        return $"Total: {tasks.Count}, Done: {doneCount}";
    }
}