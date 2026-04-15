using System.CommandLine;
using System.Reflection.Metadata;
using System.Text.Json;
using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Commands
{
    public class RemoveCommand
    {
        private readonly string _path;
        public RemoveCommand(string path)
        {
            _path = path;

            argument = new("task-id")
            {
                Description = "A positional argument that receives a task id to be removed."
            };
            command = new("delete", "Remove a task from the list.");
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
        public void Execute(string path, string result)
        {

            model = LoadTasks(path);
            int.TryParse(result, out int taskId);

            model.Tasks.RemoveAll(t => t.id == taskId);
            SaveTasks(path);

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


