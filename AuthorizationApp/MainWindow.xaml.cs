using AuthorizationApp.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace AuthorizationApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window Window { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DisablingUIKeyCommandBindings();
        }

        private void DisablingUIKeyCommandBindings()
        {
            foreach (RoutedUICommand vNavigationCommand in new RoutedUICommand[]
            {
                NavigationCommands.BrowseBack,
                NavigationCommands.BrowseForward,
                NavigationCommands.BrowseHome,
                NavigationCommands.BrowseStop,
                NavigationCommands.Refresh,
                NavigationCommands.Favorites,
                NavigationCommands.Search,
                NavigationCommands.IncreaseZoom,
                NavigationCommands.DecreaseZoom,
                NavigationCommands.Zoom,
                NavigationCommands.NextPage,
                NavigationCommands.PreviousPage,
                NavigationCommands.FirstPage,
                NavigationCommands.LastPage,
                NavigationCommands.GoToPage,
                NavigationCommands.NavigateJournal
            })
            {
                MainFrame.CommandBindings.Add(new CommandBinding(vNavigationCommand, (sender, args) => { }));
            }
        }

        private void MainWindow_OnLoad(object sender, RoutedEventArgs e)
        {
            Window = GetWindow(this);
            Window.KeyDown += OnKeyDown;
            Window.Focus();
        }

        private void MainWindow_OnUnload(object sender, RoutedEventArgs e) => Window.KeyDown -= OnKeyDown;

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                ButtonBack_OnClick(ButtonBack, null);
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Page page)
            {
                Title = $"Page - {page.Title}";

                if (page is AuthPage)
                    ButtonBack.Visibility = Visibility.Collapsed;
                else
                    ButtonBack.Visibility = Visibility.Visible;
            }
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            if (ButtonBack.Visibility == Visibility.Visible)
            {
                if (MainFrame.CanGoBack)
                    MainFrame.GoBack();
            }
        }
    }
}