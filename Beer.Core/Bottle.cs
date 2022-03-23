using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Beer.Core
{
    
    public class Tappery
    {
        private FileManager fileManager = new FileManager();

       

        public bool ReceiveBottle(Bottle b)
        {
            var existingBottle = fileManager.LoadJson<Bottle>("inbox.json");
            if (existingBottle != null)
            {
                return false;
            }
            fileManager.SaveJson(b, "inbox.json", true);
            return true;
        }

        public bool TryFillBottle()
        {
            var state = fileManager.LoadJson<TapperyState>() ?? LoadTapstate();
            if (!state.IsTapping)
            {
                var b = fileManager.LoadJson<Bottle>("inbox.json");
                if (b == default(Bottle))
                {
                    return false;
                }
                state.CurrentBottle = b;
                fileManager.SaveJson(state);


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
        public Bottle CurrentBottle { get; set; } 

        public bool IsTapping { get; set; }
    }

    public class Tank
    {
        public double FillLevel { get; set; }
        public bool IsFillingUp { get; set; }
    }


    public class Bottle
    {
        public Guid Id { get; set; }
        public string Type { get; set; }


    }
}
