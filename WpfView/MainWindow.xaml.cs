using BusinessLogic;
using Model.DatabaseModels;
using System;
using System.Windows;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Test();
            InitializeComponent();
            NavigationFrame.Navigate(new MainMenu());

            doSomething()
        }

		private async void doSomething()
		{
			SQLDatabaseManager databaseManager = new SQLDatabaseManager();

            Highscore[] highscores = await databaseManager.GetHighscores(6);
		}
	}
}