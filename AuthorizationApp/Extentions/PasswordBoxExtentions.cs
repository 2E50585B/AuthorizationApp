using System.Reflection;
using System.Windows.Controls;

namespace AuthorizationApp.Extentions
{
    /// <summary>
    /// Предоставляет методы-расширения для PasswordBox
    /// </summary>
    public static class PasswordBoxExtentions
    {
        /// <summary>
        /// Выделяет определённую область текста от <paramref name="start"/> на расстояние <paramref name="length"/>
        /// </summary>
        /// <param name="start">Позиция, откуда начнётся выделение текста</param>
        /// <param name="length">Определяет длину выделения текста</param>
        /// <remarks><b>Значения параметров должны быть больше или равны 0</b></remarks>
        /// <exception cref="TargetInvocationException"></exception>
        public static void SetSelection(this PasswordBox passwordBox, int start, int length = 0)
        {
            passwordBox.
                GetType().
                GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).
                Invoke(passwordBox, new object[] { start, length });
        }
    }
}