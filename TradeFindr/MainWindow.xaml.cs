using ExcelDataReader;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TradeFindr
{
    /// <summary>
    ///     The main page is simply a frame for hosting the other pages in the application.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            MainFrame.Navigate(new OpenFilePage()); // Home page
             //MainFrame.Navigate(new AddTotalsPage());
            //MainFrame.Navigate(new ResultsPage());
        }
<<<<<<< HEAD
=======

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
                    for (ushort i = 0; i < trades.Length; i++)
                    {
                        Trades.Add(trades[i]);
                    }
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
        
>>>>>>> aa80f9a9f1e8dea7cb85fce0bc9e311323c25865
    }
}
