using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EasyTalk
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void guna2CircleExitButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ButtonLogin_Click(object sender, EventArgs e)
        {
            // Збираємо дані з текстових полів
            var loginOrPhone = guna2TextBoxLogin1.Text.Trim(); // Поле для логіна або телефону
            var password = guna2TextBoxPassword1.Text.Trim();  // Поле для пароля

            // Перевірка на порожні поля
            if (string.IsNullOrEmpty(loginOrPhone) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Перевірка формату логіна або телефону
            if (!DataSecurity.ValidateLogin(loginOrPhone) && !DataSecurity.ValidatePhoneNumber(loginOrPhone))
            {
                MessageBox.Show("Логін повинен містити лише латинські літери та цифри, або використовуйте номер телефону у форматі 0123456789.", "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Перевірка пароля
            if (!DataSecurity.ValidatePassword(password))
            {
                MessageBox.Show("Пароль має містити від 8 до 20 символів, включаючи великі й малі літери та цифри.", "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Підключення до бази даних
                var db = new DataBase();
                using (SqlConnection connection = new SqlConnection(db.StringCon))
                {
                    // Заміна 'Users' на правильну назву таблиці
                    string query = "SELECT * FROM Clients WHERE (Login = @LoginOrPhone OR Phone = @LoginOrPhone) AND Password = @Password"; // Приклад для таблиці Clients
                    SqlCommand command = new SqlCommand(query, connection);

                    // Додаємо параметри до запиту
                    command.Parameters.AddWithValue("@LoginOrPhone", loginOrPhone);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Якщо знайдений користувач
                    if (reader.HasRows)
                    {
                        reader.Read();

                        // Збираємо дані користувача
                        string description = reader["Description"].ToString();
                        if (!string.IsNullOrEmpty(description))
                        {
                            // Відкриваємо форму адміністратора
                            var adminForm = new AdminsForm();
                            adminForm.Show();
                        }
                        else
                        {
                            // Відкриваємо форму користувача
                            var userForm = new UsersForm();
                            userForm.Show();
                        }

                        // Генерація звіту про вхід
                        var reports = new Reports();
                        reports.GenerateLoginReport();

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Користувача не знайдено, або неправильний пароль.", "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час спроби входу: {ex.Message}", "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                guna2TextBoxPassword1.UseSystemPasswordChar = false;
            }
            else
            {
                guna2TextBoxPassword1.UseSystemPasswordChar = true;
            }
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox2.Checked)
            {
                guna2TextBoxPasswordReg1.UseSystemPasswordChar = false;
                guna2TextBoxPasswordReg2.UseSystemPasswordChar = false;
            }
            else
            {
                guna2TextBoxPasswordReg1.UseSystemPasswordChar = true;
                guna2TextBoxPasswordReg2.UseSystemPasswordChar = true;
            }
        }

        private void guna2ButtonReg_Click(object sender, EventArgs e)
        {
            // Збираємо значення з полів реєстрації
            var lastName = guna2TextBoxLastName1.Text.Trim();
            var firstName = guna2TextBoxFirstName1.Text.Trim();
            var middleName = guna2TextBoxMiddleName1.Text.Trim();
            var phone = guna2TextBoxPhoneNum1.Text.Trim();
            var login = guna2TextBoxLoginReg.Text.Trim();
            var password1 = guna2TextBoxPasswordReg1.Text.Trim();
            var password2 = guna2TextBoxPasswordReg2.Text.Trim();
            var email = guna2TextBoxEmail1.Text.Trim();
            var address = guna2TextBoxAddress1.Text.Trim();
            var birthDate = guna2TextBoxDateBirth1.Text.Trim();

            // Перевірка на порожні поля
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(middleName) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(password2) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(birthDate))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Перевірка паролів
            if (password1 != password2)
            {
                MessageBox.Show("Паролі не співпадають.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!DataSecurity.ValidatePassword(password1))
            {
                MessageBox.Show("Пароль повинен містити лише латинські літери та цифри, від 8 до 20 символів, включаючи великі й малі літери та цифри.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Шифрування пароля перед збереженням у базу
            string encryptedPassword = CaesarCipher.Encrypt(password1);  // Шифруємо пароль

            // Додавання до бази даних
            try
            {
                var db = new DataBase();

                using (SqlConnection connection = new SqlConnection(db.StringCon))
                {
                    SqlCommand command = new SqlCommand("InsertClient", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Додаємо параметри до процедури
                    command.Parameters.AddWithValue("@Last_name", lastName);
                    command.Parameters.AddWithValue("@First_name", firstName);
                    command.Parameters.AddWithValue("@Middle_name", middleName);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", encryptedPassword);  // Використовуємо зашифрований пароль
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Date_of_Birth", birthDate);

                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Використовуємо SetPersonalData для автоматичного заповнення даних
                        var personalData = new PersonalData();
                        if (personalData.SetPersonalData(login, password1))
                        {
                            MessageBox.Show("Реєстрація успішна.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Генерація звіту про реєстрацію
                            var reports = new Reports();
                            reports.GenerateRegistrationReport();

                            // Відкриваємо форму користувача
                            var userForm = new UsersForm();
                            userForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Дані користувача не вдалося завантажити після реєстрації.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Сталася помилка при реєстрації. Спробуйте ще раз.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
