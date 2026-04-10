using System.CommandLine;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json;
using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Commands
{
    public class ListCommand
    {
        private readonly string _path;
        public Argument<string> argument;
        public Command command { get; }
        public Command doneSubcommand { get; }
        public Command todoSubcommand { get; }
        public Command inProgressSubcommand { get; }
        public AppDataJsonModel model { get; set; }
        public ListCommand(string path)
        {

            _path = path;

            
            command = new("list", "List the existing tasks");
            command.SetAction(Handle);
            //done
            doneSubcommand = new Command("done", "List the completed tasks");
            doneSubcommand.SetAction(HandleDone);
            //todo
            todoSubcommand = new Command("todo", "List the tasks that are yet to be done");
            todoSubcommand.SetAction(HandleTodo);
            //in-progress
            inProgressSubcommand = new Command("in-progress", "List the tasks that are in progress");
            inProgressSubcommand.SetAction(HandleInProgress);
            //subcommand handlers
            command.Subcommands.Add(doneSubcommand);
            command.Subcommands.Add(todoSubcommand);
            command.Subcommands.Add(inProgressSubcommand);
            
        }
        public void Handle(ParseResult parseResult)
        {


            

            
                Console.WriteLine("The 'list' command does not require any arguments");
                Execute(_path);
            

            


        }

        public void HandleDone(ParseResult parseResult)
        {
            Console.WriteLine("Listing completed tasks:");
            model = LoadTasks(_path);
            foreach (var task in model.Tasks)
            {
                if (task.status == TaskModel.Status.done)
                {
                    Console.WriteLine($"ID: {task.id}, Description: {task.description}, Status: {task.status}, Created At: {task.createdAt}, Updated At: {task.updatedAt}");
                }
            }
        }
        
        public void HandleTodo(ParseResult parseResult)
        {
            Console.WriteLine("Listing tasks that are yet to be done:");
            model = LoadTasks(_path);
            foreach (var task in model.Tasks)
            {
                if (task.status == TaskModel.Status.todo)
                {
                    Console.WriteLine($"ID: {task.id}, Description: {task.description}, Status: {task.status}, Created At: {task.createdAt}, Updated At: {task.updatedAt}");
                }
            }
        }
        public void HandleInProgress(ParseResult parseResult)
        {
            Console.WriteLine("Listing tasks that are in progress:");
            model = LoadTasks(_path);
            foreach (var task in model.Tasks)
            {
                if (task.status == TaskModel.Status.inProgress)
                {
                    Console.WriteLine($"ID: {task.id}, Description: {task.description}, Status: {task.status}, Created At: {task.createdAt}, Updated At: {task.updatedAt}");
                }
            }
        }
        public void Execute(string path)
        {

            model = LoadTasks(path);
            Console.WriteLine($"Tasks list: ");
           
                foreach (var task in model.Tasks)
                {
                    Console.WriteLine($"ID: {task.id}, Description: {task.description}, Status: {task.status}, Created At: {task.createdAt}, Updated At: {task.updatedAt}");
            }
        }
        private AppDataJsonModel? LoadTasks(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppDataJsonModel>(json);
        }

    }
}


