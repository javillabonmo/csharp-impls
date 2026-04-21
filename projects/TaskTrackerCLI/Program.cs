namespace TaskTrackerCLI

{
    using System.CommandLine;
    using System.Threading.Tasks;
    using TaskTrackerCLI.Commands;
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


            ListCommand listSubCommand = new ListCommand(jsonTaskRepository);

            UpdateCommand updateSubCommand = new UpdateCommand(jsonTaskRepository);
            UpdateCommand.MarkInProgressCommand markInProgressSubCommand = new UpdateCommand.MarkInProgressCommand(jsonTaskRepository);
            UpdateCommand.MarkCompletedCommand markCompletedSubCommand = new UpdateCommand.MarkCompletedCommand(jsonTaskRepository);

            RemoveCommand deleteSubCommand = new RemoveCommand(jsonTaskRepository);

            rootCommand.Subcommands.Add(addSubCommand.Command);
            rootCommand.Subcommands.Add(listSubCommand.Command);
            rootCommand.Subcommands.Add(updateSubCommand.Command);
            rootCommand.Subcommands.Add(deleteSubCommand.Command);
            rootCommand.Subcommands.Add(markInProgressSubCommand.Command);
            rootCommand.Subcommands.Add(markCompletedSubCommand.Command);
            return rootCommand.Parse(args).Invoke();

        }
        

        public static string getPath(string fileName)
        {
            string basePath = AppContext.BaseDirectory;
            string outputPath = Path.Combine(basePath, "output");
            return Path.Combine(outputPath, fileName);

        }
    }
}