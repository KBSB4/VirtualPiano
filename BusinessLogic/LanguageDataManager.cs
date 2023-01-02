using Model;
using Newtonsoft.Json;
using System.Globalization;

namespace BusinessLogic
{
    public class LanguageDataManager
    {
        private Language? currentLanguage;
        private string JSONPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PianoHero\\LanguageData.json";
        private string JSONDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PianoHero\\";


        public LanguageDataManager()
        {
            if (!File.Exists(JSONPath)) return;
            currentLanguage = GetLanguage(GetPreferredLanguage());
        }

        public LanguageData GetLanguageData()
        {
            string openedjson = File.ReadAllText(JSONPath);
            return JsonConvert.DeserializeObject<LanguageData>(openedjson);
        }

        public void SetPreferredLanguage(LanguageCode code)
        {
            LanguageData languageData = GetLanguageData();
            languageData.preferredLanguage = code;
            WriteLanguageData(languageData);
            currentLanguage = GetLanguage(GetPreferredLanguage());
        }

        public LanguageCode GetPreferredLanguage()
        {
            LanguageData languageData = GetLanguageData();

            return languageData.preferredLanguage;
        }

        private Language? GetLanguage(LanguageCode code)
        {
            LanguageData languageData = GetLanguageData();

            return languageData.languages.Where(lang => lang.Code == code).FirstOrDefault();
        }

        public string GetTranslation(TranslationKey key)
        {
            return currentLanguage.Translations[key];
        }

        public List<Language> GetAllLanguages()
        {
            return GetLanguageData().languages;
        }

        public void CreateLanguages()
        {
            if (File.Exists(JSONPath)) return;

            LanguageData languageData = new();

            Language dutch = new Language();

            //Nederlands
            dutch.Code = LanguageCode.NL;
            dutch.Name = "Nederlands";
            dutch.Translations = new() {
                //Main menu
				{TranslationKey.MainMenu_Settings, "Instellingen" },
                {TranslationKey.MainMenu_Play, "Spelen" },
                {TranslationKey.MainMenu_FreePlay, "Vrij Spel" },

                //Settings
				{TranslationKey.Settings_Volume, "Volume" },
                {TranslationKey.Settings_InputDevice, "Ingangsapparaat" },
                {TranslationKey.Settings_Language, "Taal" },

				//MenuBar
				{TranslationKey.Menubar_BackToMain, "Terug naar hoofdmenu" },
                {TranslationKey.Menubar_Settings, "Instellingen" },
                {TranslationKey.Menubar_MIDI_Play, "Afspelen" },
                {TranslationKey.Menubar_MIDI_Stop, "Stoppen" },
                {TranslationKey.Menubar_MIDI_Karaoke, "Karaoke modus" },
                {TranslationKey.Menubar_MIDI_Open, "Midi openen" },

				//Account page
				{TranslationKey.AccountPage_LoginButton, "Inloggen" },
                {TranslationKey.AccountPage_LoginPassword, "Wachtwoord" },
                {TranslationKey.AccountPage_LoginTitle, "Inloggen" },
                {TranslationKey.AccountPage_LoginUsername, "Gebruikersnaam" },

                {TranslationKey.AccountPage_RegisterTitle, "Nieuwe gebruiker" },
                {TranslationKey.AccountPage_RegisterButton, "Registreren" },
                {TranslationKey.AccountPage_RegisterEmail, "Email-adres" },
                {TranslationKey.AccountPage_RegisterPassword, "Wachtwoord" },
                {TranslationKey.AccountPage_RegisterPasswordConfirm, "Bevestig wachtwoord" },
                {TranslationKey.AccountPage_RegisterUsername, "Gebruikersnaam" },

                //SongSelect
                {TranslationKey.Menubar_SongSelect_Karaoke, "Karaoke" },
                {TranslationKey.Menubar_SongSelect_SelectSong, "Lied kiezen" },
                {TranslationKey.Menubar_SongSelect_Start, "Starten" },

				//Upload dialog box
		        {TranslationKey.Play_SongFinishedScreen_Title, "Score uploaden?" },
                {TranslationKey.Play_SongFinishedScreen_YourScore, "Uw score" },
                {TranslationKey.Play_SongFinishedScreen_MaxScore, "Maximale Score" },
                {TranslationKey.Play_SongFinishedScreen_UploadButton, "Score uploaden" },
                {TranslationKey.Play_SongFinishedScreen_MenuButton, "Terug naar hoofdmenu" },

				//Messagesboxes
				{TranslationKey.MessageBox_HighscoreHigherThanText, "Topscore is hoger dan behaalde score" },
                {TranslationKey.MessageBox_HighscoreHigherThanCaption, "Er is geen reden om uw score te uploaden" },
                {TranslationKey.MessageBox_MidiStillPlayingCaption, "Er speelt nog een midi af" },
                {TranslationKey.MessageBox_MidiStillPlayingText, "Er speelt momenteel al een midi af! Stop de huidige midi om verder te gaan" },
                {TranslationKey.MessageBox_SelectSongBeforeStartCaption, "Geen lied geselecteerd" },
                {TranslationKey.MessageBox_SelectSongBeforeStartText, "Selecteer eerst een lied voor het afspelen" },
                {TranslationKey.MessageBox_NoMidiPlayingText, "Er speelt momenteel geen midi af" },
                {TranslationKey.MessageBox_NoMidiPlayingCaption, "Er speelt geen midi" },
                {TranslationKey.MessageBox_NoMidiSelectedText, "Er is geen midi geselecteerd" },
                {TranslationKey.MessageBox_NoMidiSelectedCaption, "Selecteer eerst een midi" },
            };

            //English
            Language english = new Language();
            english.Code = LanguageCode.EN;
            english.Name = "English";
            english.Translations = new()
            {
                //Main menu
				{TranslationKey.MainMenu_Settings, "Settings" },
                {TranslationKey.MainMenu_Play, "Play" },
                {TranslationKey.MainMenu_FreePlay, "Freeplay" },

                //Settings
				{TranslationKey.Settings_Volume, "Volume" },
                {TranslationKey.Settings_InputDevice, "Input device" },
                {TranslationKey.Settings_Language, "Language" },

				//MenuBar
				{TranslationKey.Menubar_BackToMain, "Back to Mainmenu" },
                {TranslationKey.Menubar_Settings, "Settings" },
                {TranslationKey.Menubar_MIDI_Play, "Play" },
                {TranslationKey.Menubar_MIDI_Stop, "Stop" },
                {TranslationKey.Menubar_MIDI_Karaoke, "Karaoke Mode" },
                {TranslationKey.Menubar_MIDI_Open, "Open Midi" },

				//SongSelect
                {TranslationKey.Menubar_SongSelect_Karaoke, "Karaoke" },
                {TranslationKey.Menubar_SongSelect_SelectSong, "Select song" },
                {TranslationKey.Menubar_SongSelect_Start, "Start" },

				//Upload dialog box
		        {TranslationKey.Play_SongFinishedScreen_Title, "Upload Score?" },
                {TranslationKey.Play_SongFinishedScreen_YourScore, "Your Score" },
                {TranslationKey.Play_SongFinishedScreen_MaxScore, "Max Score" },
                {TranslationKey.Play_SongFinishedScreen_UploadButton, "Upload Score" },
                {TranslationKey.Play_SongFinishedScreen_MenuButton, "Back to Menu" },


				//Account page
				{TranslationKey.AccountPage_LoginButton, "Log in" },
                {TranslationKey.AccountPage_LoginPassword, "Password" },
                {TranslationKey.AccountPage_LoginTitle, "Log in" },
                {TranslationKey.AccountPage_LoginUsername, "Username" },

                {TranslationKey.AccountPage_RegisterTitle, "New account" },
                {TranslationKey.AccountPage_RegisterButton, "Sign up" },
                {TranslationKey.AccountPage_RegisterEmail, "Emailadress" },
                {TranslationKey.AccountPage_RegisterPassword, "Password" },
                {TranslationKey.AccountPage_RegisterPasswordConfirm, "Confirm Password" },
                {TranslationKey.AccountPage_RegisterUsername, "Username" },

				//Messagesboxes
				{TranslationKey.MessageBox_HighscoreHigherThanText, "Highscore is higher than current score" },
                {TranslationKey.MessageBox_HighscoreHigherThanCaption, "There is no reason to upload your score." },
                {TranslationKey.MessageBox_MidiStillPlayingCaption, "MIDI is still playing" },
                {TranslationKey.MessageBox_MidiStillPlayingText, "There is a MIDI still playing! Stop the playback of the current playing MIDI to continue" },
                {TranslationKey.MessageBox_SelectSongBeforeStartCaption, "No song selected" },
                {TranslationKey.MessageBox_SelectSongBeforeStartText, "Select a song first before playing" },
                {TranslationKey.MessageBox_NoMidiPlayingText, "There is no MIDI playing right now." },
                {TranslationKey.MessageBox_NoMidiPlayingCaption, "No MIDI playing" },
                {TranslationKey.MessageBox_NoMidiSelectedText, "No midi selected! " },
                {TranslationKey.MessageBox_NoMidiSelectedCaption, "Select a midi first before playing" },

            };

            languageData.languages = new()
            {
                english,
                dutch
            };

            //Get system language in 2 letters
            CultureInfo ci = CultureInfo.InstalledUICulture;
            var r = new RegionInfo(ci.LCID);
            string regionName = r.TwoLetterISORegionName;
            try
            {
                languageData.preferredLanguage = (LanguageCode)Enum.Parse(typeof(LanguageCode), regionName);
            }
            catch
            {
                languageData.preferredLanguage = LanguageCode.EN;

			}
            WriteLanguageData(languageData);
            currentLanguage = GetLanguage(GetPreferredLanguage());
        }

        private void WriteLanguageData(LanguageData languageData)
        {
            string JSONSerialized = JsonConvert.SerializeObject(languageData, Formatting.Indented);
            if (!Directory.Exists(JSONDirectory)) Directory.CreateDirectory(JSONDirectory);
            File.WriteAllText(JSONPath, JSONSerialized);
        }
    }
}