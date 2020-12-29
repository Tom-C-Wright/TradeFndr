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
    }
}
