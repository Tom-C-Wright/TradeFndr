using CsvHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
    
            datagrid_Results.IsReadOnly = true;
            datagrid_Results.ItemsSource = trades;

            // Run in background so the UI doesn't hang
            Thread backGroundThread = new Thread(delegate()
            {
                MatchTradesToTotals(trades, brokers);
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

        // Temporary, delete later
        public void LoadData()
        {
            ExcelReader excelReader = new ExcelReader();
            var trades = excelReader.ReadFile("E:/Software_Dev/I_dont_fucking_know_anymore/Sample_Data/Solution_1.csv");
            for (ushort i = 0; i < trades.Length; i++)
            {
                ViewModel.Trades.Add(trades[i]);

            }
            /*
            ViewModel.Brokers.Add(new Broker("Citigroup",50000, 3300, 1, 0, 0, 0));
            ViewModel.Brokers.Add(new Broker("CMC Markets", 155711, 10277, 7, 0, 0 ,0));
            ViewModel.Brokers.Add(new Broker("CommonWealth", 64000, 4224, 3, 5525, 83712, 4));
            ViewModel.Brokers.Add(new Broker("Merill lynch", 100001, 6600, 8, 5676, 86000, 3));
            ViewModel.Brokers.Add(new Broker("UBS", 0 , 0 , 0, 13200, 200000, 12));
            */
            ViewModel.Brokers.Add(new Broker("AIEX", 19607, 1000, 1, 0, 0, 0));
            ViewModel.Brokers.Add(new Broker(name: "Citigroup", buyValue: 12245, buyVolume: 249900, totalBuys: 4, sellValue: 1253, sellVolume: 26667, totalSells: 1));
            ViewModel.Brokers.Add(new Broker(name: "CMC Markets", buyValue: 5000, buyVolume: 100000, totalBuys: 2, sellValue: 0, sellVolume: 0, totalSells: 0));
            ViewModel.Brokers.Add(new Broker(name: "Commonwealth", buyValue: 54901, buyVolume: 1114765, totalBuys: 15, sellValue: 41936, sellVolume: 844345, totalSells: 12));
            ViewModel.Brokers.Add(new Broker(name: "Credit Suise", buyValue: 1823, buyVolume: 37000, totalBuys: 4, sellValue: 1078, sellVolume: 22000, totalSells: 2));
            ViewModel.Brokers.Add(new Broker(name: "Merrill Lynch", buyValue: 2553, buyVolume: 52106, totalBuys: 1, sellValue: 2553, sellVolume: 52106, totalSells: 1));
            ViewModel.Brokers.Add(new Broker(name: "Pershing", buyValue: 1413, buyVolume: 30000, totalBuys: 2, sellValue: 4214, sellVolume: 85998, totalSells: 3));
            ViewModel.Brokers.Add(new Broker(name: "Third party", buyValue: 22556, buyVolume: 450000, totalBuys: 4, sellValue: 27235, sellVolume: 555810, totalSells: 4));
            ViewModel.Brokers.Add(new Broker(name: "Virtu Financial", buyValue: 1896, buyVolume: 38700, totalBuys: 1, sellValue: 1550, sellVolume: 31000, totalSells: 1));
            ViewModel.Brokers.Add(new Broker(name: "Wealthhub sec", buyValue: 1225, buyVolume: 25000, totalBuys: 2, sellValue: 24793, sellVolume: 499152, totalSells: 12));
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
        private void MatchTradesToTotals(List<Trade> tradeList, List<Broker> brokers)
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

                for (int j = buys.Count -1; j >= 0; j--)
                {
                    if (buys[j].BuyerList.Count == 1)
                    {
                        buys.RemoveAt(j);
                    }
                }
                
                for (int j = sells.Count - 1; j >= 0; j--)
                {
                    if (sells[j].SellerList.Count == 1)
                    {
                        sells.RemoveAt(j);
                    }
                } 
                

            }
            //return trades; // Return unmatched trades
        }
    }  

}
