namespace TaskFlow.Exceptions
{
    /// <summary>
    /// Represents an exception when a requested entity cannot be found.
    /// </summary>
    public class NotFoundException : TaskFlowException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}