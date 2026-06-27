using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskFlow.Data;
using TaskFlow.Exceptions;
using TaskFlow.Helpers;
using TaskFlow.Models;
using TaskFlow.ViewModels.Requests.Attachments;
using TaskFlow.ViewModels.Responses.Attachments;
using TaskFlow.ViewModels.Settings;

namespace TaskFlow.Services
{
    public class TaskAttachmentService : ITaskAttachmentService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly AttachmentSettings _settings;

        public TaskAttachmentService(
            AppDbContext context,
            IWebHostEnvironment environment,
            IOptions<AttachmentSettings> settings)
        {
            _context = context;
            _environment = environment;
            _settings = settings.Value;
        }

        #region Public Methods

        public async Task<List<EmployeeTaskAttachment>> UploadAsync(UploadTaskAttachmentsRequest request)
        {
            if (request.Files == null || !request.Files.Any())
                throw new FileValidationException("Please select at least one file.");

            await ValidateTaskAsync(request.TaskId, request.CompanyId);

            string uploadFolder = GetUploadFolder(request.CompanyId, request.TaskId);

            var attachments = new List<EmployeeTaskAttachment>();

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var file in request.Files)
                {
                    ValidateFile(file);

                    string storedFileName = FileHelper.GenerateUniqueFileName(file.FileName);

                    string fullPath = Path.Combine(uploadFolder, storedFileName);

                    await FileHelper.SaveFileAsync(file, fullPath);

                    EmployeeTaskAttachment attachment = new EmployeeTaskAttachment
                    {
                        EmployeeTaskId = request.TaskId,
                        EmployeeId = request.EmployeeId,
                        OriginalFileName = FileHelper.GetSafeFileName(file.FileName),
                        StoredFileName = storedFileName,
                        FilePath = GetRelativePath(
                            request.CompanyId,
                            request.TaskId,
                            storedFileName),
                        ContentType = file.ContentType,
                        FileSize = file.Length,
                        UploadedDate = DateTime.UtcNow
                    };

                    attachments.Add(attachment);
                }

                _context.EmployeeTaskAttachments.AddRange(attachments);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return attachments;
            }
            catch
            {
                await transaction.RollbackAsync();

                DeleteUploadedFiles(attachments);

                throw;
            }
        }

        #endregion

        #region Private Helpers

        private async Task<EmployeeTask> ValidateTaskAsync(long taskId, long companyId)
        {
            var task = await _context.EmployeeTasks
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == taskId &&
                    x.CompanyId == companyId);

            if (task == null)
                throw new NotFoundException("Task not found.");

            return task;
        }

        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileValidationException(
                    $"'{file?.FileName}' is empty.");

            string extension = FileHelper.GetExtension(file.FileName);

            if (!_settings.AllowedExtensions.Contains(extension))
            {
                throw new FileValidationException(
                    $"Files with the '{extension}' extension are not allowed.");
            }

            long maxSize = _settings.MaxFileSizeMB * 1024L * 1024L;

            if (file.Length > maxSize)
            {
                throw new FileValidationException(
                    $"'{file.FileName}' exceeds the maximum allowed size of {_settings.MaxFileSizeMB} MB.");
            }
        }
        private string GetStorageRoot()
        {
            string root = Path.Combine(_environment.ContentRootPath, _settings.StorageRoot);

            Directory.CreateDirectory(root);

            return root;
        }

        private string GetUploadFolder(long companyId, long taskId)
        {
            string folder = Path.Combine(GetStorageRoot(), "companies", companyId.ToString(), "tasks", taskId.ToString());

            Directory.CreateDirectory(folder);

            return folder;
        }
       
        private string GetRelativePath(long companyId, long taskId, string storedFileName)
        {
            return Path.Combine("companies", companyId.ToString(), "tasks", taskId.ToString(), storedFileName).Replace("\\", "/");
        }

        private string GetPhysicalPath(string relativePath)
        {
            return Path.Combine(GetStorageRoot(), relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        }

        private void DeleteUploadedFiles(IEnumerable<EmployeeTaskAttachment> attachments)
        {
            foreach (var attachment in attachments)
            {
                string physicalPath = GetPhysicalPath(attachment.FilePath);

                FileHelper.DeleteFile(physicalPath);
            }
        }
        public async Task<List<EmployeeTaskAttachment>> GetTaskAttachmentsAsync(long taskId, long companyId)
        {
            await ValidateTaskAsync(taskId, companyId);

            return await _context.EmployeeTaskAttachments
                .Include(x => x.Employee)
                .Where(x =>
                    x.EmployeeTaskId == taskId &&
                    x.EmployeeTask.CompanyId == companyId)
                .OrderByDescending(x => x.UploadedDate)
                .ToListAsync();
        }

        public async Task<EmployeeTaskAttachment> GetAttachmentAsync(long attachmentId, long companyId)
        {
            return await GetAttachmentEntityAsync(attachmentId, companyId);
        }

        public async Task<DownloadAttachmentResponse> DownloadAsync(DownloadAttachmentRequest request)
        {
            var attachment = await GetAttachmentEntityAsync(
                request.AttachmentId,
                request.CompanyId);

            string physicalPath = GetPhysicalPath(attachment.FilePath);

            if (!File.Exists(physicalPath))
                throw new NotFoundException("The requested file could not be found.");

            return new DownloadAttachmentResponse
            {
                FileBytes = await File.ReadAllBytesAsync(physicalPath),
                ContentType = attachment.ContentType,
                FileName = attachment.OriginalFileName
            };
        }

        public async Task DeleteAsync(DeleteAttachmentRequest request)
        {
            var attachment = await GetAttachmentEntityAsync(
                request.AttachmentId,
                request.CompanyId);

            string physicalPath = GetPhysicalPath(attachment.FilePath);

            FileHelper.DeleteFile(physicalPath);

            _context.EmployeeTaskAttachments.Remove(attachment);

            await _context.SaveChangesAsync();
        }

        private async Task<EmployeeTaskAttachment> GetAttachmentEntityAsync(
            long attachmentId,
            long companyId)
        {
            var attachment = await _context.EmployeeTaskAttachments
                .Include(x => x.EmployeeTask)
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x =>
                    x.Id == attachmentId &&
                    x.EmployeeTask.CompanyId == companyId);

            if (attachment == null)
                throw new NotFoundException("Attachment not found.");

            return attachment;
        }

        
       
        #endregion
    }
}