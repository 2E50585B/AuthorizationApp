using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AuthorizationApp.Extentions
{
    /// <summary>
    /// Предоставляет методы-расширения для компонентов Image
    /// </summary>
    /// <remarks>На данный момент только для ImageSource</remarks>
    public static class ImageExtentions
    {
        [Obsolete("Работоспособность не проверена. Используйте перегрузку с BitmapEncoder")]
        /// <summary>
        /// Сериализует изображение в набор байт
        /// </summary>
        /// <returns>Закодированное изображение в набор байт. Если сериализация не успешная, метод вернёт null</returns>
        /// <exception cref="Exception"/>
        public static byte[] Serialize(this ImageSource imageSource)
        {
            try
            {
                BitmapImage bmp = imageSource as BitmapImage;
                int height = bmp.PixelHeight;
                int width = bmp.PixelWidth;
                int stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);
                byte[] photoData = new byte[height * stride];
                bmp.CopyPixels(photoData, stride, 0);
                return photoData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось сериализовать изображение", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Сериализует изображение в набор байт при помощи BitmapEncoder
        /// </summary>
        /// <param name="encoder">Кодировщик в поток изображения</param>
        /// <returns>Закодированное изображение в набор байт. Если сериализация не успешная, метод вернёт null</returns>
        public static byte[] Serialize(this ImageSource imageSource, BitmapEncoder encoder)
        {
            byte[] bytes = null;

            if (imageSource is BitmapSource bitmapSource)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        [Obsolete("Используйте AuthorizationApp.Converter.ImageConverter()")]
        /// <summary>
        /// Десериализует изображение из MemoryStream
        /// </summary>
        /// <returns>ImageSource из потока данных. Если десериализация не успешная, метод вернёт null</returns>
        /// <exception cref="NotSupportedException"/>
        public static ImageSource Deserialize(this ImageSource imageSource, MemoryStream memoryStream)
        {
            try
            {
                BitmapImage glowIcon = imageSource as BitmapImage;
                glowIcon.BeginInit();
                glowIcon.CacheOption = BitmapCacheOption.OnLoad;
                glowIcon.StreamSource = memoryStream;
                glowIcon.EndInit();
                return glowIcon;
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show(ex.Message, "Не удалось десериализовать изображение", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}