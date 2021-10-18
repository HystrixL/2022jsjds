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
using Co_work.Pages;

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

        private void ChangePageUser()
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
            }
            Change_Page.Content = new Frame()
            {
                Content = page_Setting
            };
            Tg_Btn_Project.IsChecked = false;
            Tg_Btn_Transmission.IsChecked = false;
            Tg_Btn_User.IsChecked = false;
            Tg_Btn_Setting.IsChecked = true;
        }
        #endregion


    }
}
