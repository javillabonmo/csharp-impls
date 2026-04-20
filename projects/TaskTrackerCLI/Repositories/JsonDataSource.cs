
using System.Text.Json;
using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Repositories
{
    public class JsonDataSource
    {
        public string path { get; }
        public AppDataJsonModel jsonModel { get; }
        public JsonDataSource(string filePath) {
            path = filePath;
            _ = Setup();
            jsonModel = LoadTasks().Result ?? new AppDataJsonModel();
        }

        private async Task<AppDataJsonModel?> LoadTasks()
        {
            if (!File.Exists(path)) return null;
            try
            {
                string json = await File.ReadAllTextAsync(path);
                return JsonSerializer.Deserialize<AppDataJsonModel>(json);
            }
            catch (JsonException)
            {
                Console.WriteLine("Error: The task file contains invalid data. A backup will be created and a new file will be generated.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: Unable to access the task file: {ex.Message}");
            }

            return null;
        }

        public async Task SaveTasks(AppDataJsonModel newModel)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(newModel, options);
            try
            {
                await File.WriteAllTextAsync(path, json);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: Task file not found. The application will create a new one.");
            }
            catch (JsonException)
            {
                Console.WriteLine("Error: The task file contains invalid data. A backup will be created and a new file will be generated.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: Unable to access the task file: {ex.Message}");
            }
        }

        private async Task Setup()
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

                try
                {
                    _ = File.WriteAllTextAsync(path, json);
                }
                catch (JsonException)
                {
                    Console.WriteLine("Error: The task file contains invalid data. A backup will be created and a new file will be generated.");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error: Unable to access the task file: {ex.Message}");
                }
                
            }

        }

        
    }
}
