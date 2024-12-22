using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EasyTalk
{
    public class DataBase
    {
        public string StringCon = @"Data Source=DESKTOP-S43BRP2;Initial Catalog=CellularCommunication;Integrated Security=True";

        // Словник для перекладу колонок
        private Dictionary<string, string> columnTranslations = new Dictionary<string, string>
        {
            { "Login", "Логін" },
            { "Password", "Пароль" },
            { "Last_name", "Прізвище" },
            { "First_name", "Ім'я" },
            { "Middle_name", "По батькові" },
            { "Description", "Опис" },
            { "OperatorName", "Назва оператора" },
            { "OperatorCode", "Код оператора" },
            { "NumberCount", "Кількість абонентів" },
            { "Country", "Країна" },
            { "Phone", "Номер телефону" },
            { "Address", "Адреса" },
            { "Email", "Електронна пошта" },
            { "Date_of_Birth", "Дата народження" },
            { "created_at", "Дата реєстрації" },
            { "Operator_ID", "ID Оператора зв'язку" },
            { "Client_ID", "ID Абонента" },
            { "Tariff_ID", "ID Тарифного плану" },
            { "Status", "Статус" },
            { "Installation_Date", "Дата підключення" },
            { "Debt", "Заборгованність" },
            { "Plan_Name", "Назва тарифного плану" },
            { "Cost", "Вартість тарифного плану" },
            { "Data_Limit", "Мобільний інтернет (Гб)" },
            { "Call_Minutes", "Хвилини на дзвінки" },
            { "SMS_Limit", "Кількість SMS" },
            { "Amount", "Сума" },
            { "Transaction_date", "Дата транзакції" },
            { "Transaction_type", "Тип транзакції" }
        };

        // Метод для отримання даних із таблиці
        public DataTable GetTable(string tableName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();
                    string query = $"SELECT * FROM {tableName}";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable table = new DataTable();
                    DataGridView dataGridView = new DataGridView();
                    adapter.Fill(table);

                    // Переклад назв колонок
                    foreach (DataColumn column in table.Columns)
                    {
                        if (columnTranslations.ContainsKey(column.ColumnName))
                        {
                            column.ColumnName = columnTranslations[column.ColumnName];
                        }
                    }
                    return table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка отримання даних: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void FillUpdateAdminsForm(int ID, Guna.UI2.WinForms.Guna2TextBox Login,
            Guna.UI2.WinForms.Guna2TextBox Password, Guna.UI2.WinForms.Guna2TextBox Last_name,
            Guna.UI2.WinForms.Guna2TextBox First_name, Guna.UI2.WinForms.Guna2TextBox Middle_name,
            Guna.UI2.WinForms.Guna2TextBox Description)
        {
            using (SqlConnection connection = new SqlConnection(StringCon))
            {
                string commandString = "SELECT * FROM [dbo].[Admins] WHERE ID = @ID";
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@ID", ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        Login.Text = reader["Login"] != DBNull.Value ? reader["Login"].ToString() : "";
                        Password.Text = reader["Password"] != DBNull.Value ? reader["Password"].ToString() : "";
                        Last_name.Text = reader["Last_name"] != DBNull.Value ? reader["Last_name"].ToString() : "";
                        First_name.Text = reader["First_name"] != DBNull.Value ? reader["First_name"].ToString() : "";
                        Middle_name.Text = reader["Middle_name"] != DBNull.Value ? reader["Middle_name"].ToString() : "";
                        Description.Text = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                    }
                    else
                    {
                        MessageBox.Show("Запис із вказаним ID не знайдено.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Увага! Виникла помилка: " + ex.Message);
                }
            }
        }

        public DataTable SearchAdmins(string searchQuery)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();
                    string query = @"SELECT * FROM [dbo].[Admins]
                             WHERE Login LIKE @Search
                                OR Password LIKE @Search
                                OR First_name LIKE @Search
                                OR Last_name LIKE @Search
                                OR Middle_name LIKE @Search
                                OR Description LIKE @Search";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Search", $"%{searchQuery}%");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable results = new DataTable();
                        adapter.Fill(results);

                        // Переклад назв колонок
                        foreach (DataColumn column in results.Columns)
                        {
                            if (columnTranslations.ContainsKey(column.ColumnName))
                            {
                                column.ColumnName = columnTranslations[column.ColumnName];
                            }
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка пошуку: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable GetSortedAdmins(string sortBy)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();

                    string query = "SELECT * FROM [dbo].[Admins]";
                    switch (sortBy)
                    {
                        case "За логіном (А-Я)":
                            query += " ORDER BY Login ASC";
                            break;
                        case "За логіном (Я-А)":
                            query += " ORDER BY Login DESC";
                            break;
                        case "За паролем (А-Я)":
                            query += " ORDER BY Password ASC";
                            break;
                        case "За паролем (Я-А)":
                            query += " ORDER BY Password DESC";
                            break;
                        case "За ім'ям (А-Я)":
                            query += " ORDER BY First_name ASC";
                            break;
                        case "За ім'ям (Я-А)":
                            query += " ORDER BY First_name DESC";
                            break;
                        case "За прізвищем (А-Я)":
                            query += " ORDER BY Last_name ASC";
                            break;
                        case "За прізвищем (Я-А)":
                            query += " ORDER BY Last_name DESC";
                            break;
                        default:
                            query += " ORDER BY ID ASC";
                            break;
                    }

                    Console.WriteLine($"Generated Query: {query}");
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable results = new DataTable();
                    adapter.Fill(results);

                    // Переклад назв колонок
                    foreach (DataColumn column in results.Columns)
                    {
                        if (columnTranslations.ContainsKey(column.ColumnName))
                        {
                            column.ColumnName = columnTranslations[column.ColumnName];
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка сортування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void FillUpdateCellOpersForm(int ID, Guna.UI2.WinForms.Guna2TextBox operatorname,
            Guna.UI2.WinForms.Guna2TextBox operatorcode, Guna.UI2.WinForms.Guna2TextBox numbercount,
            Guna.UI2.WinForms.Guna2TextBox country)
        {
            using (SqlConnection connection = new SqlConnection(StringCon))
            {
                string commandString = "SELECT * FROM [dbo].[CellularOperators] WHERE ID = @ID";
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@ID", ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        operatorname.Text = reader["OperatorName"] != DBNull.Value ? reader["OperatorName"].ToString() : "";
                        operatorcode.Text = reader["OperatorCode"] != DBNull.Value ? reader["OperatorCode"].ToString() : "";
                        numbercount.Text = reader["NumberCount"] != DBNull.Value ? reader["NumberCount"].ToString() : "";
                        country.Text = reader["Country"] != DBNull.Value ? reader["Country"].ToString() : "";
                    }
                    else
                    {
                        MessageBox.Show("Запис із вказаним ID не знайдено.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Увага! Виникла помилка: " + ex.Message);
                }
            }
        }

        public DataTable SearchCellOpers(string searchQuery)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();
                    string query = @"SELECT * FROM [dbo].[CellularOperators]
                             WHERE OperatorName LIKE @Search
                                OR OperatorCode LIKE @Search
                                OR NumberCount LIKE @Search
                                OR Country LIKE @Search";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Search", $"%{searchQuery}%");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable results = new DataTable();
                        adapter.Fill(results);

                        // Переклад назв колонок
                        foreach (DataColumn column in results.Columns)
                        {
                            if (columnTranslations.ContainsKey(column.ColumnName))
                            {
                                column.ColumnName = columnTranslations[column.ColumnName];
                            }
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка пошуку: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable GetSortedCellOpers(string sortBy)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();

                    string query = "SELECT * FROM [dbo].[CellularOperators]";
                    switch (sortBy)
                    {
                        case "За назвою (А-Я)":
                            query += " ORDER BY OperatorName ASC";
                            break;
                        case "За назвою (Я-А)":
                            query += " ORDER BY OperatorName DESC";
                            break;
                        case "За кодом (Від меншого)":
                            query += " ORDER BY OperatorCode ASC";
                            break;
                        case "За кодом (Від більшого)":
                            query += " ORDER BY OperatorCode DESC";
                            break;
                        default:
                            query += " ORDER BY ID ASC";
                            break;
                    }

                    Console.WriteLine($"Generated Query: {query}");
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable results = new DataTable();
                    adapter.Fill(results);

                    // Переклад назв колонок
                    foreach (DataColumn column in results.Columns)
                    {
                        if (columnTranslations.ContainsKey(column.ColumnName))
                        {
                            column.ColumnName = columnTranslations[column.ColumnName];
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка сортування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void FillUpdateClientsForm(int ID, Guna.UI2.WinForms.Guna2TextBox Last_name,
            Guna.UI2.WinForms.Guna2TextBox First_name, Guna.UI2.WinForms.Guna2TextBox Middle_name,
            Guna.UI2.WinForms.Guna2TextBox Phone, Guna.UI2.WinForms.Guna2TextBox Login,
            Guna.UI2.WinForms.Guna2TextBox Password, Guna.UI2.WinForms.Guna2TextBox Address,
            Guna.UI2.WinForms.Guna2TextBox Email, Guna.UI2.WinForms.Guna2TextBox Date_of_Birth)
        {
            using (SqlConnection connection = new SqlConnection(StringCon))
            {
                string commandString = "SELECT * FROM [dbo].[Clients] WHERE ID = @ID";
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@ID", ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        Last_name.Text = reader["Last_name"] != DBNull.Value ? reader["Last_name"].ToString() : "";
                        First_name.Text = reader["First_name"] != DBNull.Value ? reader["First_name"].ToString() : "";
                        Middle_name.Text = reader["Middle_name"] != DBNull.Value ? reader["Middle_name"].ToString() : "";
                        Phone.Text = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "";
                        Login.Text = reader["Login"] != DBNull.Value ? reader["Login"].ToString() : "";
                        Password.Text = reader["Password"] != DBNull.Value ? reader["Password"].ToString() : "";
                        Address.Text = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";
                        Email.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                    }
                    else
                    {
                        MessageBox.Show("Запис із вказаним ID не знайдено.");
                    }

                    if (reader["Date_of_Birth"] != DBNull.Value)
                    {
                        DateTime dateOfBirth = Convert.ToDateTime(reader["Date_of_Birth"]);
                        Date_of_Birth.Text = dateOfBirth.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        Date_of_Birth.Text = string.Empty;
                    }

                    string createdAt = reader["created_at"] != DBNull.Value ? reader["created_at"].ToString() : DateTime.Now.ToString();

                    // Логіка для збереження або відображення createdAt, якщо потрібно
                    Console.WriteLine($"Особистий кабінет клієнта був створений: {createdAt}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Увага! Виникла помилка: " + ex.Message);
                }
            }
        }

        public DataTable SearchClients(string searchQuery)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();
                    string query = @"SELECT * FROM [dbo].[Clients]
                             WHERE Last_name LIKE @Search
                                OR First_name LIKE @Search
                                OR Middle_name LIKE @Search
                                OR Phone LIKE @Search
                                OR Login LIKE @Search
                                OR Password LIKE @Search
                                OR Address LIKE @Search
                                OR Email LIKE @Search
                                OR Date_of_Birth LIKE @Search
                                OR created_at LIKE @Search
                                OR Operator_ID LIKE @Search";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Search", $"%{searchQuery}%");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable results = new DataTable();
                        adapter.Fill(results);

                        // Переклад назв колонок
                        foreach (DataColumn column in results.Columns)
                        {
                            if (columnTranslations.ContainsKey(column.ColumnName))
                            {
                                column.ColumnName = columnTranslations[column.ColumnName];
                            }
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка пошуку: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable GetSortedClients(string sortBy)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();

                    string query = "SELECT * FROM [dbo].[Clients]";
                    switch (sortBy)
                    {
                        case "За логіном (А-Я)":
                            query += " ORDER BY Login ASC";
                            break;
                        case "За логіном (Я-А)":
                            query += " ORDER BY Login DESC";
                            break;
                        case "За паролем (А-Я)":
                            query += " ORDER BY Password ASC";
                            break;
                        case "За паролем (Я-А)":
                            query += " ORDER BY Password DESC";
                            break;
                        case "За ім'ям (А-Я)":
                            query += " ORDER BY First_name ASC";
                            break;
                        case "За ім'ям (Я-А)":
                            query += " ORDER BY First_name DESC";
                            break;
                        case "За прізвищем (А-Я)":
                            query += " ORDER BY Last_name ASC";
                            break;
                        case "За прізвищем (Я-А)":
                            query += " ORDER BY Last_name DESC";
                            break;
                        default:
                            query += " ORDER BY ID";
                            break;
                    }

                    Console.WriteLine($"Generated Query: {query}");
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable results = new DataTable();
                    adapter.Fill(results);

                    // Переклад назв колонок
                    foreach (DataColumn column in results.Columns)
                    {
                        if (columnTranslations.ContainsKey(column.ColumnName))
                        {
                            column.ColumnName = columnTranslations[column.ColumnName];
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка сортування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void FillUpdateConnectsForm(int ID, Guna.UI2.WinForms.Guna2TextBox clientid,
            Guna.UI2.WinForms.Guna2TextBox phone, Guna.UI2.WinForms.Guna2TextBox tariffid,
            Guna.UI2.WinForms.Guna2TextBox condate, Guna.UI2.WinForms.Guna2TextBox debt,
            Guna.UI2.WinForms.Guna2TextBox status)
        {
            using (SqlConnection connection = new SqlConnection(StringCon))
            {
                string commandString = "SELECT * FROM [dbo].[Connections] WHERE ID = @ID";
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@ID", ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        clientid.Text = reader["Client_ID"] != DBNull.Value ? reader["Client_ID"].ToString() : "";
                        phone.Text = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "";
                        tariffid.Text = reader["Tariff_ID"] != DBNull.Value ? reader["Tariff_ID"].ToString() : "";

                        // Якщо заборгованість є, відобразити як ціле число
                        if (reader["Debt"] != DBNull.Value)
                        {
                            int debtValue = Convert.ToInt32(reader["Debt"]);
                            debt.Text = debtValue.ToString();
                        }
                        else
                        {
                            debt.Text = "0"; // Якщо заборгованості немає, поставимо 0
                        }

                        status.Text = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "";

                        if (reader["Installation_Date"] != DBNull.Value)
                        {
                            DateTime installationDate = Convert.ToDateTime(reader["Installation_Date"]);
                            condate.Text = installationDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            condate.Text = string.Empty;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Запис із вказаним ID не знайдено.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Увага! Виникла помилка: " + ex.Message);
                }
            }
        }

        public DataTable SearchConnects(string searchQuery)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();
                    string query = @"SELECT * FROM [dbo].[Connections]
                             WHERE Client_ID LIKE @Search
                                OR Phone LIKE @Search
                                OR Tariff_ID LIKE @Search
                                OR Installation_Date LIKE @Search
                                OR Debt LIKE @Search
                                OR Status LIKE @Search";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Search", $"%{searchQuery}%");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable results = new DataTable();
                        adapter.Fill(results);

                        // Переклад назв колонок
                        foreach (DataColumn column in results.Columns)
                        {
                            if (columnTranslations.ContainsKey(column.ColumnName))
                            {
                                column.ColumnName = columnTranslations[column.ColumnName];
                            }
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка пошуку: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable GetSortedConnects(string sortBy)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();

                    string query = "SELECT * FROM [dbo].[Connections]";
                    switch (sortBy)
                    {
                        case "За датою підключення (Старою)":
                            query += " ORDER BY Installation_Date ASC";
                            break;
                        case "За датою підключення (Новою)":
                            query += " ORDER BY Installation_Date DESC";
                            break;
                        case "За сумою боргу (Від меншого)":
                            query += " ORDER BY Debt ASC";
                            break;
                        case "За сумою боргу (Від більшого)":
                            query += " ORDER BY Debt DESC";
                            break;
                        default:
                            query += " ORDER BY ID ASC";
                            break;
                    }

                    Console.WriteLine($"Generated Query: {query}");
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable results = new DataTable();
                    adapter.Fill(results);

                    // Переклад назв колонок
                    foreach (DataColumn column in results.Columns)
                    {
                        if (columnTranslations.ContainsKey(column.ColumnName))
                        {
                            column.ColumnName = columnTranslations[column.ColumnName];
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка сортування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void FillUpdateTariffsForm(int ID, Guna.UI2.WinForms.Guna2TextBox planname,
            Guna.UI2.WinForms.Guna2TextBox plancost, Guna.UI2.WinForms.Guna2TextBox datalimit,
            Guna.UI2.WinForms.Guna2TextBox callmin, Guna.UI2.WinForms.Guna2TextBox smslimit,
            Guna.UI2.WinForms.Guna2TextBox descript, Guna.UI2.WinForms.Guna2TextBox operatorid)
        {
            using (SqlConnection connection = new SqlConnection(StringCon))
            {
                string commandString = "SELECT * FROM [dbo].[TariffPlans] WHERE ID = @ID";
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@ID", ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        planname.Text = reader["Plan_Name"] != DBNull.Value ? reader["Plan_Name"].ToString() : "";

                        // Конвертація Cost у ціле число
                        if (reader["Cost"] != DBNull.Value)
                        {
                            int costValue = Convert.ToInt32(reader["Cost"]);
                            plancost.Text = costValue.ToString();
                        }
                        else
                        {
                            plancost.Text = "0"; // Якщо значення немає, показуємо 0
                        }

                        datalimit.Text = reader["Data_Limit"] != DBNull.Value ? reader["Data_Limit"].ToString() : "";
                        callmin.Text = reader["Call_Minutes"] != DBNull.Value ? reader["Call_Minutes"].ToString() : "";
                        smslimit.Text = reader["SMS_Limit"] != DBNull.Value ? reader["SMS_Limit"].ToString() : "";
                        descript.Text = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                        operatorid.Text = reader["Operator_ID"] != DBNull.Value ? reader["Operator_ID"].ToString() : "";
                    }
                    else
                    {
                        MessageBox.Show("Запис із вказаним ID не знайдено.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Увага! Виникла помилка: " + ex.Message);
                }
            }
        }

        public DataTable SearchTariffs(string searchQuery)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();
                    string query = @"SELECT * FROM [dbo].[TariffPlans]
                             WHERE Plan_Name LIKE @Search
                                OR Cost LIKE @Search
                                OR Data_Limit LIKE @Search
                                OR Call_Minutes LIKE @Search
                                OR SMS_Limit LIKE @Search
                                OR Description LIKE @Search
                                OR Operator_ID LIKE @Search";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Search", $"%{searchQuery}%");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable results = new DataTable();
                        adapter.Fill(results);

                        // Переклад назв колонок
                        foreach (DataColumn column in results.Columns)
                        {
                            if (columnTranslations.ContainsKey(column.ColumnName))
                            {
                                column.ColumnName = columnTranslations[column.ColumnName];
                            }
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка пошуку: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable GetSortedTariffs(string sortBy)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();

                    string query = "SELECT * FROM [dbo].[TariffPlans]";
                    switch (sortBy)
                    {
                        case "За назвою тарифного плану (А-Я)":
                            query += " ORDER BY Plan_Name ASC";
                            break;
                        case "За назвою тарифного плану (Я-А)":
                            query += " ORDER BY Plan_Name DESC";
                            break;
                        case "За вартістю тарифного плану (Від меншого)":
                            query += " ORDER BY Cost ASC";
                            break;
                        case "За вартістю тарифного плану (Від більшого)":
                            query += " ORDER BY Cost DESC";
                            break;
                        case "За кількістю Гб інтернету (Від меншого)":
                            query += " ORDER BY Data_Limit ASC";
                            break;
                        case "За кількістю Гб інтернету (Від більшого)":
                            query += " ORDER BY Data_Limit DESC";
                            break;
                        case "За кількістю хвилин (Від меншого)":
                            query += " ORDER BY Call_Minutes ASC";
                            break;
                        case "За кількістю хвилин (Від більшого)":
                            query += " ORDER BY Call_Minutes DESC";
                            break;
                        case "За кількістю SMS (Від меншого)":
                            query += " ORDER BY SMS_Limit ASC";
                            break;
                        case "За кількістю SMS (Від більшого)":
                            query += " ORDER BY SMS_Limit DESC";
                            break;
                        default:
                            query += " ORDER BY ID ASC";
                            break;
                    }

                    Console.WriteLine($"Generated Query: {query}");
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable results = new DataTable();
                    adapter.Fill(results);

                    // Переклад назв колонок
                    foreach (DataColumn column in results.Columns)
                    {
                        if (columnTranslations.ContainsKey(column.ColumnName))
                        {
                            column.ColumnName = columnTranslations[column.ColumnName];
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка сортування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void FillUpdateTransactsForm(int ID, Guna.UI2.WinForms.Guna2TextBox userid,
            Guna.UI2.WinForms.Guna2TextBox amount, Guna.UI2.WinForms.Guna2TextBox transactdate,
            Guna.UI2.WinForms.Guna2TextBox transacttype, Guna.UI2.WinForms.Guna2TextBox status)
        {
            using (SqlConnection connection = new SqlConnection(StringCon))
            {
                string commandString = "SELECT * FROM [dbo].[Transactions] WHERE ID = @ID";
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@ID", ID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        userid.Text = reader["Client_ID"] != DBNull.Value ? reader["Client_ID"].ToString() : "";

                        // Конвертуємо amount у ціле число
                        if (reader["Amount"] != DBNull.Value)
                        {
                            int amountValue = Convert.ToInt32(reader["Amount"]);
                            amount.Text = amountValue.ToString();
                        }
                        else
                        {
                            amount.Text = "0"; // Якщо значення немає, відображається 0
                        }

                        transacttype.Text = reader["Transaction_type"] != DBNull.Value ? reader["Transaction_type"].ToString() : "";
                        status.Text = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "";

                        if (reader["Transaction_date"] != DBNull.Value)
                        {
                            // Форматування дати
                            DateTime transact_date = Convert.ToDateTime(reader["Transaction_date"]);
                            transactdate.Text = transact_date.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            transactdate.Text = string.Empty;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Запис із вказаним ID не знайдено.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Увага! Виникла помилка: " + ex.Message);
                }
            }
        }

        public DataTable SearchTransacts(string searchQuery)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();
                    string query = @"SELECT * FROM [dbo].[Transactions]
                             WHERE Client_ID LIKE @Search
                                OR Amount LIKE @Search
                                OR Transaction_date LIKE @Search
                                OR Transaction_type LIKE @Search
                                OR Status LIKE @Search";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Search", $"%{searchQuery}%");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable results = new DataTable();
                        adapter.Fill(results);

                        // Переклад назв колонок
                        foreach (DataColumn column in results.Columns)
                        {
                            if (columnTranslations.ContainsKey(column.ColumnName))
                            {
                                column.ColumnName = columnTranslations[column.ColumnName];
                            }
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка пошуку: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable GetSortedTransacts(string sortBy)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();

                    string query = "SELECT * FROM [dbo].[Transactions]";
                    switch (sortBy)
                    {
                        case "За сумою рахунку (Від меншого)":
                            query += " ORDER BY Amount ASC";
                            break;
                        case "За сумою рахунку (Від більшого)":
                            query += " ORDER BY Amount DESC";
                            break;
                        case "За датою транзакції (Старою)":
                            query += " ORDER BY Transaction_date ASC";
                            break;
                        case "За датою транзакції (Новою)":
                            query += " ORDER BY Transaction_date DESC";
                            break;
                        case "За типом транзакції (Від оплаченого)":
                            query += " ORDER BY Transaction_type ASC";
                            break;
                        case "За типом транзакції (Від не оплаченого)":
                            query += " ORDER BY Transaction_type DESC";
                            break;
                        case "За статусом транзакції (Скасовано)":
                            query += " ORDER BY Status ASC";
                            break;
                        case "За статусом транзакції (Успішно)":
                            query += " ORDER BY Status DESC";
                            break;
                        default:
                            query += " ORDER BY ID ASC";
                            break;
                    }

                    Console.WriteLine($"Generated Query: {query}");
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable results = new DataTable();
                    adapter.Fill(results);

                    // Переклад назв колонок
                    foreach (DataColumn column in results.Columns)
                    {
                        if (columnTranslations.ContainsKey(column.ColumnName))
                        {
                            column.ColumnName = columnTranslations[column.ColumnName];
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка сортування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Метод для виконання SQL-команд (Insert, Update, Delete)
        public bool ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (parameters != null)
                        {
                            // Шифруємо пароль перед збереженням
                            foreach (var param in parameters)
                            {
                                if (param.ParameterName == "@Password" && param.Value is string password)
                                {
                                    param.Value = DataSecurity.EncryptPassword(password); // Шифруємо пароль
                                }
                            }

                            cmd.Parameters.AddRange(parameters);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка виконання запиту: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        // Метод для виконання SQL-запитів, які повертають результат
        public DataTable ExecuteQueryWithResult(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StringCon))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка отримання результату: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
