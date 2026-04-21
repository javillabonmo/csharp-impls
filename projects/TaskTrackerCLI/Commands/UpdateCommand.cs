using System.CommandLine;
using TaskTrackerCLI.Models;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Commands
{
    public sealed class UpdateCommand
    {
        private const int MaxDescriptionLength = 200;
        private readonly ITaskRepository _repository;

        public Command Command => _command;

        private Argument<string> _argument;
        private Argument<string> _descArgument;

        public UpdateCommand(ITaskRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _argument = new("task-id")
            {
                Description = "A positional argument that receives a task id to be updated."
            };
            _descArgument = new("description")
            {
                Description = "A positional argument that receives a new description for the task."
            };

            _command = new("update", "Update an existing task");
            _command.Arguments.Add(_argument);
            _command.Arguments.Add(_descArgument);
            _command.SetAction(Handle);
        }

        private readonly Command _command;

        public async Task Handle(ParseResult parseResult)
        {
            string? result = parseResult.GetValue(_argument);
            string? newDescription = parseResult.GetValue(_descArgument);
            await Execute(result, newDescription);
        }

        public async Task Execute(string? result, string? newDescription)
        {
            string description = newDescription ?? string.Empty;

            if (description.Length > MaxDescriptionLength)
            {
                Console.WriteLine($"Error: Description exceeds maximum length of {MaxDescriptionLength} characters.");
                return;
            }

            AppDataJsonModel model = _repository.GetAllTasks();
            int.TryParse(result, out int taskId);

            model.Tasks.Where(t => t.Id == taskId).ToList().ForEach(t =>
            {
                t.Description = description;
                t.UpdatedAt = DateTime.Now;
            });

            await _repository.SaveTask(model);
        }

        public sealed class MarkCompletedCommand
        {
            private readonly ITaskRepository _repository;
            private readonly Command _command;
            public Command Command => _command;
            private Argument<string> _argument;

            public MarkCompletedCommand(ITaskRepository repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _command = new("mark-completed", "Mark a task as completed");
                _argument = new("task-id")
                {
                    Description = "A positional argument that receives a task id to be marked as completed."
                };
                _command.Arguments.Add(_argument);
                _command.SetAction(Handle);
            }

            public async Task Handle(ParseResult parseResult)
            {
                string? result = parseResult.GetValue(_argument);
                await Execute(result);
            }

            public async Task Execute(string? result)
            {
                AppDataJsonModel model = _repository.GetAllTasks();
                int.TryParse(result, out int id);
                model.Tasks.Where(t => t.Id == id).ToList().ForEach(t =>
                {
                    t.Status = Models.TaskStatus.Done;
                    t.UpdatedAt = DateTime.Now;
                });
                await _repository.SaveTask(model);
            }
        }

        public sealed class MarkInProgressCommand
        {
            private readonly ITaskRepository _repository;
            private readonly Command _command;
            public Command Command => _command;
            private Argument<string> _argument;

            public MarkInProgressCommand(ITaskRepository repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _command = new("mark-in-progress", "Mark a task as in progress");
                _argument = new("task-id")
                {
                    Description = "A positional argument that receives a task id to be marked as in progress."
                };
                _command.Arguments.Add(_argument);
                _command.SetAction(Handle);
            }

            public async Task Handle(ParseResult parseResult)
            {
                string? result = parseResult.GetValue(_argument);
                await Execute(result);
            }

            private async Task Execute(string? result)
            {
                AppDataJsonModel model = _repository.GetAllTasks();
                int.TryParse(result, out int id);
                model.Tasks.Where(t => t.Id == id).ToList().ForEach(t =>
                {
                    t.Status = Models.TaskStatus.InProgress;
                    t.UpdatedAt = DateTime.Now;
                });
                await _repository.SaveTask(model);
            }
        }
    }
}