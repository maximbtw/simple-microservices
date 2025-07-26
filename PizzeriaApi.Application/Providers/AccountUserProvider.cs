using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using PizzeriaAccounting.Contracts.AccountUser;
using PizzeriaAccounting.Contracts.AccountUser.GetUser;
using PizzeriaApi.Contracts.Providers;
using PizzeriaApi.Contracts.Shared;
using Platform.Core.DistributedCache;
using Platform.Domain;

namespace PizzeriaApi.Application.Providers;

public class AccountUserProvider : IAccountUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDistributedCache _distributedCache;
    private readonly IUserService _userService;
    
    public AccountUserProvider(
        IHttpContextAccessor httpContextAccessor, 
        IDistributedCache distributedCache, 
        IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _distributedCache = distributedCache;
        _userService = userService;
    }
    
    public async Task<AccountUserModel> GetCurrentAccountUser()
    {
        HttpContext? context = _httpContextAccessor.HttpContext;

        if (context?.User.Identity?.IsAuthenticated is false or null)
        {
            throw new InvalidOperationException("User is not authenticated.");
        }
        
        string sid = context.User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        string name = context.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        string role = context.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        
        var request = new GetUserRequest
        {
            UserId = int.Parse(sid),
            CachedValueRelevanceInterval = TimeSpan.FromMinutes(5)
        };

        GetUserResponse? response =
            await DistributedCacheHelper.GetAsync<GetUserRequest, GetUserResponse?>(_distributedCache, request);

        if (response == null)
        {
            response = await _userService.GetUser(request);
            
            await DistributedCacheHelper.SetAsync(_distributedCache, request, response);
        }

        return new AccountUserModel(
            response.AccountUser.Id,
            response.AccountUser.AccountId,
            response.AccountUser.UserId,
            Login: name,
            response.AccountUser.Email,
            ParseRole(role),
            response.AccountUser.IsActive);
    }
    
    private UserRole ParseRole(string role)
    {
        switch (role)
        {
            case "Admin": return UserRole.Admin;
            case "Customer": return UserRole.Customer;
            case "PizzeriaAccountUser": return UserRole.PizzeriaAccountUser;
            default: throw new ArgumentException();
        }
    }
}