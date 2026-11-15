namespace Task_API.DTO
{
    public class TaskListDTO
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IsCompleted { get; set; } = "false";
        public string Priority { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? Remark { get; set; }
        public string? Username { get; set; }
    }
}
