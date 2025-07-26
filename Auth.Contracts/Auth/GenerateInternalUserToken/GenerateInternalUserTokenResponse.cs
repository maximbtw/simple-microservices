using Platform.Core.Operations;

namespace Auth.Contracts.Auth.GenerateInternalUserToken;

public class GenerateInternalUserTokenResponse : OperationResponseBase<GenerateInternalUserTokenResponseErrors>
{
    public string InternalToken { get; set; } = null!;
}