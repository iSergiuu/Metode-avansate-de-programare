using Microsoft.Data.Sqlite;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskPriority = TaskManager.Core.Enums.TaskPriority;
using TaskStatus = TaskManager.Core.Enums.TaskStatus;
using NotificationType = TaskManager.Core.Enums.NotificationType;

namespace TaskManager.Data.Repositories;

public class SqliteTaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public SqliteTaskRepository(string databasePath = "tasks.db")
    {
        _connectionString = $"Data Source={databasePath}";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        CREATE TABLE IF NOT EXISTS Tasks (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Title TEXT NOT NULL,
            Description TEXT NULL,
            Status TEXT NOT NULL,
            Priority INTEGER NOT NULL,
            TaskType TEXT NOT NULL,
            NotificationType TEXT NOT NULL,
            DueDate TEXT NULL,
            RecurrenceInterval INTEGER NULL,
            CreatedAt TEXT NOT NULL
        );
        ";

        command.ExecuteNonQuery();
    }

    public IReadOnlyList<TaskItem> GetAll()
    {
        var tasks = new List<TaskItem>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Tasks";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            tasks.Add(MapTask(reader));
        }

        return tasks;
    }

    public TaskItem? GetById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Tasks WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapTask(reader);
        }

        return null;
    }

    public void Add(TaskItem task)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        INSERT INTO Tasks
        (Title, Description, Status, Priority, TaskType, NotificationType, DueDate, RecurrenceInterval, CreatedAt)
        VALUES
        (@title, @description, @status, @priority, @taskType, @notificationType, @dueDate, @recurrenceInterval, @createdAt);

        SELECT last_insert_rowid();
        ";

        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", (object?)task.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@status", task.Status.ToString());
        command.Parameters.AddWithValue("@priority", (int)task.Priority);
        command.Parameters.AddWithValue("@taskType", task.TaskType.ToString());
        command.Parameters.AddWithValue("@notificationType", task.NotificationType.ToString());
        command.Parameters.AddWithValue("@dueDate", task.DueDate?.ToString("O") ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@recurrenceInterval", task.RecurrenceInterval ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@createdAt", task.CreatedAt.ToString("O"));

        var newId = (long)command.ExecuteScalar()!;
        task.Id = (int)newId;
    }

    public void Update(TaskItem task)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        UPDATE Tasks
        SET
            Title = @title,
            Description = @description,
            Status = @status,
            Priority = @priority,
            TaskType = @taskType,
            NotificationType = @notificationType,
            DueDate = @dueDate,
            RecurrenceInterval = @recurrenceInterval,
            CreatedAt = @createdAt
        WHERE Id = @id
        ";

        command.Parameters.AddWithValue("@id", task.Id);
        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", (object?)task.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@status", task.Status.ToString());
        command.Parameters.AddWithValue("@priority", (int)task.Priority);
        command.Parameters.AddWithValue("@taskType", task.TaskType.ToString());
        command.Parameters.AddWithValue("@notificationType", task.NotificationType.ToString());
        command.Parameters.AddWithValue("@dueDate", task.DueDate?.ToString("O") ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@recurrenceInterval", task.RecurrenceInterval ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@createdAt", task.CreatedAt.ToString("O"));

        var affected = command.ExecuteNonQuery();
        if (affected == 0)
            throw new InvalidOperationException("Task-ul nu există.");
    }

    public void Delete(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Tasks WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);

        var affected = command.ExecuteNonQuery();
        if (affected == 0)
            throw new InvalidOperationException("Task-ul nu există.");
    }

    private static TaskItem MapTask(SqliteDataReader reader)
    {
        var taskTypeText = reader["TaskType"].ToString()!;
        TaskItem task = taskTypeText switch
        {
            "Deadline" => new DeadlineTask(),
            "Recurring" => new RecurringTask(),
            _ => new TaskItem()
        };

        task.Id = Convert.ToInt32(reader["Id"]);
        task.Title = reader["Title"].ToString() ?? string.Empty;
        task.Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString();
        task.Priority = (TaskPriority)Convert.ToInt32(reader["Priority"]);
        task.NotificationType = Enum.Parse<NotificationType>(reader["NotificationType"].ToString()!);
        task.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()!);
        task.SetStatus(Enum.Parse<TaskStatus>(reader["Status"].ToString()!));

        var dueDateText = reader["DueDate"] == DBNull.Value ? null : reader["DueDate"].ToString();
        var recurrenceText = reader["RecurrenceInterval"] == DBNull.Value ? null : reader["RecurrenceInterval"].ToString();

        if (task is DeadlineTask deadlineTask && !string.IsNullOrWhiteSpace(dueDateText))
        {
            deadlineTask.SetDueDate(DateTime.Parse(dueDateText));
        }

        if (task is RecurringTask recurringTask &&
            !string.IsNullOrWhiteSpace(dueDateText) &&
            !string.IsNullOrWhiteSpace(recurrenceText))
        {
            recurringTask.Configure(
                DateTime.Parse(dueDateText),
                int.Parse(recurrenceText));
        }

        return task;
    }
}