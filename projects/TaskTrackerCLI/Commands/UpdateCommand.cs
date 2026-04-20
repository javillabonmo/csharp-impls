using System.CommandLine;

using TaskTrackerCLI.Models;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Commands
{
    public sealed class UpdateCommand
    {

        private readonly ITaskRepository _repository;

        private readonly Command _command;
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

        public async Task Handle(ParseResult parseResult)
        {


            string? result = parseResult.GetValue(_argument);
            string? newDescription = parseResult.GetValue(_descArgument);
           await Execute(result, newDescription);

           

        }




        public async Task Execute(string? result, string? newDescription)
        {

            AppDataJsonModel model = _repository.GetAllTasks();
            int.TryParse(result, out int taskId);

            model.Tasks.Where(t => t.id == taskId).ToList().ForEach(t =>
            {
                t.description = newDescription ?? t.description;
                t.updatedAt = DateTime.Now;
            });

            await _repository.SaveTask(model);
        }

        public sealed class MarkStatusCommand
        {
            private readonly ITaskRepository _repository;
            public MarkStatusCommand(ITaskRepository repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
        }

        public sealed class MarkInProgressCommand
        {
            private readonly ITaskRepository _repository;
            public MarkInProgressCommand(ITaskRepository repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
        }

    }
}



