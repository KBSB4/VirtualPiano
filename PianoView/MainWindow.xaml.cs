using System;
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
            int intValue;
            string keyValue;
            GetKeyWithShift(e, out intValue, out keyValue);
            UpdateKey(e, intValue, true);
        }
        public void KeyReleased(object source, KeyEventArgs e)
        {
            int intValue;
            string keyValue;
            GetKeyWithShift(e, out intValue, out keyValue);
            UpdateKey(e, intValue, false);
        }

        private void UpdateKey(KeyEventArgs e, int intValue, Boolean PressDown)
        {
            foreach (var key in piano.CustomKeys)
            {
                if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString()))
                {
                    key.PressedDown = PressDown;
                }
            }
        }

        private static void GetKeyWithShift(KeyEventArgs e, out int intValue, out string keyValue)
        {
            intValue = (int)e.Key;
            keyValue = e.Key.ToString();
            if (Keyboard.IsKeyDown(System.Windows.Input.Key.RightShift) || Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift))
            {
                keyValue = keyValue.ToUpper();
            }
            else
            {
                keyValue = keyValue.ToLower();
            }
        }

    }
}


