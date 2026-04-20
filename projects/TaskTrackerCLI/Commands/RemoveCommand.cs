using System.CommandLine;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Commands
{
    public sealed class RemoveCommand
    {
        private readonly ITaskRepository _repository;

        public RemoveCommand(ITaskRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            _argument = new("task-id")
            {
                Description = "A positional argument that receives a task id to be removed."
            };
            _command = new("delete", "Remove a task from the list.");
            _command.SetAction(Handle);
            _command.Arguments.Add(_argument);
        }

        private readonly Argument<string> _argument;
        private readonly Command _command;

            public Command Command => _command;
        public async Task Handle(ParseResult parseResult)
        {


            string? result = parseResult.GetValue(_argument);
            await Execute(result);



        }
        public async Task Execute(string? result)
        {  
            int.TryParse(result, out int taskId);

            await _repository.RemoveTask(taskId);
        }
    }
}


