namespace CPE.Domain.Security.Credentials
{
    /// <summary>
    ///     The result of a hash computation. Contains the hash value
    ///     and the salt value used when hashing.
    /// </summary>
    public class SecuredPassword
    {
        /// <summary>
        ///     The computed hash
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        ///     The salt used when hashing
        /// </summary>
        public string Salt { get; set; }
    }
}