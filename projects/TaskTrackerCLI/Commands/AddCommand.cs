using System.CommandLine;
using System.Text.Json;
using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Commands
{
    public class AddCommand
    {
        private readonly string _path;
        public AddCommand(string path)
        {
            _path = path;

            argument = new("task")
            {
                Description = "A positional argument that receives a task."
            };
            command = new("string", "Add an entry to the file.");
            command.SetAction(Handle);
            command.Arguments.Add(argument);

        }

        public Argument<string> argument;
        public Command command { get; }
        public AppDataJsonModel model { get; set; }

        public void Handle(ParseResult parseResult)
        {


            string? result = parseResult.GetValue(argument);

            Execute(_path, result);


        }

        public void Execute(string path, string? argument)
        {

            model = LoadTasks(path);

            TaskModel newTask = new TaskModel
            {
                id = model.Tasks.Count > 0 ? model.Tasks.Max(t => t.id) + 1 : 1,
                description = argument ?? string.Empty,
                status = TaskModel.Status.todo,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };

            model.Tasks.Add(newTask);
            SaveTasks(path);

            Console.WriteLine($"Task added successfully ID: {newTask.id}");
        }

        private AppDataJsonModel? LoadTasks(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppDataJsonModel>(json);
        }

        private void SaveTasks(string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(model, options);
            File.WriteAllText(path, json);
        }

    }
}


