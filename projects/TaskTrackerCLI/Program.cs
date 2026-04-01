namespace TaskTrackerCLI

{
    using System.CommandLine;
    using System.Text.Json;
    using System.Threading.Tasks;
    using TaskTrackerCLI.Models;

    internal
        class Program
    {

        static async Task<int> Main(string[] args)
        {

            HandleJsonFile("taskdb.json");

            RootCommand rootCommand = new("Sample app for System.CommandLine");

            Argument<string> addArgument = new("add")
            {
                Description = "A positional argument that receives a task."
            };

            Command addCommand = new("add", "Add an entry to the file.");
            addCommand.SetAction((parseResult) =>
            {
                string? result = parseResult.GetValue(addArgument);
                Console.WriteLine($"You entered: {result}");

            });

            addCommand.Arguments.Add(addArgument);

            rootCommand.Subcommands.Add(addCommand);


            return rootCommand.Parse(args).Invoke();

        }
        static void HandleJsonFile(string file)
        {
            string basePath = AppContext.BaseDirectory;
            string outputPath = Path.Combine(basePath, "output");
            string path = Path.Combine(outputPath, file);

            Console.WriteLine($"Checking if json file exists at: {path}");
            Directory.CreateDirectory(outputPath);
            if (!File.Exists(path))
            {
                var configDefault = new Config
                {
                    NombreApp = "MiApp",
                    Version = "1.0.0"
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(configDefault, options);

                File.WriteAllText(path, json);
            }

        }


    }
}
