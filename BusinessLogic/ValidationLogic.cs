using Model.DatabaseModels;

namespace BusinessLogic
{
    public static class ValidationLogic
    {
        #region AccountPage Field Validation

        public static bool AccountPage_Login_UsernameIsUnique(User? user)
        {
            if (user is not null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool AccountPage_Login_UserCredentialsAreValid(User? user)
        {
            if (user is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AccountPage_NewAccount_UserCredentialsAreValid(User? user)
        {
            if (user is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AccountPage_NewAccount_PassAndConfirmPassAreEqual(string password, string confirmpass)
        {
            if (password == confirmpass)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region AdminPanel Field Validation
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

        public static string AdminPanelValidateDescription(string description)
        {
            string errorMessage = string.Empty;
            if (description.Length > 65) errorMessage = "Description must be between 0 and 65 characters.";
            return errorMessage;
        }

        public static string AdminPanelValidateDifficulty(string difficulty)
        {
            string errorMessage = string.Empty;
            if (!int.TryParse(difficulty, out int difficultyValue) || !(difficultyValue > -1 && difficultyValue < 4)) errorMessage = "Difficulty must be number between 0 {easy}, 1 {medium}, 2 {hard} or 3 {hero}.";
            return errorMessage;
        }


        public static string AdminPanelValidateMidiFile()
        {
            string errorMessage = string.Empty;
            if (MidiLogic.CurrentMidi == null) errorMessage = "MidiFile required!";
            return errorMessage;
        }
        #endregion
    }
}
