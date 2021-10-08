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
        }

        public Page_Project Owner;

        private void Btn_Project_Click(object sender, RoutedEventArgs e)
        { 
            Lb_Project.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 21, 255));
            Lb_Member.Foreground = new SolidColorBrush(Colors.Black);
            Lb_Setting.Foreground = new SolidColorBrush(Colors.Black);
            Select_Effect.Margin = new Thickness(25, 38, 0, 0);
        }

        private void Btn_Member_Click(object sender, RoutedEventArgs e)
        {
            Lb_Project.Foreground = new SolidColorBrush(Colors.Black);
            Lb_Member.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 21, 255));
            Lb_Setting.Foreground = new SolidColorBrush(Colors.Black);
            Select_Effect.Margin = new Thickness(105, 38, 0, 0);
        }

        private void Btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            Lb_Project.Foreground = new SolidColorBrush(Colors.Black);
            Lb_Member.Foreground = new SolidColorBrush(Colors.Black);
            Lb_Setting.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 21, 255));
            Select_Effect.Margin = new Thickness(185, 38, 0, 0);
        }
    }
}
