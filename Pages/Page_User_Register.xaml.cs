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
    /// Page_User_Register.xaml 的交互逻辑
    /// </summary>
    public partial class Page_User_Register : Page
    {
        public Page_User_Register()
        {
            InitializeComponent();
        }

        public Page_User_Login Owner;

        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRegister())
            {
            
            }
        }

        private bool CheckRegister()
        {
            bool result = true;

            Lb_Id_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            Lb_Name_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            Lb_Password_Instru.Foreground = new SolidColorBrush(Colors.Gray);
            Lb_RePassword_Instru.Foreground = new SolidColorBrush(Colors.Gray);

            if (Tb_Id.Text == "" || !System.Text.RegularExpressions.Regex.IsMatch(Tb_Id.Text, @"^[A-Za-z0-9_]+$"))
            {
                Lb_Id_Instru.Foreground = new SolidColorBrush(Colors.Red);
                result = false;
            }
            if (Tb_Name.Text == "")
            {
                Lb_Name_Instru.Foreground = new SolidColorBrush(Colors.Red);
                result = false;
            }
            if (Tb_Password.Password == "" || !System.Text.RegularExpressions.Regex.IsMatch(Tb_Password.Password, @"^[A-Za-z0-9]{8,20}$"))
            {
                Lb_Password_Instru.Foreground = new SolidColorBrush(Colors.Red);
                result = false;
            }
            if (Tb_Password_Repeat.Password == "" || Tb_Password_Repeat.Password != Tb_Password.Password)
            {
                Lb_RePassword_Instru.Foreground = new SolidColorBrush(Colors.Red);
                result = false;
            }
            return result;
        }
    }
}
