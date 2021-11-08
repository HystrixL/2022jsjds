using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Co_Work.Network.TCP
{
    public static class TCPServer
    {
        static Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static void Init(string serverIp, int serverPort)
        {
            tcpServer.Bind(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));

            tcpServer.Listen(Program.Configs.MaxConnectingMember);
            Console.WriteLine($"服务器已启动，端口{serverPort}，等待连接.........");
            Thread t = new Thread(WaitConnect);
            t.Start();
        }

        public static void WaitConnect()
        {
            while (true)
            {
                Socket clientSocket = tcpServer.Accept();
                Console.WriteLine((clientSocket.RemoteEndPoint as IPEndPoint).Address + "已连接");
                Client client = new Client(clientSocket);
                ClientManager.AddClient(client.ClientGuid, client);
            }
        }
    }
}