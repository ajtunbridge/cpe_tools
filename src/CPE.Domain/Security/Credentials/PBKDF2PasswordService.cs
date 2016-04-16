using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace CPE.Domain.Security.Credentials
{
    public class PBKDF2PasswordService : IPasswordService
    {
        private const byte SaltSize = 16;
        private const int Iterations = 5000;

        #region IPasswordService Members

        public SecuredPassword SecurePassword(SecureString password)
        {
            var hashed = new SecuredPassword();

            byte[] salt = GenerateSalt();

            hashed.Hash = ComputeHash(password, salt);
            hashed.Salt = Convert.ToBase64String(salt);
            
            return hashed;
        }

        public bool AreEqual(SecureString password, string hash, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            string newHash = ComputeHash(password, saltBytes);
            
            return newHash == hash;
        }

        #endregion

        //  Call this function to remove the plaintext password from memory after use
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr destination, int length);

        private string ComputeHash(SecureString text, byte[] salt)
        {
            var bstr = Marshal.SecureStringToBSTR(text);
            
            try
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(Marshal.PtrToStringBSTR(bstr), salt, Iterations))
                {
                    byte[] key = pbkdf2.GetBytes(64);
                    return Convert.ToBase64String(key);
                }
            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstr);
            }
        }

        private byte[] GenerateSalt()
        {
            using (var rnd = RandomNumberGenerator.Create()) {
                var saltBytes = new byte[SaltSize];

                rnd.GetBytes(saltBytes);

                return saltBytes;
            }
        }
    }
}