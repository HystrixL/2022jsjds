using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Page_Transmission_Download.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Transmission_Download : Page
    {
        public Page_Transmission_Download()
        {
            InitializeComponent();
            itemPercents.ListChanged += ItemPercents_ListChanged;
        }

        public Page_Transmisson Owner;

        public int lastItemCount = 0;
        public int itemCount = 0;

        private void ItemPercents_ListChanged(object sender)
        {
            itemCount = (sender as NBindingList<ItemPercent>).Count;
            if ((itemCount < lastItemCount || itemCount == 1) && itemCount != 0)
            {
                lastItemCount = itemCount;
                Owner.Owner.page_Project.page_ProjectInstance.page_ProjectInstance_Project.StartFileDownload(itemPercents[0].fileAddress, itemPercents[0].fileName);
            }
            else
            {
                lastItemCount = itemCount;
            }
        }


        public class ItemPercent : INotifyPropertyChanged
        {
            public string fileAddress;
            public string fileName { get; set; }
            public string projectName { get; set; }
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

        public delegate void ListChangedEventHandler<T>(object sender);
        public class NBindingList<T> : BindingList<T>
        {
            private int _count;
            public String Action { get; set; }
            private int NCount
            {
                get { return _count; }
                set
                {
                    if (_count != value)
                    {
                        _count = this.Count;
                        DoListChangedEvent();
                    }
                }
            }
            public event ListChangedEventHandler<T> ListChanged;
            private void DoListChangedEvent()
            {
                if (this.ListChanged != null)
                    this.ListChanged(this);
            }
            public new void Add(T t)
            {
                base.Add(t);
                this.Action = "Add";
                NCount++;
            }

            public new void Remove(T t)
            {
                base.Remove(t);
                this.Action = "Remove";
                NCount--;
            }

        }

        public void RemoveFirstItem()
        {
            itemPercents.Remove(itemPercents.First());
        }

        public NBindingList<ItemPercent> itemPercents = new NBindingList<ItemPercent>();

        public void CreateList(string fileAddress, string fileName, string projectName)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                ItemPercent itemPercent = new ItemPercent();
                itemPercent.fileAddress = fileAddress;
                itemPercent.fileName = fileName;
                itemPercent.projectName = "所在项目：" + projectName;
                itemPercents.Add(itemPercent);
                Lv_ListDownload.ItemsSource = itemPercents;
            }));
        }
    }
}
