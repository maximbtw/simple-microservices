using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.Providers;

public interface IAccountUserProvider
{
    Task<AccountUserModel> GetCurrentAccountUser();
}