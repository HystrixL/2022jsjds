using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Co_Work.Network.TCP
{
    public static class TCPServer
    {
        static readonly Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static void Init(string serverIp, int serverPort)
        {
            tcpServer.Bind(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));

            tcpServer.Listen(Program.Configs.MaxConnectingMember);
            Console.WriteLine($"服务器已启动，端口{serverPort.ToString()}，等待连接.........");
            var t = new Thread(WaitConnect);
            t.Start();
        }

        private static void WaitConnect()
        {
            while (true)
            {
                var clientSocket = tcpServer.Accept();
                Console.WriteLine((clientSocket.RemoteEndPoint as IPEndPoint)?.Address + "已连接");
                var client = new Client(clientSocket);
                ClientManager.AddClient(client.ClientGuid, client);
            }
        }
    }
}