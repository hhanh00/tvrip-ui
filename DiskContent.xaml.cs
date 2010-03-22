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

    public class Disk : INotifyPropertyChanged
    {
        private string _Image;
        public event PropertyChangedEventHandler PropertyChanged;

        public Disk(string name, Guid guid)
        {
            Guid = guid;
            Name = name;
            FirstEpisode = 1;
            FirstTitle = 1;
            Season = 1;
            Image = "";
        }

        public Guid Guid { get; private set; }
        public string Name { get; set; }
        public int Season { get; set; }
        public int FirstEpisode { get; set; }
        public int FirstTitle { get; set; }
        public bool IsMultiTrack { get; set; }
        public List<string> Audio { get; set; }
        public List<string> Subtitles { get; set; }
        public List<string> Episodes { get; set; }
        public string Image 
        {
            get { return _Image; }
            set
            {
                _Image = value;
                NotifyPropertyChanged("Image");
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DiskContentWindow : Window
    {
        private Disk _Disk;
        private DataTable _EpisodeListTable = new DataTable();
        private DataTable _AudioTable = new DataTable();
        private DataTable _SubtitlesTable = new DataTable();
 
        public DiskContentWindow(Disk disk)
        {
            _Disk = disk;

            _SubtitlesTable.Columns.Add(new DataColumn("Language", typeof(string)));
            DataRow row = _SubtitlesTable.NewRow();
            _SubtitlesTable.Rows.Add(row);
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

        public Disk Disk
        {
            get { return _Disk; }
            set { _Disk = value; }
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
            get { return _SubtitlesTable; }
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

        private void OnBrowse(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".iso";
            dlg.Filter = "Disk Image (.iso)|*.iso";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                _Disk.Image = filename;
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _Disk.Audio = new List<string>();
            foreach (DataRow row in _AudioTable.Rows)
            {
                _Disk.Audio.Add((string)row["Language"]);
            }

            _Disk.Subtitles = new List<string>();
            foreach (DataRow row in _SubtitlesTable.Rows)
            {
                _Disk.Subtitles.Add((string)row["Language"]);
            }

            _Disk.Episodes = new List<string>();
            foreach (DataRow row in _EpisodeListTable.Rows)
            {
                _Disk.Episodes.Add((string)row["Name"]);
            }
        }
    }
}
