using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace ManifestationManagementApp.model
{
    public class Repository
    {
        private static string dataFilepath = "..\\..\\resources\\data\\data.xml";
        public List<Label> Labels { get; set; }
        public List<ManifestationType> ManifestationTypes { get; set; }
        public List<Manifestation> Manifestations { get; set; }
        public List<Map> Maps { get; set; }
        private static Repository instance = null;

        private Repository()
        {
            Labels = new List<Label>();
            ManifestationTypes = new List<ManifestationType>();
            Manifestations = new List<Manifestation>();
            Maps = new List<Map>();
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

        public void SaveData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Repository));
            using(StreamWriter sw = File.CreateText(dataFilepath))
            {
                serializer.Serialize(sw, this);
            }
        }


    }
}
