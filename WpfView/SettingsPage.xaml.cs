using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice;
namespace WpfView
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private MainMenu _mainMenu;
        private int count = InputDevice.GetDevicesCount();
        public int IndexInputDevice { get; set; }


        public SettingsPage(MainMenu mainMenu)
        {

            _mainMenu = mainMenu;
            DataContext = new DataContextSettings();
            InitializeComponent();

        }

        private void MainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.Navigate(_mainMenu);
        }


        /// <summary>
        /// Shows all the available MIDI-keyboard input devices
        /// </summary>
        public void GenerateInputDevices()
        {
            if (count == InputDevice.GetDevicesCount())
            {
                foreach (var device in InputDevice.GetAll())
                {
                    if (!input.Items.Cast<ComboBoxItem>().Any(cbi => cbi.Content.Equals(device.Name)))
                    {
                        ComboBoxItem ToAddInputDevice = new() { Content = device.Name };
                        input.Items.Add(ToAddInputDevice);
                        ToAddInputDevice.Selected += ToAddInputDevice_Selected;
                    }
                }
            }
            else
            {
                if (input.Items.Count > 1)
                {
                    for (int i = 1; i < input.Items.Count; i++)
                    {
                        input.Items.RemoveAt(i);
                    }
                }
                NoneSelected.IsSelected = true; // DEFAULT VALUE NONE
                count = InputDevice.GetDevicesCount();
            }
        }

        /// <summary>
        /// Gets the index of the selected input device.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToAddInputDevice_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                IndexInputDevice = input.Items.IndexOf(sender);
            }
            catch (IndexOutOfRangeException)
            {

            }
        }

        /// <summary>
        /// Refreshes the ComboBoxItems when selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_DropDownOpened(object sender, EventArgs e)
        {
            GenerateInputDevices();
            input.Items.Refresh();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mainMenu?.FreePlay?.CheckInputDevice(IndexInputDevice);
            GenerateInputDevices();
            input.Items.Refresh();
        }
    }
}
