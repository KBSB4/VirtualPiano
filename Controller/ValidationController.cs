using BusinessLogic;
using Melanchall.DryWetMidi.Core;
using Model.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static BusinessLogic.ValidationLogic;

namespace Controller
{
    public static class ValidationController
    {
        #region AccountPage Login Validation Methods

        public static string? AccountPage_Login_ValidateUsernameField(string? username, User? user)
        {
            return ValidationLogic.AccountPage_Login_ValidateUsernameField(username, user);
        }

        public static string? AccountPage_Login_ValidatePasswordField(string? password, User? user)
        {
            return ValidationLogic.AccountPage_Login_ValidatePasswordField(password, user);
        }

        #endregion

        #region AccountPage NewAccount Validation Methods

        public static string? AccountPage_NewAccount_ValidateUsernameField(string? username, User? user)
        {
            return ValidationLogic.AccountPage_NewAccount_ValidateUsernameField(username, user);
        }

        public static string? AccountPage_NewAccount_ValidateEmailField(string? email, User[]? users)
        {
            return ValidationLogic.AccountPage_NewAccount_ValidateEmailField(email, users);
        }

        public static string? AccountPage_NewAccount_ValidatePasswordField(string? password)
        {
            return ValidationLogic.AccountPage_NewAccount_ValidatePasswordField(password);
        }

        public static string? AccountPage_NewAccount_ValidateConfirmField(string? password, string? confirmpass)
        {
            return ValidationLogic.AccountPage_NewAccount_ValidateConfirmField(password, confirmpass);
        }

        #endregion

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
     
        public static string AdminPanelValidationMessageMidiFile(MidiFile file)
        {
            return ValidationLogic.AdminPanelValidateMidiFile(file);
        }


    }
}
