#region Using directives

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace CPE.Domain.Security.Cryptography
{
    /// <summary>
    ///     Provides methods for the encryption/decryption of strings and
    ///     Byte arrays using a symmetric cryptographic algorithm
    /// </summary>
    public sealed class SymmetricCryptoProvider : IDisposable
    {
        #region Constructors

        /// <summary>
        ///     Constructs using the TripleDES algorithm by default
        /// </summary>
        public SymmetricCryptoProvider()
            : this(SymmetricCryptoAlgorithm.AES)
        {
        }

        /// <summary>
        ///     Constructs using the specified symmetric algorithm
        /// </summary>
        /// <param name="algorithm">The symmetric algorithm to use</param>
        public SymmetricCryptoProvider(SymmetricCryptoAlgorithm algorithm)
        {
            switch (algorithm) {
                case SymmetricCryptoAlgorithm.AES:
                    InitializeAES();
                    break;

                case SymmetricCryptoAlgorithm.Rijndael:
                    InitializeRijndael();
                    break;

                case SymmetricCryptoAlgorithm.TripleDES:
                    InitializeTripleDES();
                    break;

                case SymmetricCryptoAlgorithm.RC2:
                    InitializeRC2();
                    break;

                default:
                    InitializeTripleDES();
                    break;
            }

            _algorithm.Mode = CipherMode.CBC;
        }

        #endregion

        #region Initializers

        /// <summary>
        ///     Sets a default key and initialization vector for using
        ///     the AES algorithm
        /// </summary>
        private void InitializeAES()
        {
            _algorithm = new AesCryptoServiceProvider();

            _algorithm.Key = new Byte[] {
                0xda, 0x3c, 0x35, 0x6f, 0xbd, 0xd, 0x87, 0xf0,
                0x9a, 0x7, 0x6d, 0xab, 0x7e, 0x82, 0x36, 0xa,
                0x1a, 0x5a, 0x77, 0xc3, 0x74, 0xf3, 0x7f, 0xa8
            };

            _algorithm.IV = new Byte[] {
                0x1c, 0x4a, 0xf5, 0x34, 0xc7, 0x60, 0xc5, 0x33,
                0xe2, 0xa3, 0xe7, 0xc3, 0xf3, 0x39, 0xf2, 0x13
            };
        }

        /// <summary>
        ///     Sets a default key and initialization vector for using
        ///     the Rijndael algorithm
        /// </summary>
        private void InitializeRijndael()
        {
            _algorithm = new RijndaelManaged();

            _algorithm.Key = new Byte[] {
                0xda, 0x3c, 0x35, 0x6f, 0xbd, 0xd, 0x87, 0xf0,
                0x9a, 0x7, 0x6d, 0xab, 0x7e, 0x82, 0x36, 0xa,
                0x1a, 0x5a, 0x77, 0xc3, 0x74, 0xf3, 0x7f, 0xa8
            };

            _algorithm.IV = new Byte[] {
                0x1c, 0x4a, 0xf5, 0x34, 0xc7, 0x60, 0xc5, 0x33,
                0xe2, 0xa3, 0xe7, 0xc3, 0xf3, 0x39, 0xf2, 0x13
            };
        }

        /// <summary>
        ///     Sets a default key and initialization vector for using
        ///     the TripleDES algorithm
        /// </summary>
        private void InitializeTripleDES()
        {
            _algorithm = new TripleDESCryptoServiceProvider();

            _algorithm.Key = new Byte[] {
                0x1a, 0x5a, 0x77, 0xc3, 0x74, 0xf3, 0x7f, 0xa8,
                0xe2, 0xa3, 0xe7, 0xc3, 0xf3, 0x39, 0xf2, 0x13,
                0xda, 0x3c, 0x35, 0x6f, 0xbd, 0xd, 0x87, 0xf0
            };

            _algorithm.IV = new Byte[] {
                0x9a, 0x7, 0x6d, 0xab, 0x7e, 0x82, 0x36, 0xa
            };
        }

        /// <summary>
        ///     Sets a default key and initialization vector for using
        ///     the RC2 algorithm
        /// </summary>
        private void InitializeRC2()
        {
            _algorithm = new RC2CryptoServiceProvider();

            _algorithm.Key = new Byte[] {
                0xc3, 0xa1, 0xc7, 0xc2, 0xf3, 0x39, 0xf4, 0x13
            };

            _algorithm.IV = new Byte[] {
                0xe2, 0xa3, 0xe7, 0xc3, 0xf3, 0x39, 0xf2, 0x13
            };
        }

        #endregion

        #region Private Variables

        /// <summary>
        ///     The symmetric algorithm to use for cryptography
        /// </summary>
        private SymmetricAlgorithm _algorithm;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the secret key to use
        /// </summary>
        public Byte[] Key
        {
            get { return _algorithm.Key; }
            set { _algorithm.Key = value; }
        }

        /// <summary>
        ///     Gets or sets the initialization vector for the symmetric algorithm
        /// </summary>
        public Byte[] IV
        {
            get { return _algorithm.IV; }
            set { _algorithm.IV = value; }
        }

        /// <summary>
        ///     Sets the current symmetric algorithm and assigns default
        ///     values for the key and initialization vector properties
        /// </summary>
        public SymmetricCryptoAlgorithm Algorithm
        {
            set
            {
                _algorithm.Clear();

                switch (value) {
                    case SymmetricCryptoAlgorithm.Rijndael:
                        InitializeRijndael();
                        break;

                    case SymmetricCryptoAlgorithm.TripleDES:
                        InitializeTripleDES();
                        break;

                    case SymmetricCryptoAlgorithm.RC2:
                        InitializeRC2();
                        break;

                    default:
                        InitializeTripleDES();
                        break;
                }

                _algorithm.Mode = CipherMode.CBC;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Process the supplied Byte array using the ICryptoTransform object provided
        /// </summary>
        /// <param name="data">The data to process</param>
        /// <param name="startIndex">The index to start processing from</param>
        /// <param name="count">The number of bytes to process</param>
        /// <param name="cryptor">The ICryptoTransform object to use</param>
        /// <returns></returns>
        private Byte[] Process(Byte[] data, Int32 startIndex, Int32 count, ICryptoTransform cryptor)
        {
            // The memory stream granularity must match the block size
            // of the current cryptographic operation
            var capacity = count;
            var mod = count%_algorithm.BlockSize;

            if (mod > 0) {
                capacity += (_algorithm.BlockSize - mod);
            }

            var memoryStream = new MemoryStream(capacity);

            var cryptoStream = new CryptoStream(
                memoryStream,
                cryptor,
                CryptoStreamMode.Write);

            cryptoStream.Write(data, startIndex, count);
            cryptoStream.FlushFinalBlock();

            cryptoStream.Close();
            cryptoStream = null;

            cryptor.Dispose();
            cryptor = null;

            return memoryStream.ToArray();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Encrypts the supplied array of bytes
        /// </summary>
        /// <param name="plainBytes">The array of bytes to encrypt</param>
        /// <returns></returns>
        public Byte[] EncryptBuffer(Byte[] plainBytes)
        {
            Byte[] output;

            using (var cryptoTransform = _algorithm.CreateEncryptor()) {
                output = Process(plainBytes, 0, plainBytes.Length, cryptoTransform);
            }

            return output;
        }

        /// <summary>
        ///     Descrypts the supplied array of bytes
        /// </summary>
        /// <param name="cipherBytes">The array of bytes to decrypt</param>
        /// <returns></returns>
        public Byte[] DecryptBuffer(Byte[] cipherBytes)
        {
            Byte[] output;

            using (var cryptoTransform = _algorithm.CreateDecryptor()) {
                output = Process(cipherBytes, 0, cipherBytes.Length, cryptoTransform);
            }

            return output;
        }

        /// <summary>
        ///     Encrypts the supplied string
        /// </summary>
        /// <param name="plainText">The plain text string to encrypt</param>
        /// <returns></returns>
        public String EncryptString(String plainText)
        {
            return Convert.ToBase64String(EncryptBuffer(Encoding.UTF8.GetBytes(plainText)));
        }

        /// <summary>
        ///     Decrypts the supplied string
        /// </summary>
        /// <param name="cipherText">The cipher text to decrypt</param>
        /// <returns></returns>
        public String DecryptString(String cipherText)
        {
            return Encoding.UTF8.GetString(DecryptBuffer(Convert.FromBase64String(cipherText)));
        }

        /// <summary>
        ///     Generates a random secret key and initialization vector
        /// </summary>
        public void RandomizeKeyAndIV()
        {
            _algorithm.GenerateKey();
            _algorithm.GenerateIV();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        ///     Releases all resources used by the symmetric algorithm
        /// </summary>
        public void Dispose()
        {
            _algorithm.Clear();
            _algorithm.Dispose();
        }

        #endregion
    }
}