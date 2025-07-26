using System.ComponentModel.DataAnnotations;

namespace PizzeriaApi.Contracts.AuthApi.Login;

public class ApiLoginRequest : ApiRequestBase
{
    [Required]
    public string Login { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}