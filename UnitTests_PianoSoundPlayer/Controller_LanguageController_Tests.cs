using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    internal class Controller_LanguageController_Tests
    {
        /// <summary>
        /// Get translation and check if it is the right one
        /// </summary>
        [Test]
        public void LanguageController_GetTranslation()
        {
            LanguageController.SetPreferredLanguage(LanguageCode.EN);
            string translation = LanguageController.GetTranslation(TranslationKey.Menubar_BackToMain);

            Assert.AreEqual(translation, "Back to Main Menu");
        }

        /// <summary>
        /// Check if writing preferred language works
        /// </summary>
        [Test]
        public void LanguageController_SetPreferredLanguage()
        {
            LanguageCode preferred = LanguageCode.NL;
            LanguageController.SetPreferredLanguage(preferred);
            LanguageData languageData = LanguageController.GetLanguageData();

            Assert.AreEqual(preferred, languageData.PreferredLanguage);
        }
    }
}
