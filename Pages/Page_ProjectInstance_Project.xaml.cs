using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using FTPHelper;

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
            Lv_File.ContextMenu = MenuListView();
            //Pb_Upload_Progress.Visibility = Visibility.Hidden;

            string baseAddress = "/Test/";
            Address.Add(baseAddress);
        }

        public Page_ProjectInstance Owner;

        public FtpHelper ftpHelper = new FtpHelper("103.193.189.241", "Administrator", "adxq@9139");

        public List<string> Address = new List<string>();

        public class FileItem : INotifyPropertyChanged
        {
            public string name { get; set; }
            public string uper { get; set; }
            public string date { get; set; }
            public string size { get; set; }
            public int type { get; set; }//0为文件夹，1为文件，3为返回上一目录

            #region // INotifyPropertyChanged成员
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, e);
                }
            }
            #endregion
        }

        public string CurrentAddress()
        {
            string result = "";
            foreach (string item in Address)
            {
                result += item;
            }
            return result;
        }

        private void Btn_Upload_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                string fileAddress = dialog.FileName;
                string fileName = System.IO.Path.GetFileName(fileAddress);

                Owner.Owner.Owner.InitializePageTransmisson();
                Owner.Owner.Owner.page_Transmisson.InitializePages();
                Owner.Owner.Owner.page_Transmisson.page_Transmission_Upload.CreateList(fileAddress, fileName, Owner.Owner.project[Owner.Owner.selectIndex].Name);
            }
        }

        public void StartFileUpload(string fileAddress, string fileName)
        {
            FileUploadAsync(fileAddress, fileName);
        }

        private async Task FileUploadAsync(string fileAddress, string fileName)
        {
            await Task.Run(() => FileUploadTaskAsync(fileAddress, fileName));
            RefreshListView();
        }

        //int Index = 0;
        //List<float> list = new List<float>();

        //private void PercentUpdate(string fileAddress, string fileName,int index)
        //{
        //    Index++;
        //    float percent = 0;
        //    Task.Run(() => FileUpload(fileAddress, fileName, ref percent));
        //    while (percent < 100)
        //    {
        //        list[index] = percent;
        //    }
        //}

        private void FileUploadTaskAsync(string fileAddress, string fileName)
        {
            //Dispatcher.Invoke(new Action(delegate { Pb_Upload_Progress.Visibility = Visibility.Visible; }));

            //list.Add(0);
            //Task.Run(() => PercentUpdate(fileAddress, fileName, Index));
            //Index++;
            float percent = 0;
            Task.Run(() => FileUpload(fileAddress, fileName, ref percent));

            while (true)
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    //for (int i = 0; i < Index; i++)
                    //{
                    //    if (Owner.Owner.Owner.page_Transmisson.page_Transmission_Upload.itemPercents.Count > i)
                    //    {
                    //        Owner.Owner.Owner.page_Transmisson.page_Transmission_Upload.itemPercents[i].percent =
                    //            list[i];
                    //        //Pb_Upload_Progress.Value = list[i];
                    //    }
                    //}

                    //for (int i = 0; i < Index; i++)
                    //{
                        //if (Owner.Owner.Owner.page_Transmisson.page_Transmission_Upload.itemPercents.Count > i)
                        //{
                            Owner.Owner.Owner.page_Transmisson.page_Transmission_Upload.itemPercents[0].percent = percent;
                            //Pb_Upload_Progress.Value = list[i];
                        //}
                    //}
                    
                }));

                if (Owner.Owner.Owner.page_Transmisson.page_Transmission_Upload.itemPercents[0].percent > 99.9)
                {
                    
                    Dispatcher.Invoke(new Action(delegate
                    {
                        Owner.Owner.Owner.page_Transmisson.page_Transmission_Upload.RemoveFirstItem();
                    }));
                    break;
                }

                //if (list[list.Count - 1] == 100)
                //{
                //    Dispatcher.Invoke(new Action(delegate
                //    {
                //        Pb_Upload_Progress.Value = 100;
                //        Pb_Upload_Progress.Visibility = Visibility.Hidden;
                //    }));
                //    break;
                //}
            }
        }

        private void FileUpload(string fileAddress, string fileName, ref float percent)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileAddress);
            ftpHelper.Upload(fileInfo, CurrentAddress() + fileName, ref percent);
        }

        //private void FileItemCreate(string fileName, string fileSize)
        //{
        //    FileItem file = new FileItem { name = fileName, size = fileSize };
        //    ListViewItem item = new ListViewItem();
        //    item.Content = file;
        //    item.DataContext = file;
        //    Lv_File.Items.Add(item);
        //    //item.ContextMenu = MenuFile();
        //}

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            Window_RenameFile window = new Window_RenameFile();
            window.Owner1 = this;
            window.Lb_FileName.Text = (Lv_File.SelectedItem as FileItem).name;
            window.ShowDialog();
        }

        public async Task Rename(string newFileName)
        {
            await Task.Run(() =>
                Dispatcher.Invoke(new Action(delegate
                {
                    ftpHelper.Rename(CurrentAddress() + (Lv_File.SelectedItem as FileItem).name, newFileName);
                })));
            RefreshListView();
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            DeleteFile();
        }

        private async Task DeleteFile()
        {
            if (Lv_File.SelectedItem as FileItem != null)
            {
                await Task.Run(() =>
                    Dispatcher.Invoke(new Action(delegate
                    {
                        ftpHelper.Delete(CurrentAddress() + (Lv_File.SelectedItem as FileItem).name);
                    })));
            }
            RefreshListView();
        }


        private void DeleteFolder_Click(object sender, RoutedEventArgs e)
        {
            DeleteFolder();
        }

        private async Task DeleteFolder()
        {
            if (Lv_File.SelectedItem as FileItem != null)
            {
                await Task.Run(() =>
                    Dispatcher.Invoke(new Action(delegate
                    {
                        ftpHelper.DeleteDir(CurrentAddress() + (Lv_File.SelectedItem as FileItem).name);
                    })));
            }    
            RefreshListView();
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            if (ftpHelper.GetFile(CurrentAddress()).Contains((Lv_File.SelectedItem as FileItem).name))
            {
                Owner.Owner.Owner.InitializePageTransmisson();
                Owner.Owner.Owner.page_Transmisson.InitializePages();

                if (Directory.Exists(Owner.Owner.Owner.fileSaveAddress + "/" + Owner.Owner.project[Owner.Owner.selectIndex].Name) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(Owner.Owner.Owner.fileSaveAddress + "/" + Owner.Owner.project[Owner.Owner.selectIndex].Name);
                }
                if (File.Exists(Owner.Owner.Owner.fileSaveAddress + "/" + Owner.Owner.project[Owner.Owner.selectIndex].Name + "/" + (Lv_File.SelectedItem as FileItem).name))
                {
                    MessageBoxButton button = MessageBoxButton.YesNo;
                    if (MessageBox.Show("目标文件夹内已有同名文件，是否覆盖？", "", button) == MessageBoxResult.Yes)
                    {
                        File.Delete(Owner.Owner.Owner.fileSaveAddress + "/" + Owner.Owner.project[Owner.Owner.selectIndex].Name + "/" + (Lv_File.SelectedItem as FileItem).name);
                        Owner.Owner.Owner.page_Transmisson.page_Transmission_Download.CreateList(Owner.Owner.Owner.fileSaveAddress + "/" + Owner.Owner.project[Owner.Owner.selectIndex].Name + "/" + (Lv_File.SelectedItem as FileItem).name, (Lv_File.SelectedItem as FileItem).name, Owner.Owner.project[Owner.Owner.selectIndex].Name);
                    }
                }
                else
                    Owner.Owner.Owner.page_Transmisson.page_Transmission_Download.CreateList(Owner.Owner.Owner.fileSaveAddress + "/" + Owner.Owner.project[Owner.Owner.selectIndex].Name + "/" + (Lv_File.SelectedItem as FileItem).name, (Lv_File.SelectedItem as FileItem).name, Owner.Owner.project[Owner.Owner.selectIndex].Name);
            }
            else
            {
                MessageBox.Show("远程服务器不存在目标文件");
            }
        }

        public async void StartFileDownload(string fileAddress, string fileName)
        {
            await Task.Run(() => FileDownload(fileAddress, fileName));
        }

        private void FileDownload(string fileAddress, string fileName)
        {
            float percent = 0;

            Task.Run(() =>ftpHelper.Download(CurrentAddress() + fileName, fileAddress, ref percent));

            while (true)
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    Owner.Owner.Owner.page_Transmisson.page_Transmission_Download.itemPercents[0].percent = percent;

                }));

                if (Owner.Owner.Owner.page_Transmisson.page_Transmission_Download.itemPercents[0].percent > 99.9)
                {

                    Dispatcher.Invoke(new Action(delegate
                    {
                        Owner.Owner.Owner.page_Transmisson.page_Transmission_Download.RemoveFirstItem();
                    }));
                    break;
                }
            }
        }

        public BindingList<FileItem> FileList = new BindingList<FileItem>();

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshListView();
        }

        public void RefreshListView()
        {
            Task.Run(async() => await RefreshListViewAsync());
        }

        private async Task RefreshListViewAsync()
        {
            List<string> listFolder = ftpHelper.GetDirctory(CurrentAddress());
            List<string> listFile = ftpHelper.GetFile(CurrentAddress());

            Dispatcher.Invoke(new Action(delegate 
            {
                //Lv_File.Items.Clear(); 
                FileList.Clear();
            }));

            if (Address.Count != 1)
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    FileList.Add(new FileItem { name = "..", type = 3 });
                }));
            }

            foreach (var itemFolder in listFolder)
            {
                Dispatcher.Invoke(new Action(delegate 
                {
                    FileList.Add(new FileItem { name = itemFolder, type = 0 });
                    //CreateNewFolderItem(itemFolder); 
                }));
            }

            foreach (var itemFile in listFile)
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    //FileItemCreate(itemFile, ftpHelper.GetFileSize("/Test/" + itemFile).ToString());
                    FileList.Add(new FileItem { name = itemFile, size = ftpHelper.GetFileSize(CurrentAddress() + itemFile).ToString(), type = 1 });
                    //FileItemCreate(itemFile, "0");
                }));
            }
            Dispatcher.Invoke(new Action(delegate
            {
                Lv_File.ItemsSource = FileList;
            }));
            
        }

        private void Btn_NewFolder_Click(object sender, RoutedEventArgs e)
        {
            Window_NewFolder window = new Window_NewFolder();
            window.Owner1 = this;
            window.ShowDialog();
        }

        public void CreateNewFolder(string newFolderName)
        {
            ftpHelper.CreateDir(CurrentAddress() + newFolderName);
            RefreshListView();
        }

        //public void CreateNewFolderItem(string newFolderName)
        //{
        //    FileItem file = new FileItem { name = newFolderName };
        //    ListViewItem item = new ListViewItem();
        //    item.Content = file;
        //    Lv_File.Items.Add(item);
        //    item.MouseDoubleClick += ListViewItem_MouseDoubleClick;
        //    item.DataContext = file;
        //    //item.ContextMenu = MenuFolder();
        //}

        public ContextMenu MenuFile(ListViewItem sender)
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
            sender.ContextMenu = menu;

            return menu;
        }

        private ContextMenu MenuFolder(ListViewItem sender)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem itemRename = new MenuItem { Name = "Rename", Header = "重命名" };
            itemRename.Click += Rename_Click;
            MenuItem itemDelete = new MenuItem { Name = "Delete", Header = "删除文件夹" };
            itemDelete.Click += DeleteFolder_Click;
            menu.Items.Add(itemRename);
            menu.Items.Add(itemDelete);
            sender.ContextMenu = menu;

            return menu;
        }

        private ContextMenu MenuListView()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem itemRefresh = new MenuItem { Name = "Refresh", Header = "刷新" };
            itemRefresh.Click += Refresh_Click;
            MenuItem itemUpload = new MenuItem { Name = "Upload", Header = "上传文件" };
            itemUpload.Click += Btn_Upload_Click;
            MenuItem itemNewFolder = new MenuItem { Name = "NewFolder", Header = "新建文件夹" };
            itemNewFolder.Click += Btn_NewFolder_Click;
            menu.Items.Add(itemRefresh);
            menu.Items.Add(itemUpload);
            menu.Items.Add(itemNewFolder);

            return menu;
        }

        private void ListViewItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (((FileItem)Lv_File.SelectedItem).type == 0)
            {
                MenuFolder(sender as ListViewItem);
            }
            if (((FileItem)Lv_File.SelectedItem).type == 1)
            {
                MenuFile(sender as ListViewItem);
            }
        }

        void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((FileItem)Lv_File.SelectedItem).type == 0)
            {
                Address.Add(((FileItem)Lv_File.SelectedItem).name + "/");
            }
            if (((FileItem)Lv_File.SelectedItem).type == 3)
            {
                Address.Remove(Address.Last());
            }
            RefreshListView();
        }
    }
}