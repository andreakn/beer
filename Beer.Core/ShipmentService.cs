using Beer.Contracts;

namespace Beer.Core;

public class ShipmentService
{
    private IBottleShopGateway _bottleShopGateway;
    private List<BottleDto> _fermentingBottles;
    private const int _crateSize = 24;

    public ShipmentService(IBottleShopGateway bottleShopGateway)
    {
        _bottleShopGateway = bottleShopGateway;
    }

    public async Task ShipBottles()
    {
        var readyBottles = new List<BottleDto>();
        var expiredBottles = new List<BottleDto>();

        foreach (var bottle in _fermentingBottles)
        {
            if (bottle.IsExpired())
            {
                expiredBottles.Add(bottle);
                continue;
            }

            if (bottle.IsReadyToShip())
            {
                readyBottles.Add(bottle);
                continue;
            }
        }

        if (readyBottles.Count < 24)
        {
            
        }
        else
        {
            var first24 = readyBottles.Take(24);
            var caseOfBottles = new CaseDto
            {
                BottleIds = first24.Select(b => b.Id).ToList()
            };

            var result = await _bottleShopGateway.ShipCase(caseOfBottles);
        }

        foreach (var bottle in expiredBottles)
        {
            var result = await _bottleShopGateway.Recycle(bottle.Id);
        }
    }
}