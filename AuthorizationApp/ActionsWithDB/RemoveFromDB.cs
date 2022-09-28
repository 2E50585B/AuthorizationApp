using System.Data.SqlClient;

namespace AuthorizationApp.ActionsWithDB
{
    internal static class RemoveFromDB
    {
        internal static void Remove(string login)
        {
            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"DELETE FROM [User] WHERE [Login] = '{login}'"
                };

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}