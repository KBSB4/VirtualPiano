using System;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SelectedSongControl.xaml
    /// </summary>
    public partial class SelectedSongControl : UserControl
    {
        public SelectedSongControl()
        {
            InitializeComponent();
            Random random = new();
            for (int i = 0; i < 10; i++)
            {
                leaderBoard.Children.Add(new LeaderboardRecord(i, "LongnameUser" + i.ToString(), random.Next(0, 99999999)));
            }
        }
    }
}
