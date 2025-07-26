using PizzeriaAccounting.Application.Persistence.Account;
using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.Account.Create;
using PizzeriaAccounting.Contracts.Persistence.Account;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace PizzeriaAccounting.Application.Services.Account;

internal class AccountServiceCreateAccountHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public AccountServiceCreateAccountHandler(
        IAccountRepository accountRepository,
        IDbScopeProvider scopeProvider)
    {
        _accountRepository = accountRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
    {
        var response = new CreateAccountResponse();

        await using OperationModificationScope scope = _scopeProvider.GetModificationScope();

        CreateAccountResult createResult = await CreateAccount(request, scope);

        await scope.CommitChangesIfSucceededAsync(createResult.Ok);

        response.Ok = createResult.Ok;
        response.Errors.AccountWithSameAddressExists = createResult.SameAddressExists;
        response.Errors.AccountWithSameNameExists = createResult.SameNameExists;

        return response;
    }

    private async Task<CreateAccountResult> CreateAccount(
        CreateAccountRequest request,
        OperationModificationScope scope)
    {
        var createRequest = new CreateAccountParameters
        {
            Entity = new AccountDto
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                IsActive = true
            }
        };

        return await _accountRepository.Create(createRequest, scope);
    }
}