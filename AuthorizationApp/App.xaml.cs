using System.Configuration;
using System.Windows;

namespace AuthorizationApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Возвращает готовую строку подключения к Базе Данных
        /// </summary>
        public static string ConnectionString { get; private set; }

        public App()
        {
            ConnectionString = GetConnectionString();
        }

        private string GetConnectionString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Entities"].ConnectionString.ToString();

            if (connectionString.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder =
                    new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(connectionString);
                connectionString = efBuilder.ProviderConnectionString;
            }

            return connectionString;
        }
    }
}