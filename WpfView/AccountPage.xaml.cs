using Controller;
using Model;
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
            IsVisibleChanged += MainMenu_IsVisibleChanged;
        }

        /// <summary>
        /// On page visibility change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void MainMenu_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// Translate labels
        /// </summary>
		private void UpdateUI()
        {
            BackMenu.Header = LanguageController.GetTranslation(TranslationKey.Menubar_BackToMain);

            LoginTitle.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_LoginTitle);
            UsernameLabel.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_LoginUsername);
            PasswordLabel.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_LoginPassword);
            LoginButton.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_LoginButton);

            NewAccountTitle.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_RegisterTitle);
            NAUsernameLabel.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_RegisterUsername);
            NAEmailLabel.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_RegisterEmail);
            NAPasswordLabel.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_RegisterPassword);
            NAConfirmPassLabel.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_RegisterPasswordConfirm);
            CreateButton.Content = LanguageController.GetTranslation(TranslationKey.AccountPage_RegisterButton);
        }

        /// <summary>
        /// This method gets all the input from the "Login" section and validates it when the "Login" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox nameTextBox = Login_UsernameInput;
            PasswordBox passwordBox = Login_PasswordInput;
            User? existingUser = await DatabaseController.GetUserByName(nameTextBox.Text);
            User? loggingInUser = await DatabaseController.GetLoggingInUser(nameTextBox.Text, passwordBox.Password);

            Login_ValidateUsernameField(nameTextBox.Text, existingUser);
            Login_ValidatePasswordField(passwordBox.Password, loggingInUser);
        }

        /// <summary>
        /// This method gets all the filled in input from the "New Account" section and validates it when the "Create" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox nameTextBox = NewAccount_UsernameInput;
            TextBox emailTextBox = NewAccount_EmailInput;
            PasswordBox passwordBox = NewAccount_PasswordInput;
            PasswordBox confirmBox = NewAccount_ConfirmInput;
            User? existingUser = await DatabaseController.GetUserByName(nameTextBox.Text);
            User? existingUserByEmail = await DatabaseController.GetUserByEmail(emailTextBox.Text);

            NewAccount_ValidateUsernameField(nameTextBox.Text, existingUser);
            NewAccount_ValidateEmailField(emailTextBox.Text, existingUserByEmail);
            NewAccount_ValidatePasswordField(passwordBox.Password);
            NewAccount_ValidateConfirmField(passwordBox.Password, confirmBox.Password);

        }

        #region Login Validation Methods

        /// <summary>
        /// Validates the username field when trying to log in.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
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

        /// <summary>
        /// Validates the password field when trying to log in.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        private void Login_ValidatePasswordField(string? username, User? user)
        {
            string? errorMessage = ValidationController.AccountPage_Login_ValidatePasswordField(username, user);
            if (errorMessage is null)
            {
                if(user is null) return;
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

        /// <summary>
        /// Validates the username and password combination when trying to log in. When the combination is valid, logs the user in.
        /// </summary>
        /// <param name="user"></param>
        private void Login(User user)
        {
            ClearAllFields();
            SetFieldSuccesBackground(Login_PasswordInput);
            _mainMenu.LoggedInUser = user;
        }

        /// <summary>
        /// Changes the icon of the main menu and sets the <see cref="Closed"/> to true. 
        /// Also navigates the user to the desired page based on the account.
        /// </summary>
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


        /// <summary>
        /// Validates the username field when creating a new user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
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

        /// <summary>
        /// Validates the email field when creating a new user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="user"></param>
        private void NewAccount_ValidateEmailField(string? email, User? user)
        {
            string? errorMessage = ValidationController.AccountPage_NewAccount_ValidateEmailField(email, user);
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

        /// <summary>
        /// Validates the password field when creating a new user.
        /// </summary>
        /// <param name="password"></param>
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

        /// <summary>
        /// Validates the confirm password field when creating a new user.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="confirmpass"></param>
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

        /// <summary>
        /// Uploads the user to the SQL Database.
        /// </summary>
        private async void UploadNewUser()
        {
            User user = new()
            {
                Name = NewAccount_UsernameInput.Text,
                Password = NewAccount_PasswordInput.Password,
                Email = NewAccount_EmailInput.Text
            };
            await DatabaseController.UploadNewUser(user);
            ClearAllFields();
            SetFieldSuccesBackground(NewAccount_PasswordInput);
            MessageBox.Show("Use your new credentials to log in to Piano Hero!", "New user was succesfully created!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        #endregion

        #region Validation Feedback Methods

        /// <summary>
        /// Sets the background of the <paramref name="textBox"/> to an error color. And clears the input.
        /// </summary>
        /// <param name="textBox"></param>
        private static void SetFieldErrorBackground(TextBox textBox)
        {
            textBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
            textBox.Clear();
        }

        /// <summary>
        /// Sets the background of the <paramref name="passwordBox"/> to an error color. And clears the input.
        /// </summary>
        /// <param name="passwordBox"></param>
        private static void SetFieldErrorBackground(PasswordBox passwordBox)
        {
            passwordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE25A3F"));
            passwordBox.Clear();
        }

        /// <summary>
        /// Sets the background of the <paramref name="textBox"/> to white.
        /// </summary>
        /// <param name="textBox"></param>
        private static void SetFieldSuccesBackground(TextBox textBox)
        {
            textBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        /// <summary>
        /// Sets the background of the <paramref name="passwordBox"/> to white.
        /// </summary>
        /// <param name="passwordBox"></param>
        private static void SetFieldSuccesBackground(PasswordBox passwordBox)
        {
            passwordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        /// <summary>
        /// Clears just one field based on the <paramref name="textBox"/>.
        /// </summary>
        /// <param name="textBox"></param>
        private static void ClearField(TextBox textBox)
        {
            textBox.Clear();
        }

        /// <summary>
        /// Clears just one field based on the <paramref name="passwordBox"/>.
        /// </summary>
        /// <param name="passwordBox"></param>
        private static void ClearField(PasswordBox passwordBox)
        {
            passwordBox.Clear();
        }

        /// <summary>
        /// Adds the error message to the messagebox.
        /// </summary>
        /// <param name="errorMessage"></param>
        private void AddErrorMessageToMessageBox(string errorMessage)
        {
            if (FinalErrorMessage is not null) FinalErrorMessage.AppendLine(errorMessage);
            else { FinalErrorMessage = new StringBuilder(); FinalErrorMessage.AppendLine(errorMessage); }
        }

        /// <summary>
        /// Shows the error message.
        /// </summary>
        private void ShowErrorMessage()
        {
            if (FinalErrorMessage is not null)
            {
                MessageBox.Show(FinalErrorMessage.ToString(), LanguageController.GetTranslation(TranslationKey.MessageBox_Account_ErrorText), MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Clears the <see cref="FinalErrorMessage"/>.
        /// </summary>
        private void ClearErrorMessage()
        {
            FinalErrorMessage?.Clear();
        }

        /// <summary>
        /// Clears all fields.
        /// </summary>
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
