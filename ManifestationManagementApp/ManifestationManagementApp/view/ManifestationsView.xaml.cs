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
using System.Collections.ObjectModel;
using ManifestationManagementApp.model;

namespace ManifestationManagementApp.view
{
    /// <summary>
    /// Interaction logic for ManifestationsView.xaml
    /// </summary>
    public partial class ManifestationsView : Page, INotifyPropertyChanged
    {
        private ObservableCollection<model.Manifestation> manifestations;
        public ObservableCollection<model.Manifestation> Manifestations
        {
            get { return manifestations; }
            set
            {
                if (value != manifestations)
                {
                    manifestations = value;
                    OnPropertyChanged("Manifestations");
                }
            }
        }

        public model.Manifestation SelectedManifestation { get; set; }
        private MainWindow mainWindow;

        public ManifestationsView()
        {
            InitializeComponent();

            SelectedManifestation = null;
            DataContext = this;
            Manifestations = Repository.GetInstance().Manifestations;
        }

        public ManifestationsView(MainWindow parent)
        {
            InitializeComponent();

            SelectedManifestation = null;
            DataContext = this;
            Manifestations = Repository.GetInstance().Manifestations;
            mainWindow = parent;
        }

        public void scrollTo(string ManifestationId)
        {
            foreach (model.Manifestation manif in Manifestations)
            {
                if (manif.Id == ManifestationId)
                {
                    SelectedManifestation = manif;
                    ManifestationsTable.ScrollIntoView(manif);
                    break;
                }
            }
        }

        private void tagsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void editBtnClicked(object sender, RoutedEventArgs e)
        {
            model.Manifestation target = SelectedManifestation;
            if (target == null)
            {
                MessageBox.Show("Please select a manifestation to edit.", "Edit failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            mainWindow.showManifestationEditView(target.Id);
        }

        private void DeleteBtnClicked(object sender, RoutedEventArgs e)
        {
            model.Manifestation target = SelectedManifestation;
            if (target == null)
            {
                MessageBox.Show("Please select a manifestation to delete.", "Delete failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Repository rep = Repository.GetInstance();
            MessageBoxResult choice = MessageBox.Show($"Are you sure that you want to permanently delete manifestation \"{target.Id}\"?", "Delete manifestation", MessageBoxButton.YesNoCancel);
            if (choice == MessageBoxResult.Yes)
            {
                rep.DeleteManifestation(target.Id);
            }
        }

        private void filterClick(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Manifestation> manifestationsFilter = 
                new ObservableCollection<Manifestation>();
            string target = FilterInput.Text;
            if (target == "")
            {
                ManifestationsTable.ItemsSource = Manifestations;
            }
            else
            {
                foreach (Manifestation manif in Manifestations)
                {
                    if (manif.Id.Contains(target))
                    {
                        manifestationsFilter.Add(manif);
                    }
                }
                ManifestationsTable.ItemsSource = manifestationsFilter;
            }
        }

        private void searchClick(object sender, RoutedEventArgs e)
        {
            string target = SearchInput.Text;
            for (int i = 0; i < ManifestationsTable.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)ManifestationsTable.ItemContainerGenerator.ContainerFromIndex(i);

                if (row != null)
                {
                    int index = row.GetIndex();
                    Manifestation manif = row.DataContext as Manifestation;
                    if (manif.Id.Contains(target))
                    {
                        SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(100, 255, 104, 0));
                        row.Background = brush;
                    }
                    else
                    {
                        SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                        row.Background = brush;
                    }
                }
            }
        }
    }
}
