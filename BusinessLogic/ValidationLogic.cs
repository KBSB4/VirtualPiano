using Melanchall.DryWetMidi.Core;
using Microsoft.IdentityModel.Abstractions;
using Model.DatabaseModels;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class ValidationLogic
    {
        #region AccountPage Login Field Validation

        public static string? AccountPage_Login_ValidateUsernameField(string? username, User? user)
        {
            if (username is null || username.Length == 0)
            {
                return "Please fill in the Username field!";
            }
            else if (username is not null && username.Length < 4 || username.Length > 13)
            {
                return "Please use an Username between 4 and 13 characters!";
            }
            else if (user is null) 
            {
                return "PLease fill in an Username that exists!";
            }
            else
            {
                return null;
            }
        }

        public static string? AccountPage_Login_ValidatePasswordField(string? password, User? user)
        {
            if (password is null || password.Length == 0)
            {
                return "Please fill in the Password field!";
            }
            else if (password is not null && password.Length < 4 || password.Length > 13)
            {
                return "Please fill in a Password between 4 and 13 characters!";
            }
            else if (user is null)
            {
                return "Please fill in a Password that matches the username!";
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region AccountPage NewAccount Fields Validation

        public static string? AccountPage_NewAccount_ValidateUsernameField(string? username, User? user)
        {
            if (username is null || username.Length == 0)
            {
                return "Please fill in the Username field!";
            }
            else if (username is not null && username.Length < 4 || username.Length > 13)
            {
                return "Please use an Username between 4 and 13 characters!";
            }
            else if (user is not null)
            {
                return "Please fill in an Username that does not exist!";
            }
            else
            {
                return null;
            }
        }

        public static string? AccountPage_NewAccount_ValidateEmailField(string? email, User[]? users)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (email is null || email.Length == 0)
            {
                return null;
            }
            else if (users is not null)
            {
                if (re.IsMatch(email))
                {
                    foreach (User user in users)
                    {
                        if (user.Email == email)
                        {
                            return "Please fill in an Email that is unique!";
                        }
                    }
                }
                else 
                { 
                    return "Please fill in an Email that matches the right format!";
                }
                return null;
            }
            else { return null; }
        }

        public static string? AccountPage_NewAccount_ValidatePasswordField(string? password)
        {
            if (password is null || password.Length == 0)
            {
                return "Please fill in the Password field!";
            }
            else if (password is not null && password.Length < 4 || password.Length > 13)
            {
                return "Please use a Password between 4 and 13 characters!";
            }
            else
            {
                return null;
            }
        }

        public static string? AccountPage_NewAccount_ValidateConfirmField(string? password, string? confirmpass)
        {
            if (confirmpass is null || confirmpass.Length == 0)
            {
                return "Please fill in the Confirm field!";
            }
            else if (confirmpass is not null && confirmpass.Length < 4 || confirmpass.Length > 13)
            {
                return "Please fill in a Confirm between 4 and 13 characters!";
            }
            else if (password != confirmpass)
            {
                return "Please fill in a Confirm that matches the Password field!";
            }
            else
            { 
                return null; 
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
