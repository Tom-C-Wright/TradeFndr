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
        // Singleton constuctor
        public static ViewModel Instance
        {
            get
            {
                return instance;
            }
        }

        // Lists can be bound to UI, and have their conte
        public readonly ObservableCollection<Trade> Trades = new ObservableCollection<Trade>();
        public readonly ObservableCollection<Broker> Brokers = new ObservableCollection<Broker>();
    }
}
