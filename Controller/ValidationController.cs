using BusinessLogic;
using Model.DatabaseModels;

namespace Controller
{
    public static class ValidationController
    {
        public static bool AccountPage_NewAccount_PassAndConfirmPassAreEqual(string password, string confirmpass)
        {
            return ValidationLogic.AccountPage_NewAccount_PassAndConfirmPassAreEqual(password, confirmpass);
        }
        public static bool AccountPage_NewAccount_UsernameIsUnique(User? user)
        {
            return ValidationLogic.AccountPage_Login_UsernameIsUnique(user);
        }

        public static bool AccountPage_Login_UserCredentialsAreValid(User? user)
        {
            return ValidationLogic.AccountPage_Login_UserCredentialsAreValid(user);
        }

        public static bool AccountPage_NewAccount_UserCredentialsAreValid(User? user)
        {
            return ValidationLogic.AccountPage_NewAccount_UserCredentialsAreValid(user);
        }

        public static string AdminPanelValidationMessageTitle(string title)
        {
            return ValidationLogic.AdminPanelValidateTitle(title);
        }

        public static string AdminPanelValidationMessageDescription(string desc)
        {
            return ValidationLogic.AdminPanelValidateDescription(desc);
        }

        public static string AdminPanelValidationMessageDifficulty(string diff)
        {
            return ValidationLogic.AdminPanelValidateDifficulty(diff);
        }

        public static string AdminPanelValidationMessageMidiFile()
        {
            return ValidationLogic.AdminPanelValidateMidiFile();
        }


    }
}
