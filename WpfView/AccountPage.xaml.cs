using BusinessLogic;
using Controller;
using Model.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Security;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        private readonly MainMenu _mainMenu;
        private readonly PracticePlayPiano? _practicePlayPiano;

        private bool AllFieldsAreValid { get; set; }
        public bool Closed = false;

        public AccountPage(MainMenu mainMenu, PracticePlayPiano? ppp)
        {
            _mainMenu = mainMenu;
            _practicePlayPiano = ppp;
            DataContext = new DataContextSettings();
            InitializeComponent();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            Login_NameAndPassAreValid(Login_UsernameInput.Text, Login_PasswordInput.Password);
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            //NewAccount_UsernameIsUnique(NewAccount_UsernameInput.Text);
            NewAccount_NameAndPassAreUnique(NewAccount_UsernameInput.Text, NewAccount_PasswordInput.Password);
            //NewAccount_PassAndConfirmPassAreEqual(NewAccount_PasswordInput.Password, NewAccount_ConfirmInput.Password);
            NewAccount_UploadNewUser();
        }

        private async void NewAccount_UploadNewUser()
        {
            if(AllFieldsAreValid) 
            {
                User user = new();
                user.Name = NewAccount_UsernameInput.Text;
                user.Password = NewAccount_PasswordInput.Password;
                user.Email = NewAccount_EmailInput.Text;
                await DatabaseController.UploadNewUser(user);
            }
        }

        #region Validation Methods

        private async void Login_NameAndPassAreValid(string usernameInput, string passwordInput)
        {
            User? user = await DatabaseController.GetLoggingInUser(usernameInput, passwordInput);
            if (ValidationController.AccountPage_Login_UserCredentialsAreValid(user))
            {
                Login_UsernameInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                Login_PasswordInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                _mainMenu.loggedInUser = user;
                if (user is not null && user.isAdmin) NavigationService?.Navigate(_mainMenu.AdminPanel);
                else
                {
                    CloseLogin();
                }
            }
            else
            {
                AllFieldsAreValid = false;
                Login_UsernameInput.Text = string.Empty;
                Login_PasswordInput.Password= string.Empty;
                Login_UsernameInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
                Login_PasswordInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
            }
        }

        private async void NewAccount_UsernameIsUnique(string username)
        {
            User? user = await DatabaseController.GetUserByName(username);
            if (ValidationController.AccountPage_NewAccount_UsernameIsUnique(user))
            {
                AllFieldsAreValid = true;
                NewAccount_UsernameInput.Text = string.Empty;
                NewAccount_UsernameInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            }
            else
            {
                AllFieldsAreValid = false;
                NewAccount_UsernameInput.Text = string.Empty;
                NewAccount_UsernameInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
            }
        }

        private async void NewAccount_NameAndPassAreUnique(string username, string password)
        {
            User? user = await DatabaseController.GetLoggingInUser(username, password);
            if (ValidationController.AccountPage_NewAccount_UserCredentialsAreValid(user))
            {
                AllFieldsAreValid = true;
                NewAccount_UsernameInput.Text = string.Empty;
                NewAccount_PasswordInput.Password = string.Empty;
                NewAccount_ConfirmInput.Password = string.Empty;
                NewAccount_UsernameInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                NewAccount_PasswordInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            }
            else
            {
                AllFieldsAreValid = false;
                NewAccount_UsernameInput.Text = string.Empty;
                NewAccount_PasswordInput.Password = string.Empty;
                NewAccount_ConfirmInput.Password = string.Empty;
                NewAccount_UsernameInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
                NewAccount_PasswordInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
            }
            return;
        }

        private void NewAccount_PassAndConfirmPassAreEqual(string password, string confirmpass)
        {
            if (ValidationController.AccountPage_NewAccount_PassAndConfirmPassAreEqual(password, confirmpass))
            {
                AllFieldsAreValid = true;
                NewAccount_ConfirmInput.Password = string.Empty;
                NewAccount_PasswordInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                NewAccount_ConfirmInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            }
            else
            {
                AllFieldsAreValid = false;
                NewAccount_PasswordInput.Password = string.Empty;
                NewAccount_ConfirmInput.Password = string.Empty;
                NewAccount_PasswordInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
                NewAccount_ConfirmInput.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
            }
            return;
        }

        #endregion

        #region Fields clear methods

        private void ClearLoginFields()
        {
            Login_UsernameInput.Text = string.Empty;
            Login_PasswordInput.Password = string.Empty;
        }

        private void ClearNewAccountFields()
        {
            NewAccount_UsernameInput.Text = string.Empty;
            NewAccount_EmailInput.Text = string.Empty;
            NewAccount_PasswordInput.Password = string.Empty;
            NewAccount_ConfirmInput.Password = string.Empty;
        }
        #endregion

        #region Menubar event clicks

        /// <summary>
        /// lets the player go back to the main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            CloseLogin();
        }

        private void CloseLogin()
        {
            _mainMenu.Account_ChangeIconBasedOnUser();
            Closed = true;

            if (_practicePlayPiano is null)
            {
                NavigationService?.Navigate(_mainMenu);
            }
            else
            {
                NavigationService?.Navigate(_practicePlayPiano);
            }

            ClearLoginFields();
            ClearNewAccountFields();
        }

        #endregion
    }
}
