using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Notifications;
using TaskManager.Core.Services;
using TaskManager.Data.Repositories;
using TaskStatus = TaskManager.Core.Enums.TaskStatus;
using NotificationType = TaskManager.Core.Enums.NotificationType;

namespace TaskManager.UI;

public partial class Form1 : Form
{
    private readonly TaskService _taskService;

    public Form1()
    {
        InitializeComponent();

        ITaskRepository repository = new SqliteTaskRepository();
        var validator = new TaskValidator();

        IReadOnlyDictionary<NotificationType, ITaskNotifier> notifiers =
            new Dictionary<NotificationType, ITaskNotifier>
            {
                { NotificationType.Console, new ConsoleNotifier() },
                { NotificationType.Email, new EmailNotifier() },
                { NotificationType.FileLog, new FileLogNotifier() },
                { NotificationType.Slack, new SlackNotifier() }
            };

        _taskService = new TaskService(repository, validator, notifiers);
        comboBoxStatus.DataSource = Enum.GetValues(typeof(TaskStatus));

        LoadTasks();
    }

    private void LoadTasks()
    {
        var tasks = _taskService.GetAllTasks().ToList();

        dataGridViewTasks.DataSource = null;
        dataGridViewTasks.DataSource = tasks;

        dataGridViewTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridViewTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewTasks.MultiSelect = false;
        dataGridViewTasks.ReadOnly = true;
    }

    private void buttonAdd_Click(object sender, EventArgs e)
    {
        try
        {
            var title = textBoxTitle.Text.Trim();

            var task = new TaskItem
            {
                Title = title
            };

            _taskService.AddTask(task);

            textBoxTitle.Clear();
            LoadTasks();

            MessageBox.Show("Task adăugat cu succes.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare");
        }
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (dataGridViewTasks.CurrentRow == null)
            {
                MessageBox.Show("Selectează un task.");
                return;
            }

            var selectedTask = dataGridViewTasks.CurrentRow.DataBoundItem as TaskItem;
            if (selectedTask == null)
            {
                MessageBox.Show("Task invalid.");
                return;
            }

            _taskService.DeleteTask(selectedTask.Id);
            LoadTasks();

            MessageBox.Show("Task șters.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare");
        }
    }

    private void buttonRefresh_Click(object sender, EventArgs e)
    {
        LoadTasks();
    }

    private void buttonComplete_Click(object sender, EventArgs e)
    {
        try
        {
            if (dataGridViewTasks.CurrentRow == null)
            {
                MessageBox.Show("Selectează un task.");
                return;
            }

            var task = dataGridViewTasks.CurrentRow.DataBoundItem as TaskItem;

            if (task == null)
            {
                MessageBox.Show("Task invalid.");
                return;
            }

            _taskService.CompleteTask(task.Id);

            LoadTasks();

            MessageBox.Show("Task completat.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void comboBoxStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedStatus = (TaskStatus)comboBoxStatus.SelectedItem;

        var filtered = _taskService.FilterByStatus(selectedStatus);

        dataGridViewTasks.DataSource = null;
        dataGridViewTasks.DataSource = filtered;
    }
}