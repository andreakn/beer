using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beer.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Beer.Core
{
    public class Beer
    {
        public const string Pils = "pils";
        public const string Bayer = "bayer";
    }



    public class Shipper
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

        public Shipper(IBottleShopGateway gateway)
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

          

            var broken = fileManager.LoadFiles<BottleDto>(type).Where(x => x.Thing.IsBroken());
            foreach (var jsonFile in broken)
            {
                await _gateway.Recycle(jsonFile.Thing.Id);
                File.Delete(jsonFile.FileName);
            }

            var bad = fileManager.LoadFiles<BottleDto>(type).Where(x => x.Thing.IsExpired());
            foreach (var jsonFile in bad)
            {
                File.Delete(jsonFile.FileName);
            }
        }

    }

    public class Tappery
    {
       

        private FileManager fileManager = new FileManager();
        private IBrewingGateway _brewingGateway;

        public Tappery(IBrewingGateway brewingGateway)
        {
            _brewingGateway = brewingGateway;
        }

        public bool ReceiveBottle(BottleDto b)
        {
            var existingBottle = fileManager.LoadJson<BottleDto>("inbox.json");
            if (existingBottle != null)
            {
                return false;
            }
            fileManager.SaveJson(b, "inbox.json", true);
            return true;
        }

        public async Task<bool> TryFillBottle()
        {
            var state = fileManager.LoadJson<TapperyState>() ?? await LoadTapstate();
            if (!state.IsTapping)
            {
                var b = fileManager.LoadJson<BottleDto>("inbox.json");
                if (b == default(BottleDto))
                {
                    return false;
                }

                state.CurrentBottle = b;
                var currentlevel = state.CurrentBottle.Content;
                if (currentlevel < b.MaxContent)
                {
                    var diff = b.MaxContent - currentlevel;
                    if(diff > state.GetLevel(b.BeerType))



                    // call fill endpoint
                    if (true) //vi kunne starte å fylle
                    {
                        state.CurrentBottle = b; //todo: den vi fikk tilbake
                        state.IsTapping = true;
                        if (b.Content == b.MaxContent)
                        {
                            state.CurrentBottle = null;
                            state.IsTapping = false;
                            fileManager.SaveJson(b, b.Id, true, "storage");
                        }

                        fileManager.SaveJson(state);
                    }
                }

               
            }
            

            return true;
        }

        private async Task<TapperyState> LoadTapstate()
        {
            var pilsLevel = await _brewingGateway.GetLevel(Beer.Pils);
            var bayerLevel = await _brewingGateway.GetLevel(Beer.Bayer);

            if (pilsLevel.Item2 == null && bayerLevel.Item2 == null)
            {
                return new TapperyState
                {
                    BayerTank = new Tank { FillLevel = bayerLevel.Item1 },
                    PilsnerTank = new Tank { FillLevel = pilsLevel.Item1 }
                };
            }

            throw new Exception("AARGH, kan ikke lese tank-state");
        }
    }

    public class TapperyState
    {
        public Tank PilsnerTank { get; set; } = new Tank();
        public Tank BayerTank { get; set; } = new Tank();
        public BottleDto CurrentBottle { get; set; } 

        public bool IsTapping { get; set; }

        public bool NoMoreLeft(string beerType)
        {
            return GetLevel(beerType) == 0;
        }

        public int GetLevel(string beerType)
        {
            if (beerType == "bayer")
            {
                return BayerTank.FillLevel;
            }

            return PilsnerTank.FillLevel;
        }
    }

    public class Tank
    {
        public int FillLevel { get; set; }
        public bool IsFillingUp { get; set; }
    }


    
}
