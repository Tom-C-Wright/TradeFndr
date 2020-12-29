using System;
using System.Collections.Generic;
using System.Text;
using TradeFindr;

namespace TradeFindrLibrary
{
    public class Total
    {
        public string Name { get; set; }
        public double BuyValue { get; set; }
        public double BuyVolume { get; set; }
      
        public double SellValue { get; set; }
        public double SellVolume { get; set; }

        public List<Trade[]> Combinations;
        public Total(string name, double buyVol, double buyVal, double sellValue, double sellVolume)
        {
            Name = name;
            BuyVolume = buyVol;
            BuyValue = buyVal;
            SellValue = sellValue;
            SellVolume = sellVolume;
            Combinations = new List<Trade[]>();
        }
    }
}
