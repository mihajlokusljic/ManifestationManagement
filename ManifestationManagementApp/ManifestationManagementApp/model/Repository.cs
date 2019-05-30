using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ManifestationManagementApp.model
{
    public class Repository : INotifyPropertyChanged
    {
        private static string dataFilepath = "..\\..\\resources\\data\\data.xml";
        public int LabelCounter { get; set; }
        public int ManifestationTypeCounter { get; set; }
        public int ManifestationCounter { get; set; }
        public ObservableCollection<Label> labels { get; set; }
        public ObservableCollection<ManifestationType> manifestationTypes { get; set; }
        private ObservableCollection<Manifestation> manifestations;
        public List<Map> Maps { get; set; }
        private static Repository instance = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public ObservableCollection<Manifestation> Manifestations
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

        public ObservableCollection<ManifestationType> ManifestationTypes
        {
            get { return manifestationTypes; }
            set
            {
                if (value != manifestationTypes)
                {
                    manifestationTypes = value;
                    OnPropertyChanged("ManifestationTypes");
                }
            }
        }

        public ObservableCollection<Label> Labels
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
        private Repository()
        {
            Labels = new ObservableCollection<Label>();
            ManifestationTypes = new ObservableCollection<ManifestationType>();
            Manifestations = new ObservableCollection<Manifestation>();
            Maps = new List<Map>();
            LabelCounter = 0;
            ManifestationTypeCounter = 0;
            ManifestationCounter = 0;
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

        public Manifestation FindManifestation(string id)
        {
            foreach (Manifestation manif in Manifestations)
            {
                if (manif.Id == id)
                {
                    return manif;
                }
            }
            return null;
        }

        public bool AddManifestation(Manifestation newManif)
        {
            if (FindManifestation(newManif.Id) != null)
            {
                return false;
            }
            Manifestations.Add(newManif);
            SaveData();
            return true;
        }

        public bool UpdateManifestation(Manifestation newManifestationData)
        {
            Manifestation target = FindManifestation(newManifestationData.Id);
            if (target == null)
            {
                return false;
            }
            target.Name = newManifestationData.Name;
            target.Type = newManifestationData.Type;
            target.Labels = newManifestationData.Labels;
            target.SmokingAllowed = newManifestationData.SmokingAllowed;
            target.SupportHandicaped = newManifestationData.SupportHandicaped;
            target.Prices = newManifestationData.Prices;
            target.Alcohol = newManifestationData.Alcohol;
            target.IconPath = newManifestationData.IconPath;
            target.Description = newManifestationData.Description;
            SaveData();
            return true;
        }

        public bool DeleteManifestation(string id)
        {
            Manifestation target = FindManifestation(id);
            if (target == null)
            {
                return false;
            }
            Manifestations.Remove(target);
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

        public void ReadData()
        {
            XmlSerializer reader = new XmlSerializer(typeof(Repository));
            using (StreamReader file = new StreamReader(dataFilepath))
            {
                Repository rep = (Repository)reader.Deserialize(file);
                LabelCounter = rep.LabelCounter;
                ManifestationTypeCounter = rep.ManifestationTypeCounter;
                Labels = rep.Labels;
                ManifestationTypes = rep.ManifestationTypes;
                Manifestations = rep.Manifestations;
                Maps = rep.Maps;
            }
        }


    }
}
