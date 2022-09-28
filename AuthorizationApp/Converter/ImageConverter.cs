using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace AuthorizationApp.Converter
{
    public class ImageConverter : BaseConverter<ImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] imageData = (byte[])value;
            if (imageData != null)
            {
                using (MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length))
                {
                    BitmapImage glowIcon = new BitmapImage();
                    glowIcon.BeginInit();
                    glowIcon.CacheOption = BitmapCacheOption.OnLoad;
                    glowIcon.StreamSource = ms;
                    glowIcon.EndInit();
                    return glowIcon;
                }
            }
            else
                return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                BitmapSource image = (BitmapSource)value;
                if (image != null)
                    return File.ReadAllBytes(image.ToString());
                else
                    return null;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }
    }
}