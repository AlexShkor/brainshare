using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Brainshare.Infrastructure.Infrastructure
{
    // From http://crackstation.net/hashing-security.htm
    // To Store a Password:
    //   1. Generate a long random salt using a CSPRNG (for .NET -- System.Security.Cryptography.RNGCryptoServiceProvider).
    //   2. Prepend the salt to the password and hash it with a standard cryptographic hash function such as SHA256.
    //   3. Save both the salt and the hash in the user's database record.
    //
    // To Validate a Password:
    //   1. Retrieve the user's salt and hash from the database.
    //   2. Prepend the salt to the given password and hash it using the same hash function.
    //   3. Compare the hash of the given password with the hash from the database. If they match, the password is correct. Otherwise, the password is incorrect.
    public class CryptographicHelper
    {
        private const int SaltSize = 24;
        private const int Keysize = 256;
        private const string InitVector = "xx89geji340t89u2";

        public string GenerateSalt()
        {
            // Generate a cryptographic random number using the cryptographic service provider
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[SaltSize];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        public string GetPasswordHash(string password, string salt = null)
        {
            // Create a new instance of the hash crypto service provider.
            //HashAlgorithm hashAlg = new SHA256CryptoServiceProvider();
            HashAlgorithm hashAlg = new SHA256Managed();
            // Convert the data to hash to an array of Bytes.
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(salt + password);
            // Compute the Hash. This returns an array of Bytes.
            byte[] byteHash = hashAlg.ComputeHash(byteValue);
            // Represent the hash value as a base64-encoded string, 
            return Convert.ToBase64String(byteHash);
        }

        public string GetCsrfToken()
        {
            return GenerateSalt();
        }

        public static string Encrypt(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(Keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] cipherTextBytes = Convert.FromBase64String(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(Keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public string GetMd5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

}