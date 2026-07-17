using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class SprintStatus
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        [StringLength(30)]
        public string BadgeColor { get; set; } = "secondary";
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();

    }
}
