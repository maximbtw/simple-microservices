using Platform.Domain;

namespace Platform.WebApi;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireRolesAttribute : Attribute
{
    public UserRole[] Roles { get; }

    public RequireRolesAttribute(params UserRole[] roles)
    {
        Roles = roles;
    }
}