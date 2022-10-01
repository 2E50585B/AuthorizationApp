using AuthorizationApp.Converter;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            if (new ImageConverter().
                Convert(GetImageData(), typeof(ImageSource), null, null) is ImageSource imageSource)
                CustomerPhoto.Source = imageSource;
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
                    CommandText = $@"SELECT [Photo] FROM [User] WHERE [Login] = '{Customer.Login}'"
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

        private void ButtonPage2_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
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