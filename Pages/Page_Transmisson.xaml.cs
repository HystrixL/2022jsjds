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
    /// Page_Transmisson.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Transmisson : Page
    {
        public Page_Transmisson()
        {
            InitializeComponent();
            ChangePageDownload();
        }

        public void InitializePages()
        {

            if (page_Transmission_Download == null)
            {
                page_Transmission_Download = new Page_Transmission_Download();
                page_Transmission_Download.Owner = this;
            }
            if (page_Transmission_Upload == null)
            {
                page_Transmission_Upload = new Page_Transmission_Upload();
                page_Transmission_Upload.Owner = this;
            }
        }

        public MainWindow Owner;

        public Page_Transmission_Download page_Transmission_Download;
        public Page_Transmission_Upload page_Transmission_Upload;

        private void Btn_Download_Click(object sender, RoutedEventArgs e)
        {
            ChangePageDownload();
        }

        public void ChangePageDownload()
        {
            Lb_Download.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 21, 255));
            Lb_Upload.Foreground = new SolidColorBrush(Colors.Black);
            Select_Effect.Margin = new Thickness(25, 38, 0, 0);

            if (page_Transmission_Download == null)
            {
                page_Transmission_Download = new Page_Transmission_Download();
                page_Transmission_Download.Owner = this;
            }
            Change_Page.Content = new Frame()
            {
                Content = page_Transmission_Download
            };
        }

        private void Btn_Upload_Click(object sender, RoutedEventArgs e)
        {
            Lb_Download.Foreground = new SolidColorBrush(Colors.Black);
            Lb_Upload.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 21, 255));
            Select_Effect.Margin = new Thickness(105, 38, 0, 0);

            if (page_Transmission_Upload == null)
            {
                page_Transmission_Upload = new Page_Transmission_Upload();
                page_Transmission_Upload.Owner = this;
            }
            Change_Page.Content = new Frame()
            {
                Content = page_Transmission_Upload
            };
        }
    }
}
