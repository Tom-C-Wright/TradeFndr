using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TradeFindrLibrary;

namespace TradeFindr
{
    public sealed class ViewModel
    {
        private static readonly ViewModel instance = new ViewModel();
        private ViewModel() { }

        public static ViewModel Instance
        {
            get
            {
                return instance;
            }
        }

        private ObservableCollection<Trade> trades = new ObservableCollection<Trade>();
        public ObservableCollection<Trade> Trades
        {
            get { return trades; }
            set { trades = value; }
        }

        private ObservableCollection<Total> totals = new ObservableCollection<Total>();
        public ObservableCollection<Total> Totals
        {
            get { return totals; }
            set { totals = value; }
        }
    }
}
