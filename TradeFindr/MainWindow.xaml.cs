using ExcelDataReader;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
        
    }
}
