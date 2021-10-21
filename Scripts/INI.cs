using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Co_work.Scripts
{
    public class INI
    {
        public string IniFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Co-Work/Setting.ini";
        /// <summary>
        /// 写操作
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);


        /// <summary>
        /// 读操作
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="def">未读取到的默认值</param>
        /// <param name="retVal">读取到的值</param>
        /// <param name="size">大小</param>
        /// <param name="filePath">路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 读ini文件
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="defValue">未读取到值时的默认值</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public string ReadIni(string section, string key)
        {
            //string IniFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Co-Work/Setting.ini";
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, IniFilePath);
            return temp.ToString();
        }

        /// <summary>
        /// 写入ini文件
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public void WriteIni(string section, string key, string value)
        {
            //string IniFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Co-Work/Setting.ini";
            WritePrivateProfileString(section, key, value, IniFilePath);
        }
        /// <summary>
        /// 删除节
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public long DeleteSection(string section)
        {
            //string IniFilePath = @"F:\Setting.ini";
            return WritePrivateProfileString(section, null, null, IniFilePath);
        }

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public long DeleteKey(string section, string key)
        {
            //string IniFilePath = @"F:\Setting.ini";
            return WritePrivateProfileString(section, key, null, IniFilePath);
        }
    }
}
