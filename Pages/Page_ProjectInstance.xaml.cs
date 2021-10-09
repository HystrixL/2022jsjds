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

namespace Co_work.Pages
{
    /// <summary>
    /// Page_ProjectInstance.xaml 的交互逻辑
    /// </summary>
    public partial class Page_ProjectInstance : Page
    {
        public Page_ProjectInstance()
        {
            InitializeComponent();
            if (page_ProjectInstance_Project == null)
            {
                page_ProjectInstance_Project = new Page_ProjectInstance_Project();
            }
            page_ProjectInstance_Project.Owner = this;
            Change_Page.Content = new Frame()
            {
                Content = page_ProjectInstance_Project
            };
        }

        public Page_Project Owner;

        public Page_ProjectInstance_Project page_ProjectInstance_Project;
        public Page_ProjectInstance_Member page_ProjectInstance_Member;
        public Page_ProjectInstance_Setting page_ProjectInstance_Setting;

        private void Btn_Project_Click(object sender, RoutedEventArgs e)
        { 
            Lb_Project.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 21, 255));
            Lb_Member.Foreground = new SolidColorBrush(Colors.Black);
            Lb_Setting.Foreground = new SolidColorBrush(Colors.Black);
            Select_Effect.Margin = new Thickness(25, 38, 0, 0);

            if (page_ProjectInstance_Project == null)
            {
                page_ProjectInstance_Project = new Page_ProjectInstance_Project();
            }
            page_ProjectInstance_Project.Owner = this;
            Change_Page.Content = new Frame()
            {
                Content = page_ProjectInstance_Project
            };
        }

        private void Btn_Member_Click(object sender, RoutedEventArgs e)
        {
            Lb_Project.Foreground = new SolidColorBrush(Colors.Black);
            Lb_Member.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 21, 255));
            Lb_Setting.Foreground = new SolidColorBrush(Colors.Black);
            Select_Effect.Margin = new Thickness(105, 38, 0, 0);

            if (page_ProjectInstance_Member == null)
            {
                page_ProjectInstance_Member = new Page_ProjectInstance_Member();
            }
            page_ProjectInstance_Member.Owner = this;
            Change_Page.Content = new Frame()
            {
                Content = page_ProjectInstance_Member
            };
        }

        private void Btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            Lb_Project.Foreground = new SolidColorBrush(Colors.Black);
            Lb_Member.Foreground = new SolidColorBrush(Colors.Black);
            Lb_Setting.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 21, 255));
            Select_Effect.Margin = new Thickness(185, 38, 0, 0);

            if (page_ProjectInstance_Setting == null)
            {
                page_ProjectInstance_Setting = new Page_ProjectInstance_Setting();
            }
            page_ProjectInstance_Setting.Owner = this;
            Change_Page.Content = new Frame()
            {
                Content = page_ProjectInstance_Setting
            };

            page_ProjectInstance_Setting.Tb_Name.Text = this.Owner.project[this.Owner.projectIndex].Name;
            page_ProjectInstance_Setting.Tb_Intro.Text = this.Owner.project[this.Owner.projectIndex].Intro;
            page_ProjectInstance_Setting.Dp_Deadline.Text = this.Owner.project[this.Owner.projectIndex].Deadline;
            page_ProjectInstance_Setting.Tb_Progress.Text = this.Owner.project[this.Owner.projectIndex].Progress.ToString();
        }
    }
}
