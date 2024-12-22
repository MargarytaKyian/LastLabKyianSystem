using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EasyTalk
{
    public static class CaesarCipher
    {
        private const int Shift = 13;

        // Шифрування тексту
        public static string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            char[] buffer = input.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                char letter = buffer[i];
                if (char.IsLetter(letter))
                {
                    char offset = char.IsUpper(letter) ? 'A' : 'a';
                    letter = (char)(((letter + Shift - offset) % 26) + offset);
                }
                buffer[i] = letter;
            }
            return new string(buffer);
        }

        // Дешифрування тексту
        public static string Decrypt(string input)
        {
            return Encrypt(input); // ROT13 є взаємним: Encrypt == Decrypt
        }
    }
}
