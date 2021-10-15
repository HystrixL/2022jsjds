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
    /// Window_RenameFile.xaml 的交互逻辑
    /// </summary>
    public partial class Window_RenameFile : Window
    {
        public Window_RenameFile()
        {
            InitializeComponent();
            Lb_FileName.Focus();
        }

        public Page_ProjectInstance_Project Owner1;

        private void Btn_Rename_Click(object sender, RoutedEventArgs e)
        {
            if (Lb_FileName.Text != "")
            {
                Owner1.Rename(Lb_FileName.Text);
                this.Close();
            }
        }
    }
}
