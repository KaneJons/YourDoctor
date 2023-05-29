using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace YourDoctor
{
    internal class Generate
    {
        public static string Transliterate(string text)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
    {
        {"а", "a"}, {"б", "b"}, {"в", "v"}, {"г", "g"}, {"д", "d"}, {"е", "e"}, {"ё", "e"},
        {"ж", "zh"}, {"з", "z"}, {"и", "i"}, {"й", "y"}, {"к", "k"}, {"л", "l"}, {"м", "m"},
        {"н", "n"}, {"о", "o"}, {"п", "p"}, {"р", "r"}, {"с", "s"}, {"т", "t"}, {"у", "u"},
        {"ф", "f"}, {"х", "kh"}, {"ц", "ts"}, {"ч", "ch"}, {"ш", "sh"}, {"щ", "sch"}, {"ъ", ""},
        {"ы", "y"}, {"ь", ""}, {"э", "e"}, {"ю", "yu"}, {"я", "ya"},
        {"А", "A"}, {"Б", "B"}, {"В", "V"}, {"Г", "G"}, {"Д", "D"}, {"Е", "E"}, {"Ё", "E"},
        {"Ж", "Zh"}, {"З", "Z"}, {"И", "I"}, {"Й", "Y"}, {"К", "K"}, {"Л", "L"}, {"М", "M"},
        {"Н", "N"}, {"О", "O"}, {"П", "P"}, {"Р", "R"}, {"С", "S"}, {"Т", "T"}, {"У", "U"},
        {"Ф", "F"}, {"Х", "Kh"}, {"Ц", "Ts"}, {"Ч", "Ch"}, {"Ш", "Sh"}, {"Щ", "Sch"}, {"Ъ", ""},
        {"Ы", "Y"}, {"Ь", ""}, {"Э", "E"}, {"Ю", "Yu"}, {"Я", "Ya"}
    };

            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var c in text)
            {
                if (dict.TryGetValue(c.ToString(), out string value))
                {
                    sb.Append(value);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        protected static string GenerateRandomChars(int length)
        {
            const string chars = "!@#$%^&*()_+{}[]:;\"'<>?/\\|0123456789";
            byte[] data = new byte[length];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(chars[b % chars.Length]);
            }
            return result.ToString();
        }


        public static string Password(string slovo)
        {
            const int symbLength = 4;
            const int randomDigitsCount = 2;

            string[] dictionary = { "YAN", "GOO", "ROS", "TES", "AMA", "OPE", "FIR", "TEL", "DIS", "VIS", "MVI", "STE", "DNS", "EGD", "NFS", "ACB" };
            StringBuilder passwordBuilder = new StringBuilder();
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[symbLength + randomDigitsCount];
                rng.GetBytes(randomBytes);

                string randomString = dictionary[randomBytes[0] % dictionary.Length];
                passwordBuilder.Append(randomString);

                int isl = randomBytes[1] % (slovo.Length + 1);
                string symb = GenerateRandomChars(symbLength);
                if (isl == slovo.Length)
                    passwordBuilder.Append(slovo).Append(symb[0]);
                else
                    passwordBuilder.Append(slovo.Substring(0, isl)).Append(symb[0]).Append(slovo.Substring(isl));

                for (int i = 0; i < randomDigitsCount; i++)
                {
                    passwordBuilder.Append(randomBytes[symbLength + i] % 10);
                }
            }
            return passwordBuilder.ToString();
        }

        public static string FormatPhoneNumber(string phoneNumber)
        {
            // Удаляем все символы, кроме цифр
            phoneNumber = Regex.Replace(phoneNumber, @"[^\d]", "");

            // Проверяем, что номер телефона состоит из 11 цифр
            if (phoneNumber.Length != 11)
            {
                // Номер телефона не соответствует требуемому формату
                // Выводим сообщение об ошибке
                throw new ArgumentException("Номер телефона должен содержать 11 цифр в формате 8XXXXXXXXXX или +7XXXXXXXXXX");
            }

            // Если первая цифра 8, заменяем ее на +7
            if (phoneNumber[0] == '8')
            {
                phoneNumber = "7" + phoneNumber.Substring(1);
            }

            // Применяем шаблон к номеру телефона
            string formattedPhoneNumber = Regex.Replace(phoneNumber, @"(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})", "+$1 $2 $3-$4-$5");

            return formattedPhoneNumber;
        }

        public static string transHash (string password) {

            // генерируем соль
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);

            // хэшируем пароль с использованием соли
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // соединяем соль и хэш в один массив для сохранения в базе данных
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // сохраняем хэш и соль в базу данных, например, в таблицу пользователей
            return Convert.ToBase64String(hashBytes);

        }

    }
}
