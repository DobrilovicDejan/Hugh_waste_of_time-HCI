using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class Entity : BindableBase
    {
        private string id;
        private double entityValue;
        private string name;
        private ProtokType type;

        public ProtokType Type
        {
            get { return type; }
            set
            {
                if(value != null)
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if(name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                if(id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public double EntityValue
        {
            get { return entityValue; }
            set
            {
                if(entityValue != value)
                {
                    entityValue = value;
                    OnPropertyChanged("EntityValue");
                }
            }
        }

        public override string ToString()
        {
            return $"Id: {Id},  Name: {Name},  Value: {EntityValue}";
        }
    }
}
