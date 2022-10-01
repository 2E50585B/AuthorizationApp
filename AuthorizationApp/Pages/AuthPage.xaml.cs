using AuthorizationApp.Extentions;
using AuthorizationApp.Pages.Roles;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AuthorizationApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private Window Window { get; set; }
        private bool _passwordIsHidden = true;

        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthPage_OnLoad(object sender, RoutedEventArgs e)
        {
            Window = Window.GetWindow(this);
            Window.KeyDown += KeyEnter_OnDown;
            Window.Focus();
        }

        private void AuthPage_OnUnload(object sender, RoutedEventArgs e)
        {
            Window.KeyDown -= KeyEnter_OnDown;
            TextBoxLogin.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            PasswordText.Text = string.Empty;
        }

        private void ShowHidePassword_OnClick(object sender, RoutedEventArgs e)
        {
            _passwordIsHidden = !_passwordIsHidden;

            if (_passwordIsHidden)
            {
                PasswordBox.Visibility = Visibility.Visible;
                PasswordText.Visibility = Visibility.Collapsed;
                PasswordBox.Password = PasswordText.Text;
                PasswordBox.Focus();
                PasswordBox.SetSelection(PasswordBox.Password.Length);
            }
            else
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordText.Visibility = Visibility.Visible;
                PasswordText.Text = PasswordBox.Password;
                PasswordText.Focus();
                PasswordText.CaretIndex = PasswordText.Text.Length;
            }
        }

        private void KeyEnter_OnDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
                ButtonEnter_OnClick(ButtonEnter, null);
        }

        private void ButtonEnter_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxLogin.Text.IsEmpty() || PasswordBox.Password.IsEmpty())
            {
                MessageBox.Show("Введите Логин и Пароль!", "Пустые поля", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            using (Entities db = new Entities())
            {
                User user = db.User.
                    AsNoTracking().
                    FirstOrDefault(u => u.Login == TextBoxLogin.Text);

                //var users = db.User.AsNoTracking().Where(u => u.Login.StartsWith("test")).ToList();

                //var allUsers = db.User.AsNoTracking().ToList();

                if (user == null)
                {
                    MessageBox.Show("Проверьте Логин", "Пользователь не найден!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if (user.Password != PasswordBox.Password)
                    {
                        MessageBox.Show($"Проверьте Пароль", "Не верный пароль!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                switch (user.Role)
                {
                    case "Customer":
                        NavigationService?.Navigate(new CustomerMenu(user));
                        break;
                    case "Заказчик":
                        NavigationService?.Navigate(new Menu());
                        break;
                    case "Директор":
                        NavigationService?.Navigate(new DirectorMenu(user));
                        break;
                    default:
                        MessageBox.Show($"Должности {user.Role} - нет!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }

        private void ButtonRegistration_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }
    }
}