using System.Windows;

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
            // Take us to the home page
            MainFrame.Navigate(new OpenFilePage());
        }
    }
}
