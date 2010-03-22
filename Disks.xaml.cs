using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Windows.Controls;

namespace tvrip_ui
{
    /// <summary>
    /// Interaction logic for Disks.xaml
    /// </summary>
    public partial class Disks : Window
    {
        DataTable _Table = new DataTable();

        public Disks()
        {
            _Table.Columns.Add(new DataColumn("Name", typeof(string)));
            _Table.Columns.Add(new DataColumn("Guid", typeof(Guid)));

            InitializeComponent();
        }

        public DataTable Table
        {
            get { return _Table; }
        }

        private Dictionary<Guid, Disk> _Disks = new Dictionary<Guid, Disk>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = ((FrameworkElement)sender).DataContext as DataRowView;
            if (drv == null)
                return;
            Disk disk = NewDisk(drv);
            DiskContentWindow diskContentWindow = new DiskContentWindow(disk);
            diskContentWindow.ShowDialog();
        }

        private Disk NewDisk(DataRowView drv)
        {
            if (drv["Guid"] == DBNull.Value)
            {
                Guid newGuid = Guid.NewGuid();
                drv["Guid"] = newGuid;
                Disk disk = new Disk((string)drv["Name"], newGuid);
                _Disks.Add(newGuid, disk);
                return disk;
            }
            return null;
        }

        private void DiskGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                e.Cancel = false;
                return;
            }

            if (e.EditAction == DataGridEditAction.Commit)
            {
                DataGridRow dgr = e.Row;
                DataRowView drv = (DataRowView)dgr.DataContext;
                NewDisk(drv);
                e.Cancel = true;
            }
        }

        private void Break_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debugger.Break();
        }
    }
}
