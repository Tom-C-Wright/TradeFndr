using CsvHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TradeFindrLibrary;

namespace TradeFindr
{
    /// <summary>
    /// Interaction logic for ResultsPage.xaml
    /// </summary>
    public partial class ResultsPage : Page
    {
        private readonly ViewModel ViewModel;
        public ResultsPage()
        {
            ViewModel = ViewModel.Instance;
            InitializeComponent();
            var trades = new List<Trade>(ViewModel.Trades);
            var brokers = new List<Broker>(ViewModel.Brokers);
            
            datagrid_Results.IsReadOnly = false;
            datagrid_Results.Visibility = Visibility.Hidden;
            Loading_Spinner.Visibility = Visibility.Visible;
        
            // Run in background so the UI doesn't hang
            var backGroundThread = new Thread(delegate ()
            {
                MatchTradesToTotals(trades, brokers);
                // Once the method finishes, call the main thread to show the results
                this.Dispatcher.Invoke(() =>
                {
                    datagrid_Results.ItemsSource = ViewModel.Trades;
                    datagrid_Results.Visibility = Visibility.Visible;
                    Loading_Spinner.Visibility = Visibility.Hidden;
                });
            });
            
            backGroundThread.Start();
        }
        
        public void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(ViewModel.Trades);
                }
            }
        }

        public void btnUploadNew_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OpenFilePage());
        }

        public void btnEditTotals_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddTotalsPage());
        }

        public Tuple<int, int> AddUpTotals(Trade[] trades)
        {
            double totalValue = 0;
            double totalVolume = 0;
            for (int i = 0; i < trades.Length; i++)
            {
                totalValue += trades[i].Value;
                totalVolume += trades[i].Volume;
            }
            return new Tuple<int, int>((int)totalValue, (int)totalVolume);
        }
        // Links Trades to 
        private bool MatchTradesToTotals(List<Trade> tradeList, List<Broker> brokers)
        {
            /* We copy our data into two sets for buys and sells, so we can remove items from each 
             * set as we find matches without interfering with the other set. */
            
            List<Trade> buys = new List<Trade>(tradeList);
            List<Trade> sells = new List<Trade>(tradeList);
            List<Broker> buyers = new List<Broker>(brokers);
            List<Broker> sellers = new List<Broker>(brokers);

            /* They are sorted in ascending order as its easier to find matches with a low count, 
             * when we remove these matches it then speeds up the next loop as we have fewer elements. */

            buyers.Sort((x, y) => x.TotalBuys.CompareTo(y.TotalBuys));
            sellers.Sort((x, y) => x.TotalSells.CompareTo(y.TotalSells));
            
            uint lastBuyCount = 0;
            uint lastSellCount = 0;
            Combinator<Trade> buysCombinator = new Combinator<Trade>(buys, lastBuyCount);
            Combinator<Trade> sellsCombinator = new Combinator<Trade>(sells, lastSellCount);
            // Its safe to use brokers.Count because we never add/remove from this collection
            for (int i = 0; i < brokers.Count; i++)
            {
                var skip = true;
                // If we're looking at a trade with a buy or sell count that hasn't been evaluated, reset the combinator so the loop runs
                if (lastBuyCount != buyers[i].TotalBuys)
                {
                    lastBuyCount = buyers[i].TotalBuys;
                    buysCombinator = new Combinator<Trade>(buys, lastBuyCount);
                    buysCombinator.SetCount(lastBuyCount);
                    skip = false;
                }

                if (lastSellCount != sellers[i].TotalSells)
                {
                    lastSellCount = sellers[i].TotalSells;
                    sellsCombinator = new Combinator<Trade>(sells, lastSellCount);
                    sellsCombinator.SetCount(lastSellCount);
                    skip = false;
                }
                // Simple flag to skip if nothing has changed since the last iteration
                if (skip) continue;

                // We're doing these simultaneously so its way easier to get a result
                while (!buysCombinator.AtEnd() || !sellsCombinator.AtEnd())
                {
                    if (!buysCombinator.AtEnd())
                    {
                        var buysCombo = buysCombinator.GetNextCombo();
                        var buysTotals = AddUpTotals(buysCombo);

                        for (int j = i; j < buyers.Count && buyers[j].TotalBuys <= lastBuyCount; j++)
                        {
                            if (buyers[j].TotalBuys == buysCombo.Length &&
                                buyers[j].BuyValue == buysTotals.Item1 &&
                                buyers[j].BuyVolume == buysTotals.Item2)
                            {
                                foreach (Trade trade in buysCombo)
                                {
                                    buyers[j].Buys.Add(trade);
                                    trade.BuyerList.Add(buyers[j]);
                                }
                            }
                        }
                    }
                    
                    if (!sellsCombinator.AtEnd())
                    {
                      
                        var sellsCombo = sellsCombinator.GetNextCombo();
                        var sellsTotals = AddUpTotals(sellsCombo);

                        for (int j = i; j < sellers.Count && sellers[j].TotalSells <= lastSellCount; j++)
                        {
                            if (sellers[j].TotalSells == sellsCombo.Length &&
                                sellers[j].SellValue == sellsTotals.Item1 &&
                                sellers[j].SellVolume == sellsTotals.Item2)
                            {
                                foreach (Trade trade in sellsCombo)
                                {
                                    sellers[j].Sells.Add(trade);
                                    trade.SellerList.Add(sellers[j]);
                                }
                            }
                        }
                    
                    }
                }   
                
                // Prune trades once matched to a total
                foreach (Broker buyer in buyers)
                {
                    if (buyer.TotalBuys == buyer.Buys.Count)
                    {
                        foreach (Trade buy in buyer.Buys)
                        {
                            buys.Remove(buy);
                        }
                    }
                }
                
                foreach (Broker seller in sellers)
                {
                    if (seller.TotalSells == seller.Sells.Count)
                    {
                        foreach (Trade sell in seller.Sells)
                        {
                            sells.Remove(sell);
                        }
                    }
                } 
            }
            return true; 
        }
    }  

}
