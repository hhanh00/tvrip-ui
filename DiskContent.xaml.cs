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

namespace tvrip_ui
{
    public class Disk
    {
        public string Name { get; set; }
        public int FirstEpisodeNumber { get; set; }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            bool param = bool.Parse(parameter as string);
            bool val = (bool)value;

            return val == param ?
              Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
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
        BindingList<Episode> _EpisodeList = new BindingList<Episode>();
 
        public DiskContent()
        {
            _Disk.Name = "Two and a Half Men";
            _Disk.FirstEpisodeNumber = 10;
            _EpisodeList.Add(new Episode { Disk = _Disk, Name = "Back Off, Mary Poppins", Index = 0 });
            _EpisodeList.Add(new Episode { Disk = _Disk, Name = "Enjoy Those Garlic Balls", Index = 1 });

            InitializeComponent();
        }

        public BindingList<Episode> EpisodeList
        {
            get { return _EpisodeList; }
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            _EpisodeList.Add(new Episode { Disk = _Disk, Name = "New Episode", Index = _EpisodeList.Count });
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = DiskContentView.SelectedIndex;
            if (selectedIndex > 0)
            {
                _EpisodeList[selectedIndex].Index--;
                _EpisodeList[selectedIndex - 1].Index++;
                Episode episode = _EpisodeList[selectedIndex];
                _EpisodeList.RemoveAt(selectedIndex);
                _EpisodeList.Insert(selectedIndex - 1, episode);
                _EpisodeList.ResetItem(selectedIndex);
                DiskContentView.SelectedIndex = selectedIndex - 1;
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = DiskContentView.SelectedIndex;
            if (selectedIndex < _EpisodeList.Count - 1)
            {
                _EpisodeList[selectedIndex].Index++;
                _EpisodeList[selectedIndex + 1].Index--;
                Episode episode = _EpisodeList[selectedIndex];
                _EpisodeList.RemoveAt(selectedIndex);
                _EpisodeList.Insert(selectedIndex + 1, episode);
                _EpisodeList.ResetItem(selectedIndex);
                DiskContentView.SelectedIndex = selectedIndex + 1;
            }
        }
    }
}
