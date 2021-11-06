using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Diagnostics;
using Co_work;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Co_Work.Network;
using Co_Work.Core.Project;
using Co_Work.Core;

namespace Co_work.Pages
{
    /// <summary>
    /// Page_Project.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Project : Page
    {
        public Page_Project()
        {
            InitializeComponent();
            //project = new Projects[255];

            if (page_ProjectInstance == null)
                page_ProjectInstance = new Page_ProjectInstance();
            page_ProjectInstance.Owner = this;
        }

        //public struct Projects
        //{
        //    public string Name;
        //    public string Intro;
        //    public string Creator;
        //    public string StartTime;
        //    public string Deadline;
        //    public float Progress;
        //}

        public List<Project> project = new List<Project>();
        public Project newProject = new Project();

        public List<string> membersGUID = new List<string>();

        //public Projects[] project;
        //public int projectIndex = -1;


        public void ReceiveMessageUpdate() //接收消息
        {
            int length = 0;
            while (Owner.isConnected)
            {
                if (Owner.clientScoket.Connected == true)
                {
                    try
                    {
                        length = Owner.clientScoket.Receive(Owner.data);
                    }
                    catch (Exception e)
                    {
                        Owner.isConnected = false;
                    }

                    if (length != 0)
                    {
                        string message = Encoding.UTF8.GetString(Owner.data, 0, length);
                        var received = TransData<Response.GetProjectsInfoFromEmployee>.Convert(message);
                        project = received.Content.Projects;

                        

                        Dispatcher.Invoke(new Action(delegate
                        {
                            //清空已有按钮
                            for (int i = 0; i <= project.Count; i++)
                            {
                                Button btn = WP.FindName("Btn_Project_" + i.ToString()) as Button;
                                if (btn != null)
                                {
                                    WP.Children.Remove(btn);
                                    WP.UnregisterName("Btn_Project_" + i.ToString());
                                }
                            }
                            Btn_AddProject.Margin = new Thickness(20, 20, 0, 0);
                            //重新创建按钮
                            for (int i = 0; i <= project.Count - 1; i++)
                            {
                                Thickness Mov;

                                Mov = Btn_AddProject.Margin;

                                Button ProjectBtn = new Button();
                                ProjectBtn.Width = 200;
                                ProjectBtn.Height = 185;
                                ProjectBtn.Margin = Mov;
                                ProjectBtn.VerticalAlignment = VerticalAlignment.Top;
                                ProjectBtn.HorizontalAlignment = HorizontalAlignment.Left;
                                ProjectBtn.BorderThickness = new Thickness(0);
                                ProjectBtn.Background = new SolidColorBrush(Colors.White);
                                WP.Children.Add(ProjectBtn);
                                WP.RegisterName("Btn_Project_" + i.ToString(), ProjectBtn);
                                ProjectBtn.Name = "Btn_Project_" + i.ToString();

                                TextBlock ProjectInfo = new TextBlock();
                                ProjectInfo.Width = 200;
                                ProjectInfo.Height = 185;
                                ProjectInfo.FontSize = 12;

                                string endDate;
                                if (project[i].EndDate == DateTime.MinValue)
                                    endDate = "无";
                                else
                                    endDate = project[i].EndDate.ToLongDateString();

                                ProjectInfo.Text = "项目名称：" + project[i].Name + Environment.NewLine + Environment.NewLine + "简介：" + project[i].Note + Environment.NewLine + "创建者：" + project[i].Creator.Name + Environment.NewLine + "创建日期：" + project[i].StartDate.ToLongDateString() + Environment.NewLine + "截止日期：" + endDate + Environment.NewLine + "进度：" + project[i].ProgressRate + "%";
                                ProjectInfo.Margin = new Thickness(5, 5, 0, 0);
                                ProjectInfo.Foreground = new SolidColorBrush(Colors.Black);
                                ProjectInfo.VerticalAlignment = VerticalAlignment.Top;
                                ProjectInfo.HorizontalAlignment = HorizontalAlignment.Left;
                                ProjectInfo.MaxWidth = 195;
                                ProjectInfo.TextWrapping = TextWrapping.Wrap;

                                ProjectBtn.Content = ProjectInfo;

                                ProjectBtn.Click += ProjectBtn_Click;


                                if ((i + 1) % 5 == 0)
                                {
                                    Thickness Mov2;
                                    Mov2 = Mov;
                                    Mov2.Left = 20;
                                    Mov2.Top += 205;
                                    Btn_AddProject.Margin = Mov2;
                                    WP.Height = Mov2.Top + 800;
                                }
                                else
                                {
                                    Thickness Mov2;
                                    Mov2 = Mov;
                                    Mov2.Left += 210;
                                    Btn_AddProject.Margin = Mov2;
                                    WP.Height = Mov2.Top + 800;
                                }
                            }
                        }));

                        break;
                    }
                }
            }
        }

        public void ReceiveMessageCreate() //接收消息
        {
            int length = 0;
            while (Owner.isConnected)
            {
                if (Owner.clientScoket.Connected == true)
                {
                    try
                    {
                        length = Owner.clientScoket.Receive(Owner.data);
                    }
                    catch (Exception e)
                    {
                        Owner.isConnected = false;
                    }

                    if (length != 0)
                    {
                        string message = Encoding.UTF8.GetString(Owner.data, 0, length);
                        var received = TransData<Response.CreatProject>.Convert(message);

                        Dispatcher.Invoke(new Action(delegate
                        {
                            RefreshProject();
                            Owner.ChangePageProject();
                        }));

                        break;
                    }
                }
            }
        }

        


        private void Btn_AddProject_Click(object sender, RoutedEventArgs e)
        {
            if (Owner.isLogined == true)
            {
                newProject = new Project();
                ChangePageProjectCreate();
            }
            else
                MessageBox.Show("请登录");
        }

        public void CreatProject()
        {
            Thread sendT;
            sendT = new Thread(Owner.SendMessageCreateProject);
            sendT.Start();

            //Thickness Mov;

            //Mov = Btn_AddProject.Margin;

            //Button ProjectBtn = new Button();
            //ProjectBtn.Width = 200;
            //ProjectBtn.Height = 185;
            //ProjectBtn.Margin = Mov;
            //ProjectBtn.VerticalAlignment = VerticalAlignment.Top;
            //ProjectBtn.HorizontalAlignment = HorizontalAlignment.Left;
            //ProjectBtn.BorderThickness = new Thickness(0);
            //ProjectBtn.Background = new SolidColorBrush(Colors.White);
            //Grid1.Children.Add(ProjectBtn);
            //Grid1.RegisterName("Btn_Project_"+ (project.Count-1).ToString(), ProjectBtn);
            //ProjectBtn.Name = "Btn_Project_" + (project.Count-1).ToString();

            //TextBlock ProjectInfo = new TextBlock();
            //ProjectInfo.Width = 200;
            //ProjectInfo.Height = 185;
            //ProjectInfo.FontSize = 12;


            //ProjectInfo.Text = "项目名称：" + project[project.Count-1].Name + Environment.NewLine + Environment.NewLine + "简介：" + project[project.Count-1].Intro + Environment.NewLine + "创建者：" + Environment.NewLine + "创建日期：" + project[project.Count-1].StartTime + Environment.NewLine + "截止日期：" + project[project.Count-1].Deadline + Environment.NewLine + "进度：" + project[project.Count-1].Progress + "%";
            //ProjectInfo.Margin = new Thickness(5, 5, 0, 0);
            //ProjectInfo.Foreground = new SolidColorBrush(Colors.Black);
            //ProjectInfo.VerticalAlignment = VerticalAlignment.Top;
            //ProjectInfo.HorizontalAlignment = HorizontalAlignment.Left;
            //ProjectInfo.MaxWidth = 195;
            //ProjectInfo.TextWrapping = TextWrapping.Wrap;

            //ProjectBtn.Content = ProjectInfo;

            
            //ProjectBtn.Click += ProjectBtn_Click;

            //Thickness Mov2;
            //Mov2 = Mov;
            //Mov2.Left += 220;
            //Btn_AddProject.Margin = Mov2;
        }

        public void RefreshProject()
        {
            if (Owner.isLogined)
            {
                Thread sendT;
                sendT = new Thread(Owner.SendMessageUpdateProjects);
                sendT.Start();
            }
            else
            {
                for (int i = 0; i <= project.Count; i++)
                {
                    Button btn = WP.FindName("Btn_Project_" + i.ToString()) as Button;
                    if (btn != null)
                    {
                        WP.Children.Remove(btn);
                        WP.UnregisterName("Btn_Project_" + i.ToString());
                    }
                }
                Btn_AddProject.Margin = new Thickness(20, 20, 0, 0);
            }
        }

        public MainWindow Owner;
        Page_Project_Create page_ProjectCreate;
        private void ChangePageProjectCreate()
        {
            MainWindow mainWindow = new MainWindow();

            if (page_ProjectCreate == null)
            {
                page_ProjectCreate = new Page_Project_Create();
            }
            page_ProjectCreate.Owner = this;
            (this.Owner as MainWindow).Change_Page.Content = new Frame()
            {
                Content = page_ProjectCreate
            };
            page_ProjectCreate.newProjectName.Focus();
        }

        public Page_ProjectInstance page_ProjectInstance;
        public int selectIndex;

        private void ProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.Owner as MainWindow).Change_Page.Content = new Frame()
            {
                Content = page_ProjectInstance
            };
            page_ProjectInstance.Owner = this;
            page_ProjectInstance.ChangePageProject();
            selectIndex = Convert.ToInt32((sender as Button).Name.Replace("Btn_Project_", ""));
            page_ProjectInstance.Lb_ProjectName.Content = project[selectIndex].Name;
            page_ProjectInstance.page_ProjectInstance_Project.Lb_Intro.Text = "简介：" + Environment.NewLine + project[selectIndex].Note;
            page_ProjectInstance.page_ProjectInstance_Project.Lb_Creator.Content = "创建者：" + project[selectIndex].Creator.Name;
            page_ProjectInstance.page_ProjectInstance_Project.Lb_StartTime.Content = "创建日期：" + project[selectIndex].StartDate.ToLongDateString();
            string endDate;
            if (project[selectIndex].EndDate == DateTime.MinValue)
                endDate = "无";
            else
                endDate = project[selectIndex].EndDate.ToLongDateString();
            page_ProjectInstance.page_ProjectInstance_Project.Lb_Deadline.Content = "截止日期：" + endDate;
            page_ProjectInstance.page_ProjectInstance_Project.Pb_Progress.Value = project[selectIndex].ProgressRate;
            page_ProjectInstance.page_ProjectInstance_Project.Lb_Progress.Content = project[selectIndex].ProgressRate + "%";

            page_ProjectInstance.page_ProjectInstance_Project.SetRootAddress();


            membersGUID.Clear();
            foreach (Employee member in project[selectIndex].Members)
                membersGUID.Add(member.GUID);
            //newProject = project[selectIndex];
        }
    }
}
