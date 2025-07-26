namespace PizzeriaAccounting.Contracts.AccountUser.Register;

public class RegisterUserRequest
{
    public int AccountId { get; set; }
    
    public string Login { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
}