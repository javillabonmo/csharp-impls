namespace TaskTrackerCLI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        required public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
