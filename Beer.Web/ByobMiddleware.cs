using Beer.Contracts;
using Beer.Core;
using Newtonsoft.Json;

public class ByobMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IConveyorBeltGateway _conveyorBelt;
    private readonly Tappery _tappery;
    private readonly Shippery _shippery;

    public ByobMiddleware(RequestDelegate next, IConveyorBeltGateway conveyorBelt, Tappery tappery, Shippery shippery)
    {
        _next = next;
        _conveyorBelt = conveyorBelt;
        _tappery = tappery;
        _shippery = shippery;

        _tappery.Start();
        _shippery.Start();
    }

    public async Task Invoke(HttpContext context)
    {
        Console.WriteLine("got request: "+ context.Request.Path);
        if (context.Request.Path.StartsWithSegments("/api/byob"))
        {
            var bottle = await context.Request.ReadFromJsonAsync<BottleDto>();
            _tappery.ReceiveBottle(bottle);
            
            Console.WriteLine($"Got bottle: {bottle.Id} ({bottle.BeerType})");
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