using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Domain;

namespace PizzeriaApi.Contracts.Shared;

public record AccountUser(int Id, int AccountId, string Login, string Email)
{
    public static AccountUser MapFromServiceModel(AccountUserDto accountUser, string login) => new(
        accountUser.Id,
        accountUser.AccountId,
        login,
        accountUser.Email);
}

public record AccountUserModel(
    int Id,
    int AccountId,
    int UserId,
    string Login,
    string Email,
    UserRole Role,
    bool IsActive);
