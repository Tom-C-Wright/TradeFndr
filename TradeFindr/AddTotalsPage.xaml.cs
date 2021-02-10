using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TradeFindrLibrary;

namespace TradeFindr
{
    /// <summary>
    /// Interaction logic for AddTotalsPage.xaml
    /// </summary>
    public partial class AddTotalsPage : Page
    {
        ViewModel viewModel;
        public AddTotalsPage()
        {
            viewModel = ViewModel.Instance;
            InitializeComponent();
            DataGrid_Totals.ItemsSource = viewModel.Brokers;
        }

        private void AddTotal(Object sender, RoutedEventArgs e)
        {
            if (TextBox_BuyValue.Text.Length > 0 && TextBox_BuyVolume.Text.Length > 0 && TextBox_SellValue.Text.Length > 0 && TextBox_SellVolume.Text.Length > 0)
            {
                // Get values from form
                var title = TextBox_Title.Text.ToString();
                var buyVal = Convert.ToUInt32(TextBox_BuyValue.Text);
                var buyVol = Convert.ToUInt32(TextBox_BuyVolume.Text);
                var sellVal = Convert.ToUInt32(TextBox_SellValue.Text);
                var sellVol = Convert.ToUInt32(TextBox_SellVolume.Text);
                var totalBuys = Convert.ToUInt32(TextBox_TotalBuys.Text);
                var totalSells = Convert.ToUInt32(TextBox_TotalSells.Text);
                viewModel.Brokers.Add(new Broker(
                            name: title,
                            buyVolume: buyVol,
                            buyValue: buyVal,
                            totalBuys: totalBuys,
                            sellValue: sellVal,
                            sellVolume: sellVol,
                            totalSells: totalSells
                            ));
                
                // Reset form fields
                ClearForm(sender, e);
            }
        }

        private void ClearForm(Object sender, RoutedEventArgs e)
        {  
            TextBox_Title.Clear();
            TextBox_BuyValue.Clear();
            TextBox_BuyVolume.Clear();
            TextBox_TotalBuys.Clear();
            TextBox_SellValue.Clear();
            TextBox_SellVolume.Clear();
            TextBox_TotalSells.Clear();
        }

        // Removes a selected total from the viewmodel
        private void DeleteTotal(Object sender, RoutedEventArgs e)
        {
            int index = DataGrid_Totals.SelectedIndex;
            // If a row is selected
            if (index != -1)
            {
                viewModel.Brokers.RemoveAt(index);
            } 
            // If no totals exist to select
            else if (viewModel.Brokers.Count == 0)
            {
                MessageBox.Show("No totals to delete!");
            }
            else
            {
                MessageBox.Show("No totals selected!");
            }
        }

        private void Btn_NextPage(Object sender, RoutedEventArgs e)
        {
            if (viewModel.Brokers.Count > 0)
            {
                this.NavigationService.Navigate(new ResultsPage());
            } 
            else
            {
                MessageBox.Show("At least one total is needed for matching");
            }
        }

        private void Btn_PrevPage(Object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        // Only allows numbers in textbox
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Only allows Alphabetical characters & spaces in text box
        private void TextValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z ]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
