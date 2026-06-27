using Microsoft.AspNetCore.Http;
using TaskFlow.Models;
using TaskFlow.ViewModels.Requests.Attachments;
using TaskFlow.ViewModels.Responses.Attachments;

namespace TaskFlow.Services
{
    public interface ITaskAttachmentService
    {
        Task<List<EmployeeTaskAttachment>> UploadAsync(UploadTaskAttachmentsRequest request);
        Task<List<EmployeeTaskAttachment>> GetTaskAttachmentsAsync(long taskId, long companyId);
        Task<EmployeeTaskAttachment> GetAttachmentAsync(long attachmentId, long companyId);
        Task<DownloadAttachmentResponse> DownloadAsync(DownloadAttachmentRequest request);
        Task DeleteAsync(DeleteAttachmentRequest request);
    }
}