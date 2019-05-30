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
    /// Interaction logic for LabelsView.xaml
    /// </summary>
    public partial class LabelsView : Page, INotifyPropertyChanged
    {
        private ObservableCollection<model.Label> labels;
        public ObservableCollection<model.Label> Labels
        {
            get { return labels; }
            set
            {
                if (value != labels)
                {
                    labels = value;
                    OnPropertyChanged("Labels");
                }
            }
        }

        public model.Label SelectedLabel { get; set; }
        private MainWindow mainWindow;

        public LabelsView()
        {
            InitializeComponent();

            SelectedLabel = null;
            DataContext = this;
            Labels = Repository.GetInstance().Labels;
        }

        public LabelsView(MainWindow parent)
        {
            InitializeComponent();

            SelectedLabel = null;
            DataContext = this;
            Labels = Repository.GetInstance().Labels;
            mainWindow = parent;
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

        private void DeleteBtnClicked(object sender, RoutedEventArgs e)
        {
            model.Label target = SelectedLabel;
            if(target == null)
            {
                MessageBox.Show("Please select a label to delete.", "Delete failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Repository rep = Repository.GetInstance();
            if(rep.LabelIsReferenced(target.Id))
            {
                MessageBox.Show($"Label {target.Id} can not be deleted because it being used by a manifestation. Please update or delete all manifestations using label {target.Id} and try again.", "Delete failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult choice = MessageBox.Show($"Are you sure that you want to permanently delete label \"{target.Id}\"?", "Delete label", MessageBoxButton.YesNoCancel);
            if(choice == MessageBoxResult.Yes)
            {
                rep.DeleteLabel(target.Id);
            }

        }

        private void editBtnClicked(object sender, RoutedEventArgs e)
        {
            model.Label target = SelectedLabel;
            if(target == null)
            {
                MessageBox.Show("Please select a label to edit.", "Edit failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            mainWindow.showLabelEditView(target.Id);
        }
    }
}
