using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class EmployeeTaskAttachment
    {
        public long Id { get; set; }
        public int EmployeeTaskId { get; set; }
        public int EmployeeId { get; set; }
        [Required]
        [MaxLength(250)]
        public string OriginalFileName { get; set; }
        [Required]
        [MaxLength(300)]
        public string StoredFileName { get; set; }
        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; }
        [MaxLength(100)]
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedDate { get; set; }
        public EmployeeTask EmployeeTask { get; set; }
        public Employee Employee { get; set; }
    }
}
