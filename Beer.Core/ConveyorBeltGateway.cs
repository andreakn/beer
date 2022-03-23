using System.Net;
using System.Net.Http.Json;
using Beer.Contracts;

namespace Beer.Core;

public class ConveyorBeltGateway : IConveyorBeltGateway
{
    private readonly HttpClient _client;

    public ConveyorBeltGateway(HttpClient client)
    {
        _client = client;
    }

    public async Task<(bool, ProblemDetailsDto)> Start()
    {
        var path = $"/api/conveyorbelt/start";
        var result = await _client.PostAsync(path, null);
        if (result.StatusCode == HttpStatusCode.OK) return (true, null)!;
        return (false, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }

    public async Task<(bool, ProblemDetailsDto)> Stop()
    {
        var path = $"/api/conveyorbelt/stop";
        var result = await _client.PostAsync(path, null);
        if (result.StatusCode == HttpStatusCode.OK) return (true, null)!;
        return (false, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }

    public async Task<(BottleDto, NotBottleDto, ProblemDetailsDto)> Step()
    {
        var path = $"/api/conveyorbelt/step";
        var result = await _client.PostAsync(path, null);
        if (result.StatusCode == HttpStatusCode.OK) 
            return (await result.Content.ReadFromJsonAsync<BottleDto>(), null,null)!;
        if (result.StatusCode == HttpStatusCode.Accepted)
            return (null, await result.Content.ReadFromJsonAsync<NotBottleDto>(), null)!;
        return (null,null, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }

    public async Task<(ConveyorBeltState, ProblemDetailsDto)> GetState()
    {
        var path = $"/api/conveyorbelt/state";
        var result = await _client.GetAsync(path);
        if (result.StatusCode == HttpStatusCode.OK)
            return (ConveyorBeltState.Parse(await result.Content.ReadAsStringAsync()), null)!;
        return (null, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }

    public async Task<(bool, ProblemDetailsDto)> Fix()
    {
        var path = $"/api/conveyorbelt/fix";
        var result = await _client.PostAsync(path, null);
        if (result.StatusCode == HttpStatusCode.OK) return (true, null)!;
        return (false, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }
}