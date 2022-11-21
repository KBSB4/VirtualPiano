using Controller;
using Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Piano Piano { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _ = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);

            //Create piano
            Piano = PianoController.CreatePiano();
            //Add keydown event for the keys
            this.KeyDown += KeyDownEventHandler;
            this.KeyUp += KeyUpEventHandler;
        }

        /// <summary>
        /// Eventhandler for when the key gets pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            char HeldDownKey = GetCharForPianoHandler(e);

            Piano.KeyDownHandler(HeldDownKey);
            e.Handled = true;
        }

        /// <summary>
        /// Eventhandler for when the key gets let go
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyUpEventHandler(object sender, KeyEventArgs e)
        {
            char HeldDownKey = GetCharForPianoHandler(e);

            Piano.KeyUpHandler(HeldDownKey);
            e.Handled = true;
        }

        /// <summary>
        /// Gets the correct char for the piano handler so it can use it to get the correct pianokey
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Char to be sent to the piano handler</returns>
        private static char GetCharForPianoHandler(KeyEventArgs e)
        {
            KeyConverter kc = new KeyConverter();
            string str = kc.ConvertToString(e.Key);
            char HeldDownKey = (char)0;

            try
            {
                HeldDownKey = char.Parse(str);
            }
            catch (FormatException ex)
            {
                //Ignore
            }

            HeldDownKey += (char)32;

            if (Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift) || Keyboard.IsKeyDown(System.Windows.Input.Key.RightShift) || Keyboard.IsKeyDown(System.Windows.Input.Key.CapsLock))
            {
                HeldDownKey -= (char)32;
            }

            return HeldDownKey;
        }
    }
}
