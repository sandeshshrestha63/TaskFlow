using System.ComponentModel.DataAnnotations;

namespace TaskFlow.ViewModels.Requests.Attachments
{
    public class DownloadAttachmentRequest
    {
        [Required]
        public long AttachmentId { get; set; }

        [Required]
        public long CompanyId { get; set; }
    }
}
