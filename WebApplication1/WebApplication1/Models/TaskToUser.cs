using DeadLiner.Models;

namespace DeadLiner.Models
{
    public class TaskToUser
    {
        public int Id { get; set; }
        public int UserIdInt { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual TasksModel TaskModel { get; set; }
    }
}