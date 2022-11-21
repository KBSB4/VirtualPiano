using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift))
            {
                foreach (var key in piano.CustomKeys)
                {
                    if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString().ToUpper()))
                    {
                        key.PressedDown = true;
                        b.Text = key.KeyBindChar.ToString();
                    }
                }
            }
            else
            {

                foreach (var key in piano.CustomKeys)
                {
                    if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString().ToLower()))
                    {
                        key.PressedDown = true;
                        b.Text = key.KeyBindChar.ToString();
                    }
                }

            }

        }


        public void KeyReleased(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;
            if (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift))
            {
                foreach (var key in piano.CustomKeys)
                {
                    if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString().ToUpper()))
                    {
                        key.PressedDown = false;
                    }
                }
            }
            else
            {

                foreach (var key in piano.CustomKeys)
                {
                    if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString().ToLower()))
                    {
                        key.PressedDown = false;
                    }
                }

            }

        }






    }
}
