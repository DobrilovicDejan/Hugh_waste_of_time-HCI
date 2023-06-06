using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class Filter : BindableBase
    {
        string protok;
        bool greaterThan;
        bool lessThan;
        bool equalAs;
        string filterValue;

        public string Protok
        {
            get { return protok; }
            set
            {
                if(protok != value)
                {
                    protok = value;
                    OnPropertyChanged("protok");
                }
            }
        }

        public bool GreaterThan
        {
            get { return greaterThan; }
            set
            {
                if(greaterThan != value)
                {
                    greaterThan = value;
                    OnPropertyChanged("GreaterThan");
                }
            }
        }

        public bool EqualAs
        {
            get { return equalAs; }
            set
            {
                if (equalAs != value)
                {
                    equalAs = value;
                    OnPropertyChanged("EqualAs");
                }
            }
        }

        public bool LessThan
        {
            get { return lessThan; }
            set
            {
                if (lessThan != value)
                {
                    lessThan = value;
                    OnPropertyChanged("LessThan");
                }
            }
        }

        public string FilterValue
        {
            get { return filterValue; }
            set
            {
                if (filterValue != value)
                {
                    filterValue = value;
                    OnPropertyChanged("FilterValue");
                }
            }
        }
    }
}
