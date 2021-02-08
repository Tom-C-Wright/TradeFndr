using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Data;
using TradeFindrLibrary;

namespace TradeFindr
{
    public sealed class ViewModel
    {
        private static readonly ViewModel instance = new ViewModel();
        private ViewModel() 
        {
            
        }

        public static ViewModel Instance
        {
            get
            {
                return instance;
            }
        }

        public readonly ObservableCollection<Trade> Trades = new ObservableCollection<Trade>();
        public readonly ObservableCollection<Broker> Brokers = new ObservableCollection<Broker>();

        // Exposed observables for Views
        public ObservableCollection<Trade> ObservableTrades
        {
            get { return Trades; }

        }

        public ObservableCollection<Broker> ObservableTotals
        {
            get { return Brokers;  }
        }
    }
}
