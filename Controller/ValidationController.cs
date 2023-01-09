using BusinessLogic;
using Model.DatabaseModels;

namespace Controller
{
    public static class ValidationController
    {
        //TODO Summaries
        #region AccountPage Login Validation Methods

        /// <summary>
        /// Validate Username for Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string? AccountPage_Login_ValidateUsernameField(string? username, User? user)
        {
            return ValidationLogic.AccountPage_Login_ValidateUsernameField(username, user, LanguageController.GetLanguageDataManagerForAccountValid());
        }

        /// <summary>
        /// Validate Password for Login
        /// </summary>
        /// <param name="password"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string? AccountPage_Login_ValidatePasswordField(string? password, User? user)
        {
            return ValidationLogic.AccountPage_Login_ValidatePasswordField(password, user, LanguageController.GetLanguageDataManagerForAccountValid());
        }

        #endregion

        #region AccountPage NewAccount Validation Methods

        /// <summary>
        /// Validate Username for New Account
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string? AccountPage_NewAccount_ValidateUsernameField(string? username, User? user)
        {
            return ValidationLogic.AccountPage_NewAccount_ValidateUsernameField(username, user, LanguageController.GetLanguageDataManagerForAccountValid());
        }

        /// <summary>
        /// Validate Email for New Account
        /// </summary>
        /// <param name="email"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public static string? AccountPage_NewAccount_ValidateEmailField(string? email, User? users)
        {
            return ValidationLogic.AccountPage_NewAccount_ValidateEmailField(email, users, LanguageController.GetLanguageDataManagerForAccountValid());
        }

        /// <summary>
        /// Validate Password for New Account
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string? AccountPage_NewAccount_ValidatePasswordField(string? password)
        {
            return ValidationLogic.AccountPage_NewAccount_ValidatePasswordField(password, LanguageController.GetLanguageDataManagerForAccountValid());
        }

        /// <summary>
        /// Validate Confirm Password for New Account
        /// </summary>
        /// <param name="password"></param>
        /// <param name="confirmpass"></param>
        /// <returns></returns>
        public static string? AccountPage_NewAccount_ValidateConfirmField(string? password, string? confirmpass)
        {
            return ValidationLogic.AccountPage_NewAccount_ValidateConfirmField(password, confirmpass, LanguageController.GetLanguageDataManagerForAccountValid());
        }

        #endregion

        /// <summary>
        /// Message title for message box in Admin
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string AdminPanelValidationMessageTitle(string title)
        {
            return ValidationLogic.AdminPanelValidateTitle(title);
        }

        /// <summary>
        /// Message description for message box in Admin
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string AdminPanelValidationMessageDescription(string desc)
        {
            return ValidationLogic.AdminPanelValidateDescription(desc);
        }

        /// <summary>
        /// Validate Midi File in admin panel
        /// </summary>
        /// <returns></returns>
        public static string AdminPanelValidationMessageMidiFile()
        {
            return ValidationLogic.AdminPanelValidateMidiFile();
        }
    }
}