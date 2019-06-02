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
        private Manifestation focusedManifestation;
        private MainWindow mainWindow;
        private static int iconSize = 64;

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

        public MapView(Map mapToShow, MainWindow parentWindow)
        {
            InitializeComponent();
            DataContext = this;
            mainWindow = parentWindow;
            MapToShow = mapToShow;
            LoadManifestations("");
            drawManifPointers();
        }

        private void LoadManifestations(string filterTarget)
        {
            AvailableMaifs = new ObservableCollection<Manifestation>();
            ManifsOnMap = new ObservableCollection<Manifestation>();

            bool manifOnMap;
            foreach (Manifestation manif in Repository.GetInstance().Manifestations)
            {
                if (manif.MapCoordinates == null)
                {
                    continue;
                }
                if(!manif.Id.Contains(filterTarget) && !manif.Name.Contains(filterTarget))
                {
                    continue;
                }
                manifOnMap = false;
                foreach (Coordinates coords in manif.MapCoordinates)
                {
                    if (coords.ParentMap.Id == MapToShow.Id)
                    {
                        ManifsOnMap.Add(manif);
                        manifOnMap = true;
                        break;
                    }
                }
                if (!manifOnMap)
                {
                    AvailableMaifs.Add(manif);
                }
            }
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
            ListView listView = sender as ListView;
            ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
            if(listViewItem != null)
            {
                Mouse.SetCursor(Cursors.Hand);
            }

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {

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

        
        private void ShowManifPointer(Manifestation manif, int x, int y)
        {
            MapToShow.PlaceManifAtPos(manif, x, y);
            if(AvailableMaifs.Contains(manif))
            {
                manifsOnMap.Add(manif);
                AvailableMaifs.Remove(manif);
            }
            drawManifPointers();
            Repository.GetInstance().SaveData();
        }

        

        private void Map_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("manifestation"))
            {
                Point dropPosition = e.GetPosition(Map);
                Manifestation manifToDrop = e.Data.GetData("manifestation") as Manifestation;
                Manifestation underlyingManifestation = ClickedManifestation((int)dropPosition.X, (int)dropPosition.Y);
                if (underlyingManifestation != null)
                {
                    MessageBox.Show($"Unable to place manifestation \"{manifToDrop.Name}\" on given position because manifestation \"{underlyingManifestation.Name}\" is in the way. Please try draging manifestation \"{manifToDrop.Name}\" to a different position.",
                        "Overlapping manifestations not allowed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (manifToDrop.MapCoordinates.Count > 0 && AvailableMaifs.Contains(manifToDrop))
                {
                    MessageBoxResult choince = MessageBox.Show($"Manifestation \"{manifToDrop.Id}\" is already placed on another map. Do you want do move it to this map?",
                        $"Change location of {manifToDrop.Id}", MessageBoxButton.YesNoCancel);
                    if(choince != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }
                ShowManifPointer(manifToDrop, (int)dropPosition.X, (int)dropPosition.Y);
            }
        }

        private void drawManifPointers()
        {
            Map.Children.Clear();
            foreach (Manifestation manif in ManifsOnMap)
            {
                if (File.Exists(manif.IconPath))
                {
                    foreach(Coordinates coords in manif.MapCoordinates)
                    {
                        if(coords.ParentMap.Id == MapToShow.Id)
                        {
                            Image manifIcon = new Image();
                            manifIcon.Source = new BitmapImage(new Uri(manif.IconPath));
                            manifIcon.Width = iconSize;
                            manifIcon.Height = iconSize;
                            manifIcon.ToolTip = manif.Name;
                            Map.Children.Add(manifIcon);
                            Canvas.SetLeft(manifIcon, coords.X - iconSize / 2);
                            Canvas.SetTop(manifIcon, coords.Y - iconSize / 2);
                            
                            break;
                        }
                    }
                }
            }
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(Map);
            Vector diff = startPoint - mousePosition;

            Manifestation manifestation = ClickedManifestation((int)mousePosition.X, (int)mousePosition.Y);
            if(manifestation != null)
            {
                Mouse.SetCursor(Cursors.Hand);
            }
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                

                if (manifestation != null)
                {
                    DataObject dragData = new DataObject("manifestation", manifestation);
                    DragDrop.DoDragDrop(Map, dragData, DragDropEffects.Move);
                }
            }
        }

        

        Manifestation ClickedManifestation(int X_click, int Y_click)
        {
            foreach(Manifestation manif in ManifsOnMap)
            {
                foreach(Coordinates coords in manif.MapCoordinates)
                {
                    if(coords.ParentMap.Id == MapToShow.Id)
                    {
                        if (Math.Sqrt(Math.Pow((X_click - coords.X), 2) + Math.Pow((Y_click - coords.Y), 2)) <= iconSize / 2)
                        {
                            return manif;
                        }
                    }
                }
            }
            return null;
        }

        private void Map_ContextMenu(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(Map);
            focusedManifestation = ClickedManifestation((int)mousePosition.X, (int)mousePosition.Y);

            if (focusedManifestation != null)
            {
                var cmnu = this.FindResource("mapContextMenu") as ContextMenu;
                cmnu.IsOpen = true;
            }
        }

        private void EditManifClicked(object sender, RoutedEventArgs e)
        {
            Manifestation target = focusedManifestation;
            if(target == null)
            {
                return;
            }
            mainWindow.showManifestationEditView(target.Id);
        }

        private void DeleteManifClicked(object sender, RoutedEventArgs e)
        {
            Manifestation target = focusedManifestation;
            if(target == null)
            {
                return;
            }
            MessageBoxResult choice = MessageBox.Show($"Are you sure that you want to permanently delete manifestation \"{target.Id}\"?", "Delete manifestation", MessageBoxButton.YesNoCancel);
            if (choice == MessageBoxResult.Yes)
            {
                ManifsOnMap.Remove(target);
                drawManifPointers();
                Repository.GetInstance().DeleteManifestation(target.Id);
            }
        }

        private void RemoveManifPointersClicked(object sender, RoutedEventArgs e)
        {
            Manifestation target = focusedManifestation;
            if (target == null)
            {
                return;
            }
            MessageBoxResult choice = MessageBox.Show($"Are you sure that you want to remove pointers for manifestation \"{target.Id}\" on all maps?", "Remove manifestation map pointers", MessageBoxButton.YesNoCancel);
            if (choice == MessageBoxResult.Yes)
            {
                ManifsOnMap.Remove(target);
                drawManifPointers();
                target.MapCoordinates.Clear();
                AvailableMaifs.Add(target);
                Repository.GetInstance().SaveData();
            }
        }

        private void filterBtnClicked(object sender, RoutedEventArgs e)
        {
            string target = FilterInput.Text;
            LoadManifestations(target);
            drawManifPointers();
        }

        private void FilterKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string target = FilterInput.Text;
                LoadManifestations(target);
                drawManifPointers();
            }
        }
    }
}
