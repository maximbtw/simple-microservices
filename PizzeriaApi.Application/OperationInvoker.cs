using PizzeriaApi.Contracts;
using Platform.Core.Operations;

namespace PizzeriaApi.Application;

internal static class OperationInvoker
{
    public static async Task<TApiResponse> InvokeServiceAsync<TServiceResponse, TServiceErrors, TApiResponse>(
        Func<Task<TServiceResponse>> func,
        Action<TServiceResponse, TApiResponse>? onSuccess = null,
        Action<TServiceResponse, TApiResponse>? onFailed = null)
        where TServiceResponse : OperationResponseBase<TServiceErrors>
        where TServiceErrors : OperationResponseStandardErrors, new()
        where TApiResponse : ApiResponseBase, new()
    {
        var response = new TApiResponse();
        try
        {
            TServiceResponse serviceResponse = await func();
            if (serviceResponse.Ok)
            {
                onSuccess?.Invoke(serviceResponse, response);
            }
            else
            {
                MapStandardErrors(response, serviceResponse.Errors);

                onFailed?.Invoke(serviceResponse, response);
            }
        }
        catch (Exception)
        {
            response.AddError(Error.CreateError(ErrorCode.ServerError, "Server error"));
            // TODO: LOG
        }

        return response;
    }
    
    public static async Task<TApiResponse> InvokeServiceAsync<TServiceResponse, TApiResponse>(
        Func<Task<TServiceResponse>> func,
        Action<TServiceResponse, TApiResponse>? onSuccess = null,
        Action<TServiceResponse, TApiResponse>? onFailed = null)
        where TServiceResponse : OperationResponseBase
        where TApiResponse : ApiResponseBase, new()
    {
        var response = new TApiResponse();
        try
        {
            TServiceResponse serviceResponse = await func();
            if (serviceResponse.Ok)
            {
                onSuccess?.Invoke(serviceResponse, response);
            }
            else
            {
                MapStandardErrors(response, serviceResponse.Errors);

                onFailed?.Invoke(serviceResponse, response);
            }
        }
        catch (Exception)
        {
            response.AddError(Error.CreateError(ErrorCode.ServerError, "Server error"));
            // TODO: LOG
        }

        return response;
    }

    public static void MapStandardErrors<TApiResponse>(
        TApiResponse apiResponse,
        OperationResponseStandardErrors errors)
        where TApiResponse : ApiResponseBase
    {
        if (errors.InternalServerError)
        {
            apiResponse.AddError(Error.CreateError(ErrorCode.ServerError));
        }

        if (errors.AccessDenied)
        {
            switch (errors.AccessDeniedReason)
            {
                case AccessDeniedReason.AuthorizationHeaderNotFound:
                    apiResponse.AddError(Error.CreateError(
                        ErrorCode.NotAuthorized,
                        "Authorization header not found"));
                    break;

                case AccessDeniedReason.AuthorizationHeaderIncorrectFormat:
                    apiResponse.AddError(Error.CreateError(
                        ErrorCode.NotAuthorized,
                        "Authorization header has incorrect format"));
                    break;

                case AccessDeniedReason.OperationAccessDenied:
                    apiResponse.AddError(Error.CreateError(
                        ErrorCode.AccessDenied,
                        "You do not have permission to perform this operation"));
                    break;

                case AccessDeniedReason.UserNotFound:
                    apiResponse.AddError(Error.CreateError(
                        ErrorCode.NotFound,
                        "User not found"));
                    break;

                case AccessDeniedReason.UserNotActive:
                    apiResponse.AddError(Error.CreateError(
                        ErrorCode.AccessDenied,
                        "User account is not active"));
                    break;

                case AccessDeniedReason.AuthorizationTokenExpired:
                    apiResponse.AddError(Error.CreateError(
                        ErrorCode.NotAuthorized,
                        "Authorization token has expired"));
                    break;

                case AccessDeniedReason.InvalidSignature:
                    apiResponse.AddError(Error.CreateError(
                        ErrorCode.NotAuthorized,
                        "Token signature is invalid"));
                    break;

                case null:
                    apiResponse.AddError(Error.CreateError(
                        ErrorCode.AccessDenied,
                        "Access denied: unknown reason"));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}