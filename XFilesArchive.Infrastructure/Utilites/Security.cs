using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Infrastructure.Utilites
{
    public static class Security
    {
        public const int SECTION_SIZE = 4096;
        public static int MB_SIZE = (int)Math.Pow(2, 20);

        #region CalculateHash
        public static string CalculateHash(string clearTextPassword, string salt)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash as a base64 encoded string to be compared to the stored password
            return Convert.ToBase64String(hash);
        }
        #endregion

        public static string ComputeMD5Checksum(string path)
        {
            using (Stream stream = System.IO.File.OpenRead(path))
            {
                byte[] buffer = new byte[MB_SIZE];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead != 0)
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    MemoryStream ms = new MemoryStream();

                    ms.Write(buffer, 0, buffer.Length);
                    byte[] checkSum = md5.ComputeHash(ms.GetBuffer());
                    string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                    return result;
                }
                return null;
            }
        }
    } 
}
