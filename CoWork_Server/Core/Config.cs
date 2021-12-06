using System;
using System.IO;
using YamlDotNet.Serialization;

namespace Co_Work.Core
{
    public class Config
    {
        [YamlMember(Alias = "MaxConnectingMember", ApplyNamingConventions = false, Description = "服务端最大同时连接人数")]
        public int MaxConnectingMember { get; set; } = 1000;

        [YamlMember(Alias = "ServerPort", ApplyNamingConventions = false, Description = "服务端开放端口")]
        public int ServerPort { get; set; } = 2333;

        [YamlMember(Alias = "DatabasePath", ApplyNamingConventions = false, Description = "数据库路径")]
        public string DatabasePath { get; set; } = Path.Combine(Environment.CurrentDirectory, "CoWorkData/CoWork.db");
        
        [YamlMember(Alias = "Ftp", ApplyNamingConventions = false, Description = "FTP服务端相关设置")]
        public Ftp Ftp { get; set; } = new Ftp();
        [YamlMember(Alias = "DebugMode", ApplyNamingConventions = false, Description = "调试模式")]
        public bool DebugMode { get; set; } = false;
    }

    public class Ftp
    {
        [YamlMember(Alias = "Account", ApplyNamingConventions = false)]
        public string Account { get; set; } = "admin";

        [YamlMember(Alias = "Password", ApplyNamingConventions = false)]
        public string Password { get; set; } = "123456";

        [YamlMember(Alias = "RootPath", ApplyNamingConventions = false)]
        public string RootPath { get; set; } =
            Path.Combine(Environment.CurrentDirectory, "CoWorkData/ProjectFiles");
    }
}