using BusinessLogic;
using Model;

namespace Controller
{
    public static class LanguageController
    {
        private static readonly LanguageDataManager languageDataManager = new();

        /// <summary>
        /// Get translation by <paramref name="translationKey"/>
        /// </summary>
        /// <param name="translationKey"></param>
        /// <returns><<see cref="string"/>/returns>
        public static string? GetTranslation(TranslationKey translationKey)
        {
            return languageDataManager.GetTranslation(translationKey);
        }

        /// <summary>
        /// Set preferredlanguage with <paramref name="code"/>
        /// </summary>
        /// <param name="code"></param>
        public static void SetPreferredLanguage(LanguageCode code)
        {
            languageDataManager.SetPreferredLanguage(code);
        }

        /// <summary>
        /// Get <see cref="LanguageData"/>
        /// </summary>
        /// <returns><see cref="LanguageData"/></returns>
        public static LanguageData? GetLanguageData()
        {
            return languageDataManager.GetLanguageData();
        }

        /// <summary>
        /// Returns list with all languages
        /// </summary>
        /// <returns>List of <see cref="Language"/></returns>
        public static List<Language>? GetAllLanguages()
        {
            return languageDataManager.GetAllLanguages();
        }

        /// <summary>
        /// Create JSON file with all translations
        /// </summary>
        public static void CreateJSON()
        {
            languageDataManager.CreateLanguages();
        }
    }
}