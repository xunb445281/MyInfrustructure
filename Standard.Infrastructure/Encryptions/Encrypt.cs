using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Standard.Infrastructure.Encryptions
{
    public static class Encrypt
    {

        #region Md5加密

        /// <summary>
        /// Md5加密
        /// </summary>
        private static string Md5(string value, Encoding encoding, int? startIndex, int? length)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            var md5 = new MD5CryptoServiceProvider();
            string result;
            try
            {
                var hash = md5.ComputeHash(encoding.GetBytes(value));
                result = startIndex == null ? BitConverter.ToString(hash) : BitConverter.ToString(hash, startIndex.SafeValue(), length.SafeValue());
            }
            finally
            {
                md5.Clear();
            }
            return result.Replace("-", "");
        }

        /// <summary>
        /// Md5加密，返回32位结果
        /// </summary>
        /// <param name="value">值</param>
        public static string Md5By32(string value)
        {
            return Md5By32(value, Encoding.UTF8);
        }

        /// <summary>
        /// Md5加密，返回32位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        public static string Md5By32(string value, Encoding encoding)
        {
            return Md5(value, encoding, null, null);
        }

        /// <summary>
        /// Md5加密，返回16位结果
        /// </summary>
        /// <param name="value">值</param>
        public static string Md5By16(string value)
        {
            return Md5By16(value, Encoding.UTF8);
        }

        /// <summary>
        /// Md5加密，返回16位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        public static string Md5By16(string value, Encoding encoding)
        {
            return Md5(value, encoding, 4, 8);
        }

        #endregion

        #region Sha1加密

        public static byte[] CreateSha1ByByte(string content)
        {
            var sha1 = SHA1.Create();
            var data = sha1.ComputeHash(Encoding.UTF8.GetBytes(content));
            sha1.Dispose();

            return data;
        }

        public static string CreateSha1(string content)
        {
            var data = CreateSha1ByByte(content);
            string result = BitConverter.ToString(data);
            result = result.Replace("-", "");
            return result;
        }

        public static string CreateSha1ByLower(string content)
        {
            var result = CreateSha1(content);
            return result.ToLower();
        }

        #endregion

        #region RSA签名

        /// <summary>
        /// RSA加密，采用 SHA1 算法
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        public static string RsaSign(string value, string key)
        {
            return RsaSign(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// RSA加密，采用 SHA1 算法
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">编码</param>
        public static string RsaSign(string value, string key, Encoding encoding)
        {
            return RsaSign(value, key, encoding, RSAType.RSA);
        }

        /// <summary>
        /// RSA加密，采用 SHA256 算法
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        public static string Rsa2Sign(string value, string key)
        {
            return Rsa2Sign(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// RSA加密，采用 SHA256 算法
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">编码</param>
        public static string Rsa2Sign(string value, string key, Encoding encoding)
        {
            return RsaSign(value, key, encoding, RSAType.RSA2);
        }

        /// <summary>
        /// Rsa加密
        /// </summary>
        private static string RsaSign(string value, string key, Encoding encoding, RSAType type)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
                return string.Empty;
            var rsa = new RsaOpenSSLHelper(type, encoding, key);
            return rsa.Sign(value);
        }

        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string RsaDecrypt(string value, string key, Encoding encoding, RSAType type)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
                return string.Empty;

            var rsa = new RsaOpenSSLHelper(RSAType.RSA2, Encoding.UTF8, key);

            return rsa.Decrypt(value);
        }

        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string RsaEncrypt(string value, string key, Encoding encoding, RSAType type)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
                return string.Empty;

            var rsa = new RsaOpenSSLHelper(type, Encoding.UTF8, "", key);

            return rsa.Encrypt(value);
        }

        #endregion
    }
}
