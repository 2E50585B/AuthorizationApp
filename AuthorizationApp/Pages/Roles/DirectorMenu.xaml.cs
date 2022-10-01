using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AuthorizationApp.ActionsWithDB;
using AuthorizationApp.Converter;

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
            if (new ImageConverter().Convert(GetImageData(), typeof(ImageSource), null, null) is ImageSource imageSource)
                DirectorPhoto.Source = imageSource;

            LoadUsersList();
        }

        private byte[] GetImageData()
        {
            byte[] photoData = null;
            string connectionString = App.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT [Photo] FROM [User] WHERE [Login] = '{Director.Login}'"
                };

                SqlDataReader dataReader = command.ExecuteReader();

                try
                {
                    while (dataReader.Read())
                        photoData = (byte[])dataReader["Photo"];
                }
                catch (InvalidCastException)
                {
                    photoData = null;
                }
                finally
                {
                    dataReader.Close();
                }

                connection.Close();
                return photoData;
            }
        }

        private void LoadUsersList() => LoadData.Load(GetSqlCommandText(SortingValues.None), ref UsersList);

        private string GetSqlCommandText(SortingValues sortingValue) =>
            string.Concat(@"SELECT [Role] AS Должность, [FIO] AS ФИО, [Login] AS Логин, [Password] AS Пароль FROM [User] ", GetCondition(sortingValue));

        private string GetCondition(SortingValues sortingValues)
        {
            switch (sortingValues)
            {
                case SortingValues.None:
                    goto default;
                case SortingValues.Role:
                    return $"WHERE [Role] = '{Role}'";
                case SortingValues.FIO:
                    return $"WHERE [FIO] = '{SortFIO.Text}'";
                case SortingValues.Login:
                    return $"WHERE [Login] = '{SortLogin.Text}'";
                default:
                    return $"WHERE [Login] != '{Director.Login}'";
            }
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton item)
            {
                switch (item.Content)
                {
                    case "Все":
                        goto default;
                    case "Должность":
                        SortingValue = SortingValues.Role;
                        break;
                    case "ФИО":
                        SortingValue = SortingValues.FIO;
                        break;
                    case "Логин":
                        SortingValue = SortingValues.Login;
                        break;
                    default:
                        SortingValue = SortingValues.None;
                        break;
                }
                ChangeSortingComponentsVisibility();
            }
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
            if (UsersList.SelectedItem is DataRowView item)
            {
                string loginToRemove = item.Row["Логин"].ToString();
                string message =
                    "Вы уверены, что хотите удалить этого пользователя?\n"
                    + $"\nДолжность:\t{item.Row["Должность"]}\nЛогин:\t{loginToRemove}\nФИО:\t{item.Row["ФИО"]}"
                    + "\n\nОтменить это действие будет невозможно!";

                if (MessageBox.Show(message, $"Удалить пользователя {loginToRemove}?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    item.Delete();
                    if (loginToRemove != string.Empty && loginToRemove != null)
                        RemoveFromDB.Remove(loginToRemove);
                }
            }
        }

        private void ChangeSortingComponentsVisibility()
        {
            switch (SortingValue)
            {
                case SortingValues.None:
                    goto default;
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