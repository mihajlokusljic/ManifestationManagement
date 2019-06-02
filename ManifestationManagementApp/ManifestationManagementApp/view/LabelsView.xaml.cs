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
            FilterInput.Focus();
        }

        private void ShowHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.mainWindow.MainContent.Content = new HelpView("ShowLabelsHelp");
        }

        public void scrollTo(string labelId)
        {
            foreach(model.Label lab in Labels)
            {
                if(lab.Id == labelId)
                {
                    SelectedLabel = lab;
                    LabelsTable.ScrollIntoView(lab);
                    break;
                }
            }
        }

        public LabelsView(MainWindow parent)
        {
            InitializeComponent();

            SelectedLabel = null;
            DataContext = this;
            Labels = Repository.GetInstance().Labels;
            mainWindow = parent;
            FilterInput.Focus();
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

        private void filterClick(object sender, RoutedEventArgs e)
        {
            searchMessage.Content = "";
            ObservableCollection<model.Label> labelsFilter =
                new ObservableCollection<model.Label>();
            string target = FilterInput.Text;
            if (target == "")
            {
                LabelsTable.ItemsSource = Labels;
            }
            else
            {
                foreach (model.Label label in Labels)
                {
                    if (label.Id.Contains(target))
                    {
                        labelsFilter.Add(label);
                    }
                }
                LabelsTable.ItemsSource = labelsFilter;
            }
        }

        private void searchClick(object sender, RoutedEventArgs e)
        {
            string target = SearchInput.Text;
            int numberOfFound = 0;
            for (int i = 0; i < LabelsTable.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)LabelsTable.ItemContainerGenerator.ContainerFromIndex(i);

                if (row != null)
                {
                    int index = row.GetIndex();
                    model.Label label = row.DataContext as model.Label;
                    if (label.Id.Contains(target))
                    {
                        numberOfFound = numberOfFound + 1;
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
            if (numberOfFound == 0)
            {
                searchMessage.Content = "Nothing found with search!";
                searchMessage.Foreground = Brushes.Red;
            }
            else
            {
                searchMessage.Content = "";
            }
        }
    }
}
