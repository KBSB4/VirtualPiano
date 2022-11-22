using System.Windows;
using System.Windows.Input;
using VirtualPiano;

namespace PianoView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Piano piano = new Piano();


        public MainWindow()
        {
            InitializeComponent();

            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;
        }

        public void KeyPressed(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;
            string keyValue = e.Key.ToString();

            if (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift))
            {
                keyValue = keyValue.ToUpper();
            }
            else
            {
                keyValue = keyValue.ToLower();
            }
                foreach (var key in piano.CustomKeys)
                {
                    if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString()))
                    {
                        key.PressedDown = true;
                    b.Text = keyValue;
                    }
                }
        }

        public void KeyReleased(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;
            string keyValue = e.Key.ToString();

            if (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift))
            {
                keyValue = keyValue.ToUpper();
            }
            else
            {
                keyValue = keyValue.ToLower();
            }

            foreach (var key in piano.CustomKeys)
            {
                if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(keyValue))
                {
                    key.PressedDown = false;
                }
            }
        }
    }
}


