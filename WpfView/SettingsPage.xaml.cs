using Controller;
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
        //TODO PUT THIS BACK TO MAINMENU UNTIL ACCOUNT PAGE IS IMPLEMENTED
        private readonly Page _mainMenu;
        private int count = InputDevice.GetDevicesCount();
        public static int IndexInputDevice { get; set; }
        public bool Closed = false;

        public SettingsPage(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            DataContext = new DataContextSettings();
            InitializeComponent();
        }

        //NOTE TEMPORARY, AWAITING FOR ACCOUNT
        public SettingsPage(PracticePlayPiano ppp)
        {
            _mainMenu = ppp;
            DataContext = new DataContextSettings();
            InitializeComponent();
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
                NoneSelected.IsSelected = true;
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
            catch (Exception ex)
            {
                if (ex is IndexOutOfRangeException)
                {
                    MessageBox.Show($"Selected item in combobox {input.Name} was out of range");
                }
            }
        }

        /// <summary>
        /// Return to main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.Navigate(_mainMenu);
            //TODO MOVE TO ACCOUNT PAGE
            //Tell that we are closed to the page
            Closed = true;
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
        /// Update inputdevices items in settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO REMOVE IF ELSE WHEN ACCOUNT PAGE IS IMPLEMENTED
            if (_mainMenu is MainMenu ls)
            {
                ls?.CheckInputDevice(IndexInputDevice);
                GenerateInputDevices();
                input.Items.Refresh();
            }
        }

		private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
            //Volume changed
            PianoController.SetVolume((float)(e.NewValue / ((Slider)e.Source).Maximum));
		}
	}
}