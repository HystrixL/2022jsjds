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
        private Socket _clientSocket;
        public string Ip;
        public string ClientGuid;

        public Client(Socket socket)
        {
            ClientGuid = Guid.NewGuid().ToString();
            _clientSocket = socket;
            _clientSocket.ReceiveBufferSize = 100 * 1024 * 1024;
            Ip = (_clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
            var t = new Thread(ReceiveRequest);
            t.Start();
        }

        private void ReceiveRequest() //接收消息
        {
            byte[] data = new byte[1024*1024];
            while (true)
            {
                int length = 0;
                try
                {
                    length = _clientSocket.Receive(data);
                }
                catch (Exception)
                {
                    Console.WriteLine((_clientSocket.RemoteEndPoint as IPEndPoint)?.Address + " 连接已断开");
                    ClientManager.RemoveClient(ClientGuid);
                }

                if (_clientSocket.Poll(10, SelectMode.SelectRead)) //
                {
                    _clientSocket.Close();
                    break;
                }

                if (length != 0)
                {
                    string request = Encoding.UTF8.GetString(data, 0, length);
                    Console.WriteLine("来自" + (_clientSocket.RemoteEndPoint as IPEndPoint)?.Address + ":" +
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
            Console.WriteLine(response);
            byte[] data = Encoding.UTF8.GetBytes(response);
            _clientSocket.Send(data);
        }

        public bool Connected //获取该客户端的状态
        {
            get { return _clientSocket.Connected; }
        }
    }
}