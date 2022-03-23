using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beer.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Beer.Core
{
    public class Beer
    {
        public const string Pils = "pils";
        public const string Bayer = "bayer";

        



    }


    public class Tappery
    {



        private FileManager fileManager = new FileManager();
        private IBrewingGateway _brewingGateway;
        private bool _running;
        private Task _loop;

        public Tappery(IBrewingGateway brewingGateway)
        {
            _brewingGateway = brewingGateway;
        }

        public void Start()
        {
            if (_running)
            {
                return;
            }
            _running = true;
            _loop = Task.Run(TryToFillBottles);
        }

        private async Task TryToFillBottles()
        {
            await TryFillBottles(Beer.Pils);
            await TryFillBottles(Beer.Bayer);
            await Task.Delay(500);
        }

        private async Task TryFillBottles(string type)
        {
            var state = fileManager.LoadJson<TapperyState>() ?? await LoadTapstate();
            var inbox = fileManager.LoadFiles<BottleDto>("inbox").Where(x=>x.Thing.BeerType == type);

            foreach (var jsonFile in inbox)
            {
                var bottle = jsonFile.Thing;
                if (bottle.IsBroken() || bottle.IsFull())
                {
                    Processed(jsonFile);
                    continue;
                }
                else
                {
                    var levelResult = await _brewingGateway.GetLevel(type);
                    if(levelResult.Item2==null && levelResult.Item1 < (bottle.MaxContent - bottle.Content))
                    {
                        await _brewingGateway.FillContainer(bottle.BeerType);
                    }

                    var result = await _brewingGateway.FillBottle(bottle.Id);
                    if (result.Item1)
                    {
                        jsonFile.Thing = result.Item2;
                        ReQueue(jsonFile);
                    }
                    else
                    {
                        Console.WriteLine("we gots problems filling: "+JsonConvert.SerializeObject(result.Item3));
                        Console.WriteLine("Lets try refilling "+bottle.BeerType);
                    }
                }
            }
        }

        private void ReQueue(JsonFile<BottleDto> jsonFile)
        {
            File.Delete(jsonFile.FileName);
            var bottle = jsonFile.Thing;
            fileManager.SaveJson(bottle, bottle.FileName, true, bottle.BeerType);
        }

        private void Processed(JsonFile<BottleDto> jsonFile)
        {
            File.Delete(jsonFile.FileName);
            var bottle = jsonFile.Thing;
            fileManager.SaveJson(bottle, bottle.FileName, true, bottle.BeerType);

        }


        public bool ReceiveBottle(BottleDto b)
        {
           fileManager.SaveJson(b, b.FileName, true,"inbox");
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
