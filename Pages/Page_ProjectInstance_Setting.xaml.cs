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
            { }
            else if (Tb_Name.Text != "" && Convert.ToSingle(Tb_Progress.Text) >= 0 && Convert.ToSingle(Tb_Progress.Text) <= 100)
            {
                this.Owner.Owner.project[this.Owner.Owner.projectIndex].Name = Tb_Name.Text;
                this.Owner.Owner.project[this.Owner.Owner.projectIndex].Intro = Tb_Intro.Text;
                if (Dp_Deadline.SelectedDate == null)
                    this.Owner.Owner.project[this.Owner.Owner.projectIndex].Deadline = "无";
                else
                    this.Owner.Owner.project[this.Owner.Owner.projectIndex].Deadline = Dp_Deadline.Text;
                this.Owner.Owner.project[this.Owner.Owner.projectIndex].Progress = progress;

                this.Owner.Lb_ProjectName.Content = Tb_Name.Text;
                this.Owner.page_ProjectInstance_Project.Lb_Intro.Text = "简介："  + Environment.NewLine + Tb_Intro.Text;
                if (Dp_Deadline.SelectedDate == null)
                    this.Owner.page_ProjectInstance_Project.Lb_Deadline.Content = "截止日期：无";
                else
                    this.Owner.page_ProjectInstance_Project.Lb_Deadline.Content = "截止日期：" + Dp_Deadline.Text;
                this.Owner.page_ProjectInstance_Project.Pb_Progress.Value = progress;
                this.Owner.page_ProjectInstance_Project.Lb_Progress.Content = Tb_Progress.Text + "%";
            }
        }

    }
}
