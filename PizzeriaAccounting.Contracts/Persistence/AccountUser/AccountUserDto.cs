using Platform.Core.Persistence;

namespace PizzeriaAccounting.Contracts.Persistence.AccountUser;

public class AccountUserDto : IEntityDto
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public int AccountId { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
    
    public int UserId { get; set; }
}