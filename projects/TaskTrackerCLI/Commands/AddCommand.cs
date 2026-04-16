using System.CommandLine;

using TaskTrackerCLI.Models;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Commands
{
    public class AddCommand
    {
        private readonly string _path;

        private readonly ITaskRepository _repository;
        public AddCommand(ITaskRepository repository)
        {
            _repository = repository;
            _path = repository.filePath;

            argument = new("task")
            {
                Description = "A positional argument that receives a task."
            };
            command = new("add", "Add an entry to the file.");
            command.SetAction(Handle);
            command.Arguments.Add(argument);

        }

        public Argument<string> argument;
        public Command command { get; }

        public void Handle(ParseResult parseResult)
        {


            string? result = parseResult.GetValue(argument);

            Execute(_path, result);


        }

        public void Execute(string path, string? argument)
        {
            AppDataJsonModel model = _repository.GetAllTasks();

            TaskModel newTask = new TaskModel
            {
                id = model.Tasks.Count > 0 ? model.Tasks.Max(t => t.id) + 1 : 1,
                description = argument ?? string.Empty,
                status = TaskModel.Status.todo,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };

            model.Tasks.Add(newTask);
            _repository.Save(model);

            Console.WriteLine($"Task added successfully ID: {newTask.id}");
        }

        

    }
}


