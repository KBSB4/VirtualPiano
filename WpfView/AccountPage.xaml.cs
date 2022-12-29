using Controller;
using Model.DatabaseModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        //TODO Summaries
        private readonly MainMenu _mainMenu;
        private readonly PracticePlayPiano? _practicePlayPiano;

        private StringBuilder? FinalErrorMessage { get; set; }

        public bool Closed = false; //For UploadScore dialog

        public AccountPage(MainMenu mainMenu, PracticePlayPiano? ppp)
        {
            _mainMenu = mainMenu;
            _practicePlayPiano = ppp;
            DataContext = new DataContextSettings();
            InitializeComponent();
        }

        private async void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox nameTextBox = Login_UsernameInput;
            PasswordBox passwordBox = Login_PasswordInput;
            User? existingUser = await DatabaseController.GetUserByName(nameTextBox.Text);
            User? loggingInUser = await DatabaseController.GetLoggingInUser(nameTextBox.Text, passwordBox.Password);

            Login_ValidateUsernameField(nameTextBox.Text, existingUser);
            Login_ValidatePasswordField(passwordBox.Password, loggingInUser);
        }

        private async void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox nameTextBox = NewAccount_UsernameInput;
            TextBox emailTextBox = NewAccount_EmailInput;
            PasswordBox passwordBox = NewAccount_PasswordInput;
            PasswordBox confirmBox = NewAccount_ConfirmInput;
            User? existingUser = await DatabaseController.GetUserByName(nameTextBox.Text);
            User[]? allUsers = await DatabaseController.GetAllUsers();

            NewAccount_ValidateUsernameField(nameTextBox.Text, existingUser);
            NewAccount_ValidateEmailField(emailTextBox.Text, allUsers);
            NewAccount_ValidatePasswordField(passwordBox.Password);
            NewAccount_ValidateConfirmField(passwordBox.Password, confirmBox.Password);

        }

        #region Login Validation Methods

        private void Login_ValidateUsernameField(string? username, User? user)
        {
            string? errorMessage = ValidationController.AccountPage_Login_ValidateUsernameField(username, user);
            if (errorMessage is null)
            {
                SetFieldSuccesBackground(Login_UsernameInput);
            }
            else
            {
                SetFieldErrorBackground(Login_UsernameInput);
                ClearField(Login_UsernameInput);
                AddErrorMessageToMessageBox(errorMessage);
            }
        }

        private void Login_ValidatePasswordField(string? username, User? user)
        {
            if (user is null) return;
            string? errorMessage = ValidationController.AccountPage_Login_ValidatePasswordField(username, user);
            if (errorMessage is null)
            {
                Login(user);
                if (user is not null && user.IsAdmin)
                {
                    NavigationService?.Navigate(_mainMenu.AdminPanel);
                }
                else
                {
                    CloseLogin();
                }
            }
            else
            {
                SetFieldErrorBackground(Login_PasswordInput);
                ClearField(Login_PasswordInput);
                AddErrorMessageToMessageBox(errorMessage);
                ShowErrorMessage();
                ClearErrorMessage();
            }
        }

        private void Login(User user)
        {
            ClearAllFields();
            SetFieldSuccesBackground(Login_PasswordInput);
            _mainMenu.LoggedInUser = user;
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
            ClearAllFields();
        }

        #endregion

        #region New Account Validation Methods


        private void NewAccount_ValidateUsernameField(string? username, User? user)
        {
            string? errorMessage = ValidationController.AccountPage_NewAccount_ValidateUsernameField(username, user);
            if (errorMessage is null)
            {
                SetFieldSuccesBackground(NewAccount_UsernameInput);
            }
            else
            {
                SetFieldErrorBackground(NewAccount_UsernameInput);
                ClearField(NewAccount_UsernameInput);
                AddErrorMessageToMessageBox(errorMessage);
            }
        }

        private void NewAccount_ValidateEmailField(string? email, User[]? users)
        {
            string? errorMessage = ValidationController.AccountPage_NewAccount_ValidateEmailField(email, users);
            if (errorMessage is null)
            {
                SetFieldSuccesBackground(NewAccount_EmailInput);
            }
            else
            {
                SetFieldErrorBackground(NewAccount_EmailInput);
                ClearField(NewAccount_EmailInput);
                AddErrorMessageToMessageBox(errorMessage);
            }
        }

        private void NewAccount_ValidatePasswordField(string? password)
        {
            string? errorMessage = ValidationController.AccountPage_NewAccount_ValidatePasswordField(password);
            if (errorMessage is null)
            {
                SetFieldSuccesBackground(NewAccount_PasswordInput);
            }
            else
            {
                SetFieldErrorBackground(NewAccount_PasswordInput);
                ClearField(NewAccount_PasswordInput);
                AddErrorMessageToMessageBox(errorMessage);
            }
        }

        private void NewAccount_ValidateConfirmField(string? password, string? confirmpass)
        {
            string? errorMessage = ValidationController.AccountPage_NewAccount_ValidateConfirmField(password, confirmpass);
            if (errorMessage is null)
            {
                SetFieldSuccesBackground(NewAccount_ConfirmInput);
                UploadNewUser();
            }
            else
            {
                SetFieldErrorBackground(NewAccount_ConfirmInput);
                ClearField(NewAccount_ConfirmInput);
                AddErrorMessageToMessageBox(errorMessage);
                ShowErrorMessage();
                ClearErrorMessage();
            }
        }

        private async void UploadNewUser()
        {
            User user = new();
            user.Name = NewAccount_UsernameInput.Text;
            user.Password = NewAccount_PasswordInput.Password;
            user.Email = NewAccount_EmailInput.Text;
            await DatabaseController.UploadNewUser(user);
            ClearAllFields();
            SetFieldSuccesBackground(NewAccount_PasswordInput);
            MessageBox.Show("Use your new credentials to log in to Piano Hero!", "New user was succesfully created!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        #endregion

        #region Validation Feedback Methods

        private void SetFieldErrorBackground(TextBox textBox)
        {
            textBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
            textBox.Clear();
        }

        private void SetFieldErrorBackground(PasswordBox passwordBox)
        {
            passwordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
            passwordBox.Clear();
        }

        private void SetFieldSuccesBackground(TextBox textBox)
        {
            textBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        private void SetFieldSuccesBackground(PasswordBox passwordBox)
        {
            passwordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        private void ClearField(TextBox textBox)
        {
            textBox.Clear();
        }

        private void ClearField(PasswordBox passwordBox)
        {
            passwordBox.Clear();
        }

        private void AddErrorMessageToMessageBox(string errorMessage)
        {
            if (FinalErrorMessage is not null) FinalErrorMessage.AppendLine(errorMessage);
            else { FinalErrorMessage = new StringBuilder(); FinalErrorMessage.AppendLine(errorMessage); }
        }

        private void ShowErrorMessage()
        {
            if (FinalErrorMessage is not null)
            {
                MessageBox.Show(FinalErrorMessage.ToString(), "Not all fields met the requirements...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ClearErrorMessage()
        {
            if (FinalErrorMessage is not null)
                FinalErrorMessage.Clear();
        }

        private void ClearAllFields()
        {
            Login_UsernameInput.Clear();
            Login_PasswordInput.Clear();
            NewAccount_UsernameInput.Clear();
            NewAccount_EmailInput.Clear();
            NewAccount_PasswordInput.Clear();
            NewAccount_ConfirmInput.Clear();
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
        #endregion
    }
}
