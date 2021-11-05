using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Co_Work.Core
{
    public static class MD5withSalt
    {
        public static string Encrypt(string password)
        {
            var salt = new Salt();
            var saltString = salt.Generate();

            password = salt.Join(password, saltString);
            var result = _Md5Encrypt(password);
            result = salt.Join(result, _Reverse(saltString));
            result = _Reverse(result).ToLower();
            return result;
        }

        public static bool Check(string password, string encryptResult)
        {
            var salt = new Salt();
            encryptResult = _Reverse(encryptResult);

            var saltString = _Reverse(salt.Extract(encryptResult)).ToUpper();
            password = salt.Join(password, saltString);
            var result = _Reverse(_Md5Encrypt(password));
            result = salt.Join(result, saltString).ToLower();
            return result == _Reverse(encryptResult);
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
                    var isNumOrLetter = new Random().Next(0, 2);
                    if (isNumOrLetter == 0)
                    {
                        var saltBit = new Random().Next(0, 10);
                        salt += saltBit.ToString();
                    }
                    else
                    {
                        var saltBit = (char)new Random().Next(65, 91);
                        salt += saltBit.ToString();
                    }
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