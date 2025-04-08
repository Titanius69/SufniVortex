using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SufniBrowser
{

    public static class DataManager
    {
        private static readonly string HistoryFile = "history.dat";
        private static readonly string PasswordsFile = "passwords.dat";
        private static readonly string CookiesFile = "cookies.dat";
        private static readonly string BookmarksFile = "bookmarks.dat";



        // Böngészési előzmények mentése és betöltése
        public static void SaveHistory(List<string> history)
        {
            string json = JsonSerializer.Serialize(history);
            string encrypted = CustomCrypto.Encrypt(json);
            File.WriteAllText(HistoryFile, encrypted);
        }

        public static List<string> LoadHistory()
        {
            if (!File.Exists(HistoryFile))
                return new List<string>();

            string encrypted = File.ReadAllText(HistoryFile);
            string json = CustomCrypto.Decrypt(encrypted);
            return JsonSerializer.Deserialize<List<string>>(json);
        }

        // Jelszavak mentése és betöltése
        public static void SavePasswords(List<Credential> credentials)
        {
            string json = JsonSerializer.Serialize(credentials);
            string encrypted = CustomCrypto.Encrypt(json);
            File.WriteAllText(PasswordsFile, encrypted);
        }

        public static List<Credential> LoadPasswords()
        {
            if (!File.Exists(PasswordsFile))
                return new List<Credential>();

            string encrypted = File.ReadAllText(PasswordsFile);
            string json = CustomCrypto.Decrypt(encrypted);
            return JsonSerializer.Deserialize<List<Credential>>(json);
        }






        // Könyvjelzők mentése és betöltése
        public static void SaveBookmarks(List<string> bookmarks)
        {
            string json = JsonSerializer.Serialize(bookmarks);
            string encrypted = CustomCrypto.Encrypt(json);
            File.WriteAllText(BookmarksFile, encrypted);
        }

        public static List<string> LoadBookmarks()
        {
            if (!File.Exists(BookmarksFile))
                return new List<string>();

            string encrypted = File.ReadAllText(BookmarksFile);
            string json = CustomCrypto.Decrypt(encrypted);
            return JsonSerializer.Deserialize<List<string>>(json);
        }

        public static void SaveCookies(List<CookieInfo> newCookies)
        {
            List<CookieInfo> existingCookies = LoadCookies();

 
            foreach (var newCookie in newCookies)
            {
                var existingCookie = existingCookies.Find(c => c.Name == newCookie.Name && c.Domain == newCookie.Domain && c.Path == newCookie.Path);

                if (existingCookie != null)
                {

                    existingCookie.Value = newCookie.Value;
                    existingCookie.Expires = newCookie.Expires;
                }
                else
                {
            
                    existingCookies.Add(newCookie);
                }
            }

       
            string json = JsonSerializer.Serialize(existingCookies);
            string encrypted = CustomCrypto.Encrypt(json);
            File.WriteAllText(CookiesFile, encrypted);
        }


        public static List<CookieInfo> LoadCookies()
        {
            if (!File.Exists(CookiesFile))
                return new List<CookieInfo>();

            string encrypted = File.ReadAllText(CookiesFile);
            string json = CustomCrypto.Decrypt(encrypted);
            return JsonSerializer.Deserialize<List<CookieInfo>>(json);
        }

        public static void ClearCookies()
        {
            if (File.Exists(CookiesFile))
                File.Delete(CookiesFile);
        }
    }
}

