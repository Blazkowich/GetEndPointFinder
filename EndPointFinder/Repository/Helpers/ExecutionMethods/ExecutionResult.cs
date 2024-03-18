namespace EndPointFinder.Repository.Helpers.ExecutionMethods;

public class ExecutionResult
{
    public ExecutionResultType ResultType { get; set; }

    public string Message { get; set; }

    public bool IsValid()
    {
        return (int)ResultType <= (int)ExecutionResultType.Ok;
    }

    public static ExecutionResultType GetExecutionResultType(Exception exception)
    {
        return exception switch
        {
            InvalidCastException => ExecutionResultType.BadRequest,
            ArgumentException => ExecutionResultType.BadRequest,
            UnauthorizedAccessException => ExecutionResultType.Unauthorized,
            _ => ExecutionResultType.InternalServerError,
        };
    }
}

public class ExecutionResult<T> : ExecutionResult
{
    public T Value { get; set; }
}

