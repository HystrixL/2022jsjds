using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Co_Work.Core
{
    static class MD5withSalt
    {
        public static string Encrypt(string password)
        {
            var salt = new Salt();
            //生成salt
            var saltString = salt.Generate();

            password = salt.Join(password, saltString);

            //MD5加密
            var result = _Md5Encrypt(password);

            result = salt.Join(result, saltString);
            //结果逆序
            result = _Reverse(result);
            return result;
        }

        public static bool Check(string password, string encryptResult)
        {
            var salt = new Salt();
            encryptResult = _Reverse(encryptResult);

            //salt提取
            var saltString = salt.Extract(encryptResult);
            password = salt.Join(password, saltString);

            //MD5加密
            var result =  _Md5Encrypt(password);

            result = salt.Join(result, saltString);

            return result == encryptResult;
        }

        private static string _Md5Encrypt(string originalStr)
        {
            var md5 = new MD5CryptoServiceProvider();
            var result = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(originalStr)));
            result = result.Replace("-", "");
            return result;
        }

        private static string _Reverse(string originalStr)
        {
            return string.Join("", originalStr.Reverse());
        }

        class Salt
        {
            public string Generate()
            {
                var salt = "";
                for (var i = 0; i < 8; i++)
                {
                    var saltBit = new Random().Next(0, 10);
                    salt += saltBit.ToString();
                }

                return salt;
            }

            public string Join(string originalStr, string salt)
            {
                return salt.Substring(0, 4) + originalStr + salt.Substring(salt.Length - 4, 4);
            }

            public string Extract(string originalStr)
            {
                return originalStr.Substring(0, 4) + originalStr.Substring(originalStr.Length - 4, 4);
            }
        }
    }
}