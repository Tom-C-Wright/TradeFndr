using System;
using System.Collections.Generic;
using System.Text;

namespace TradeFindr
{
    public enum Reason
    {
        MATCH,
        BID,
        ASK
    }
    public class Trade
    {
        public double Value;
        public double Volume;
        public double Price;
        public DateTime Time;
        public Reason Reason;

        public Trade(DateTime time, double price, double value, double volume, Reason reason)
        {
            Time = time;
            Price = price;
            Value = value;
            Volume = volume;
            Reason = reason;
        }
    }
}
