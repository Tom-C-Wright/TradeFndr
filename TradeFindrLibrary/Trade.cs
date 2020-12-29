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
    public struct Trade
    {
        // Properties have to be used for WPF databinding
        public double Value { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public DateTime Time { get; set; }
        public Reason Reason { get; set; }

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
