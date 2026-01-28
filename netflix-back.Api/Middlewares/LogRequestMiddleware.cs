namespace netflix_back.Api.Middlewares;

public class LogRequestMiddleware
{
    private readonly RequestDelegate _next;

    public LogRequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Este método se ejecuta en cada petición
    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Lógica antes de que el controlador reciba la petición
        Console.WriteLine($"[LOG] Petición entrante: {context.Request.Method} {context.Request.Path}");

        // 2. Llamar al siguiente middleware en la lista (o al controlador)
        await _next(context);

        // 3. Lógica después de que el controlador termine
        Console.WriteLine($"[LOG] Respuesta enviada con estado: {context.Response.StatusCode}");
    }
}