using Co_Work.Network;
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
    /// Page_Project_Create.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Project_Create : Page
    {
        public Page_Project Owner;
        public Page_Project_Create()
        {
            InitializeComponent();
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (newProjectName.Text == "")
            {
                MessageBox.Show("请填写项目名称", "错误", MessageBoxButton.OK);
            }
            else
            {
                Owner.newProject.Name = newProjectName.Text;
                if (newProjectIntro.Text == "")
                    newProjectIntro.Text = "无";
                Owner.newProject.Note = newProjectIntro.Text;
                Owner.newProject.StartTime = DateTime.Today.Date;
                if (newProjectDeadline.SelectedDate == null)
                    Owner.newProject.Deadline = "无";
                else
                    Owner.newProject.Deadline = newProjectDeadline.Text;
                //Owner.project.Add(Owner.newProject);

                ////if((this.Owner as Page_Project).project[(this.Owner as Page_Project).projectIndex].Name != "")
                //(this.Owner as Page_Project).projectIndex++;
                //(this.Owner as Page_Project).project[(this.Owner as Page_Project).projectIndex].Name = newProjectName.Text;
                //if (newProjectIntro.Text == "")
                //    newProjectIntro.Text = "无";
                //(this.Owner as Page_Project).project[(this.Owner as Page_Project).projectIndex].Intro = newProjectIntro.Text;
                //(this.Owner as Page_Project).project[(this.Owner as Page_Project).projectIndex].StartTime = DateTime.Today.ToLongDateString();
                //if (newProjectDeadline.SelectedDate == null)
                //    (this.Owner as Page_Project).project[(this.Owner as Page_Project).projectIndex].Deadline = "无";
                //else
                //    (this.Owner as Page_Project).project[(this.Owner as Page_Project).projectIndex].Deadline = newProjectDeadline.Text;

                ChangePageProject();
                Owner.CreatProject();
                newProjectName.Text = "";
                newProjectIntro.Text = "";
                newProjectDeadline.Text = "";
            }
        }

        private void ChangePageProject()
        {
            (this.Owner.Owner as MainWindow).Change_Page.Content = new Frame()
            {
                Content = this.Owner
            };
        }
    }
}
