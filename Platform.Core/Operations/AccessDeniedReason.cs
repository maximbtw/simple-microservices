namespace Platform.Core.Operations;

public enum AccessDeniedReason
{
    AuthorizationHeaderNotFound,
    AuthorizationHeaderIncorrectFormat,
    OperationAccessDenied,
    UserNotFound,
    UserNotActive,
    AuthorizationTokenExpired,
    InvalidSignature
}