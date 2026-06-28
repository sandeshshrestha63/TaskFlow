using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskFlow.Helpers;

namespace TaskFlow.Models
{
    public class Notification
    {
        [Key]
        public long NotificationId { get; set; }

        public int CompanyId { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        public NotificationType NotificationType { get; set; }

        public ReferenceType ReferenceType { get; set; }

        public int? ReferenceId { get; set; }

        [StringLength(300)]
        public string? Url { get; set; }

        public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;

        public bool IsRead { get; set; } = false;

        public DateTime? ReadDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } 

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation Properties

        public virtual Company Company { get; set; } = null!;

        public virtual Employee Employee { get; set; } = null!;

        [ForeignKey(nameof(CreatedBy))]
        public virtual Employee? CreatedByEmployee { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public virtual Employee? UpdatedByEmployee { get; set; }
    }
}