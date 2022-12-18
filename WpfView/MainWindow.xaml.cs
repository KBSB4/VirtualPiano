using System.Diagnostics;
using System.Windows;
using BusinessLogic;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationFrame.Navigate(new MainMenu());
            
        }

        
    }
}