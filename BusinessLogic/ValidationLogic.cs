using Model;
using Model.DatabaseModels;
using System.Text.RegularExpressions;

namespace BusinessLogic
{
    public static class ValidationLogic
    {
        #region AccountPage Login Field Validation

        /// <summary>
        /// Validates on the <b>AccountPage</b> from the <b>Login</b> section the <paramref name="username"/> field.<br></br>
        /// Check 1: Checks if <paramref name="username"/> field is not empty. <br></br>
        /// Check 2: Checks if <paramref name="username"/> is between 4 and 13 characters.<br></br>
        /// Check 3: Checks if <paramref name="username"/> exists in database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        /// <returns>If all checks came out true the <paramref name="username"/> was valid and returns null. Otherwise returns corresponding error message.</returns>
        public static string? AccountPage_Login_ValidateUsernameField(string? username, User? user, LanguageDataManager LDM)
        {
            if (username is null || username.Length == 0)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_Account_NoUsername); 
            }
            else if (username is not null && username.Length < 4 || username?.Length > 13)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_Account_UsernameNotEnoughChars);
            }
            else if (user is null)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_Account_UsernameDoesNotExist);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Validates on the <b>AccountPage</b> from the <b>Login</b> section the <paramref name="password"/> field.<br></br>
        /// Check 1: Checks if <paramref name="password"/> field is not empty. <br></br>
        /// Check 2: Checks if <paramref name="password"/> is between 4 and 13 characters.<br></br>
        /// Check 3: Checks if <paramref name="password"/> matches the username.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="user"></param>
        /// <returns>If all checks came out true the <paramref name="password"/> was valid and returns null. Otherwise returns corresponding error message.</returns>
        public static string? AccountPage_Login_ValidatePasswordField(string? password, User? user, LanguageDataManager LDM)
        {
            if (password is null || password.Length == 0)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_Account_NoPassword);
            }
            else if (password is not null && password.Length < 4 || password?.Length > 13)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_Account_PasswordNotEnoughChars);
            }
            else if (user is null)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_Account_PasswordDoesNotExist);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region AccountPage NewAccount Fields Validation

        /// <summary>
        /// Validates on the <b>AccountPage</b> from the <b>NewAccount</b> section the <paramref name="username"/> field.<br></br>
        /// Check 1: Checks if <paramref name="username"/> field is not empty. <br></br>
        /// Check 2: Checks if <paramref name="username"/> is between 4 and 13 characters.<br></br>
        /// Check 3: Checks if <paramref name="username"/> does not exist in database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        /// <returns>If all checks came out true the <paramref name="username"/> was valid and returns null. Otherwise returns corresponding error message.</returns>
        public static string? AccountPage_NewAccount_ValidateUsernameField(string? username, User? user, LanguageDataManager LDM)
        {
            if (username is null || username.Length == 0)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_NewAccount_NoUsername);
            }
            else if (username is not null && username.Length < 4 || username?.Length > 13)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_NewAccount_UsernameNotEnoughChars);
            }
            else if (user is not null)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_NewAccount_UsernameDoesNotExist);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Validates on the <b>AccountPage</b> from the <b>NewAccount</b> section the <paramref name="email"/> field.<br></br>
        /// Check 1: Checks if <paramref name="email"/> field is empty.<br></br>
        /// Check 2: Checks if <paramref name="email"/> is unique. (by checking if <paramref name="user"/> is not null)<br></br> 
        /// Check 3: Checks if <paramref name="email"/> matches the given format using: <see cref="Regex"/>.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="user"></param>
        /// <returns>If all checks came out true the <paramref name="email"/> was valid and returns null (note that email is not required). Otherwise returns corresponding error message.</returns>
        public static string? AccountPage_NewAccount_ValidateEmailField(string? email, User[]? user, LanguageDataManager LDM)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new(strRegex);
            if (email is null || email.Length == 0)
            {
                return null;
            }
            else if (user is null)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_NewAccount_EmailIsNotUnique);
            }
            else if (!re.IsMatch(email))
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_NewAccount_EmailIsWrongFormat);
            }
            else { return null; }
        }

        /// <summary>
        /// Validates on the <b>AccountPage</b> from the <b>NewAccount</b> section the <paramref name="password"/> field.<br></br>
        /// Check 1: Checks if <paramref name="password"/> field is not empty. <br></br>
        /// Check 2: Checks if <paramref name="password"/> is between 4 and 13 characters.<br></br>
        /// </summary>
        /// <param name="password"></param>
        /// <returns>If all checks came out true the <paramref name="password"/> was valid and returns null. Otherwise returns corresponding error message.</returns>
        public static string? AccountPage_NewAccount_ValidatePasswordField(string? password, LanguageDataManager LDM)
        {
            if (password is null || password.Length == 0)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_NewAccount_NoPassword);
            }
            else if (password is not null && password.Length < 4 || password?.Length > 13)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_NewAccount_PasswordNotEnoughChars);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Validates on the <b>AccountPage</b> from the <b>NewAccount</b> section the <paramref name="confirmpass"/> field.<br></br>
        /// Check 1: Checks if <paramref name="confirmpass"/> field is not empty. <br></br>
        /// Check 2: Checks if <paramref name="confirmpass"/> is between 4 and 13 characters.<br></br>
        /// Check 3: Checks if <paramref name="confirmpass"/> matches the <paramref name="password"/>.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="confirmpass"></param>
        /// <returns>If all checks came out true the <paramref name="confirmpass"/> was valid and returns null. Otherwise returns corresponding error message.</returns>
        public static string? AccountPage_NewAccount_ValidateConfirmField(string? password, string? confirmpass, LanguageDataManager LDM)
        {
            if (confirmpass is null || confirmpass.Length == 0)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_NewAccount_NoConfirmPass);
            }
            else if (confirmpass is not null && confirmpass.Length < 4 || confirmpass?.Length > 13)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_NewAccount_ConfirmPassNotEnoughChars);
            }
            else if (password != confirmpass)
            {
                return LDM.GetTranslation(TranslationKey.MessageBox_Account_ConfirmPassDoesNotExist);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region AdminPanel Field Validation

        /// <summary>
        /// Validates on the <b>AdminPanel</b> the <paramref name="title"/> field. <br></br>
        /// Check 1: Checks if <paramref name="title"/> field is not empty. <br></br>
        /// Check 2: Checks if <paramref name="title"/> is between 1 and 30 characters. <br></br>
        /// </summary>
        /// <param name="title"></param>
        /// <returns>If all checks came out true the <paramref name="title"/> was valid and returns null. Otherwise returns corresponding error message.</returns>
        public static string AdminPanelValidateTitle(string title)
        {
            string errorMessage = string.Empty;
            if (title.Length == 0)
            {
                errorMessage = "Title is required!";
            }
            else if (title.Length > 30)
            {
                errorMessage = "Title must be between 1 and 30 characters.";
            }

            return errorMessage;
        }

        /// <summary>
        /// Validates on the <b>AdminPanel</b> the <paramref name="description"/> field. <br></br>
        /// Check 1: Checks if <paramref name="description"/> is between 0 and 65 characters. <br></br>
        /// </summary>
        /// <param name="description"></param>
        /// <returns>If all checks came out true the <paramref name="description"/> was valid and returns null. Otherwise returns corresponding error message.</returns>
        public static string AdminPanelValidateDescription(string description)
        {
            string errorMessage = string.Empty;
            if (description.Length > 65) errorMessage = "Description must be between 0 and 65 characters.";
            return errorMessage;
        }

        /// <summary>
        /// Validates on the <b>AdminPanel</b> the <paramref name="difficulty"/> field. <br></br>
        /// Check 1: Checks if <paramref name="difficulty"/> is a value between 0 and 3.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns>If all checks came out true the <paramref name="difficulty"/> was valid and returns null. Otherwise returns corresponding error message.</returns>
        public static string AdminPanelValidateDifficulty(string difficulty)
        {
            string errorMessage = string.Empty;
            if (!int.TryParse(difficulty, out int difficultyValue) || !(difficultyValue > -1 && difficultyValue < 4)) errorMessage = "Difficulty must be a number between 0 {easy}, 1 {medium}, 2 {hard} or 3 {hero}.";
            return errorMessage;
        }

        /// <summary>
        /// Validates on the <b>AdminPanel</b> the <see cref="MidiLogic.CurrentMidi"/>. <br></br>
        /// Check 1: Checks if <see cref="MidiLogic.CurrentMidi"/> is not null.
        /// </summary>
        /// <returns>If all checks came out true the <see cref="MidiLogic.CurrentMidi"/> was valid and returns null. Otherwise returns corresponding error message.</returns>
        public static string AdminPanelValidateMidiFile()
        {
            string errorMessage = string.Empty;
            if (MidiLogic.CurrentMidi == null) errorMessage = "MidiFile required!";
            return errorMessage;
        }
        #endregion
    }
}