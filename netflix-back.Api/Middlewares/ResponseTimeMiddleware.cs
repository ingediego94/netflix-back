using System.Diagnostics;

namespace netflix_back.Api.Middlewares;

public class ResponseTimeMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Iniciamos el cronómetro
        var watch = Stopwatch.StartNew();

        // Agregamos un evento para cuando la respuesta esté a punto de enviarse
        context.Response.OnStarting(() => {
            watch.Stop();
            var responseTimeForMs = watch.ElapsedMilliseconds;
            
            // Añadimos el tiempo de respuesta a los headers (opcional, muy útil para debug)
            context.Response.Headers["X-Response-Time-ms"] = responseTimeForMs.ToString();
            
            Console.WriteLine($"[PERFORMANCE] {context.Request.Method} {context.Request.Path} tardó {responseTimeForMs}ms");
            
            return Task.CompletedTask;
        });

        // Continuar con el siguiente middleware/controlador
        await _next(context);
    }
}