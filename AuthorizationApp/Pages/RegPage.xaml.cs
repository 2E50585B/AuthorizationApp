using AuthorizationApp.DialogService;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AuthorizationApp.Extentions;
using System;

namespace AuthorizationApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        private bool _passwordIsHidden = true;
        private string _role;

        public RegPage()
        {
            InitializeComponent();
        }

        private void ShowHidePassword_OnClick(object sender, RoutedEventArgs e)
        {
            _passwordIsHidden = !_passwordIsHidden;

            if (_passwordIsHidden)
            {
                NewPassword.Visibility = Visibility.Visible;
                PasswordText.Visibility = Visibility.Collapsed;
                NewPassword.Password = PasswordText.Text;
                NewPassword.Focus();
                NewPassword.SetSelection(NewPassword.Password.Length);
            }
            else
            {
                NewPassword.Visibility = Visibility.Collapsed;
                PasswordText.Visibility = Visibility.Visible;
                PasswordText.Text = NewPassword.Password;
                PasswordText.Focus();
                PasswordText.CaretIndex = PasswordText.Text.Length;
            }
        }

        private void NewRole_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = NewRole.SelectedValue as ComboBoxItem;
            TextBlock content = item?.Content as TextBlock;
            _role = content?.Text ?? "Customer";
        }

        private void GetPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            DefaultDialogService dialogService = new DefaultDialogService();
            if (dialogService.OpenFileDialog())
                UserPhoto.Source = GetImageSource(dialogService.FilePath);
        }

        private ImageSource GetImageSource(string filePath)
        {
            try
            {
                BitmapImage glowIcon = new BitmapImage();
                glowIcon.BeginInit();
                glowIcon.UriSource = new System.Uri(filePath);
                glowIcon.EndInit();
                return glowIcon;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.TargetSite.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return UserPhoto.Source;
            }
        }

        private void Registration_OnClick(object sender, RoutedEventArgs e)
        {
            RegistrationBtn.IsEnabled = false;
            StringBuilder stringBuilder = new StringBuilder();

            if (NewLogin.Text.IsEmpty())
                stringBuilder.AppendLine("Неверный логин");

            if (NewPassword.Password.IsEmpty())
                stringBuilder.AppendLine("Неверный пароль");
            else if (NewPasswordTwice.Password.IsEmpty())
                stringBuilder.AppendLine("Нужно повторить пароль!");
            else if (NewPassword.Password != NewPasswordTwice.Password)
                stringBuilder.AppendLine("Пароли не совпадают");

            if (NewFIO.Text.IsEmpty())
                stringBuilder.AppendLine("Неверные ФИО");

            if (stringBuilder.Length == 0)
            {
                if (!LoginIsAvailable())
                    stringBuilder.AppendLine("Этот логин уже используется!");
                else
                {
                    stringBuilder.Append("Вы зарегестрированы!!!");
                    ClearFields();
                }
            }

            MessageBox.Show(stringBuilder.ToString(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
            stringBuilder.Clear();
            RegistrationBtn.IsEnabled = true;
        }

        private bool LoginIsAvailable()
        {
            bool loginIsAvaliable = true;

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT 1 FROM [User] WHERE [Login] = '{NewLogin.Text}'"
                };

                SqlDataReader dataReader = command.ExecuteReader();

                try
                {
                    while (dataReader.Read())
                        loginIsAvaliable = dataReader is null;
                }
                catch (IndexOutOfRangeException ex)
                {
                    MessageBox.Show(ex.Message);
                    loginIsAvaliable = false;
                }
                finally
                {
                    dataReader.Close();
                }

                if (loginIsAvaliable)
                {
                    command.CommandText = @"INSERT INTO [User] VALUES (@Login, @Password, @Role, @FIO, @Photo)";
                    command.Parameters.Add("@Login", SqlDbType.NVarChar, 20);
                    command.Parameters.Add("@Password", SqlDbType.NVarChar, 20);
                    command.Parameters.Add("@Role", SqlDbType.NVarChar, 20);
                    command.Parameters.Add("@FIO", SqlDbType.NVarChar, 100);
                    command.Parameters.Add("@Photo", SqlDbType.VarBinary, int.MaxValue);

                    command.Parameters["@Login"].Value = NewLogin.Text;
                    command.Parameters["@Password"].Value = NewPassword.Password;
                    command.Parameters["@Role"].Value = _role;
                    command.Parameters["@FIO"].Value = NewFIO.Text;
                    command.Parameters["@Photo"].Value = UserPhoto.Source.Serialize(new BmpBitmapEncoder());

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return loginIsAvaliable;
        }

        private void ClearFields_OnClick(object sender, RoutedEventArgs e) => ClearFields();

        private void ClearFields()
        {
            NewLogin.Text = string.Empty;
            NewPassword.Password = string.Empty;
            NewPasswordTwice.Password = string.Empty;
            NewFIO.Text = string.Empty;
        }
    }
}