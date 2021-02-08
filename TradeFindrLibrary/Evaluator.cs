using TradeFindr.Structs;
using System;
using System.Collections.Generic;
using System.Text;
using TradeFindrLibrary;

namespace TradeFindr
{
    class Evaluator
    {
        private Broker[] Totals;
        private Trade[] Trades;
        public Evaluator(Broker[] totals, Trade[] trades)
        {
            Totals = totals;
            Trades = trades;
        }
    }

  
}
