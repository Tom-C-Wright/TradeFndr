using System;
using System.Collections.Generic;
using System.Text;
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
        public ResultsPage()
        {
            InitializeComponent();
            LoadData();
            CalculateCombinations();
        }
        
        // Temporary, delete later
        public void LoadData()
        {
            ExcelReader excelReader = new ExcelReader();
            var trades = excelReader.ReadFile("E:/Software_Dev/I_dont_fucking_know_anymore/Sample_Data/Solution_1.csv");
            for (ushort i = 0; i < trades.Length; i++)
            {
                ViewModel.Instance.Trades.Add(trades[i]);
            }
        }

        private void CalculateCombinations()
        {
            if (ViewModel.Instance.Totals.Count > 0 && ViewModel.Instance.Trades.Count > 0)
            {
                // Convienience variables
                var totals = ViewModel.Instance.Totals;
                var trades = ViewModel.Instance.Trades;

                var date = new DateTime();
                MessageBox.Show(date.TimeOfDay.ToString());
            }
        }
    }
}
