using Microsoft.AspNetCore.Http;

namespace TaskFlow.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Generates a unique file name while preserving the extension.
        /// </summary>
        public static string GenerateUniqueFileName(string originalFileName)
        {
            string extension = Path.GetExtension(originalFileName);

            return $"{Guid.NewGuid():N}{extension}";
        }

        /// <summary>
        /// Returns only the file name to prevent directory traversal.
        /// </summary>
        public static string GetSafeFileName(string fileName)
        {
            return Path.GetFileName(fileName);
        }

        /// <summary>
        /// Returns the file extension in lower case.
        /// </summary>
        public static string GetExtension(string fileName)
        {
            return Path.GetExtension(fileName).ToLowerInvariant();
        }

        /// <summary>
        /// Returns true if the file is an image.
        /// </summary>
        public static bool IsImage(string contentType)
        {
            return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Converts bytes into a readable file size.
        /// </summary>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };

            double length = bytes;
            int order = 0;

            while (length >= 1024 && order < sizes.Length - 1)
            {
                order++;
                length /= 1024;
            }

            return $"{length:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Saves the uploaded file to disk.
        /// </summary>
        public static async Task SaveFileAsync(IFormFile file, string fullPath)
        {
            await using FileStream stream = new(fullPath, FileMode.Create);

            await file.CopyToAsync(stream);
        }

        /// <summary>
        /// Deletes a file if it exists.
        /// </summary>
        public static void DeleteFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}