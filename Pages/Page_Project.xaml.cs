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
        }

        public string newProjectName;
        public string newProjectIntro;
        public string newProjectStartTime;
        public string newProjectDeadline;

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

            TextBlock ProjectInfo = new TextBlock();
            ProjectInfo.Width = 200;
            ProjectInfo.Height = 100;
            ProjectInfo.FontSize = 12;
            ProjectInfo.Text = "项目名称：" + newProjectName + Environment.NewLine + Environment.NewLine + "简介：" + newProjectIntro + Environment.NewLine + "创建者：" + Environment.NewLine + "创建日期：" + DateTime.Today.ToLongDateString() + Environment.NewLine + "截止日期：" + newProjectDeadline;
            ProjectInfo.Margin = new Thickness(5, -60, 0, 0);
            ProjectInfo.Foreground = new SolidColorBrush(Colors.Black);
            ProjectInfo.VerticalAlignment = VerticalAlignment.Top;
            ProjectInfo.HorizontalAlignment = HorizontalAlignment.Left;
            ProjectInfo.MaxWidth = 195;
            ProjectInfo.TextWrapping = TextWrapping.Wrap;

            ProjectBtn.Content = ProjectInfo;

            page_ProjectInstance = new Page_ProjectInstance();
            page_ProjectInstance.Owner = this;
            ProjectBtn.Click += ProjectBtn_Click;

            Thickness Mov2;
            Mov2 = Mov;
            Mov2.Left += 220;
            Btn_AddProject.Margin = Mov2;
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
        }
    }
}
