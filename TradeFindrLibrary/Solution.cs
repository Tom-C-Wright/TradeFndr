using System;
using System.Collections.Generic;
using System.Text;

namespace TradeFindr.Structs
{
    
    public struct Solution
    {

        private Trade Total;
        public double TotalValue
        {
            get { return Total.Value; }
        }
        public double TotalVolume
        {
            get { return Total.Volume; }
        }
        public List<Trade[]> PossibleCombos;
        public Solution(Trade total)
        {
            Total = total;
            PossibleCombos = new List<Trade[]>();
        }
    }
}
