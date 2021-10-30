using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Co_Work.Core;

namespace Co_Work.Network.TCP
{
    public class Client
    {
        private Socket clientSocket;
        private Thread t;
        public string ip;
        public string ClientGuid;

        public Client(Socket socket)
        {
            ClientGuid = Guid.NewGuid().ToString();
            clientSocket = socket;
            ip = (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
            t = new Thread(ReceiveRequest);
            t.Start();
        }

        private void ReceiveRequest() //接收消息
        {
            byte[] data = new byte[4096];
            while (true)
            {
                int length = 0;
                try
                {
                    length = clientSocket.Receive(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine((clientSocket.RemoteEndPoint as IPEndPoint).Address + " 连接已断开");
                    ClientManager.RemoveClient(ClientGuid);
                }

                if (clientSocket.Poll(10, SelectMode.SelectRead)) //
                {
                    clientSocket.Close();
                    break;
                }

                if (length != 0)
                {
                    string request = Encoding.UTF8.GetString(data, 0, length);
                    Console.WriteLine("来自" + (clientSocket.RemoteEndPoint as IPEndPoint).Address + ":" +
                                      request);
                    Task.Run(() =>
                    {
                        RequestParser requestParser = new RequestParser(request);
                        requestParser.Parse(this);
                    });
                }
            }
        }

        public void SendResponse(string response) //发送消息
        {
            byte[] data = Encoding.UTF8.GetBytes(response);
            clientSocket.Send(data);
        }

        public bool Connected //获取该客户端的状态
        {
            get { return clientSocket.Connected; }
        }
    }
}