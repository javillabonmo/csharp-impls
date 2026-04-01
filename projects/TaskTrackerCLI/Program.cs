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

            HandleJsonFile("taskdb.json");

            RootCommand rootCommand = new("Sample app for System.CommandLine");

            AddCommand addSubCommand = new AddCommand();

            

            
            

            rootCommand.Subcommands.Add(addSubCommand.command);


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
