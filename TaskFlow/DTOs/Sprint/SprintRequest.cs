using System.ComponentModel.DataAnnotations;

namespace TaskFlow.DTOs.Sprint
{
    public class SprintRequest
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Goal { get; set; }

        public string? Description { get; set; }

        [Required]
        public int SprintStatusId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
