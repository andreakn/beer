using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beer.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Beer.Core
{
    
    public class Tappery
    {
        private FileManager fileManager = new FileManager();
        
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
            var state = fileManager.LoadJson<TapperyState>() ?? LoadTapstate();
            if (!state.IsTapping)
            {
                var b = fileManager.LoadJson<BottleDto>("inbox.json");
                if (b == default(BottleDto))
                {
                    return false;
                }
                
                state.CurrentBottle = b;
                
                // call fill endpoint
                if (true) //vi kunne starte å fylle
                {
                    state.IsTapping = true;
                    fileManager.SaveJson(state);
                }
            }
            

            return true;
        }

        private TapperyState LoadTapstate()
        {

            return new TapperyState();
        }
    }

    public class TapperyState
    {
        public Tank PilsnerTank { get; set; } = new Tank();
        public Tank BayerTank { get; set; } = new Tank();
        public BottleDto CurrentBottle { get; set; } 

        public bool IsTapping { get; set; }
    }

    public class Tank
    {
        public double FillLevel { get; set; }
        public bool IsFillingUp { get; set; }
    }


    
}
