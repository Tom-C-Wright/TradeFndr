using System;
using System.Collections.Generic;
using System.Text;
using TradeFindr;

namespace TradeFindrLibrary
{
    public enum TransactionType
    {
        BUY,
        SELL
    }

    public class Broker
    {
        public string Company { get; set; }
      

        // Accessors for XML binding
        public uint BuyValue { get; set; }
        public uint BuyVolume { get; set; }
        public uint TotalBuys { get; set; } 
        public uint SellValue { get; set; }
        public uint SellVolume { get; set; }
        public uint TotalSells { get; set; }

        public List<Trade> Buys; 
        public List<Trade> Sells;
        public Broker(string name, uint buyVolume, uint buyValue, uint totalBuys, uint sellValue, uint sellVolume, uint totalSells)
        {
            Company = name;
            BuyValue = buyValue;
            BuyVolume = buyVolume;
            TotalBuys = totalBuys;
            SellValue = sellValue;
            SellVolume = sellVolume;
            TotalSells = totalSells;
            Buys = new List<Trade>();
            Sells = new List<Trade>();
        }
    }


}