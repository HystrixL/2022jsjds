using Co_Work.Network;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Co_work.Pages
{
    /// <summary>
    /// Page_ProjectInstance_Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Page_ProjectInstance_Setting : Page
    {
        public Page_ProjectInstance_Setting()
        {
            InitializeComponent();
        }

        public Page_ProjectInstance Owner;

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (!float.TryParse(Tb_Progress.Text, out float progress))
            {
                MessageBox.Show("进度只能为数字", "错误", MessageBoxButton.OK);
            }
            else if (!(Convert.ToSingle(Tb_Progress.Text) >= 0 && Convert.ToSingle(Tb_Progress.Text) <= 100))
            {
                MessageBox.Show("进度只能在0%到100%之间", "错误", MessageBoxButton.OK);
            }
            else if (Tb_Name.Text == "")
            {
                MessageBox.Show("请填写项目名称", "错误", MessageBoxButton.OK);
            }
            else
            {
                //Owner.Owner.project.RemoveAt(Owner.Owner.selectIndex);
                //Owner.Owner.project.Insert(Owner.Owner.selectIndex, Owner.Owner.newProject);

                //this.Owner.Owner.project[this.Owner.Owner.selectIndex].Name = Tb_Name.Text;
                //this.Owner.Owner.project[this.Owner.Owner.selectIndex].Intro = Tb_Intro.Text;
                //if (Dp_Deadline.SelectedDate == null)
                //    this.Owner.Owner.project[this.Owner.Owner.selectIndex].Deadline = "无";
                //else
                //    this.Owner.Owner.project[this.Owner.Owner.selectIndex].Deadline = Dp_Deadline.Text;
                //this.Owner.Owner.project[this.Owner.Owner.selectIndex].Progress = progress;

                Owner.Owner.newProject.Name = Tb_Name.Text;
                Owner.Owner.newProject.Note = Tb_Intro.Text;
                Owner.Owner.newProject.StartDate = Owner.Owner.project[Owner.Owner.selectIndex].StartDate;
                if (Dp_Deadline.SelectedDate == null)
                    Owner.Owner.newProject.EndDate = DateTime.MinValue;
                else
                    Owner.Owner.newProject.EndDate = Dp_Deadline.SelectedDate.Value;
                Owner.Owner.newProject.ProgressRate = progress;

                Thread sendT;
                sendT = new Thread(Owner.Owner.Owner.SendMessageUpdateProject);
                sendT.Start();

            }
        }

        public void ReceiveMessageUpdate() //接收消息
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
                        var received = TransData<Response.UpdateProject>.Convert(message);

                        Dispatcher.Invoke(new Action(delegate
                        {
                            this.Owner.Lb_ProjectName.Content = Tb_Name.Text;
                            this.Owner.page_ProjectInstance_Project.Lb_Intro.Text = "简介：" + Environment.NewLine + Tb_Intro.Text;
                            if (Dp_Deadline.SelectedDate == null)
                                this.Owner.page_ProjectInstance_Project.Lb_Deadline.Content = "截止日期：无";
                            else
                                this.Owner.page_ProjectInstance_Project.Lb_Deadline.Content = "截止日期：" + Dp_Deadline.Text;
                            this.Owner.page_ProjectInstance_Project.Pb_Progress.Value = float.Parse(Tb_Progress.Text);
                            this.Owner.page_ProjectInstance_Project.Lb_Progress.Content = Tb_Progress.Text + "%";

                            MessageBox.Show("保存成功", "", MessageBoxButton.OK);
                        }));

                        break;
                    }
                }
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定要删除“" + this.Owner.Owner.project[this.Owner.Owner.selectIndex].Name + "”项目吗？", "删除项目", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //Owner.Owner.project.RemoveAt(Owner.Owner.selectIndex);
                //this.Owner.Owner.RefreshProject();
                //this.Owner.Owner.Owner.ChangePageProject();
                Thread sendT;
                sendT = new Thread(Owner.Owner.Owner.SendMessageDeleteProject);
                sendT.Start();
            }
        }

        public void ReceiveMessageDelete() //接收消息
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
                        var received = TransData<Response.DeleteProject>.Convert(message);

                        Dispatcher.Invoke(new Action(delegate
                        {
                            Owner.Owner.RefreshProject();
                            Owner.Owner.Owner.ChangePageProject();
                        }));

                        break;
                    }
                }
            }
        }

        private void Sl_Progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Tb_Progress.Text = e.NewValue.ToString("f1");
        }

        private void Tb_Progress_LostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Tb_Progress.Text, out double d))
                Sl_Progress.Value = double.Parse(Tb_Progress.Text);
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DefaultFocus.Focus();
        }
    }
}
