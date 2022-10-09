using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken_Petrol_Ltd
{
    internal class Pump //each pump will be an object of this class
    {

        public int pumpNumber;
        public double lpgDispensed;
        public double dieselDispensed;
        public double unleadedDispensed;
        public bool inUse;

        public Pump(int pumpNum)
        {
            pumpNumber = pumpNum;
            lpgDispensed = 0;
            dieselDispensed = 0;
            unleadedDispensed = 0;
            inUse = false;

        }


    }
}
