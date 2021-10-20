
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
using System.Threading;
using System.Windows.Threading;

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

            itemPercents.ListChanged += ItemPercents_ListChanged;
        }

        public int lastItemCount = 0;
        public int itemCount = 0;

        private void ItemPercents_ListChanged(object sender)
        {
            itemCount = (sender as NBindingList<ItemPercent>).Count;
            if ((itemCount < lastItemCount || itemCount == 1) && itemCount != 0)
            {
                lastItemCount = itemCount;
                Owner.Owner.page_Project.page_ProjectInstance.page_ProjectInstance_Project.StartFileUpload(itemPercents[0].fileAddress, itemPercents[0].fileName);
            }
            else
            {
                lastItemCount = itemCount;
            }
        }

        public Page_Transmisson Owner;

        //public float percent;

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
            //ThreadPool.QueueUserWorkItem(delegate
            //{
            //    SynchronizationContext.SetSynchronizationContext(new
            //        DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));
            //    SynchronizationContext.Current.Post(pl =>
            //    {
            //        MessageBox.Show(itemPercents.Count.ToString());
            //        itemPercents.RemoveAt(0);
            //        RefreshListView();
            //    }, null);
            //});
            //itemPercents.RemoveAt(0);
            itemPercents.Remove(itemPercents.First());
            //if (itemPercents.Count != 0)
            //{
            //    Owner.Owner.page_Project.page_ProjectInstance.page_ProjectInstance_Project.StartFileUpload(itemPercents[0].fileAddress, itemPercents[0].fileName);
            //}
            //RefreshListView();

        }

        public NBindingList<ItemPercent> itemPercents = new NBindingList<ItemPercent>();

        //public void RefreshListView()
        //{
        //    foreach (ItemPercent item in itemPercents)
        //    {
        //        itemPercents.Add(item);
        //    }

        //}


        public void CreateList(string fileAddress, string fileName, string projectName)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                //ListViewItem item = new ListViewItem();
                ItemPercent itemPercent = new ItemPercent();
                itemPercent.fileAddress = fileAddress;
                itemPercent.fileName = fileName;
                itemPercent.projectName = "所在项目：" + projectName;
                itemPercents.Add(itemPercent);
                Lv_ListUpload.ItemsSource = itemPercents;
                //Lv_ListUpload.Items.Add(item);
                //Task.Run(async () =>
                //{
                //    while (true)
                //    {
                //        UpdateProgress(item, itemPercent);
                //         //await Task.Delay(1000);
                //     }

                //});
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
