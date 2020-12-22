using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TradeFindr
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
