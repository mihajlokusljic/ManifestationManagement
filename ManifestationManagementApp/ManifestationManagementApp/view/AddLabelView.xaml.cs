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
using ManifestationManagementApp.model;

namespace ManifestationManagementApp.view
{
    /// <summary>
    /// Interaction logic for AddLabelView.xaml
    /// </summary>
    public partial class AddLabelView : Page
    {
        private MainWindow mainWindow;
        public bool Editing { get; set; }

        public AddLabelView()
        {
            InitializeComponent();
            Editing = false;
            descriptionInput.Focus();
        }

        public AddLabelView(MainWindow parent, bool editMode)
        {
            InitializeComponent();
            mainWindow = parent;
            Editing = editMode;
            descriptionInput.Focus();
        }

        private void ShowHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.mainWindow.MainContent.Content = new HelpView("AddLabelHelp");
        }

        private void AddOrEditButtonClick(object sender, RoutedEventArgs e)
        {
            bool isAutoChecked = autoGenerateId.IsChecked.Value;
            if (colorPicker.SelectedColor.ToString() == "")
            {
                AddedLabelMessage.Content = "Please, pick some color.";
                AddedLabelMessage.Foreground = Brushes.Red;
            } else if (descriptionInput.Text == "")
            {
                AddedLabelMessage.Content = "Please, add some description.";
                AddedLabelMessage.Foreground = Brushes.Red;
            } else if (idInput.Text == "" && !isAutoChecked)
            {
                AddedLabelMessage.Content = "Please, insert id for this label.";
                AddedLabelMessage.Foreground = Brushes.Red;
            }
            else if (idInput.Text != "" && !isAutoChecked && Repository.GetInstance().FindLabel(idInput.Text) != null && !Editing)
            {
                AddedLabelMessage.Content = "This id is already taken, please select another one.";
                AddedLabelMessage.Foreground = Brushes.Red;
            }
            else
            {
                
                model.Label retVal = new model.Label();
                if (isAutoChecked && !Editing)
                {
                    Repository.GetInstance().LabelCounter = Repository.GetInstance().LabelCounter + 1;
                    retVal.Id = "lab" + Repository.GetInstance().LabelCounter;
                    idInput.Text = retVal.Id;
                    while (Repository.GetInstance().FindLabel(retVal.Id) != null)
                    {
                        Repository.GetInstance().LabelCounter = Repository.GetInstance().LabelCounter + 1;
                        retVal.Id = "lab" + Repository.GetInstance().LabelCounter;
                        idInput.Text = retVal.Id;
                    }
                }
                else
                {
                    retVal.Id = idInput.Text;
                }
                retVal.Color = colorPicker.SelectedColor.ToString();
                retVal.Description = descriptionInput.Text;
                Repository rep = Repository.GetInstance();
                if (!Editing)
                {
                    rep.Addlabel(retVal);
                    AddedLabelMessage.Content = "Label \"" + retVal.Id + "\" has been added successfully.";
                    AddedLabelMessage.Foreground = Brushes.Green;
                    descriptionInput.Text = "";
                    colorPicker.SelectedColor = null;

                    if (autoGenerateId.IsChecked.Value)
                    {
                        //ako je izabrano automatsko inkrementiranje, azurira se vrijednost za id
                        idInput.Text = $"lab{Repository.GetInstance().LabelCounter + 1}";
                        while (Repository.GetInstance().FindLabel(idInput.Text) != null)
                        {
                            Repository.GetInstance().LabelCounter = Repository.GetInstance().LabelCounter + 1;
                            idInput.Text = $"lab{Repository.GetInstance().LabelCounter + 1}";
                        }
                        descriptionInput.Focus();
                    }
                    else
                    {
                        idInput.Text = "";
                        idInput.Focus();
                    }
                }
                else
                {
                    rep.UpdateLabel(retVal);
                    LabelsView labels = new LabelsView(mainWindow);
                    labels.scrollTo(retVal.Id);
                    mainWindow.MainContent.Content = labels;
                }
            }
        }

        private void CancelBtnClicked(object sender, RoutedEventArgs e)
        {
            LabelsView labels = new LabelsView(mainWindow);
            if(Editing)
            {
                labels.scrollTo(idInput.Text);
            }
            mainWindow.MainContent.Content = labels;
        }

        private void AutoGenerateIdClicked(object sender, RoutedEventArgs e)
        {
            bool isAutoChecked = autoGenerateId.IsChecked.Value;
            if (isAutoChecked)
            {
                idInput.IsEnabled = false;
                idInput.Text = $"lab{Repository.GetInstance().LabelCounter + 1}";
                while(Repository.GetInstance().FindLabel(idInput.Text) != null)
                {
                    Repository.GetInstance().LabelCounter = Repository.GetInstance().LabelCounter + 1;
                    idInput.Text = $"lab{Repository.GetInstance().LabelCounter + 1}";
                }
            }
            else
            {
                idInput.IsEnabled = true;
                idInput.Text = "";
            }
        }
    }
}
