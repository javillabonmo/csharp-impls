using System.CommandLine;
using TaskTrackerCLI.Models;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Commands
{
    public sealed class AddCommand
    {
        private const int MaxDescriptionLength = 200;
        private readonly ITaskRepository _repository;

        public Command Command => _command;

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

        public async Task Handle(ParseResult parseResult)
        {
            string? result = parseResult.GetValue(_argument);
            await Execute(result);
        }

        public async Task Execute(string? argument)
        {
            string description = argument ?? string.Empty;
            
            if (description.Length > MaxDescriptionLength)
            {
                Console.WriteLine($"Error: Description exceeds maximum length of {MaxDescriptionLength} characters.");
                return;
            }

            AppDataJsonModel model = _repository.GetAllTasks();

            TaskItem newTask = new TaskItem
            {
                Id = model.Tasks.Count > 0 ? model.Tasks.Max(t => t.Id) + 1 : 1,
                Description = description,
                Status = Models.TaskStatus.Todo,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            model.Tasks.Add(newTask);
            await _repository.SaveTask(model);

            Console.WriteLine($"Task added successfully ID: {newTask.Id}");
        }
    }
}