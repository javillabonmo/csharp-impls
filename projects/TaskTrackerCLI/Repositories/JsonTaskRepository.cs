
using TaskTrackerCLI.Models;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Repositories
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly JsonRepository _context;
        public string filePath { get; }

        public JsonTaskRepository(string filePath)
        {
            this.filePath = filePath;
            _context = new JsonRepository(filePath);
            
        }

        public AppDataJsonModel GetAllTasks()
        {
            return _context.LoadTasks();
        }
        public void Save(AppDataJsonModel model)
        {
            _context.SaveTasks(model);
        }

       

    }
}
