namespace TaskTrackerCLI.Models
{
    public class TaskModel
    {
        required public int id { get; set; }
        required public string description { get; set; }
        public enum Status { todo, inProgress, done };
        public Status status {  get; set; }
        private DateTime createdAt { get; set; }
        private DateTime updatedAt { get; set; }

    }
}
