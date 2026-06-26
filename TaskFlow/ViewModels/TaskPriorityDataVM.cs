using System.ComponentModel.DataAnnotations;

namespace TaskFlow.ViewModels
{
    public class TaskPriorityDataVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Prioroty Name")]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; }
        public string ColorCode { get; set; }
    }
}
