namespace TaskFlow.ViewModels.Settings
{
    public class AttachmentSettings
    {
        public int MaxFileSizeMB { get; set; } = 25;
        public List<string> AllowedExtensions { get; set; } = new()
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".webp",

            ".pdf",

            ".doc",
            ".docx",

            ".xls",
            ".xlsx",

            ".ppt",
            ".pptx",

            ".txt",
            ".csv",

            ".zip",
            ".rar",
            ".7z"
        };
    }
}
