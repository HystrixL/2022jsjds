using Co_Work.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Co_work.Windows;
using System.Threading;
using Co_Work.Network;

namespace Co_work.Pages
{
    /// <summary>
    /// Page_ProjectInstance_Member.xaml 的交互逻辑
    /// </summary>
    public partial class Page_ProjectInstance_Member : Page
    {
        public Page_ProjectInstance_Member()
        {
            InitializeComponent();
            Lv_Member.ItemsSource = Members;
        }

        public Page_ProjectInstance Owner;

        public BindingList<Employee> Members = new BindingList<Employee>();

        //public class Member
        //{
        //    public Employee MemberInfo { get; set; }
        //}

        //public List<Member> MemberList = new List<Member>();

        public void RefreshMember()
        {
            Members.Clear();
            foreach (Employee member in Owner.Owner.project[Owner.Owner.selectIndex].Members)
                Members.Add(member);
        }

        private void ListViewItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //(Employee)Lv_Member.SelectedItem).GUID
            Menu(sender as ListViewItem);
        }

        private ContextMenu Menu(ListViewItem sender)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem itemRemove = new MenuItem { Name = "Delete", Header = "移除成员" };
            itemRemove.Click += Remove_Click;
            menu.Items.Add(itemRemove);
            sender.ContextMenu = menu;

            return menu;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Owner.Owner.membersGUID.Remove((Lv_Member.SelectedItem as Employee).GUID);

            Thread sendT;
            sendT = new Thread(Owner.Owner.Owner.SendMessageRemoveMember);
            sendT.Start();
        }

        public void ReceiveMessageRemove() //接收消息
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
                            RefreshMember();
                        }));

                        break;
                    }
                }
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Window_AddMember window = new Window_AddMember();
            window.Owner = this;
            window.ShowDialog();
        }

        public string newMemberId;

        public void AddMember()
        {
            Thread sendT;
            sendT = new Thread(Owner.Owner.Owner.SendMessageGetUserGUID);
            sendT.Start();
        }

        public void ReceiveMessageGetUserGUID() //接收消息
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
                        var received = TransData<Response.GetEmployeeInfoFromId>.Convert(message);

                        if (Owner.Owner.project[Owner.Owner.selectIndex].AddMember(received.Content.Employee))
                        {
                            Owner.Owner.membersGUID.Add(received.Content.Employee.GUID);

                            Thread sendT;
                            sendT = new Thread(Owner.Owner.Owner.SendMessageAddMember);
                            sendT.Start();
                        }

                        break;
                    }
                }
            }
        }

        public void ReceiveMessageAddMember() //接收消息
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
                            RefreshMember();
                        }));

                        break;
                    }
                }
            }
        }
    }
}
