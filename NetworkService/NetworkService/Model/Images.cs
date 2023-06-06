using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkService.Model
{
    public class Images:BindableBase
    {
        ImageBrush image;
        bool used;

        public ImageBrush Image
        {
            get { return image; }
            set
            {
                if(image != value)
                {
                    image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        public bool Used
        {
            get { return used; }
            set
            {
                if(used != value)
                {
                    used = value;
                    OnPropertyChanged("Used");
                }
            }
        }
    }
}
