namespace TaskFlow.Exceptions
{
    /// <summary>
    /// Base exception for all TaskFlow custom exceptions.
    /// </summary>
    public abstract class TaskFlowException : Exception
    {
        protected TaskFlowException(string message)
            : base(message)
        {
        }

        protected TaskFlowException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

