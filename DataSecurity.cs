using System;
using System.Text.RegularExpressions;

namespace EasyTalk
{
    public static class DataSecurity
    {
        // Перевірка логіна
        public static bool ValidateLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return false;

            string pattern = @"^[a-zA-Z0-9]{5,20}$"; // Тільки латинські літери та цифри, 5-20 символів
            return Regex.IsMatch(login, pattern);
        }

        // Перевірка пароля
        public static bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,20}$";
            // Тільки латинські літери (великі/малі) та цифри, мінімум одна велика, одна мала літера та цифра
            return Regex.IsMatch(password, pattern);
        }

        // Перевірка текстів
        public static bool ValidateTextField1(string text)
        {
            string pattern = @"^[a-zA-Zа-яА-ЯёЁіІїЇєЄ'\x20-]+$"; // Тільки літери (підтримка української)
            return Regex.IsMatch(text, pattern);
        }

        // Перевірка текстів
        public static bool ValidateTextField2(string text)
        {
            string pattern = @"^[а-яА-ЯіІїЇєЄ'\x20]+$"; // Тільки українські літери та апостроф
            return Regex.IsMatch(text, pattern);
        }

        // Перевірка текстів
        public static bool ValidateTextField3(string text)
        {
            string pattern = @"^[а-яА-ЯіІїЇєЄ'.,-]+(\s[а-яА-ЯіІїЇєЄ'.,-]+)*$";
            return Regex.IsMatch(text, pattern);
        }

        // Перевірка текстів
        public static bool ValidateTextField4(string text)
        {
            string pattern = @"^[а-яА-ЯіІїЇєЄ0-9'.,-]+(\s[а-яА-ЯіІїЇєЄ0-9'.,-]+)*$";
            return Regex.IsMatch(text, pattern);
        }

        // Перевірка email
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        // Перевірка телефонного номера (без змін)
        public static bool ValidatePhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"^\d{10}$";
            return Regex.IsMatch(phone, pattern);
        }


        // Перевірка дати народження (без змін)
        public static bool ValidateDateOfBirth(string dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(dateOfBirth))
                return false;

            return DateTime.TryParse(dateOfBirth, out _);
        }

        public static string EncryptPassword(string password)
        {
            return CaesarCipher.Encrypt(password); // Шифруємо за допомогою CaesarCipher
        }

        // Дешифрування пароля
        public static string DecryptPassword(string encryptedPassword)
        {
            return CaesarCipher.Decrypt(encryptedPassword); // Дешифруємо за допомогою CaesarCipher
        }

    }
}