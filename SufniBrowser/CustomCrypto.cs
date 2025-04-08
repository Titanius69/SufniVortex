using System;
using System.Text;

namespace SufniBrowser
{
    public static class CustomCrypto
    {



        private static readonly string Key = "La~ar4123aRRG*/DaSKi-+asbidey+!12*s/d_Sigmaaaaaaaaaaaa@/@@aaaa24aaaaaaaaaa41aaaaaaaaaas/_Sufnix&Amata_in_love";


        public static string Encrypt(string plainText)
        {


            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = new byte[plainBytes.Length];
            for (int i = 0; i < plainBytes.Length; i++)
            {
                cipherBytes[i] = (byte)(plainBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }
            return Convert.ToBase64String(cipherBytes);
        }

        public static string Decrypt(string cipherText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] plainBytes = new byte[cipherBytes.Length];
            for (int i = 0; i < cipherBytes.Length; i++)
            {
                plainBytes[i] = (byte)(cipherBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }
            return Encoding.UTF8.GetString(plainBytes);
            
        }
    }
}
