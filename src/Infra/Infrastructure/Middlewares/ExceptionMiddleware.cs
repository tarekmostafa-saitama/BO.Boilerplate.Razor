﻿using System.Net;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Tenants;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using ValidationException = FluentValidation.ValidationException;

namespace Infrastructure.Middlewares;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentUserService _currentUser;
    private readonly ISerializerService _jsonSerializer;
    private readonly ITenantService _tenantService;

    public ExceptionMiddleware(
        ICurrentUserService currentUser,
        ITenantService tenantService,
        ISerializerService jsonSerializer)
    {
        _currentUser = currentUser;
        _tenantService = tenantService;
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var userId = _currentUser.Id ?? "Anonymous";
            var tenant = _tenantService.GetTenant().Name ?? string.Empty;
            LogContext.PushProperty("UserId", userId);
            if (!string.IsNullOrEmpty(tenant))
                LogContext.PushProperty("Tenant", tenant);
            var errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);
            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
                SupportMessage = string.Format("Provide the ErrorId {0} to the support team for further analysis.",
                    errorId)
            };

            if (exception is not CustomException && exception.InnerException != null)
                while (exception.InnerException != null)
                    exception = exception.InnerException;

            if (exception is ValidationException fluentException)
            {
                errorResult.Exception = "One or More Validations failed.";
                foreach (var error in fluentException.Errors) errorResult.Messages.Add(error.ErrorMessage);
            }

            switch (exception)
            {
                case CustomException e:
                    errorResult.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null) errorResult.Messages = e.ErrorMessages;

                    break;

                case KeyNotFoundException:
                    errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ValidationException:
                    errorResult.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error(
                $"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode} and Error Id {errorId}.");
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }
}