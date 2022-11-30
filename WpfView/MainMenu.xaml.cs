using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void FreePlay_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("FreePlayPiano.xaml", UriKind.Relative));
            //MoveFocus(new TraversalRequest(0));
            //Also possible (no string usage) but might make a new one every time
            //this.NavigationService.Navigate(new FreePlayPiano());
        }
    }
}
