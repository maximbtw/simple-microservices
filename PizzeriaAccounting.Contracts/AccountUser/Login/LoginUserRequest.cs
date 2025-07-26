namespace PizzeriaAccounting.Contracts.AccountUser.Login;

public class LoginUserRequest
{
    public string Login { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
}