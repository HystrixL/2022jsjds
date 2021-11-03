using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using Co_Work.Network;

namespace Co_work.Pages
{
    /// <summary>
    /// Page_User_Register.xaml 的交互逻辑
    /// </summary>
    public partial class Page_User_Register : Page
    {
        public Page_User_Register()
        {
            InitializeComponent();
        }

        public Page_User_Login Owner;

        private static Socket clientScoket;
        private static byte[] data = new byte[1024];
        private static bool isConnected = false;

        static void ConnectToServer()
        {
            Thread receiveT;
            var ipPort = "103.193.189.241:2333";
            var serverIp = ipPort.Split(":")[0];
            var serverPort = int.Parse(ipPort.Split(":")[1]);
            clientScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientScoket.Connect(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
            Console.WriteLine("已连接到服务器");
            isConnected = true;
            receiveT = new Thread(ReceiveMessage); //开启线程执行循环接收消息
            receiveT.Start();
        }

        void SendMessage() //发送消息
        {
            //while (isConnected)
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    Request.Register register =
                    new Request.Register(Tb_Id.Text, Tb_Password.Password, Tb_Name.Text, 0, "2021-10-30");
                    var message = new TransData<Request.Register>(register, "ertgwergf", "etyhtgyetyh").ToString();
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    if (isConnected)
                    {
                        clientScoket.Send(data);
                    }
                    else ConnectToServer();
                    MessageBox.Show("注册成功！");
                    Owner.Owner.CheckLoginState();
                }));
            }

        }

        static void ReceiveMessage() //接收消息
        {
            int length = 0;
            while (isConnected)
            {
                if (clientScoket.Connected == true)
                {
                    try
                    {
                        length = clientScoket.Receive(data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("服务器已断开连接");
                        isConnected = false;
                    }

                    if (length != 0)
                    {
                        string message = Encoding.UTF8.GetString(data, 0, length);
                        var received = TransData<Response.Register>.Convert(message);
                        Console.WriteLine(received.Content.RegisterResult);
                        Console.WriteLine(message);
                    }
                }
            }
        }

        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRegister())
            {
                Thread sendT;
                sendT = new Thread(SendMessage);
                sendT.Start();
            }
        }

        private bool CheckRegister()
        {
            bool result = true;

            Lb_Id_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            Lb_Name_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            Lb_Password_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            Lb_RePassword_Instru.Foreground = new SolidColorBrush(Colors.Gray);

            if (Tb_Id.Text == "" || !System.Text.RegularExpressions.Regex.IsMatch(Tb_Id.Text, @"^[A-Za-z0-9_]+$"))
            {
                Lb_Id_Instru.Foreground = new SolidColorBrush(Colors.Red);
                result = false;
            }
            if (Tb_Name.Text == "")
            {
                Lb_Name_Instru.Foreground = new SolidColorBrush(Colors.Red);
                result = false;
            }
            if (Tb_Password.Password == "" || !System.Text.RegularExpressions.Regex.IsMatch(Tb_Password.Password, @"^[A-Za-z0-9]{8,20}$"))
            {
                Lb_Password_Instru.Foreground = new SolidColorBrush(Colors.Red);
                result = false;
            }
            if (Tb_Password_Repeat.Password == "" || Tb_Password_Repeat.Password != Tb_Password.Password)
            {
                Lb_RePassword_Instru.Foreground = new SolidColorBrush(Colors.Red);
                result = false;
            }
            return result;
        }
    }
}
