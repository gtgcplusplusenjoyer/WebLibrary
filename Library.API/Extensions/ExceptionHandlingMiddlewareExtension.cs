using Library.API.Middleware;

namespace Library.API.Extensions
{
    public static class ExceptionHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }
    }
}
