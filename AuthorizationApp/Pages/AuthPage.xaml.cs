using AuthorizationApp.Pages.Roles;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AuthorizationApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private bool LoginIsNull => string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrWhiteSpace(TextBoxLogin.Text);

        private bool PasswordIsNull => string.IsNullOrEmpty(PasswordBox.Password) || string.IsNullOrWhiteSpace(PasswordBox.Password);

        private void ButtonEnter_OnClick(object sender, RoutedEventArgs e)
        {
            if (LoginIsNull || PasswordIsNull)
            {
                MessageBox.Show("Write Login and Password!");
                return;
            }

            using (Entities db = new Entities())
            {
                User user = db.User.
                    AsNoTracking().
                    FirstOrDefault(u => u.Login == TextBoxLogin.Text && u.Password == PasswordBox.Password);

                //var users = db.User.AsNoTracking().Where(u => u.Login.StartsWith("test")).ToList();

                //var allUsers = db.User.AsNoTracking().ToList();

                if (user == null)
                {
                    MessageBox.Show("Check Login & Password", "User not found!");
                    return;
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
                        NavigationService?.Navigate(new Menu());
                        break;
                    default:
                        MessageBox.Show($"Такой роли нет - |{user.Role}|!");
                        break;
                }
            }
        }

        private void ButtonRegistration_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Page2());
        }
    }
}