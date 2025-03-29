namespace Expensi.Api;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext httpContext)
    {
        if (httpContext.Items["UserId"] is Guid userId)
        {
            return userId;
        }

        // Fallback to the hardcoded value if not found in HttpContext
        return Guid.Parse("11111111-1111-1111-1111-111111111111");
    }
}
