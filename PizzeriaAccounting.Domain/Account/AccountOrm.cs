using System.ComponentModel.DataAnnotations.Schema;
using Platform.Domain.EF;

namespace PizzeriaAccounting.Domain.Account;

[Table("Accounts")]
public class AccountOrm : IOrm
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
}