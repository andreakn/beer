using System.Net;
using System.Net.Http.Json;
using Beer.Contracts;

namespace Beer.Core;

public class BrewingGateway : IBrewingGateway
{
    private readonly HttpClient _client;

    public BrewingGateway(HttpClient client)
    {
        _client = client;
    }

    public async Task<(bool,BottleDto, ProblemDetailsDto)> FillBottle(string bottleId)
    {
        var path = $"/api/brewingmachine/fillbottle/{bottleId}";
        var result = await _client.PostAsync(path, null);
        if (result.StatusCode == HttpStatusCode.OK) // Fill ok
            return (true,await result.Content.ReadFromJsonAsync<BottleDto>(), null)!;
        if (result.StatusCode == HttpStatusCode.Accepted) // Machine busy
            return (false,await result.Content.ReadFromJsonAsync<BottleDto>(), null)!;
        return (false,null, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!; // Not success
    }

    public async Task<(int, ProblemDetailsDto)> GetLevel(string beerType)
    {
        var path = $"/api/brewingmachine/level/{beerType}";
        var result = await _client.GetAsync(path);
        if (result.StatusCode == HttpStatusCode.OK)
            return (int.Parse(await result.Content.ReadAsStringAsync()), null)!;
        return (-1, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }

    public async Task<(IEnumerable<(string, int)>, ProblemDetailsDto)> GetLevels()
    {
        var path = $"/api/brewingmachine/levels";
        var result = await _client.GetAsync(path);
        if (result.StatusCode == HttpStatusCode.OK)
            return (new List<(string,int)>(), null)!; // TODO fix this
        return (null, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }

    public async Task<(bool, ProblemDetailsDto)> FillContainer(string beerType)
    {
        var path = $"/api/brewingmachine/fillcontainer/{beerType}";
        var result = await _client.PostAsync(path, null);
        if (result.StatusCode == HttpStatusCode.OK) 
            return (true, null)!;
        return (false, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }
}