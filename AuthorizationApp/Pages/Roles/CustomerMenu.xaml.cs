using System.Windows;
using System.Windows.Controls;

namespace AuthorizationApp.Pages.Roles
{
    /// <summary>
    /// Логика взаимодействия для CustomerMenu.xaml
    /// </summary>
    public partial class CustomerMenu : Page
    {
        private User Customer { get; set; }

        public CustomerMenu(User customer)
        {
            InitializeComponent();
            Customer = customer;
        }

        private void CustomerMenu_Loaded(object sender, RoutedEventArgs e)
        {
            TextFIO.Text = Customer.FIO;
            TextRole.Text = Customer.Role;
            //CustomerPhoto.Source = Customer.Photo;
        }

        private void ButtonPage2_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Page2());
        }

        private void ButtonAuthPage_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }

        private void ButtonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Menu());
        }
    }
}