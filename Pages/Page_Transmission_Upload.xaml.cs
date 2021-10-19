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
using System.ComponentModel;

namespace Co_work.Pages
{
    /// <summary>
    /// Page_Transmission_Upload.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Transmission_Upload : Page
    {
        public Page_Transmission_Upload()
        {
            InitializeComponent();
        }

        public Page_Transmisson Owner;

        //public float percent;

        public class ItemPercent : INotifyPropertyChanged
        {
            public float p;
            public float percent
            {
                get { return p; }
                set { p = value; OnPropertyChanged(new PropertyChangedEventArgs("percent")); }
            }

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

        public BindingList<ItemPercent> itemPercents = new BindingList<ItemPercent>();

        public void CreateList()
        {
            Dispatcher.Invoke(new Action(delegate
            {
                ListViewItem item = new ListViewItem();
                ItemPercent itemPercent = new ItemPercent();
                itemPercents.Add(itemPercent);
                Lv_ListUpload.ItemsSource = itemPercents;
                //Lv_ListUpload.Items.Add(item);
                // Task.Run(async () =>
                // {
                //     while (true)
                //     {
                //         UpdateProgress(item, itemPercent);
                //         //await Task.Delay(1000);
                //     }
                //
                // });
            }));
        }

        // public void UpdateProgress(ListViewItem item, ItemPercent itemPercent)
        // {
        //     Dispatcher.Invoke(new Action(delegate
        //     {
        //         //item.Content = null;
        //         //item.Content = itemPercent;
        //         //((ItemPercent)item.Content).percent = itemPercent.percent;
        //         //BindingExpression be = item.GetBindingExpression(ProgressBar.ValueProperty);
        //         //be.UpdateSource();
        //         //MessageBox.Show(item.Content.GetType().Name.ToString());
        //     }));
        // }
    }
}
