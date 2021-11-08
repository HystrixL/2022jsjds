using System;
using System.IO;
using Co_Work.Local;
using Co_Work.Network.TCP;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Co_Work.Core
{
    public class Init
    {
        public static void RunServer()
        {
            if (!File.Exists(".\\CoWorkData\\Config.yml")) Initialization();
            string configText = File.ReadAllText(".\\CoWorkData\\Config.yml");
            Program.Configs = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build()
                .Deserialize<Config>(configText);
            Console.WriteLine("已读取配置文件");
            if (!Directory.Exists(Program.Configs.Ftp.RootPath))
                Directory.CreateDirectory(Program.Configs.Ftp.RootPath);

            var serverIp = Tools.GetAddressIP();
            var serverPort = Program.Configs.ServerPort;
            Console.WriteLine("本机IP地址为:" + serverIp);
            TCPServer.Init(serverIp,serverPort);
            DataBaseManager.Init(Program.Configs.DatabasePath);
        }

        public static void Initialization()
        {
            if (!Directory.Exists(".\\CoWorkData"))
            {
                Directory.CreateDirectory(".\\CoWorkData");
            }

            if (!File.Exists(".\\CoWorkData\\Config.yml"))
            {
                Config config = new Config();
                var yaml = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build().Serialize(config);
                File.WriteAllText(".\\CoWorkData\\Config.yml",yaml);
            }

            Console.WriteLine("已完成服务端初始化.");
        }
    }
}