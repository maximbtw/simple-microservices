using System.ComponentModel.DataAnnotations;
using Utilities.CustomAttributes;

namespace PizzeriaApi.Contracts.AuthApi.Register;

public class ApiRegisterRequest : ApiRequestBase
{
    [Required]
    [MaxLength(50)]
    public string Login { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; } = string.Empty;
    
    [Positive]
    public int AccountId { get; set; }
}