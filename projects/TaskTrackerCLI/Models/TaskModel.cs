namespace TaskTrackerCLI.Models
{
    public class TaskModel
    {
        public int id { get; set; }
        required public string description { get; set; }
        public enum Status { todo, inProgress, done };
        public Status status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

    }
}
