using System.CommandLine;
using System.Reflection.Metadata;
using TaskTrackerCLI.Models;

namespace TaskTrackerCLI.Commands
{
    public class RemoveCommand
    {
        private readonly string _path;
        public RemoveCommand(string path) {
            _path = path;

            argument = new("task-id")
            {
                Description = "A positional argument that receives a task id to be removed."
            };
            command = new("delete", "Remove a task from the list.");
            command.SetAction(Handle);
            command.Arguments.Add(argument);
        }

        public Argument<string> argument;
        public Command command { get; }
        public AppDataJsonModel model { get; set; }
        public void Handle(ParseResult parseResult)
        {


            string? result = parseResult.GetValue(argument);

           


        }
    }
}


