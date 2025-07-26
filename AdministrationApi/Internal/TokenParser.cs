using System.Security.Claims;
using Platform.Domain;
using Platform.WebApi.Context;
using Utilities;

namespace AdministrationApi.Internal;

internal static class TokenParser
{
    public static string CreateInternalTokenFromClaims(ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated is false or null)
        {
            return string.Empty;
        }

        string sid = user.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        string name = user.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        string role = user.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        
        ICurrentUser currentUser = new CurrentUser
        {
            Id = int.Parse(sid),
            Role = ParseRole(role),
            Login = name
        };
        
        return SerializationHelper.SerializeToBase64(currentUser);
    }
    
    private static UserRole ParseRole(string role)
    {
        switch (role)
        {
            case "Admin": return UserRole.Admin;
            case "Customer": return UserRole.Customer;
            case "PizzeriaAccountUser": return UserRole.PizzeriaAccountUser;
            default: throw new ArgumentException();
        }
    }
    
    private class CurrentUser : ICurrentUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}