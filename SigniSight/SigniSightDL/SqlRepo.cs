using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SigniSightModel;

namespace SigniSightDL
{
    public class SqlRepo : IRepo
    {
        private readonly string connectionString;

        public SqlRepo(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public User AddUser(User userToAdd)
        {
            string commandString = "INSERT INTO USERS (UserName, Password, AccountType) VALUES (@Username, @Password, @accountType)";
            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(commandString, connection);



            command.Parameters.AddWithValue("@Username", userToAdd.Username);
            command.Parameters.AddWithValue("@Password", userToAdd.Password);
            command.Parameters.AddWithValue("@accountType", userToAdd.AccountType);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return userToAdd;
        }


        public List<User> GetAllUsers()
        {
            string commandString = "SELECT * FROM USERS";

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new SqlCommand(commandString, connection);
            IDataAdapter adapter = new SqlDataAdapter(command);
            DataSet data = new();
            connection.Open();
            adapter.Fill(data);
            connection.Close();

            var users = new List<User>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                users.Add(new User
                {
                    Username = (string)row[1],
                    Password = (string)row[2],
                    AccountType = (string)row[3]
                });
            }

            return users;
        }
    }
}