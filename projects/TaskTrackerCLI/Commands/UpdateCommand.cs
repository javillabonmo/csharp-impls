using System.CommandLine;
using System.Reflection.Metadata;
using System.Text.Json;
using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Commands
{
    public class UpdateCommand
    {
        private readonly string _path;
        public AppDataJsonModel model { get; set; }
        public Command command { get; }


        public Argument<string> argument;
        public Argument<string> descArgument;


        public UpdateCommand(string path)
        {
            _path = path;
            argument = new("task-id")
            {
                Description = "A positional argument that receives a task id to be updated."
            };
            descArgument = new("description")
            {
                Description = "A positional argument that receives a new description for the task."
            };


            command = new("update", "Update an existing task");




            command.Arguments.Add(argument);
            command.Arguments.Add(descArgument);
            command.SetAction(Handle);


        }

        public void Handle(ParseResult parseResult)
        {


            string? result = parseResult.GetValue(argument);
            string? newDescription = parseResult.GetValue(descArgument);
            Execute(_path, result, newDescription);

           

        }




        public void Execute(string path, string result, string newDescription)
        {

            model = LoadTasks(path);
            int.TryParse(result, out int taskId);
            model.Tasks.Where(t => t.id == taskId).ToList().ForEach(t =>
            {
                t.description = newDescription;
                t.updatedAt = DateTime.Now;
            });
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



