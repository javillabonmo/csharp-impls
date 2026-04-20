using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        public AppDataJsonModel GetAllTasks();
        public string FilePath { get; }
        public Task SaveTask(AppDataJsonModel model);

        public Task RemoveTask(int taskId);

        public void PrintTasksByStatus(TaskModel.Status status);
        public void PrintTask(TaskModel task);
    }
}
