using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace LoggerTest.Utility
{
    public class DataHelper
    {
        public string ConnectionString
        {
            get;
            private set;
        }

        public DataHelper()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Logger"].ConnectionString;
        }

        public void EmptyLogTable()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "TRUNCATE TABLE LOG";
                var command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        public bool IsLogEmpty()
        {
            bool result = true;

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM LOG";
                var command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = false;
                        break;
                    }
                }

            }
            return result;
        }
    }
}
