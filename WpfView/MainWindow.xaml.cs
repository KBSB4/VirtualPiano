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
        }

        public async void Test()
        {
            //Song song = new()
            //{
            //    FullFile = File.ReadAllBytes("C:\\Users\\jaelk\\source\\repos\\VirtualPiano\\WpfView\\DebugMidi\\BEET.mid"),
            //    Name = "BEET2",
            //    Difficulty = Difficulty.Easy,
            //    Description = "De nieuwe beethoom"
            //};


            //SQLDatabaseManager sQLDatabaseManager = new();
            //await sQLDatabaseManager.UploadSong(song);
        }
    }
}