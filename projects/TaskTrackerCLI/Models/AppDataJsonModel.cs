

namespace TaskTrackerCLI.Models
{
    public class AppDataJsonModel
    {
        public Config Config { get; set; } = new();
        public List<TaskItem> Tasks { get; set; } = new();
    }
}
