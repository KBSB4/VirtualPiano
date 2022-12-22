using Controller;
using Model;
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
        public static int IndexLanguage { get; set; }
        public bool Closed = false;

        public SettingsPage(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            DataContext = new DataContextSettings();
            InitializeComponent();
        }

        public SettingsPage(PracticePlayPiano ppp)
        {
            _mainMenu = ppp;
            DataContext = new DataContextSettings();
            InitializeComponent();
        }

        public void GenerateLanguages()
        {
            LanguageData languageData = LanguageController.GetAllLanguages();
            if (LanguageBox is null) return;

            foreach (Language language in languageData.languages)
            {
                if (!LanguageBox.Items.Cast<ComboBoxItem>().Any(cbi => cbi.Content.Equals(language.Name)))
                {
                    ComboBoxItem ToAddLanguage = new() { Content = language.Name };
                    LanguageBox.Items.Add(ToAddLanguage);
                    ToAddLanguage.Selected += ToAddLanguage_Selected;
                }
            }

            //TODO set current language
           // LanguageBox.SelectedIndex = LanguageBox.Items.IndexOf(languageData.languages.Where(lang => lang.Code == languageData.preferredLanguage).First().Name);
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

        private void ToAddLanguage_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem selecedItem = (ComboBoxItem)sender;
                IndexLanguage = LanguageBox.Items.IndexOf(sender);
                Language language = LanguageController.GetAllLanguages().languages.Where(lang => lang.Name.Equals(selecedItem.Content)).FirstOrDefault();
                LanguageController.SetPreferredLanguage(language.Code);
            }
            catch (Exception ex)
            {
              
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
        }

        /// <summary>
        /// Refreshes the ComboBoxItems when selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropDownOpened(object sender, EventArgs e)
        {
            RefreshBoxes();
        }

        public void RefreshBoxes()
        {
            GenerateInputDevices();
            input.Items.Refresh();
            if (LanguageBox is not null)
            {
                GenerateLanguages();
                LanguageBox.Items.Refresh();
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshBoxes();
        }

		private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
            //Volume changed
            PianoController.SetVolume((float)(e.NewValue / ((Slider)e.Source).Maximum));
		}
	}
}