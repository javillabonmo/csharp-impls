using System.CommandLine;

using TaskTrackerCLI.Models;
using TaskTrackerCLI.Repositories.Interfaces;

namespace TaskTrackerCLI.Commands
{
    public sealed class ListCommand
    {
        private readonly ITaskRepository _repository;
        private readonly Command _command;
        private readonly Command _doneSubcommand;
        private readonly Command _todoSubcommand;
        private readonly Command _inProgressSubcommand;

        public Command Command => _command;

        public ListCommand(ITaskRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            _command = new Command("list", "List the existing tasks");
            _command.SetAction(Handle);

            _doneSubcommand = new Command("done", "List the completed tasks");
            _doneSubcommand.SetAction(HandleDone);

            _todoSubcommand = new Command("todo", "List the tasks that are yet to be done");
            _todoSubcommand.SetAction(HandleTodo);

            _inProgressSubcommand = new Command("in-progress", "List the tasks that are in progress");
            _inProgressSubcommand.SetAction(HandleInProgress);

            _command.Subcommands.Add(_doneSubcommand);
            _command.Subcommands.Add(_todoSubcommand);
            _command.Subcommands.Add(_inProgressSubcommand);
        }

        public void Handle(ParseResult parseResult)
        {
            Execute();
        }

        public void HandleDone(ParseResult parseResult)
        {
            Console.WriteLine("Listing completed tasks:");
             _repository.PrintTasksByStatus(TaskModel.Status.done);
        }

        public  void HandleTodo(ParseResult parseResult)
        {
            Console.WriteLine("Listing tasks that are yet to be done:");
            _repository.PrintTasksByStatus(TaskModel.Status.todo);
        }

        public void HandleInProgress(ParseResult parseResult)
        {
            Console.WriteLine("Listing tasks that are in progress:");
            _repository.PrintTasksByStatus(TaskModel.Status.inProgress);
        }

        public void Execute()
        {
            AppDataJsonModel model = _repository.GetAllTasks();
            if (model.Tasks.Count == 0)
            {
                Console.WriteLine("Tasks list: (empty)");
                return;
            }

            Console.WriteLine("Tasks list:");
            foreach (var task in model.Tasks)
            {
                _repository.PrintTask(task);
            }
        }
    }
}