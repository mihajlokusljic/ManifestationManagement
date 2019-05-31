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
    /// Interaction logic for ManifestationTypesView.xaml
    /// </summary>
    public partial class ManifestationTypesView : Page, INotifyPropertyChanged
    {
        private ObservableCollection<model.ManifestationType> types;
        public ObservableCollection<model.ManifestationType> Types
        {
            get { return types; }
            set
            {
                if (value != types)
                {
                    types = value;
                    OnPropertyChanged("Types");
                }
            }
        }

        public model.ManifestationType SelectedManifestationType { get; set; }
        private MainWindow mainWindow;

        public ManifestationTypesView()
        {
            InitializeComponent();

            SelectedManifestationType = null;
            DataContext = this;
            Types = Repository.GetInstance().ManifestationTypes;
        }

        public ManifestationTypesView(MainWindow parent)
        {
            InitializeComponent();

            SelectedManifestationType = null;
            DataContext = this;
            Types = Repository.GetInstance().ManifestationTypes;
            mainWindow = parent;
        }

        public void scrollTo(string TypeId)
        {
            foreach (model.ManifestationType type in Types)
            {
                if (type.Id == TypeId)
                {
                    SelectedManifestationType = type;
                    ManifestationTypesTable.ScrollIntoView(type);
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
            model.ManifestationType target = SelectedManifestationType;
            if (target == null)
            {
                MessageBox.Show("Please select a manifestation type to edit.", "Edit failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            mainWindow.showManifestationTypeEditView(target.Id);
        }

        private void DeleteBtnClicked(object sender, RoutedEventArgs e)
        {
            model.ManifestationType target = SelectedManifestationType;
            if (target == null)
            {
                MessageBox.Show("Please select a manifestation type to delete.", "Delete failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Repository rep = Repository.GetInstance();
            if (rep.ManifestationTypeIsReferenced(target.Id))
            {
                MessageBox.Show($"Manifestation type {target.Id} can not be deleted because it being used by a manifestation. Please update or delete all manifestations using manifestation type {target.Id} and try again.", "Delete failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult choice = MessageBox.Show($"Are you sure that you want to permanently delete manifestation type \"{target.Id}\"?", "Delete manifestation type", MessageBoxButton.YesNoCancel);
            if (choice == MessageBoxResult.Yes)
            {
                rep.DeleteManifestationType(target.Id);
            }

        }
    }
}
