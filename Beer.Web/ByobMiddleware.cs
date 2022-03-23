using Beer.Contracts;
using Newtonsoft.Json;

public class ByobMiddleware
{
    private readonly RequestDelegate _next;
    
    public ByobMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        Console.WriteLine("got request: "+ context.Request.Path);
        if (context.Request.Path.StartsWithSegments("/api/byob"))
        {
            var bottle = await context.Request.ReadFromJsonAsync<BottleDto>();

            Console.WriteLine($"Got bottle: {bottle.Id} ({bottle.BeerType})");
            Console.WriteLine(JsonConvert.SerializeObject(bottle));
            context.Response.StatusCode = 204;
        }
        else if (context.Request.Path.StartsWithSegments("/api/start"))
        {

        }
        else
        {
            await _next(context);
        }
    }

}