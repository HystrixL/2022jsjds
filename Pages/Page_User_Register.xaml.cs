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

        public void ReceiveMessage() //接收消息
        {
            int length = 0;
            while (Owner.Owner.Owner.isConnected)
            {
                if (Owner.Owner.Owner.clientScoket.Connected == true)
                {
                    try
                    {
                        length = Owner.Owner.Owner.clientScoket.Receive(Owner.Owner.Owner.data);
                    }
                    catch (Exception e)
                    {
                        Owner.Owner.Owner.isConnected = false;
                    }

                    if (length != 0)
                    {
                        string message = Encoding.UTF8.GetString(Owner.Owner.Owner.data, 0, length);
                        var received = TransData<Response.Register>.Convert(message);
                        //MessageBox.Show(received.Content.RegisterResult.ToString());
                        if (received.Content.RegisterResult == Response.Register.RegisterResultEnum.Succeed)
                        {
                            MessageBox.Show("注册成功！");
                            Dispatcher.Invoke(new Action(delegate
                            {
                                Owner.Owner.CheckLoginState();
                            }));
                        }
                        else if (received.Content.RegisterResult == Response.Register.RegisterResultEnum.IdAlreadyExists)
                        {
                            MessageBox.Show("用户名已存在");
                        }

                        break;
                    }
                }
            }
        }

        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRegister())
            {
                Thread sendT;
                sendT = new Thread(Owner.Owner.Owner.SendMessageRegister);
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
