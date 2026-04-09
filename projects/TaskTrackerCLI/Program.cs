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

            RootCommand rootCommand = new("Sample app for System.CommandLine");

            AddCommand addSubCommand = new AddCommand(getPath(file));
            addSubCommand.model = jsonModel;






            rootCommand.Subcommands.Add(addSubCommand.command);


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
    }
}
