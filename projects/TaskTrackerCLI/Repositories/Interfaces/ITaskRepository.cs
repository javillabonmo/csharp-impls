using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        public Task<AppDataJsonModel> GetAllTasks();
        public string filePath { get; }
        public Task SaveTask(AppDataJsonModel model);
    }
}
