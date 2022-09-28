using System.Data;
using System.Windows;
using System.Windows.Controls;
using AuthorizationApp.ActionsWithDB;

namespace AuthorizationApp.Pages.Roles
{
    /// <summary>
    /// Логика взаимодействия для DirectorMenu.xaml
    /// </summary>
    public partial class DirectorMenu : Page
    {
        private User Director { get; set; }
        private SortingValues SortingValue { get; set; }
        private string Role { get; set; }

        public DirectorMenu(User director)
        {
            InitializeComponent();
            Director = director;
        }

        private void DirectorMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextFIO.Text = Director.FIO;
            TextRole.Text = Director.Role;

            LoadUsersList();
        }

        private void LoadUsersList()
        {
            LoadData.Load(GetSqlCommandText(SortingValues.None), ref UsersList);
        }

        private string GetSqlCommandText(SortingValues sortingValue)
        {
            switch (sortingValue)
            {
                case SortingValues.Role:
                    return $@"SELECT [Role] AS Должность, [FIO] AS ФИО, [Login] AS Логин, [Password] AS Пароль FROM [User] WHERE [Role] = '{Role}'";
                case SortingValues.FIO:
                    return $@"SELECT [Role] AS Должность, [FIO] AS ФИО, [Login] AS Логин, [Password] AS Пароль FROM [User] WHERE [FIO] = '{SortFIO.Text}'";
                case SortingValues.Login:
                    return $@"SELECT [Role] AS Должность, [FIO] AS ФИО, [Login] AS Логин, [Password] AS Пароль FROM [User] WHERE [Login] = '{SortLogin.Text}'";
                default:
                    return $@"SELECT [Role] AS Должность, [FIO] AS ФИО, [Login] AS Логин, [Password] AS Пароль FROM [User]";
            }
        }
        
        private void Role_OnChecked(object sender, RoutedEventArgs e)
        {
            SortingValue = SortingValues.Role;
            ChangeSortingComponentsVisibility();
        }

        private void FIO_OnChecked(object sender, RoutedEventArgs e)
        {
            SortingValue = SortingValues.FIO;
            ChangeSortingComponentsVisibility();
        }

        private void Login_OnChecked(object sender, RoutedEventArgs e)
        {
            SortingValue = SortingValues.Login;
            ChangeSortingComponentsVisibility();
        }

        private void None_OnChecked(object sender, RoutedEventArgs e)
        {
            SortingValue = SortingValues.None;
            ChangeSortingComponentsVisibility();
        }

        private void SortRole_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = SortRole.SelectedValue as ComboBoxItem;
            TextBlock content = item?.Content as TextBlock;
            Role = content?.Text ?? "Consumer";
        }

        private void SortButton_OnClick(object sender, RoutedEventArgs e)
        {
            return;
            //LoadUsersList();
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataRowView item = UsersList.SelectedItem as DataRowView;
            string loginToRemove = item?.Row["Логин"].ToString();
            item?.Delete();
            if (loginToRemove != string.Empty && loginToRemove != null)
                RemoveFromDB.Remove(loginToRemove);
        }

        private void ChangeSortingComponentsVisibility()
        {
            switch (SortingValue)
            {
                case SortingValues.Role:
                    SortRole.Visibility = Visibility.Visible;
                    SortLogin.Visibility = Visibility.Collapsed;
                    SortFIO.Visibility = Visibility.Collapsed;
                    break;
                case SortingValues.Login:
                    SortRole.Visibility = Visibility.Collapsed;
                    SortLogin.Visibility = Visibility.Visible;
                    SortFIO.Visibility = Visibility.Collapsed;
                    break;
                case SortingValues.FIO:
                    SortRole.Visibility = Visibility.Collapsed;
                    SortLogin.Visibility = Visibility.Collapsed;
                    SortFIO.Visibility = Visibility.Visible;
                    break;
                default:
                    SortRole.Visibility = Visibility.Collapsed;
                    SortLogin.Visibility = Visibility.Collapsed;
                    SortFIO.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private enum SortingValues
        {
            None,
            Role,
            FIO,
            Login
        }
    }
}