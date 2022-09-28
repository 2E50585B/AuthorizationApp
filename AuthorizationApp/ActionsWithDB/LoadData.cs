using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace AuthorizationApp.ActionsWithDB
{
    internal static class LoadData
    {
        internal static void Load(string sqlCommandText, ref DataGrid dataGrid)
        {
            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                DataTable dataTable = new DataTable();
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = sqlCommandText
                };

                _ = new SqlDataAdapter(command).Fill(dataTable);

                dataGrid.ItemsSource = dataTable.DefaultView;
                connection.Close();
            }
        }
    }
}