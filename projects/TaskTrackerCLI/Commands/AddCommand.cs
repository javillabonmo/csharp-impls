using System.CommandLine;

using TaskTrackerCLI.Models;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Commands
{
    public sealed class AddCommand
    {

        private readonly ITaskRepository _repository;
        public AddCommand(ITaskRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            

            _argument = new("task")
            {
                Description = "A positional argument that receives a task."
            };

            _command = new("add", "Add an entry to the file.");
            _command.SetAction(Handle);
            _command.Arguments.Add(_argument);

        }

        private readonly Argument<string> _argument;

        private readonly Command _command;
        public Command Command => _command;

        public void Handle(ParseResult parseResult)
        {


            string? result = parseResult.GetValue(_argument);

             Execute(result);


        }

        public async Task Execute(string? argument)
        {
            AppDataJsonModel model =  _repository.GetAllTasks();

            TaskModel newTask = new TaskModel
            {
                id = model.Tasks.Count > 0 ? model.Tasks.Max(t => t.id) + 1 : 1,
                description = argument ?? string.Empty,
                status = TaskModel.Status.todo,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };

            model.Tasks.Add(newTask);
            await _repository.SaveTask(model);

            Console.WriteLine($"Task added successfully ID: {newTask.id}");
        }

        

    }
}


