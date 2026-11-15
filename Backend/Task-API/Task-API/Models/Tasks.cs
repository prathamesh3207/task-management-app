using System.ComponentModel.DataAnnotations;

namespace Task_API.Models
{
    public class Tasks
    {

        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IsCompleted { get; set; } = "false"; 

        public string Priority { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; }

        public string? Remark { get; set; } 
        public string? Username { get; set; }

    }
}
