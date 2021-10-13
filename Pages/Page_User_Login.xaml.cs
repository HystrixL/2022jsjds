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
    /// Page_User_Login.xaml 的交互逻辑
    /// </summary>
    public partial class Page_User_Login : Page
    {
        public Page_User_Login()
        {
            InitializeComponent();
        }

        public Page_User Owner;

        public Page_User_Register page_User_Register;

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (Tb_Id.Text == "" || Tb_Password.Text == "")
            {
                MessageBox.Show("请填写用户名或密码", "错误", MessageBoxButton.OK);
            }

        }
        
        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            if (page_User_Register == null)
            {
                page_User_Register = new Page_User_Register();
                page_User_Register.Owner = this;
            }
            Owner.Owner.Change_Page.Content = new Frame()
            {
                Content = page_User_Register
            };

            page_User_Register.Tb_Id.Text = "";
            page_User_Register.Tb_Name.Text = "";
            page_User_Register.Tb_Password.Password = "";
            page_User_Register.Tb_Password_Repeat.Password = "";
            page_User_Register.Lb_Id_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            page_User_Register.Lb_Name_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            page_User_Register.Lb_Password_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            page_User_Register.Lb_RePassword_Instru.Foreground = new SolidColorBrush(Colors.Gray);

            page_User_Register.Tb_Id.Focus();
        }
    }
}
