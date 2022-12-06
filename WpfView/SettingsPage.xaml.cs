using System;
using System.Management;
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

        public int IndexInputDevice { get; set; }
        public int IndexOutputDevice { get; set; }
        private MainMenu _mainMenu;
        public SettingsPage(MainMenu mainMenu)
        {
            IndexInputDevice = -1;
            InitializeComponent();
            _mainMenu = mainMenu;
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
            input.Items.Clear();
            foreach (var device in InputDevice.GetAll())
            {
                ComboBoxItem ToAddInputDevice = new() { Content = device.Name };
                this.input.Items.Add(ToAddInputDevice);

                ToAddInputDevice.Selected += ToAddInputDevice_Selected;

            }
        }

        /// <summary>
        /// Gets the index of the selected input device
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
                GenerateInputDevices();
            }

        }




        /// <summary>
        /// Shows all the available MIDI-keyboard output devices 
        /// </summary>
        public void GenerateOutputDevices()
        {
            output.Items.Clear();
            try
            {


                ManagementObjectSearcher objSearcher = new("SELECT * FROM Win32_SoundDevice");

                ManagementObjectCollection objCollection = objSearcher.Get();

                foreach (ManagementObject obj in objCollection)
                {
                    foreach (PropertyData property in obj.Properties)
                    {
                        if (property.Name.Equals("Description"))
                        {
                            ComboBoxItem ToAddOutputDevice = new() { Content = property.Value };
                            this.output.Items.Add(ToAddOutputDevice);
                            ToAddOutputDevice.Selected += ToAddOutputDevice_Selected;
                            break;
                        }
                    }
                }

            }
            catch (IndexOutOfRangeException)
            {
                GenerateOutputDevices();
            }
        }

        /// <summary>
        /// Gets the index of the selected output device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToAddOutputDevice_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                IndexOutputDevice = output.Items.IndexOf(sender);
            }
            catch (IndexOutOfRangeException)
            {
                GenerateOutputDevices();
            }
        }
    }
}
