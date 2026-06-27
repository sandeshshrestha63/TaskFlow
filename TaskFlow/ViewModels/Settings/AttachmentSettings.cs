namespace TaskFlow.ViewModels.Settings
{
    public class AttachmentSettings
    {
        public string StorageRoot { get; set; } = "TaskFlowFiles";
        public int MaxFileSizeMB { get; set; }
        public List<string> AllowedExtensions { get; set; } = new();
    }
}
