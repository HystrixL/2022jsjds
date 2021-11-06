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
using System.Windows.Shapes;
using Co_work.Pages;

namespace Co_work.Windows
{
    /// <summary>
    /// Window_AddMember.xaml 的交互逻辑
    /// </summary>
    public partial class Window_AddMember : Window
    {
        public Window_AddMember()
        {
            InitializeComponent();
        }

        public Page_ProjectInstance_Member Owner;

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Owner.newMemberId = Lb_Id.Text;
            Owner.AddMember();
            this.Close();
        }
    }
}
