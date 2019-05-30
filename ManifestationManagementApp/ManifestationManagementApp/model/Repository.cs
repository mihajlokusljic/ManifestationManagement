using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ManifestationManagementApp.model
{
    public class Repository: INotifyPropertyChanged
    {
        private static string dataFilepath = "..\\..\\resources\\data\\data.xml";
        private ObservableCollection<Label> labels;
        public ObservableCollection<Label> Labels
        {
            get { return labels; }
            set
            {
                if(labels != value)
                {
                    labels = value;
                    OnPropertyChanged("Labels");
                }
            }
        }

        public ObservableCollection<ManifestationType> ManifestationTypes { get; set; }
        public ObservableCollection<Manifestation> Manifestations { get; set; }
        public ObservableCollection<Map> Maps { get; set; }
        private static Repository instance = null;

        private Repository()
        {
            Labels = new ObservableCollection<Label>();
            ManifestationTypes = new ObservableCollection<ManifestationType>();
            Manifestations = new ObservableCollection<Manifestation>();
            Maps = new ObservableCollection<Map>();
        }

        public static Repository GetInstance()
        {
            if(instance == null)
            {
                instance = new Repository();
            }
            return instance;
        }

        public Label FindLabel(string id)
        {
            foreach(Label lab in Labels)
            {
                if(lab.Id == id)
                {
                    return lab;
                }
            }
            return null;
        }

        public bool Addlabel(Label newLabel)
        {
            if(FindLabel(newLabel.Id) != null)
            {
                return false;
            }
            Labels.Add(newLabel);
            SaveData();
            return true;
        }

        public bool UpdateLabel(Label newLabelData)
        {
            Label target = FindLabel(newLabelData.Id);
            if (target == null)
            {
                return false;
            }
            target.Color = newLabelData.Color;
            target.Description = newLabelData.Description;
            SaveData();
            return true;
        }

        public bool DeleteLabel(string id)
        {
            Label target = FindLabel(id);
            if(target == null)
            {
                return false;
            }
            Labels.Remove(target);
            SaveData();
            return true;
        }

        public ManifestationType FindManifestationType(string id)
        {
            foreach(ManifestationType type in ManifestationTypes)
            {
                if(type.Id == id)
                {
                    return type;
                }
            }
            return null;
        }

        public bool AddManifestationType(ManifestationType newType)
        {
            if(FindLabel(newType.Id) != null)
            {
                return false;
            }
            ManifestationTypes.Add(newType);
            ManifestationType.counter++;
            SaveData();
            return true;
        }

        public bool UpdateManifestationType(ManifestationType newTypeData)
        {
            ManifestationType target = FindManifestationType(newTypeData.Id);
            if (target == null)
            {
                return false;
            }
            target.Name = newTypeData.Name;
            target.IconPath = newTypeData.IconPath;
            target.Description = newTypeData.Description;
            SaveData();
            return true;
        }

        public bool DeleteManifestationType(string id)
        {
            ManifestationType target = FindManifestationType(id);
            if(target == null)
            {
                return false;
            }
            ManifestationTypes.Remove(target);
            SaveData();
            return true;
        }



        public void SaveData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Repository));
            using(StreamWriter sw = File.CreateText(dataFilepath))
            {
                serializer.Serialize(sw, this);
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


    }
}
