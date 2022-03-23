using System.Net;
using System.Net.Http.Json;
using System.Text;
using Beer.Contracts;
using Newtonsoft.Json;

namespace Beer.Core;

public class BottleShopGateway : IBottleShopGateway
{
    private readonly HttpClient _client;

    public BottleShopGateway(HttpClient client)
    {
        _client = client;
    }

    public async Task<(bool, ProblemDetailsDto)> Recycle(string bottleId)
    {
        var path = $"/api/bottleshop/recycle/{bottleId}";
        var result = await _client.PostAsync(path, null);
        if (result.StatusCode == HttpStatusCode.OK) return (true, null)!;
        return (false, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }

    public async Task<(ShipOperationResultDto, ProblemDetailsDto)> Ship(string bottleId)
    {
        var path = $"/api/bottleshop/ship/{bottleId}";
        var result = await _client.PostAsync(path, null);
        if (result.StatusCode == HttpStatusCode.OK) 
            return (await result.Content.ReadFromJsonAsync<ShipOperationResultDto>(), null)!;
        return (null, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }

    public async Task<(ShipOperationResultDto, ProblemDetailsDto)> ShipCase(CaseDto caseDto)
    {
        var path = $"/api/bottleshop/ship/case";
        var content = new StringContent(JsonConvert.SerializeObject(caseDto), Encoding.UTF8, "application/json");
        var result = await _client.PostAsync(path, content);
        if (result.StatusCode == HttpStatusCode.OK) 
            return (await result.Content.ReadFromJsonAsync<ShipOperationResultDto>(), null)!;
        return (null, await result.Content.ReadFromJsonAsync<ProblemDetailsDto>())!;
    }
}