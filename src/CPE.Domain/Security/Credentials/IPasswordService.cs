using System.Security;

namespace CPE.Domain.Security.Credentials
{
    public interface IPasswordService
    {
        /// <summary>
        ///     Hashes the supplied SecureString value and returns the
        ///     hash and salt value that were generated
        /// </summary>
        /// <param name="password">The password to compute the hash of</param>
        /// <returns></returns>
        SecuredPassword SecurePassword(SecureString password);

        /// <summary>
        ///     Checks if the plain password supplied matches the supplied hash
        ///     when hashed using the supplied salt value
        /// </summary>
        /// <param name="password">The secure password to verify</param>
        /// <param name="salt">The salt to use when hashing the password</param>
        /// <param name="hash">The hashed password to compare to</param>
        /// <returns></returns>
        bool AreEqual(SecureString password, string hash, string salt);
    }
}