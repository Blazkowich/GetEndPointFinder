using AutoMapper;
using EndPointFinder.Repository.Helpers.ExecutionMethods;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ScaNet.Helpers;

public static class ExecutionResultConverter
{
    public static IActionResult ToActionResult(this ExecutionResult executionResult)
    {
        if (executionResult.IsValid())
        {
            return new OkResult();
        }

        if (executionResult.Message != null)
        {
            return new ObjectResult(new
            {
                message = executionResult.Message,
            })
            {
                StatusCode = (int)StatusCodeFromExecutionTypeResult(executionResult.ResultType),
            };
        }

        return new StatusCodeResult((int)StatusCodeFromExecutionTypeResult(executionResult.ResultType));
    }

    public static IActionResult ToActionResult<T>(this ExecutionResult<T> executionResult)
    {
        if (executionResult.IsValid())
        {
            return new OkObjectResult(new
            {
                result = executionResult.Value,
            });
        }

        return ToActionResult((ExecutionResult)executionResult);
    }

    public static IActionResult ToActionResult<T, TU>(this ExecutionResult<T> executionResult, IMapper mapper)
    {
        if (executionResult.IsValid())
        {
            var mappedValue = mapper.Map<T, TU>(executionResult.Value);
            return new OkObjectResult(new { result = mappedValue });
        }

        return executionResult.ToActionResult();
    }

    public static HttpStatusCode StatusCodeFromExecutionTypeResult(ExecutionResultType type)
    {
        return type switch
        {
            ExecutionResultType.Ok => HttpStatusCode.OK,
            ExecutionResultType.BadRequest => HttpStatusCode.BadRequest,
            ExecutionResultType.NotFound => HttpStatusCode.NotFound,
            ExecutionResultType.Created => HttpStatusCode.Created,
            ExecutionResultType.Accepted => HttpStatusCode.Accepted,
            ExecutionResultType.NoContent => HttpStatusCode.NoContent,
            ExecutionResultType.Unauthorized => HttpStatusCode.Unauthorized,
            ExecutionResultType.Forbidden => HttpStatusCode.Forbidden,
            ExecutionResultType.InternalServerError => HttpStatusCode.InternalServerError,
            _ => throw new InvalidCastException($"value {type} cannot be converted to ${nameof(HttpStatusCode)} enum"),
        };
    }
}
