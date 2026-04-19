namespace TaskTrackerCLI

{
    using System.CommandLine;
    using System.Text.Json;
    using System.Threading.Tasks;
    using TaskTrackerCLI.Commands;
    using TaskTrackerCLI.Models;
    using TaskTrackerCLI.Repositories;
    using TaskTrackerCLI.Repositories.Interfaces;

    internal
        class Program
    {

        static async Task<int> Main(string[] args)
        {
            string file = "tasks.json";
            ITaskRepository jsonTaskRepository = new JsonTaskRepository(getPath(file));

            
            


            

            RootCommand rootCommand = new("The command line accept user actions and inputs as arguments, and store the tasks in a JSON file");

            AddCommand addSubCommand = new AddCommand(jsonTaskRepository);


            //ListCommand listSubCommand = new ListCommand(getPath(file));
            //listSubCommand.model = jsonModel;

            //UpdateCommand updateSubCommand = new UpdateCommand(getPath(file));
            //updateSubCommand.model = jsonModel;

            //RemoveCommand deleteSubCommand = new RemoveCommand(getPath(file));
            //deleteSubCommand.model = jsonModel;

            rootCommand.Subcommands.Add(addSubCommand.Command);
            //rootCommand.Subcommands.Add(listSubCommand.command);
            //rootCommand.Subcommands.Add(updateSubCommand.command);
            //rootCommand.Subcommands.Add(deleteSubCommand.command);
            //rootCommand.Subcommands.Add(Program.InProgress(jsonModel));
            //rootCommand.Subcommands.Add(Program.Completed(jsonModel));
            return rootCommand.Parse(args).Invoke();

        }
        

        public static string getPath(string fileName)
        {
            string basePath = AppContext.BaseDirectory;
            string outputPath = Path.Combine(basePath, "output");
            return Path.Combine(outputPath, fileName);

        }

        public static Command InProgress(AppDataJsonModel model)
        {
            Command markInProgress;

            markInProgress = new("mark-in-progress", "Mark a task as in progress");


            Argument<string> markInProgressArgument;

            markInProgressArgument = new("task-id")
            {
                Description = "A positional argument that receives a task id to be marked as in progress."
            };

            markInProgress.Arguments.Add(markInProgressArgument);
            model = LoadTasks(getPath("config.json"));

            markInProgress.SetAction((parseResult) => {
                string? result = parseResult.GetValue(markInProgressArgument);

                int.TryParse(result, out int id);

                model.Tasks.ForEach(task =>
                {
                    if (task.id == id)
                    {
                     task.status = TaskModel.Status.inProgress;
                    }
                });

                Console.WriteLine(model.ToString());

                
                SaveTasks(getPath("config.json"), model);

            });

            return markInProgress;
        }

        public static Command Completed(AppDataJsonModel model)
        {
            Command markCompleted;
            markCompleted = new("mark-completed", "Mark a task as completed");
            Argument<string> markCompletedArgument;

            markCompletedArgument = new("task-id")
            {
                Description = "A positional argument that receives a task id to be marked as completed."
            };
            markCompleted.Arguments.Add(markCompletedArgument);
            model = LoadTasks(getPath("config.json"));
            markCompleted.SetAction((parseResult) => {
                string? result = parseResult.GetValue(markCompletedArgument);

                int.TryParse (result, out int id);

                model.Tasks.ForEach(task =>
                {
                    if (task.id == id)
                    {
                        task.status = TaskModel.Status.done;
                    }
                });
               
                SaveTasks(getPath("config.json"), model);

            } );


            return markCompleted;
        }

        private static void SaveTasks(string path, AppDataJsonModel model)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(model, options);
            File.WriteAllText(path, json);
        }
        private static AppDataJsonModel? LoadTasks(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppDataJsonModel>(json);
        }

    }
}