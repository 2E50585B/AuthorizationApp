using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            /* Не удаётся правильно закодировать/декодировать изображение
            ImageSource source = GetPhoto();
            if (source != null)
                CustomerPhoto.Source = source;
            */
        }

        private ImageSource GetPhoto()
        {
            if (Customer.Photo == null)
                return null;
            else
            {
                try
                {
                    BitmapImage glowIcon = new BitmapImage();
                    MemoryStream ms = new MemoryStream(Customer.Photo);
                    glowIcon.BeginInit();
                    glowIcon.CacheOption = BitmapCacheOption.OnLoad;
                    glowIcon.StreamSource = ms;
                    glowIcon.EndInit();
                    return glowIcon;
                }
                catch(System.NotSupportedException ex)
                {
                    MessageBox.Show(ex.Message, "Failed to decode the image", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }

        #region ---Getting Photo From DB---
        //private ImageSource GetPhoto(string userLogin)
        //{
        //    BitmapImage glowIcon = new BitmapImage();
        //    byte[] photoData = null;
        //    string connectionString = ConfigurationManager.ConnectionStrings["Entities"].ConnectionString.ToString();

        //    if (connectionString.ToLower().StartsWith("metadata="))
        //    {
        //        System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder =
        //            new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(connectionString);
        //        connectionString = efBuilder.ProviderConnectionString;
        //    }

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        SqlCommand command = new SqlCommand
        //        {
        //            Connection = connection,
        //            CommandText = $@"SELECT [Photo] FROM [User] WHERE [Login] = '{userLogin}'"
        //        };

        //        SqlDataReader dataReader = command.ExecuteReader();

        //        try
        //        {
        //            while (dataReader.Read())
        //                photoData = (byte[])dataReader["Photo"];
        //        }
        //        catch (System.InvalidCastException)
        //        {
        //            photoData = null;
        //        }
        //        finally
        //        {
        //            dataReader.Close();
        //        {

        //        connection.Close();
        //    }

        //    if (photoData != null)
        //    {
        //        MemoryStream ms = new MemoryStream(photoData);
        //        glowIcon.BeginInit();
        //        glowIcon.CacheOption = BitmapCacheOption.OnLoad;
        //        glowIcon.StreamSource = ms;
        //        glowIcon.EndInit();
        //    }
        //    else
        //        return null;

        //    return glowIcon;
        //}
        #endregion

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