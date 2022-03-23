using Beer.Contracts;

namespace Beer.Core;

public class Shippery
{
    private FileManager fileManager = new FileManager();
    private IBottleShopGateway _gateway ;

    public void Start()
    {
        if (_running)
        {
            return;
        }
        _running = true;
        _loop = Task.Run(TryToShip);
    }


    public async Task TryToShip()
    {
        await TryToShip(Beer.Pils);
        await TryToShip(Beer.Bayer);
        await Task.Delay(500);
    }

    private int CaseSize = 24;
    private bool _running;
    private Task _loop;

    public Shippery(IBottleShopGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task TryToShip(string type)
    {
        var beers = fileManager.LoadFiles<BottleDto>(type);
        var readyToShip = beers.Where(x => x.Thing.IsReadyToShip()).Take(CaseSize);
        if (readyToShip.Count() == CaseSize)
        {
            var le_case = new CaseDto
            {
                BottleIds = readyToShip.Select(x => x.Thing.Id)
            };
            await _gateway.ShipCase(le_case);
            foreach (var jsonFile in readyToShip)
            {
                File.Delete(jsonFile.FileName);
            }
        }

        var almostBad = fileManager.LoadFiles<BottleDto>(type).Where(x => x.Thing.IsAboutToExpire());
        foreach (var jsonFile in almostBad)
        {
            await _gateway.Ship(jsonFile.Thing.Id);
            File.Delete(jsonFile.FileName);
        }
        
        var recyclable = fileManager.LoadFiles<BottleDto>(type).Where(x => x.Thing.CanBeRecycled());
        foreach (var jsonFile in recyclable)
        {
            await _gateway.Recycle(jsonFile.Thing.Id);
            File.Delete(jsonFile.FileName);
        }  

        var broken = fileManager.LoadFiles<BottleDto>(type).Where(x => x.Thing.IsBroken());
        foreach (var jsonFile in broken)
        {
              File.Delete(jsonFile.FileName);
        }

        var bad = fileManager.LoadFiles<BottleDto>(type).Where(x => x.Thing.IsExpired());
        foreach (var jsonFile in bad)
        {
            File.Delete(jsonFile.FileName);
        }
    }

}