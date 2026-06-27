namespace TaskFlow.ViewModels.Responses.Attachments
{
    public class AttachmentResponse
    {
        public long Id { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadedDate { get; set; }
        public long EmployeeId { get; set; }
    }
}
