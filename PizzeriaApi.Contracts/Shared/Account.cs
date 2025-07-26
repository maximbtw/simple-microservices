namespace PizzeriaApi.Contracts.Shared;

public record Account(
    int Id, 
    string Name, 
    string Address, 
    string Phone, 
    string Email,
    bool IsActive);