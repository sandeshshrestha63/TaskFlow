namespace TaskFlow.Exceptions
{
    /// <summary>
    /// Represents a validation error related to uploaded files.
    /// </summary>
    public class FileValidationException : ValidationException
    {
        public FileValidationException(string message)
            : base(message)
        {
        }

        public FileValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}