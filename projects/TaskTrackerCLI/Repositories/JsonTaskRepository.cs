
using TaskTrackerCLI.Models;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Repositories
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly JsonDataSource _context;
        public string filePath { get; }

        public JsonTaskRepository(string filePath)
        {
            this.filePath = filePath;
            _context = new JsonDataSource(filePath);
            
        }

        public async Task<AppDataJsonModel> GetAllTasks()
        {
            AppDataJsonModel? model = await _context.LoadTasks();
            return model ?? throw new Exception("Error: the task path doesnt exist.");
        }

        public async Task SaveTask(AppDataJsonModel model)
        {
            await _context.SaveTasks(model);
        }

       

    }
}
