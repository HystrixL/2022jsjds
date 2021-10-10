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
using Co_work.Windows;

namespace Co_work.Pages
{
    /// <summary>
    /// Page_ProjectInstance_Project.xaml 的交互逻辑
    /// </summary>
    public partial class Page_ProjectInstance_Project : Page
    {
        public Page_ProjectInstance_Project()
        {
            InitializeComponent();
        }

        public Page_ProjectInstance Owner;

        

        class FileItem
        { 
            public string name { get; set;}
            public string uper { get; set; }
            public string date { get; set; }
            public string size { get; set; }
        }

        private void Btn_Upload_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                string fileAddress = dialog.FileName;
                string fileName = System.IO.Path.GetFileName(fileAddress);
                FileItem file = new FileItem { name = fileName };
                ListViewItem item = new ListViewItem();
                item.Content = file;
                Lv_File.Items.Add(item);
                item.MouseRightButtonUp += ListViewItem_MouseRightButtonUp;
                item.ContextMenu = MenuFile();
            }
        }

        void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        void ListViewItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            

        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {

        }

        public string newFolderName;

        private void Btn_NewFolder_Click(object sender, RoutedEventArgs e)
        {
            Window_NewFolder window = new Window_NewFolder();
            window.Owner1 = this;
            window.ShowDialog();
        }

        public void CreateNewFolder()
        {
            FileItem file = new FileItem { name = newFolderName };
            ListViewItem item = new ListViewItem();
            item.Content = file;
            Lv_File.Items.Add(item);
            item.MouseDoubleClick += ListViewItem_MouseDoubleClick;
            item.MouseRightButtonUp += ListViewItem_MouseRightButtonUp;
            item.ContextMenu = MenuFolder();
        }

        private ContextMenu MenuFile()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem itemRename = new MenuItem { Name = "Rename", Header = "重命名" };
            itemRename.Click += Rename_Click;
            MenuItem itemDelete = new MenuItem { Name = "Delete", Header = "删除文件" };
            itemDelete.Click += DeleteFile_Click;
            MenuItem itemDownload = new MenuItem { Name = "Download", Header = "下载文件" };
            itemDownload.Click += Download_Click;
            menu.Items.Add(itemRename);
            menu.Items.Add(itemDelete);
            menu.Items.Add(itemDownload);

            return menu;
        }

        private ContextMenu MenuFolder()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem itemRename = new MenuItem { Name = "Rename", Header = "重命名" };
            itemRename.Click += Rename_Click;
            MenuItem itemDelete = new MenuItem { Name = "Delete", Header = "删除文件夹" };
            itemDelete.Click += DeleteFolder_Click;
            menu.Items.Add(itemRename);
            menu.Items.Add(itemDelete);

            return menu;
        }

    }
}
