using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        public AppDataJsonModel GetAllTasks();
        public string FilePath { get; }
        public Task SaveTask(AppDataJsonModel model);

        public Task RemoveTask(int taskId);

        public void PrintTasksByStatus(Models.TaskStatus status);
        public void PrintTask(TaskItem task);
    }
}
