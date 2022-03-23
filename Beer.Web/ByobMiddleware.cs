using Beer.Contracts;
using Beer.Core;
using Newtonsoft.Json;

public class ByobMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IConveyorBeltGateway _conveyorBelt;
    private readonly Tappery _tappery;

    public ByobMiddleware(RequestDelegate next, IConveyorBeltGateway conveyorBelt, Tappery tappery)
    {
        _next = next;
        _conveyorBelt = conveyorBelt;
        _tappery = tappery;
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
            var result = await _conveyorBelt.Start();
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("okelidokeli - started" + JsonConvert.SerializeObject(result));

        }
        else if (context.Request.Path.StartsWithSegments("/api/stop"))
        {
            var result = await _conveyorBelt.Stop();
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("okelidokeli - stopped - "+JsonConvert.SerializeObject(result));
        }
        else
        {
            await _next(context);
        }
    }

}