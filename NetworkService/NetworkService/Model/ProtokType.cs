using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{

    public class ProtokType : BindableBase
    {
        private string protok;
        private string imageSource;

        public string Protok
        {
            get
            {
                return protok;
            }
            set
            {
                if(protok != value)
                {
                    if(value.Equals("Zapreminski")  || value.Equals("Turbinski") || value.Equals("Elektronski"))
                    {
                        protok = value;
                        OnPropertyChanged("protok");

                        switch (value.ToLower())
                        {
                            case "zapreminski":
                                ImageSource = "pack://application:,,,/ViewModel/Images/zapreminski.jpg";
                                OnPropertyChanged("ImageSource");
                                break;
                            case "turbinski":
                                ImageSource = "pack://application:,,,/ViewModel/Images/turbinski.jpg";
                                OnPropertyChanged("ImageSource"); 
                                break;
                            case "elektronski":
                                ImageSource = "pack://application:,,,/ViewModel/Images/elektronski.jpg";
                                OnPropertyChanged("ImageSource"); 
                                break;

                            default:
                                ImageSource = "pack://application:,,,/ViewModel/Images/error.png";
                                OnPropertyChanged("ImageSource");
                                break;
                        }
                        /*
                        if (value.Equals("Otvoren"))
                        {
                            ImageSource = "pack://application:,,,/ViewModel/Images/protokEmpty.jpg";
                            OnPropertyChanged("ImageSource");
                        }
                        else if()
                        {
                            ImageSource = "pack://application:,,,/ViewModel/Images/protokFull.jpg";
                            OnPropertyChanged("ImageSource");
                        }
                        */
                    }
                }
            }
        }


        public string ImageSource
        {
            get { return imageSource; }
            set
            {
                if(imageSource != value)
                {
                    imageSource = value;
                    OnPropertyChanged("ImageSource");

                }
            }
        }

        public override string ToString()
        {
            return protok;
        }
    }
}
