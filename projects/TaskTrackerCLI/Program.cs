namespace TaskTrackerCLI

{
    using System.CommandLine;
    using System.Threading.Tasks;
    using TaskTrackerCLI.Commands;
    using TaskTrackerCLI.Repositories;
    using TaskTrackerCLI.Repositories.Interfaces;

    internal class Program
    {


        static async Task<int> Main(string[] args)
        {
            string file = "tasks.json";
            ITaskRepository jsonTaskRepository = new JsonTaskRepository(GetPath(file));

            RootCommand rootCommand = new("The command line accept user actions and inputs as arguments, and store the tasks in a JSON file");

            AddCommand addSubCommand = new(jsonTaskRepository);


            ListCommand listSubCommand = new(jsonTaskRepository);

            UpdateCommand updateSubCommand = new(jsonTaskRepository);
            UpdateCommand.MarkInProgressCommand markInProgressSubCommand = new(jsonTaskRepository);
            UpdateCommand.MarkCompletedCommand markCompletedSubCommand = new(jsonTaskRepository);

            RemoveCommand deleteSubCommand = new(jsonTaskRepository);

            rootCommand.Subcommands.Add(addSubCommand.Command);
            rootCommand.Subcommands.Add(listSubCommand.Command);
            rootCommand.Subcommands.Add(updateSubCommand.Command);
            rootCommand.Subcommands.Add(deleteSubCommand.Command);
            rootCommand.Subcommands.Add(markInProgressSubCommand.Command);
            rootCommand.Subcommands.Add(markCompletedSubCommand.Command);
            return rootCommand.Parse(args).Invoke();

        }


        public static string GetPath(string fileName)
        {
            string basePath = AppContext.BaseDirectory;
            string outputPath = Path.Combine(basePath, "output");
            return Path.Combine(outputPath, fileName);

        }
    }
}
