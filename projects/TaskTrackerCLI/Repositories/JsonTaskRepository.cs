
using System.Text.Json;
using System.Text.Json.Nodes;
using TaskTrackerCLI.Models;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Repositories
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly JsonDataSource _context;
        public string FilePath { get; }

        public JsonTaskRepository(string filePath)
        {
            this.FilePath = filePath;
            _context = new JsonDataSource(filePath);
            
        }

        public AppDataJsonModel GetAllTasks()
        {
            AppDataJsonModel? model = _context.jsonModel;
            return model ?? throw new Exception("Error: the task path doesnt exist.");
        }

        public async Task SaveTask(AppDataJsonModel model)
        {
            await _context.SaveTasks(model);
        }

        public async Task RemoveTask(int taskId)
        {
           _context.jsonModel.Tasks.RemoveAll(t => t.Id == taskId);
           await _context.SaveTasks(_context.jsonModel);
        }

        public void PrintTasksByStatus(Models.TaskStatus status)
        {
            var filteredTasks = _context.jsonModel.Tasks.Where(t => t.Status == status).ToList();
            foreach (var task in filteredTasks)
            {
                PrintTask(task);
            }

            if (filteredTasks.Count == 0)
            {
                Console.WriteLine($"No tasks with status '{status}' found.");
            }
        }

        public void PrintTask(TaskItem task)
        {
            Console.WriteLine(JsonSerializer.Serialize(task, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
