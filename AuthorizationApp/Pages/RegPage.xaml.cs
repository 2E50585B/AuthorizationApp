using AuthorizationApp.DialogService;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AuthorizationApp.Extentions;

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
            _role = content?.Text ?? "Consumer";
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
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, ex.TargetSite.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return UserPhoto.Source;
            }
        }

        private void Registration_OnClick(object sender, RoutedEventArgs e)
        {
            RegistrationBtn.IsEnabled = false;
            StringBuilder stringBuilder = new StringBuilder();

            if (IsEmpty(NewLogin.Text))
                stringBuilder.AppendLine("Неверный логин");

            if (IsEmpty(NewPassword.Password))
                stringBuilder.AppendLine("Неверный пароль");
            else if (NewPasswordTwice.Password == string.Empty)
                stringBuilder.AppendLine("Нужно повторить пароль!");
            else if (NewPassword.Password != NewPasswordTwice.Password)
                stringBuilder.AppendLine("Пароли не совпадают");

            if (IsEmpty(NewFIO.Text))
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

        private bool IsEmpty(string text) => string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);

        private bool LoginIsAvailable()
        {
            bool loginIsAvaliable = true;

            string connectionString = ConfigurationManager.ConnectionStrings["Entities"].ConnectionString.ToString();

            if (connectionString.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder =
                    new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(connectionString);
                connectionString = efBuilder.ProviderConnectionString;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
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
                catch (System.IndexOutOfRangeException ex)
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
                    /* Не удаётся правильно закодировать/декодировать изображение
                    command.Parameters.Add("@Photo", System.Data.SqlDbType.Binary, int.MaxValue);

                    BitmapImage bmp = UserPhoto.Source as BitmapImage;
                    int height = bmp.PixelHeight;
                    int width = bmp.PixelWidth;
                    int stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);
                    byte[] photoData = new byte[height * stride];
                    bmp.CopyPixels(photoData, stride, 0);

                    command.Parameters["@Photo"].Value = photoData;*/
                    command.Parameters["@Login"].Value = NewLogin.Text;
                    command.Parameters["@Password"].Value = NewPassword.Password;
                    command.Parameters["@Role"].Value = _role;
                    command.Parameters["@FIO"].Value = NewFIO.Text;

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