namespace AuthorizationApp.Extentions
{
    /// <summary>
    /// Предоставляет методы-расширения для String
    /// </summary>
    public static class StringExtentions
    {
        /// <summary>
        /// Позволяет узнать пустая строка или нет
        /// </summary>
        /// <returns>Возвращает true, если значение строки - string.Empty / null / пробелы, иначе false</returns>
        public static bool IsEmpty(this string text) => string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
    }
}