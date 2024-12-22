using System;
using System.Data.SqlClient;

namespace EasyTalk
{
    public class PersonalData
    {
        public static int ID { get; set; }
        public static string First_name { get; set; }
        public static string Last_name { get; set; }
        public static string Middle_name { get; set; }
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static string Address { get; set; }
        public static string Phone { get; set; }
        public static string Description { get; set; }
        public static string Email { get; set; }
        public static string Date_of_Birth { get; set; }
        public static string created_at { get; set; }

        public bool SetPersonalData(string identifier, string password)
        {
            var db = new DataBase();

            // Шифруємо пароль перед виконанням запиту
            string encryptedPassword = DataSecurity.EncryptPassword(password);

            string sqlExpression = @"
        SELECT TOP 1 *
        FROM (
            SELECT 
                [ID], [Login], [Password], [First_name], [Last_name], [Middle_name], 
                [Description], NULL AS Address, NULL AS Phone, NULL AS Email, NULL AS Date_of_Birth, NULL AS created_at
            FROM Admins
            UNION ALL
            SELECT 
                [ID], [Login], [Password], [First_name], [Last_name], [Middle_name], 
                NULL AS Description, [Address], [Phone], [Email], [Date_of_Birth], [created_at]
            FROM Clients
        ) AS Users
        WHERE 
            ([Login] = @Identifier OR [Phone] = @Identifier) AND [Password] = @Password";

            using (SqlConnection connection = new SqlConnection(db.StringCon))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                {
                    command.Parameters.AddWithValue("@Identifier", identifier);
                    command.Parameters.AddWithValue("@Password", encryptedPassword); // Використовуємо зашифрований пароль

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            ID = (int)reader["ID"];
                            Login = reader["Login"].ToString();
                            Password = DataSecurity.DecryptPassword(reader["Password"].ToString()); // Дешифруємо пароль
                            First_name = reader["First_name"].ToString();
                            Last_name = reader["Last_name"].ToString();
                            Middle_name = reader["Middle_name"].ToString();
                            Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null;
                            Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null;
                            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null;
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null;

                            // Перетворення дати народження в DateTime, якщо вона не є null
                            if (reader["Date_of_Birth"] != DBNull.Value)
                            {
                                DateTime dateOfBirth = Convert.ToDateTime(reader["Date_of_Birth"]);
                                Date_of_Birth = dateOfBirth.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                Date_of_Birth = null;
                            }

                            created_at = reader["created_at"] != DBNull.Value ? reader["created_at"].ToString() : null;

                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
