using System;
using System.Security.Cryptography;

namespace BrainShare.Infostructure
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
        public const int SaltSize = 24;

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
    }
}