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
    /// Page_User_Login.xaml 的交互逻辑
    /// </summary>
    public partial class Page_User_Login : Page
    {
        public Page_User_Login()
        {
            InitializeComponent();
        }

        public Page_User Owner;

        public Page_User_Register page_User_Register;

        public void ReceiveMessage() //接收消息
        {
            int length = 0;
            while (Owner.Owner.isConnected)
            {
                if (Owner.Owner.clientScoket.Connected == true)
                {
                    try
                    {
                        length = Owner.Owner.clientScoket.Receive(Owner.Owner.data);
                    }
                    catch (Exception e)
                    {
                        Owner.Owner.isConnected = false;
                    }

                    if (length != 0)
                    {
                        string message = Encoding.UTF8.GetString(Owner.Owner.data, 0, length);
                        var received = TransData<Response.Login>.Convert(message);
                        if (received.Content.LoginResult == Response.Login.LoginResultEnum.Succeed)
                        {
                            Owner.Owner.User = received.Content.Employee;
                            Owner.Owner.isLogined = true;
                            Dispatcher.Invoke(new Action(delegate
                            {
                                Owner.Owner.ChangePageUser();
                                Tb_Id.Text = "";
                                Tb_Password.Password = "";
                            }));
                        }
                        else if (received.Content.LoginResult == Response.Login.LoginResultEnum.UnknownAccount)
                        {
                            MessageBox.Show("用户名不存在");
                            Dispatcher.Invoke(new Action(delegate
                            {
                                Tb_Id.Text = "";
                                Tb_Password.Password = "";
                            }));
                        }
                        else if (received.Content.LoginResult == Response.Login.LoginResultEnum.WrongPassword)
                        {
                            MessageBox.Show("密码错误");
                            Dispatcher.Invoke(new Action(delegate
                            {
                                Tb_Password.Password = "";
                            }));
                        }

                        break;
                    }
                }
            }
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (Tb_Id.Text == "" || Tb_Password.Password == "")
            {
                MessageBox.Show("请填写用户名或密码", "错误", MessageBoxButton.OK);
            }
            else 
            {
                Thread sendT;
                sendT = new Thread(Owner.Owner.SendMessageLogin);
                sendT.Start();
            }

        }
        
        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            if (page_User_Register == null)
            {
                page_User_Register = new Page_User_Register();
                page_User_Register.Owner = this;
            }
            Owner.Owner.Change_Page.Content = new Frame()
            {
                Content = page_User_Register
            };

            page_User_Register.Tb_Id.Text = "";
            page_User_Register.Tb_Name.Text = "";
            page_User_Register.Tb_Password.Password = "";
            page_User_Register.Tb_Password_Repeat.Password = "";
            page_User_Register.Lb_Id_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            page_User_Register.Lb_Name_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            page_User_Register.Lb_Password_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            page_User_Register.Lb_RePassword_Instru.Foreground = new SolidColorBrush(Colors.Gray);

            page_User_Register.Tb_Id.Focus();
        }
    }
}
