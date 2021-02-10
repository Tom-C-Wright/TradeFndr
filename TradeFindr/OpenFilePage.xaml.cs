using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace TradeFindr
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class OpenFilePage : Page
    {
        private readonly ViewModel ViewModel;
        public OpenFilePage()
        {
            ViewModel = ViewModel.Instance;
            InitializeComponent();
            datagrid_TradePreview.ItemsSource = ViewModel.Trades;
            
        }

        private void btn_OpenFile(Object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ExcelReader excelReader = new ExcelReader();
                    var trades = excelReader.ReadFile(openFileDialog.FileName);
                    File_Path.Text = openFileDialog.FileName;
                    ViewModel.Trades.Clear(); 
                    for (ushort i = 0; i < trades.Length; i++)
                    {
                        ViewModel.Trades.Add(trades[i]);
                    }
                    
                    datagrid_TradePreview.Visibility = Visibility.Visible;
                    TextBox_Step2.Visibility = Visibility.Visible;
                    Btn_Next.Visibility = Visibility.Visible;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show("Unable to open file: \n\n" + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occured: \n\n" + ex.Message);
                }
            }
        }

        private void btn_NextScreen(Object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Is all Trade data correct?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.NavigationService.Navigate(new AddTotalsPage());
            }
        }
    }
}
