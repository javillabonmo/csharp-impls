
using System.Text.Json;
using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Repositories
{
    public class JsonRepository
    {
        public string path { get; }
        public AppDataJsonModel jsonModel { get; }
        public JsonRepository(string filePath) {
            path = filePath;
            Setup();
            jsonModel = LoadTasks() ?? new AppDataJsonModel();
        }

        public AppDataJsonModel? LoadTasks()
        {
            if (!File.Exists(path)) return null;
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppDataJsonModel>(json);
        }

        public void SaveTasks(AppDataJsonModel newModel)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(newModel, options);
            File.WriteAllText(path, json);
        }

        public void Setup()
        {
            Console.WriteLine($"Checking if json file exists at: {path}");

            string? directory = Path.GetDirectoryName(path);
            if (directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(path))
            {
                string json = JsonSerializer.Serialize(jsonModel ?? new AppDataJsonModel());
                File.WriteAllText(path, json);
            }

        }
    }
}
