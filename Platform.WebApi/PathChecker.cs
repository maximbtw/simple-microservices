using Microsoft.AspNetCore.Http;

namespace Platform.WebApi;

public static class PathChecker
{
    private static readonly HashSet<string> ExcludedPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/health",
        "/metrics",
        "/swagger"
    };

    public static bool IsExcludedPath(HttpContext context)
    {
        return ExcludedPaths.Contains(context.Request.Path);
    }
}