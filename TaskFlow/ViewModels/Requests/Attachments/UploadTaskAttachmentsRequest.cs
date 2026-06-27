using System.ComponentModel.DataAnnotations;

namespace TaskFlow.ViewModels.Requests.Attachments
{
    public class UploadTaskAttachmentsRequest
    {
        [Required]
        public int TaskId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please select at least one file.")]
        public List<IFormFile> Files { get; set; } = new();
    }
}
