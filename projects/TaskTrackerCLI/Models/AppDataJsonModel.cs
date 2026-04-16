

namespace TaskTrackerCLI.Models
{
    public class AppDataJsonModel
    {
        public Config Config { get; set; } = new();
        public List<TaskModel> Tasks { get; set; } = new();
    }
}
