using BusinessLogic;
using Model;

namespace Controller
{
    public static class LanguageController
    {
        private static LanguageDataManager languageDataManager = new();

        public static string GetTranslation(TranslationKey translationKey)
        {
            return languageDataManager.GetTranslation(translationKey);
        }

        public static void SetPreferredLanguage(LanguageCode code)
        {
            languageDataManager.SetPreferredLanguage(code);
        }

        public static LanguageCode GetPreferredLanguage()
        {
            return languageDataManager.GetPreferredLanguage();
        }

        public static LanguageData GetLanguageData()
        {
           
            return languageDataManager.GetLanguageData();
        }

        public static List<Language> GetAllLanguages()
        {
            return languageDataManager.GetAllLanguages();
        }

        public static void CreateJSON()
        {
            languageDataManager.CreateLanguages();
        }
    }
}