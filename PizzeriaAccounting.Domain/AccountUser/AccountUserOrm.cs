using System.ComponentModel.DataAnnotations.Schema;
using PizzeriaAccounting.Domain.Account;
using Platform.Domain.EF;

namespace PizzeriaAccounting.Domain.AccountUser;

[Table("AccountUsers")]
public class AccountUserOrm : IOrm
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public int AccountId { get; set; }

    public string Email { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public virtual AccountOrm Account { get; set; } = null!;
}