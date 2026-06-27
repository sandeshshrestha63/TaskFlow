namespace TaskFlow.ViewModels.Responses.Attachments
{
    public class DownloadAttachmentResponse
    {
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
