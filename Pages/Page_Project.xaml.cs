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
            project = new Projects[255];
        }

        public struct Projects
        {
            public string Name;
            public string Intro;
            public string Creator;
            public string StartTime;
            public string Deadline;
            public float Progress;
        }

        public Projects[] project;
        public int projectIndex = -1;

        private void Btn_AddProject_Click(object sender, RoutedEventArgs e)
        {
            ChangePageProjectCreate();
        }

        public void CreatProject()
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
            Grid1.Children.Add(ProjectBtn);
            Grid1.RegisterName("Btn_Project_"+ projectIndex.ToString(), ProjectBtn);

            TextBlock ProjectInfo = new TextBlock();
            ProjectInfo.Width = 200;
            ProjectInfo.Height = 100;
            ProjectInfo.FontSize = 12;
            project[projectIndex].StartTime = DateTime.Today.ToLongDateString();
            ProjectInfo.Text = "项目名称：" + project[projectIndex].Name + Environment.NewLine + Environment.NewLine + "简介：" + project[projectIndex].Intro + Environment.NewLine + "创建者：" + Environment.NewLine + "创建日期：" + project[projectIndex].StartTime + Environment.NewLine + "截止日期：" + project[projectIndex].Deadline;
            ProjectInfo.Margin = new Thickness(5, -60, 0, 0);
            ProjectInfo.Foreground = new SolidColorBrush(Colors.Black);
            ProjectInfo.VerticalAlignment = VerticalAlignment.Top;
            ProjectInfo.HorizontalAlignment = HorizontalAlignment.Left;
            ProjectInfo.MaxWidth = 195;
            ProjectInfo.TextWrapping = TextWrapping.Wrap;

            ProjectBtn.Content = ProjectInfo;

            if(page_ProjectInstance == null)
                page_ProjectInstance = new Page_ProjectInstance();
            page_ProjectInstance.Owner = this;
            ProjectBtn.Click += ProjectBtn_Click;

            Thickness Mov2;
            Mov2 = Mov;
            Mov2.Left += 220;
            Btn_AddProject.Margin = Mov2;
        }

        public void RefreshProject()
        {
            //清空已有按钮
            for (int i = 0; i <= projectIndex; i++)
            {
                Button btn = Grid1.FindName("Btn_Project_" + i) as Button;
                Grid1.Children.Remove(btn);
                Grid1.UnregisterName("Btn_Project_" + i);
            }
            Btn_AddProject.Margin = new Thickness(20, 20, 572, 260);
            //重新创建按钮
            for (int i = 0; i <= projectIndex; i++)
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
                Grid1.Children.Add(ProjectBtn);
                Grid1.RegisterName("Btn_Project_" + i.ToString(), ProjectBtn);

                TextBlock ProjectInfo = new TextBlock();
                ProjectInfo.Width = 200;
                ProjectInfo.Height = 100;
                ProjectInfo.FontSize = 12;
                project[projectIndex].StartTime = DateTime.Today.ToLongDateString();
                ProjectInfo.Text = "项目名称：" + project[i].Name + Environment.NewLine + Environment.NewLine + "简介：" + project[i].Intro + Environment.NewLine + "创建者：" + Environment.NewLine + "创建日期：" + project[i].StartTime + Environment.NewLine + "截止日期：" + project[i].Deadline;
                ProjectInfo.Margin = new Thickness(5, -60, 0, 0);
                ProjectInfo.Foreground = new SolidColorBrush(Colors.Black);
                ProjectInfo.VerticalAlignment = VerticalAlignment.Top;
                ProjectInfo.HorizontalAlignment = HorizontalAlignment.Left;
                ProjectInfo.MaxWidth = 195;
                ProjectInfo.TextWrapping = TextWrapping.Wrap;

                ProjectBtn.Content = ProjectInfo;

                ProjectBtn.Click += ProjectBtn_Click;

                Thickness Mov2;
                Mov2 = Mov;
                Mov2.Left += 220;
                Btn_AddProject.Margin = Mov2;
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
        }

        public Page_ProjectInstance page_ProjectInstance;

        private void ProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.Owner as MainWindow).Change_Page.Content = new Frame()
            {
                Content = page_ProjectInstance
            };
            page_ProjectInstance.Owner = this;
            Button btn = sender as Button;
            int index = ((int)btn.Margin.Left + 10) / 220;
            page_ProjectInstance.Lb_ProjectName.Content = project[index].Name;
            page_ProjectInstance.page_ProjectInstance_Project.Lb_Intro.Text = "简介：" + Environment.NewLine + project[index].Intro;
            //page_ProjectInstance.page_ProjectInstance_Project.Lb_Creator.Content = "创建者：" + project[index].Creator;
            page_ProjectInstance.page_ProjectInstance_Project.Lb_StartTime.Content = "创建日期：" + project[index].StartTime;
            page_ProjectInstance.page_ProjectInstance_Project.Lb_Deadline.Content = "截止日期：" + project[index].Deadline;
        }
    }
}
