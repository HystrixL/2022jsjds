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
    /// Window_NewFolder.xaml 的交互逻辑
    /// </summary>
    public partial class Window_NewFolder : Window
    {
        public Window_NewFolder()
        {
            InitializeComponent();
        }

        public Page_ProjectInstance_Project Owner1;

        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            if (Lb_FolderName.Text != "")
            {
                Owner1.CreateNewFolder(Lb_FolderName.Text);
                this.Close();
            }
        }
    }
}
