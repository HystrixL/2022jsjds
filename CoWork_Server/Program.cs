using System;
using System.Net;
using System.Net.Sockets;
using Co_Work.Core;
using Co_Work.Local;
using Co_Work.Network;
using Co_Work.Network.TCP;

namespace CoWork_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverIp = Tools.GetAddressIP();
            var serverPort = 2333;
            Console.WriteLine("本机IP地址为:" + serverIp);
            TCPServer.Init(serverIp,serverPort);
            DataBaseManager.Init();
        }
    }
}