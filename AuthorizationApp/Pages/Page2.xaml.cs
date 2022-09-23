using AuthorizationApp.DialogService;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AuthorizationApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void GetPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            DefaultDialogService dialogService = new DefaultDialogService();
            if (dialogService.OpenFileDialog() == true)
            {
                UserPhoto.Source = GetImageSource(dialogService.FilePath);
            }
        }

        private ImageSource GetImageSource(string fileName)
        {
            try
            {
                BitmapImage glowIcon = new BitmapImage();
                glowIcon.BeginInit();
                glowIcon.UriSource = new System.Uri(fileName);
                glowIcon.EndInit();
                return glowIcon;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return UserPhoto.Source;
            }
        }
    }
}