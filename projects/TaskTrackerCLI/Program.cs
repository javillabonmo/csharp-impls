namespace TaskTrackerCLI

{
    using System.CommandLine;
    using System.CommandLine.Parsing;
    using System.Threading.Tasks;
    
    internal 
        class Program
    {

    static async Task<int> Main(string[] args)
        {
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
        
    }

}
