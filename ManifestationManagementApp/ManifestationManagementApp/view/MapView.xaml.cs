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
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ManifestationManagementApp.model;


namespace ManifestationManagementApp.view
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
   
    public partial class MapView : Page, INotifyPropertyChanged
    {
        private Point startPoint = new Point();

        private Map mapToShow;
        public Map MapToShow
        {
            get { return mapToShow; }
            set
            {
                if (value != mapToShow)
                {
                    mapToShow = value;
                    OnPropertyChanged("MapToShow");
                }
            }
        }

        private string mapImagePath;
        public string MapImagePath
        {
            get { return mapImagePath; }
            set
            {
                if (value != mapImagePath)
                {
                    mapImagePath = value;
                    OnPropertyChanged("MapImagePath");
                }
            }
        }

        private ObservableCollection<Manifestation> manifestations;
        public ObservableCollection<Manifestation> AvailableMaifs
        {
            get { return manifestations; }
            set
            {
                if (value != manifestations)
                {
                    manifestations = value;
                    OnPropertyChanged("AvailableMaifs");
                }
            }
        }

        private ObservableCollection<Manifestation> manifsOnMap;
        public ObservableCollection<Manifestation> ManifsOnMap
        {
            get { return manifsOnMap; }
            set
            {
                if (value != manifsOnMap)
                {
                    manifsOnMap = value;
                    OnPropertyChanged("ManifsOnMap");
                }
            }
        }

        public MapView()
        {
            InitializeComponent();
            DataContext = this;
            AvailableMaifs = new ObservableCollection<Manifestation>();
            ManifsOnMap = new ObservableCollection<Manifestation>();
        }

        public MapView(Map mapToShow)
        {
            InitializeComponent();
            DataContext = this;
            MapImagePath = mapToShow.ImagePath;
            AvailableMaifs = new ObservableCollection<Manifestation>();
            ManifsOnMap = new ObservableCollection<Manifestation>();
            MapToShow = mapToShow;
            bool manifOnMap;
            foreach(Manifestation manif in Repository.GetInstance().Manifestations)
            {
                if(manif.MapCoordinates == null)
                {
                    continue;
                }
                manifOnMap = false;
               foreach(Coordinates coords in manif.MapCoordinates)
                {
                    if(coords.ParentMap.Id == MapToShow.Id)
                    {
                        ManifsOnMap.Add(manif);
                        manifOnMap = true;
                        break;
                    }
                }
               if(!manifOnMap)
                {
                    AvailableMaifs.Add(manif);
                }
            }
            drawManifPointers();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void HideManifsBtnClicked(object sender, RoutedEventArgs e)
        {
            ManifestationList.Visibility = Visibility.Collapsed;
            ShowManifestationListBtn.Visibility = Visibility.Visible;
        }

        private void showManifestationListBtnClicked(object sender, RoutedEventArgs e)
        {
            ShowManifestationListBtn.Visibility = Visibility.Collapsed;
            ManifestationList.Visibility = Visibility.Visible;
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(null);
            Vector diff = startPoint - mousePosition;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem != null)
                {
                    Manifestation manifestation = (Manifestation)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                    DataObject dragData = new DataObject("manifestation", manifestation);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void Map_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("manifestation"))
            {
                Point dropPosition = e.GetPosition(Map);
                Manifestation manifToDrop = e.Data.GetData("manifestation") as Manifestation;

                manifToDrop.MapCoordinates.Add(new Coordinates { X = (int)dropPosition.X, Y = (int)dropPosition.Y, ParentMap = MapToShow });
                manifsOnMap.Add(manifToDrop);
                AvailableMaifs.Remove(manifToDrop);
                drawManifPointers();
                Repository.GetInstance().SaveData();
            }
        }

        private void drawManifPointers()
        {
            foreach(Manifestation manif in ManifsOnMap)
            {
                if (File.Exists(manif.IconPath))
                {
                    foreach(Coordinates coords in manif.MapCoordinates)
                    {
                        if(coords.ParentMap.Id == MapToShow.Id)
                        {
                            Image manifIcon = new Image();
                            manifIcon.Source = new BitmapImage(new Uri(manif.IconPath));
                            manifIcon.Width = 64;
                            manifIcon.Height = 64;
                            Map.Children.Add(manifIcon);
                            Canvas.SetLeft(manifIcon, coords.X);
                            Canvas.SetTop(manifIcon, coords.Y);
                            break;
                        }
                    }
                }
            }
        }
    }
}
