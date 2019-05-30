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


        public LabelsView()
        {
            InitializeComponent();

            SelectedLabel = null;
            DataContext = this;
            Labels = Repository.GetInstance().Labels;
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
    }
}
