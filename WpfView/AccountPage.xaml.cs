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
        private SecureString? loginUsername = new SecureString();
        private SecureString? loginPassword = new SecureString();
        private SecureString? newAccountUsername = new SecureString();
        private SecureString? newAccountPassword = new SecureString();
        private SecureString? newAccountConfirm = new SecureString();
        public AccountPage(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            DataContext = new DataContextSettings();
            InitializeComponent();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            loginUsername = SaveStringSecure(Login_UsernameInput.Text);
            loginPassword = SaveStringSecure(Login_PasswordInput.Password);
            ClearLoginInput();
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            newAccountUsername = SaveStringSecure(NewAccount_UsernameInput.Text);
            newAccountPassword = SaveStringSecure(NewAccount_PasswordInput.Password);
            newAccountConfirm = SaveStringSecure(NewAccount_ConfirmInput.Password);
            ClearNewAccountInput();
        }

        private SecureString SaveStringSecure(string password)
        {
            SecureString secureString = new SecureString();
            foreach (char charachter in password)
            {
                secureString.AppendChar(charachter);
            }
            return secureString;
        }

        private void ClearLoginInput()
        {
            Login_UsernameInput.Text = "";
            Login_PasswordInput.Password = "";
        }

        private void ClearNewAccountInput()
        {
            NewAccount_UsernameInput.Text = "";
            NewAccount_PasswordInput.Password = "";
            NewAccount_ConfirmInput.Password = "";
        }

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
