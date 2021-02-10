using System;
using System.Collections.Generic;
using System.Text;
using TradeFindrLibrary;

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
        // Properties have to be used for WPF databinding
        public int Value { get; set; }
        public int Volume { get; set; }
        public double Price { get; set; }
        public DateTime Time { get; set; }
        public Reason Reason { get; set; }
        public string Buyer
        {
            get
            {
                var result = "";
                foreach (Broker b in BuyerList) result += b.Company;
                return result;
            }
        }

        public String Seller
        {
            get
            {
                var result = "";
                foreach (Broker b in SellerList) result += b.Company + " | ";
                return result;
            }
        }

        public readonly List<Broker> BuyerList;
        public readonly List<Broker> SellerList;
        public Trade(DateTime time, double price, int value, int volume, Reason reason, string buyer = "", string seller = "")
        {
            Time = time;
            Price = price;
            Value = value;
            Volume = volume;
            Reason = reason;
            BuyerList = new List<Broker>();
            SellerList = new List<Broker>();
        }

        

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Trade other = (Trade)obj;
                return this.Value == other.Value
                    && this.Volume == other.Volume
                    && this.Price == other.Price
                    && this.Time == other.Time
                    && this.Reason == other.Reason;
            }
        }
    }
}
