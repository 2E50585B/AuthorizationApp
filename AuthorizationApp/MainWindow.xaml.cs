using AuthorizationApp.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AuthorizationApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            if (!(e.Content is Page page))
                return;

            Title = $"Page - {page.Title}";

            if (page is AuthPage)
                ButtonBack.Visibility = Visibility.Hidden;
            else
                ButtonBack.Visibility = Visibility.Visible;
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
        }
    }
}