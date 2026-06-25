using System.ComponentModel.DataAnnotations;

namespace TaskFlow.ViewModels.EmployeeTask
{
    public class TaskCommentVM
    {
        public int TaskId { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
