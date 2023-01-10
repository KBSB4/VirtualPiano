using BusinessLogic;
using Controller;
using Microsoft.Identity.Client;
using Model.DatabaseModels;

namespace UnitTests
{
    public class BusinessLogic_ValidationLogic_Tests
    {
        [TestCase(null)]
        [TestCase("")]
        public async Task AccountPage_Login_UsernameIsEmptyCheck(string username)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User? user = await DatabaseController.GetUserByName(username);
            string? error = ValidationLogic.AccountPage_Login_ValidateUsernameField(username, user, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in the Username field!"));
        }

        [TestCase("123")]
        [TestCase("123456789123456")]
        public async Task AccountPage_Login_UsernameIsBetween4and13CharsCheck(string username)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User? user = await DatabaseController.GetUserByName(username);
            string? error = ValidationLogic.AccountPage_Login_ValidateUsernameField(username, user, languageManager);
            Assert.That(error, Is.EqualTo("Please use an Username between 4 and 13 characters!"));
        }

        [TestCase("testtest5")]
        [TestCase("doesnotexist5")]
        public async Task AccountPage_Login_UsernameExistsCheck(string username)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User? user = await DatabaseController.GetUserByName(username);
            string? error = ValidationLogic.AccountPage_Login_ValidateUsernameField(username, user, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in an Username that exists!"));
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task AccountPage_Login_PasswordIsEmptyCheck(string password)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User? user = await DatabaseController.GetLoggingInUser("kees", password);
            string? error = ValidationLogic.AccountPage_Login_ValidatePasswordField(password, user, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in the Password field!"));
        }

        [TestCase("123")]
        [TestCase("123456789123456")]
        public async Task AccountPage_Login_PasswordIsBetween4and13CharsCheck(string password)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User? user = await DatabaseController.GetLoggingInUser("kees", password);
            string? error = ValidationLogic.AccountPage_Login_ValidatePasswordField(password, user, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in a Password between 4 and 13 characters!"));
        }

        [TestCase("test12")]
        [TestCase("test")]
        public async Task AccountPage_Login_PasswordCombinationCheck(string password)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User? user = await DatabaseController.GetLoggingInUser("kees", password);
            string? error = ValidationLogic.AccountPage_Login_ValidatePasswordField(password, user, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in a Password that matches the username!"));
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task AccountPage_NewAccount_UsernameIsEmptyCheck(string username)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User? user = await DatabaseController.GetUserByName(username);
            string? error = ValidationLogic.AccountPage_NewAccount_ValidateUsernameField(username, user, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in the Username field!"));
        }

        [TestCase("123")]
        [TestCase("123456789123456")]
        public async Task AccountPage_NewAccount_UsernameIsBetween4and13CharsCheck(string username)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User? user = await DatabaseController.GetUserByName(username);
            string? error = ValidationLogic.AccountPage_NewAccount_ValidateUsernameField(username, user, languageManager);
            Assert.That(error, Is.EqualTo("Please use an Username between 4 and 13 characters!"));
        }

        [TestCase("Jael")]
        [TestCase("kees")]
        public async Task AccountPage_NewAccount_UsernameDoesNotExistsCheck(string username)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User? user = await DatabaseController.GetUserByName(username);
            string? error = ValidationLogic.AccountPage_NewAccount_ValidateUsernameField(username, user, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in an Username that does not exist!"));
        }

        [TestCase("something@some.neffzl")]
        [TestCase("something@.com")]
        [TestCase("@something.com")]
        [TestCase("@something.nl")]
        public async Task AccountPage_NewAccount_EmailMatchesFormatCheck(string email)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            User[]? users = await DatabaseController.GetAllUsers();
            string? error = ValidationLogic.AccountPage_NewAccount_ValidateEmailField(email, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in an Email that matches the right format!"));
        }

        [TestCase(null)]
        [TestCase("")]
        public void AccountPage_NewAccount_PasswordIsNotEmptyCheck(string password)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            string? error = ValidationLogic.AccountPage_NewAccount_ValidatePasswordField(password, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in the Password field!"));
        }

        [TestCase("123")]
        [TestCase("123456789123456")]
        public void AccountPage_NewAccount_PasswordIsBetween4and13CharsCheck(string password)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            string? error = ValidationLogic.AccountPage_NewAccount_ValidatePasswordField(password, languageManager);
            Assert.That(error, Is.EqualTo("Please use a Password between 4 and 13 characters!"));
        }

        [TestCase("test", null)]
        [TestCase("test", "")]
        public void AccountPage_NewAccount_ConfirmIsNotEmptyCheck(string password, string confirmpass)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            string? error = ValidationLogic.AccountPage_NewAccount_ValidateConfirmField(password, confirmpass, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in the Confirm field!"));
        }

        [TestCase("test", "123")]
        [TestCase("test", "123456789123456")]
        public void AccountPage_NewAccount_ConfirmIsBetween4and13CharsCheck(string password, string confirmpass)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            string? error = ValidationLogic.AccountPage_NewAccount_ValidateConfirmField(password, confirmpass, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in a Confirm between 4 and 13 characters!"));
        }

        [TestCase("testteest", "testtest")]
        [TestCase("test12", "test123")]
        public void AccountPage_NewAccount_PasswordMatchesConfirmCheck(string password, string confirmpass)
        {
            LanguageDataManager languageManager = new LanguageDataManager();
            languageManager.SetPreferredLanguage(Model.LanguageCode.EN);
            string? error = ValidationLogic.AccountPage_NewAccount_ValidateConfirmField(password, confirmpass, languageManager);
            Assert.That(error, Is.EqualTo("Please fill in a Confirm that matches the Password field!"));
        }
    }
}