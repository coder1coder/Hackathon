using System;
using System.Collections.Generic;
using System.Net;
using BackendTools.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Extensions;

public static class OperationResultExtensions
{
    public static ProblemDetails ToProblemDetails<T>(this Result<T> result, string instance = null)
    {
        if (result.IsSuccess)
            return new ProblemDetails
            {
                Status = (int) HttpStatusCode.OK
            };

        var status = result.Errors.Type switch
        {
            OperationErrorType.None => HttpStatusCode.InternalServerError,
            OperationErrorType.Validation => HttpStatusCode.BadRequest,
            OperationErrorType.NotFound => HttpStatusCode.NotFound,
            OperationErrorType.Forbidden => HttpStatusCode.Forbidden,
            _ => throw new ArgumentOutOfRangeException()
        };

        var problemDetails = new ProblemDetails
        {
            Status = (int) status,
            Title = result.Errors.Type.ToDescription(),
            Instance = instance
        };

        foreach (var (key, value) in result.Errors.Values)
            problemDetails.Extensions.TryAdd(key, value);

        return problemDetails;
    }

    public static ProblemDetails ToProblemDetails(this Result result, string instance = null)
    {
        if (result.IsSuccess)
            return new ProblemDetails
            {
                Status = (int) HttpStatusCode.OK
            };

        var status = result.Errors.Type switch
        {
            OperationErrorType.None => HttpStatusCode.InternalServerError,
            OperationErrorType.Validation => HttpStatusCode.BadRequest,
            OperationErrorType.NotFound => HttpStatusCode.NotFound,
            OperationErrorType.Forbidden => HttpStatusCode.Forbidden,
            _ => throw new ArgumentOutOfRangeException()
        };

        var problemDetails = new ProblemDetails
        {
            Status = (int) status,
            Title = result.Errors.Type.ToDescription(),
            Instance = instance
        };

        problemDetails.Extensions["errors"] = result.Errors.Values;

        // foreach (var (key, value) in result.Errors.Values)
        //     problemDetails.Extensions.TryAdd(key, value);

        return problemDetails;
    }
}
