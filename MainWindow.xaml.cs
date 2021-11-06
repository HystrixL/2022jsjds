using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Co_work.Pages;
using Co_work.Scripts;
using Co_Work.Core;
using Co_Work.Network;

namespace Co_work
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChangePageProject();
            ConnectToServer();

            if (ini.ReadIni("Setting", "fileSaveAddress") == "")
                ini.WriteIni("Setting", "fileSaveAddress", fileSaveAddress);
            else
                fileSaveAddress = ini.ReadIni("Setting", "fileSaveAddress");
        }

        public INI ini = new INI();

        public bool isLogined = false;

        public Co_Work.Core.Employee User;

        public Socket clientScoket;
        public byte[] data = new byte[1024 * 1024];
        public bool isConnected = false;

        public void ConnectToServer()
        {
            var ipPort = "103.193.189.241:2333";
            var serverIp = ipPort.Split(":")[0];
            var serverPort = int.Parse(ipPort.Split(":")[1]);
            try
            {
                clientScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientScoket.ReceiveBufferSize = 100 * 1024 * 1024;
                clientScoket.Connect(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
                isConnected = true;
            }
            catch
            {
                MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
            }
        }

        public void SendMessageLogin() //发送登录请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                string password;
                if (ini.ReadIni("Setting", "RememberPassword") == "true")
                    password = MD5withSalt.Encrypt(ini.ReadIni("TNUOCA", "DROWSSAP"));
                else
                    password = MD5withSalt.Encrypt(page_User.page_User_Login.Tb_Password.Password);
                
                //MessageBox.Show(password);
                Request.Login login =
                       new Request.Login(page_User.page_User_Login.Tb_Id.Text, password);
                var message = new TransData<Request.Login>(login, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_User.page_User_Login.ReceiveMessage); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }

        public void SendMessageRegister() //发送注册请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                Request.Register register =
                   new Request.Register(page_User.page_User_Login.page_User_Register.Tb_Id.Text, page_User.page_User_Login.page_User_Register.Tb_Password.Password, page_User.page_User_Login.page_User_Register.Tb_Name.Text, 0, "2021-10-30");
                var message = new TransData<Request.Register>(register, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_User.page_User_Login.page_User_Register.ReceiveMessage); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }

        public void SendMessageUpdateProjects() //发送更新项目列表请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                Request.GetProjectsInfoFromEmployee getProjects =
                       new Request.GetProjectsInfoFromEmployee(User.GUID);
                var message = new TransData<Request.GetProjectsInfoFromEmployee>(getProjects, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_Project.ReceiveMessageUpdate); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }

        public void SendMessageCreateProject() //发送创建项目请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                string endDate;
                if (page_Project.newProject.EndDate == DateTime.MinValue)
                    endDate = "";
                else 
                    endDate = ToTimeString(page_Project.newProject.EndDate);

                Request.CreatProject createProject =
                       new Request.CreatProject(page_Project.newProject.Name, page_Project.newProject.Note, page_Project.newProject.ProgressRate, ToTimeString(page_Project.newProject.StartDate), endDate, User.GUID, new List<string>() { User.GUID });
                var message = new TransData<Request.CreatProject>(createProject, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_Project.ReceiveMessageCreate); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }

        public void SendMessageDeleteProject() //发送删除项目请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                Request.DeleteProject deleteProject =
                       new Request.DeleteProject(page_Project.project[page_Project.selectIndex].GUID, User.GUID);
                var message = new TransData<Request.DeleteProject>(deleteProject, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_Project.page_ProjectInstance.page_ProjectInstance_Setting.ReceiveMessageDelete); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }

        public void SendMessageUpdateProject() //发送更新项目请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                string endDate;
                if (page_Project.newProject.EndDate == DateTime.MinValue)
                    endDate = "";
                else
                    endDate = ToTimeString(page_Project.newProject.EndDate);

                Request.UpdateProject updateProject =
                       new Request.UpdateProject(page_Project.project[page_Project.selectIndex].GUID, page_Project.newProject.Name, page_Project.newProject.Note, page_Project.newProject.ProgressRate, ToTimeString(page_Project.newProject.StartDate), endDate, User.GUID, page_Project.membersGUID);
                var message = new TransData<Request.UpdateProject>(updateProject, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_Project.page_ProjectInstance.page_ProjectInstance_Setting.ReceiveMessageUpdate); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }

        public void SendMessageRemoveMember() //发送删除成员请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                string endDate;
                if (page_Project.project[page_Project.selectIndex].EndDate == DateTime.MinValue)
                    endDate = "";
                else
                    endDate = ToTimeString(page_Project.project[page_Project.selectIndex].EndDate);

                Request.UpdateProject updateProject =
                       new Request.UpdateProject(page_Project.project[page_Project.selectIndex].GUID, page_Project.project[page_Project.selectIndex].Name, page_Project.project[page_Project.selectIndex].Note, page_Project.project[page_Project.selectIndex].ProgressRate, ToTimeString(page_Project.project[page_Project.selectIndex].StartDate), endDate, User.GUID, page_Project.membersGUID);
                var message = new TransData<Request.UpdateProject>(updateProject, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_Project.page_ProjectInstance.page_ProjectInstance_Member.ReceiveMessageRemove); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }

        public void SendMessageAddMember() //发送添加成员请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                string endDate;
                if (page_Project.project[page_Project.selectIndex].EndDate == DateTime.MinValue)
                    endDate = "";
                else
                    endDate = ToTimeString(page_Project.project[page_Project.selectIndex].EndDate);

                Request.UpdateProject updateProject =
                       new Request.UpdateProject(page_Project.project[page_Project.selectIndex].GUID, page_Project.project[page_Project.selectIndex].Name, page_Project.project[page_Project.selectIndex].Note, page_Project.project[page_Project.selectIndex].ProgressRate, ToTimeString(page_Project.project[page_Project.selectIndex].StartDate), endDate, User.GUID, page_Project.membersGUID);
                var message = new TransData<Request.UpdateProject>(updateProject, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_Project.page_ProjectInstance.page_ProjectInstance_Member.ReceiveMessageRemove); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }

        public void SendMessageFileInfo() //发送获取文件信息请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                Request.GetFileInfo fileInfo =
                       new Request.GetFileInfo(page_Project.project[page_Project.selectIndex].GUID);
                var message = new TransData<Request.GetFileInfo>(fileInfo, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_Project.page_ProjectInstance.page_ProjectInstance_Project.ReceiveMessageFileInfo); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }

        public void SendMessageGetUserGUID() //发送获取用户GUID请求
        {
            Dispatcher.Invoke(new Action(delegate
            {
                Request.GetEmployeeInfoFromId employeeInfo =
                       new Request.GetEmployeeInfoFromId(page_Project.page_ProjectInstance.page_ProjectInstance_Member.newMemberId);
                var message = new TransData<Request.GetEmployeeInfoFromId>(employeeInfo, "ertgwergf", "etyhtgyetyh").ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (isConnected)
                {
                    try
                    {
                        clientScoket.Send(data);
                        Thread receiveT;
                        receiveT = new Thread(page_Project.page_ProjectInstance.page_ProjectInstance_Member.ReceiveMessageGetUserGUID); //开启线程执行循环接收消息
                        receiveT.Start();
                    }
                    catch
                    {
                        isConnected = false;
                        MessageBox.Show("服务器被玩坏了，一定不是Co-work的问题QWQ");
                    }
                }
                else ConnectToServer();
            }));
        }


        public void InitializePageTransmisson()
        {
            if (page_Transmisson == null)
            {
                page_Transmisson = new Page_Transmisson();
                page_Transmisson.Owner = this;
            }
        }

        #region 导航栏
        public Page_Project page_Project;
        public Page_Transmisson page_Transmisson;
        public Page_User page_User;
        public Page_Setting page_Setting;

        private void Tg_Btn_Project_Click(object sender, RoutedEventArgs e)
        {
            ChangePageProject();
        }
        private void Tg_Btn_Transmission_Click(object sender, RoutedEventArgs e)
        {
            ChangePageTransmisson();
        }
        private void Tg_Btn_User_Click(object sender, RoutedEventArgs e)
        {
            ChangePageUser();
        }
        private void Tg_Btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            ChangePageSetting();
        }

        public void ChangePageProject()
        {
            if (page_Project == null)
            {
                page_Project = new Page_Project();
                page_Project.Owner = this;
                //page_Project.page_ProjectInstance = new Page_ProjectInstance();
            }
            Change_Page.Content = new Frame()
            {
                Content = page_Project
            };
            Tg_Btn_Project.IsChecked = true;
            Tg_Btn_Transmission.IsChecked = false;
            Tg_Btn_User.IsChecked = false;
            Tg_Btn_Setting.IsChecked = false;
            page_Project.RefreshProject();
            if(page_Project.page_ProjectInstance.page_ProjectInstance_Project.FileList != null)
                page_Project.page_ProjectInstance.page_ProjectInstance_Project.FileList.Clear();
        }
        private void ChangePageTransmisson()
        {
            if (page_Transmisson == null)
            {
                page_Transmisson = new Page_Transmisson();
                page_Transmisson.Owner = this;
            }
            Change_Page.Content = new Frame()
            {
                Content = page_Transmisson
            };
            Tg_Btn_Project.IsChecked = false;
            Tg_Btn_Transmission.IsChecked = true;
            Tg_Btn_User.IsChecked = false;
            Tg_Btn_Setting.IsChecked = false;
        }

        public void ChangePageUser()
        {
            if (page_User == null)
            {
                page_User = new Page_User();
                page_User.Owner = this;
            }
            Change_Page.Content = new Frame()
            {
                Content = page_User
            };
            Tg_Btn_Project.IsChecked = false;
            Tg_Btn_Transmission.IsChecked = false;
            Tg_Btn_User.IsChecked = true;
            Tg_Btn_Setting.IsChecked = false;
            page_User.CheckLoginState();
        }

        private void ChangePageSetting()
        {
            if (page_Setting == null)
            {
                page_Setting = new Page_Setting();
                page_Setting.Owner = this;
            }
            Change_Page.Content = new Frame()
            {
                Content = page_Setting
            };
            Tg_Btn_Project.IsChecked = false;
            Tg_Btn_Transmission.IsChecked = false;
            Tg_Btn_User.IsChecked = false;
            Tg_Btn_Setting.IsChecked = true;
            page_Setting.EnterPage();
        }

        #endregion

        public string ToTimeString(DateTime time)
        {
            return time.Year + "-" + time.Month.ToString().PadLeft(2, '0') + "-" + time.Day.ToString().PadLeft(2, '0');
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        public string fileSaveAddress =  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Co-Work";

        public void RememberPassword(string id, string password)
        {
            ini.WriteIni("Setting", "RememberPassword", "true");
            ini.WriteIni("TNUOCA", "RESU", id);
            ini.WriteIni("TNUOCA", "DROWSSAP", password);
        }

        public void UnRememberPassword()
        {
            ini.WriteIni("Setting", "RememberPassword", "false");
            ini.WriteIni("TNUOCA", "RESU", "");
            ini.WriteIni("TNUOCA", "DROWSSAP", "");
        }

    }
}
