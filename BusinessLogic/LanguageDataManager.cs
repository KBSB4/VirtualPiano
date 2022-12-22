﻿using Model;
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

		private static LanguageData GetLanguageData()
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


			};

			languageData.languages = new();
			languageData.languages.Add(dutch);
			languageData.preferredLanguage = LanguageCode.NL;
			WriteLanguageData(languageData);
		}

		private static void WriteLanguageData(LanguageData languageData)
		{
			string rararar = JsonConvert.SerializeObject(languageData, Formatting.Indented);
			File.WriteAllText("C:\\Users\\Harris\\Source\\Repos\\KBSB4\\VirtualPiano\\BusinessLogic\\LanguageData.json", rararar);
		}
	}
}