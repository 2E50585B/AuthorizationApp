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

        private void ButtonEnter_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Write Login and Password!");
                return;
            }

            using (var db = new Entities())
            {
                var user = db.User.
                    AsNoTracking().
                    FirstOrDefault(u => u.Login == TextBoxLogin.Text && u.Password == PasswordBox.Password);

                //var users = db.User.AsNoTracking().Where(u => u.Login.StartsWith("max")).ToList();

                //var allUsers = db.User.AsNoTracking().ToList();

                if (user == null)
                {
                    MessageBox.Show("User not found!");
                    return;
                }

                MessageBox.Show("User Successfully Found!");

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
                        MessageBox.Show($"Такой роли нет в Базе Данных - {user.Role}!");
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