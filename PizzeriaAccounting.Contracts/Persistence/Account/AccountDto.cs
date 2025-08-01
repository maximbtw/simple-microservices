﻿using Platform.Core.Persistence;

namespace PizzeriaAccounting.Contracts.Persistence.Account;

public class AccountDto : IEntityDto
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
}