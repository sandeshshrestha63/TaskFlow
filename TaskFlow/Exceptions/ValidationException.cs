namespace TaskFlow.Exceptions
{
    /// <summary>
    /// Represents a validation error caused by invalid user input.
    /// </summary>
    public class ValidationException : TaskFlowException
    {
        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}