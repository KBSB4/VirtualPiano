using BusinessLogic;
using Model;

namespace Controller
{
    public static class LanguageController
    {
        private static LanguageDataManager languageDataManager { get; set; }

        public static string GetTranslation(TranslationKey translationKey)
        {
            return "";
        }

        public static void SetPreferredLanguage(LanguageCode code)
        {

        }
    }
}