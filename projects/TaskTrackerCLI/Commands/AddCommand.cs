using System.CommandLine;

namespace TaskTrackerCLI.Commands
{
    public class AddCommand
    {
        public AddCommand()
        {
            argument = new("add")
            {
                Description = "A positional argument that receives a task."
            };
            command = new("add", "Add an entry to the file.");
            command.SetAction(Handle);
            command.Arguments.Add(argument);

        }

        public Argument<string> argument;
        public Command command { get; }
        public void Handle(ParseResult parseResult) {

            
                string? result = parseResult.GetValue(argument);
                Console.WriteLine($"You entered: {result}");

        
        }

    }
}

    
