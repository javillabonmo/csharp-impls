namespace TaskTrackerCLI

{
    using System.CommandLine;
    using System.Text.Json;
    using System.Threading.Tasks;
    using TaskTrackerCLI.Commands;
    using TaskTrackerCLI.Models;

    internal
        class Program
    {

        static async Task<int> Main(string[] args)
        {



            string file = "config.json";
            AppDataJsonModel jsonModel = new AppDataJsonModel();


            setup(getPath(file), jsonModel);

            RootCommand rootCommand = new("The command line accept user actions and inputs as arguments, and store the tasks in a JSON file");

            AddCommand addSubCommand = new AddCommand(getPath(file));
            addSubCommand.model = jsonModel;

            ListCommand listSubCommand = new ListCommand(getPath(file));
            listSubCommand.model = jsonModel;

            UpdateCommand updateSubCommand = new UpdateCommand(getPath(file));
            updateSubCommand.model = jsonModel;

            RemoveCommand deleteSubCommand = new RemoveCommand(getPath(file));
            deleteSubCommand.model = jsonModel;

            rootCommand.Subcommands.Add(addSubCommand.command);
            rootCommand.Subcommands.Add(listSubCommand.command);
            rootCommand.Subcommands.Add(updateSubCommand.command);
            rootCommand.Subcommands.Add(deleteSubCommand.command);
            rootCommand.Subcommands.Add(Program.InProgress());
            rootCommand.Subcommands.Add(Program.Completed());
            return rootCommand.Parse(args).Invoke();

        }
        static void setup(string path, AppDataJsonModel model)
        {


            Console.WriteLine($"Checking if json file exists at: {path}");

            if (!File.Exists(path))
            {
                string basePath = AppContext.BaseDirectory;
                string outputPath = Path.Combine(basePath, "output");
                Directory.CreateDirectory(outputPath);

                string json = JsonSerializer.Serialize(model);
                File.WriteAllText(path, json);
            }

        }

        public static string getPath(string fileName)
        {
            string basePath = AppContext.BaseDirectory;
            string outputPath = Path.Combine(basePath, "output");
            return Path.Combine(outputPath, fileName);

        }

        public static Command InProgress()
        {
            Command markInProgress;

            markInProgress = new("mark-in-progress", "Mark a task as in progress");


            Argument<string> markInProgressArgument;

            markInProgressArgument = new("task-id")
            {
                Description = "A positional argument that receives a task id to be marked as in progress."
            };

            markInProgress.Arguments.Add(markInProgressArgument);


            markInProgress.SetAction(HandleMarkInProgress);

            return markInProgress;
        }

        public static Command Completed()
        {
            Command markCompleted;
            markCompleted = new("mark-completed", "Mark a task as completed");
            Argument<string> markCompletedArgument;

            markCompletedArgument = new("task-id")
            {
                Description = "A positional argument that receives a task id to be marked as completed."
            };
            markCompleted.Arguments.Add(markCompletedArgument);
            markCompleted.SetAction(HandleMarkCompleted);

            return markCompleted;
        }
        public static void HandleMarkInProgress(ParseResult parseResult)
        {

        }
        public static void HandleMarkCompleted(ParseResult parseResult)
        {

        }
    }
}