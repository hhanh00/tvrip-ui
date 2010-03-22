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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace tvrip_ui
{
    public class Languages : ObservableCollection<string>
    {
        public Languages()
        {
            Add("English");
            Add("French");
            Add("Spanish");
        }
    }

    public class Disk
    {
        public string Name { get; set; }
        public int FirstEpisodeNumber { get; set; }
    }

    public class Episode : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(Episode), new UIPropertyMetadata(null));

        public Disk Disk
        {
            get; set;
        }

        public string Name 
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public int Index
        {
            get; set;
        }

        public string Number
        {
            get
            {
                return (Disk.FirstEpisodeNumber + Index).ToString();
            }
        }
    }

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DiskContent : Window
    {
        Disk _Disk = new Disk();
        DataTable _EpisodeListTable = new DataTable();
        DataTable _AudioTable = new DataTable();
        DataTable _SubtitleTable = new DataTable();
 
        public DiskContent()
        {
            _Disk.Name = "Two and a Half Men";
            _Disk.FirstEpisodeNumber = 10;

            _SubtitleTable.Columns.Add(new DataColumn("Language", typeof(string)));
            DataRow row = _SubtitleTable.NewRow();
            _SubtitleTable.Rows.Add(row);
            row["Language"] = "English";

            _AudioTable.Columns.Add(new DataColumn("Language", typeof(string)));
            row = _AudioTable.NewRow();
            _AudioTable.Rows.Add(row);
            row["Language"] = "English";

            _EpisodeListTable.Columns.Add(new DataColumn("Name", typeof(string)));
            row = _EpisodeListTable.NewRow();
            _EpisodeListTable.Rows.Add(row);
            row["Name"] = "Back Off, Mary Poppins";

            row = _EpisodeListTable.NewRow();
            _EpisodeListTable.Rows.Add(row);
            row["Name"] = "Enjoy Those Garlic Balls";

            InitializeComponent();
        }

        public DataTable EpisodeListTable
        {
            get { return _EpisodeListTable; }
        }

        public DataTable AudioListTable
        {
            get { return _AudioTable; }
        }

        public DataTable SubtitleListTable
        {
            get { return _SubtitleTable; }
        }

        private void MoveTo(int oldIndex, int newIndex)
        {
            DataRow selectedRow = _EpisodeListTable.Rows[oldIndex];
            DataRow newRow = _EpisodeListTable.NewRow();
            newRow.ItemArray = selectedRow.ItemArray;
            _EpisodeListTable.Rows.Remove(selectedRow);
            _EpisodeListTable.Rows.InsertAt(newRow, newIndex);
            EpisodeListGrid.SelectedIndex = newIndex;
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = EpisodeListGrid.SelectedIndex;
            if (selectedIndex > 0)
            {
                MoveTo(selectedIndex, selectedIndex - 1);
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = EpisodeListGrid.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < EpisodeListTable.Rows.Count - 1)
            {
                MoveTo(selectedIndex, selectedIndex + 1);
            }
        }
    }
}
