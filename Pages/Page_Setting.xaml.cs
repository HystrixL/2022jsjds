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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Runtime.InteropServices;
using Co_work.Scripts;

namespace Co_work.Pages
{
    /// <summary>
    /// Page_Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Setting : Page
    {
        public Page_Setting()
        {
            InitializeComponent();
        }

        public void EnterPage()
        {
            Tb_SaveAddress.Text = Owner.fileSaveAddress;
        }

        public INI ini = new INI();

        public MainWindow Owner;

        private void Btn_SaveAddress_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            //dialog.ShowDialog();
            if ((int)dialog.ShowDialog() == 1)
            {
                Owner.fileSaveAddress = dialog.FileName;
                Tb_SaveAddress.Text = Owner.fileSaveAddress;
                ini.WriteIni("Setting", "fileSaveAddress", Owner.fileSaveAddress);
            }
        }

        private void Btn_Default_Click(object sender, RoutedEventArgs e)
        {
            Owner.SetDefault();
            Tb_SaveAddress.Text = Owner.fileSaveAddress;
        }
    }
}
