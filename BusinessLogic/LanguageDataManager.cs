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

        //TODO Temp
        public static void CreateLanguages()
        {
            LanguageData languageData = new();

            Language language = new Language();
            language.Code = LanguageCode.NL;
            language.Name = "Nederlands";
            language.Translations = new() {
                {TranslationKey.MainMenu_Settings, "Instellingen" },
                {TranslationKey.Settings_Volume, "Volume" },
                {TranslationKey.Settings_InputDevice, "Ingangsapparaat" },
                {TranslationKey.MainMenu_Play, "Spelen" },
                {TranslationKey.MainMenu_FreePlay, "Vrij Spel" }
            };

            languageData.languages = new();
            languageData.languages.Add(language);
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