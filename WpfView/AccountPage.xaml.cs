using Controller;
using Model.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Security;
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
        public AccountPage(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            DataContext = new DataContextSettings();
            InitializeComponent();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            if(Login_PasswordIsValid().Result == true) { Debug.WriteLine("Password is valid!"); }
            ClearLoginInput();
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            UploadNewUser();
            ClearNewAccountInput();
        }

        private async void UploadNewUser()
        {
            User user = new();
            user.Name = NewAccount_UsernameInput.Text;
            user.Password = NewAccount_PasswordInput.Password;
            user.Email = NewAccount_EmailInput.Text;
            await DatabaseController.UploadNewUser(user);
        }

        private void ClearLoginInput()
        {
            Login_UsernameInput.Text = string.Empty;
            Login_PasswordInput.Password = string.Empty;
        }

        private void ClearNewAccountInput()
        {
            NewAccount_UsernameInput.Text = string.Empty;
            NewAccount_EmailInput.Text= string.Empty;
            NewAccount_PasswordInput.Password = string.Empty;
            NewAccount_ConfirmInput.Password = string.Empty;
        }

        #region Validation Methods

        private async void Login_NameAndPassAreValid()
        {
            bool isValid = true;
            string usernameInput = Login_UsernameInput.Text;
            string passwordInput = Login_PasswordInput.Password;

            User[]? userList = await DatabaseController.GetAllUsernamesAndPassphrases();
            foreach (User user in userList)
            {
                if (user.Name.Equals(usernameInput))
                {
                    
                }
                else 
                {
                    isValid = false;
                    NewAccount_UsernameInput.Foreground = new SolidColorBrush(Color.FromRgb(10,10,10));
                }
            }
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
            NavigationService?.Navigate(_mainMenu);
        }

        #endregion
    }
}
