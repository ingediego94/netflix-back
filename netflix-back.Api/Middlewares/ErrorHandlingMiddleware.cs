namespace netflix_back.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new 
            { 
                error = "Ocurrió un error inesperado.", 
                details = ex.Message 
            };
            
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}