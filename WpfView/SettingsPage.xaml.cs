using Controller;
using Model;
using System;
using System.Collections.Generic;
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
        private readonly MainMenu _mainMenu;
        private int count = InputDevice.GetDevicesCount();
        public static int IndexInputDevice { get; set; }

        /// <summary>
        /// Constructor for settingspage, creates new datacontext
        /// </summary>
        /// <param name="mainMenu"></param>
        public SettingsPage(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            DataContext = new DataContextSettings();
            InitializeComponent();
            IsVisibleChanged += SettingsPage_IsVisibleChanged;
        }

        /// <summary>
        /// Generatelanguages and update UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GenerateLanguages();
            UpdateUI();
        }

        /// <summary>
        /// Translate labels
        /// </summary>
        private void UpdateUI()
        {
            LanguageLabel.Content = LanguageController.GetTranslation(TranslationKey.Settings_Language);
            VolumeLabel.Content = LanguageController.GetTranslation(TranslationKey.Settings_Volume);
            InputDeviceLabel.Content = LanguageController.GetTranslation(TranslationKey.Settings_InputDevice);
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
            catch
            {
                // Ignore
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

        /// <summary>
        /// Refresh inputdevices
        /// </summary>
        public void RefreshBoxes()
        {
            GenerateInputDevices();
            input.Items.Refresh();
        }

        /// <summary>
        /// If language selection has changed, update preferredlanguage and UI. Always refresh boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.Equals(LanguageBox))
            {
                List<Language>? languages = LanguageController.GetAllLanguages();
                if (languages is not null)
                {
                    LanguageCode code = languages[comboBox.SelectedIndex].Code;

                    LanguageController.SetPreferredLanguage(code);
                    UpdateUI();
                }
            }

            RefreshBoxes();
        }

        /// <summary>
        /// Add all available langauges to the box
        /// </summary>
        public void GenerateLanguages()
        {
            LanguageData? languageData = LanguageController.GetLanguageData();
            if (LanguageBox is not null && languageData?.Languages is not null)
            {
                foreach (Language language in languageData.Languages)
                {
                    if (!LanguageBox.Items.Cast<ComboBoxItem>().Any(cbi => cbi.Content.Equals(language.Name)))
                    {
                        ComboBoxItem ToAddLanguage = new() { Content = language.Name };
                        LanguageBox.Items.Add(ToAddLanguage);
                    }
                }
                LanguageBox.SelectedIndex = (int)languageData.PreferredLanguage;
            }
        }

        /// <summary>
        /// If volumeslider gets moved, update volume
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Volume changed
            PianoController.SetVolume((float)(e.NewValue / ((Slider)e.Source).Maximum));
        }
    }
}