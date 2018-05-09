using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Toramp.Model
{
    class DbToramp
    {
        private string connectionString = null;
        private SqlConnection connection = null;

        public DbToramp()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "301-12";
            builder.InitialCatalog = "Toramp";
            builder.IntegratedSecurity = true;

            connectionString = builder.ConnectionString;
            connection = new SqlConnection(connectionString);
        }
        public void Dispose()
        {
            connection.Close();
        }


        public List<Ganre> GetGanres()
        {
            string commandText = "SELECT * FROM Ganres";
            List<Ganre> ganreList = new List<Ganre>();

            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = commandText;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ganreList.Add(new Ganre()
                    {
                        Name = (string)reader["Name"]
                    });
                }

                reader.Close();

            }
            finally
            {
                connection.Close();
            }

            return ganreList;
        }


        public List<Serial> GetSerials()
        {
            string commandText = "SELECT * FROM Serials";
            List<Serial> serilsList = new List<Serial>();

            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = commandText;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string img;
                    if(reader["Image"] is DBNull)
                    {
                        img = null;
                    }
                    else
                    {
                        img = (string)reader["Image"];
                    }
                    serilsList.Add(new Serial()
                    {
                        Description = (string)reader["Description"],
                        Name = (string)reader["Name"],
                        Id = (int)reader["Id"],
                        Image = img,
                        YearStart = (int)reader["YearStart"]
                    });
                }

                reader.Close();

            }
            finally
            {
                connection.Close();
            }

            return serilsList;
        }


    }
}
