using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        public AppDataJsonModel GetAllTasks();
        public string filePath { get; }
        public void Save(AppDataJsonModel model);
    }
}
