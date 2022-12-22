using Model;
using Newtonsoft.Json;

namespace BusinessLogic
{
	public class LanguageDataManager
	{
		private Language? currentLanguage;

		public LanguageDataManager()
		{
			CreateLanguages();
			currentLanguage = GetLanguage(GetPreferredLanguage());
		}

		public LanguageData GetLanguageData()
		{
			string openedjson = File.ReadAllText("LanguageData.json");
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


		//Menubar_BackToMain,
		//      Menubar_Settings,
		//      Menubar_MIDI_Open,
		//      Menubar_MIDI_Play,
		//      Menubar_MIDI_Stop,
		//      Menubar_MIDI_Karaoke,

		//      MainMenu_Settings,
		//		MainMenu_FreePlay,
		//		MainMenu_Play,

		//		Settings_Volume,
		//      Settings_InputDevice,
		//      Settings_Language,

		//      Play_SongFinishedScreen_Title,
		//      Play_SongFinishedScreen_YourScore,
		//      Play_SongFinishedScreen_MaxScore,
		//      Play_SongFinishedScreen_UploadButton,
		//      Play_SongFinishedScreen_MenuButton,

		//      AccountPage_LoginTitle,
		//      AccountPage_LoginUsername,
		//      AccountPage_LoginPassword,
		//      AccountPage_LoginButton,

		//      AccountPage_RegisterTitle,
		//      AccountPage_RegisterUsername,
		//		AccountPage_RegisterEmail,
		//		AccountPage_RegisterPassword,
		//		AccountPage_RegisterPasswordConfirm,
		//		AccountPage_RegisterButton,

		//      MessageBox_HighscoreHigherThan,
		//      MessageBox_SelectSongBeforeStart,
		//      MessageBox_LogOut,
		//      MessageBox_MidiStillPlaying,
		//      MessageBox_NoMidiSelected,
		//      MessageBox_NoMidiPlaying,

		//      Menubar_SongSelect_SelectSong,
		//      Menubar_SongSelect_Start,
		//      Menubar_SongSelect_Karaoke,

		//TODO Temp
		public static void CreateLanguages()
		{
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

				///MenuBar
				{TranslationKey.Menubar_SongSelect_Karaoke, "Karaoke" },
				{TranslationKey.Menubar_SongSelect_SelectSong, "Lied kiezen" },
				{TranslationKey.Menubar_SongSelect_Start, "Starten" },

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

				//Messagesboxes
				{TranslationKey.MessageBox_HighscoreHigherThanText, "NL Highscore is higher than current score" },
				{TranslationKey.MessageBox_HighscoreHigherThanCaption, "NL There is no reason to upload your score." },
				{TranslationKey.MessageBox_MidiStillPlayingCaption, "NL MIDI is still playing" },
				{TranslationKey.MessageBox_MidiStillPlayingText, "NL There is a MIDI still playing! Stop the playback of the current playing MIDI to continue" },
				{TranslationKey.MessageBox_SelectSongBeforeStartCaption, "NL No MIDI selected" },
				{TranslationKey.MessageBox_SelectSongBeforeStartText, "NL Select a MIDI File first before playing" },
				{TranslationKey.MessageBox_NoMidiPlayingText, "NL There is no MIDI playing right now." },
				{TranslationKey.MessageBox_NoMidiPlayingCaption, "NL No MIDI playing" },
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
				{TranslationKey.Menubar_SongSelect_Karaoke, "Karaoke" },
				{TranslationKey.Menubar_SongSelect_SelectSong, "Select song" },
				{TranslationKey.Menubar_SongSelect_Start, "Start" },

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
				{TranslationKey.MessageBox_SelectSongBeforeStartCaption, "No MIDI selected" },
				{TranslationKey.MessageBox_SelectSongBeforeStartText, "Select a MIDI File first before playing" },
				{TranslationKey.MessageBox_NoMidiPlayingText, "There is no MIDI playing right now." },
				{TranslationKey.MessageBox_NoMidiPlayingCaption, "No MIDI playing" },

			};

			languageData.languages = new();
			languageData.languages.Add(english);
			languageData.languages.Add(dutch);
			languageData.preferredLanguage = LanguageCode.NL;
			WriteLanguageData(languageData);
		}

		private static void WriteLanguageData(LanguageData languageData)
		{
			string rararar = JsonConvert.SerializeObject(languageData, Formatting.Indented);
			File.WriteAllText("LanguageData.json", rararar);
		}
	}
}