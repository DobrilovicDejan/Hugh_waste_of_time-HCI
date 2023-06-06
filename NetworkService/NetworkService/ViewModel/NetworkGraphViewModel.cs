using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class NetworkGraphViewModel :BindableBase
    {

        public static ObservableCollection<Entity> Entities { get; set; } = new ObservableCollection<Entity>();
        public ObservableCollection<string> Ids { get; private set; } = new ObservableCollection<string>();
        private int value0;
        private int value1;
        private int value2;
        private int value3;
        private int value4; 
        private int gvalue0;
        private int gvalue1;
        private int gvalue2;
        private int gvalue3;
        private int gvalue4;
        private int lvalue0;
        private int lvalue1;
        private int lvalue2;
        private int lvalue3;



        public int Value0
        {
            get { return value0; }
            set
            {
                if (value != value0)
                {
                    value0 = value;
                    gvalue0 = (int)(value *0.8 -200);
                    lvalue0 = (gvalue0 < gvalue1) ? gvalue0 +15 : gvalue1 +15;
                    OnPropertyChanged("Lvalue0");
                    OnPropertyChanged("Value0");
                    OnPropertyChanged("Gvalue0");
                }
            }
        }

        public int Value1
        {
            get { return value1; }
            set
            {
                if (value != value1)
                {
                    value1 = value;
                    gvalue1 = (int)(value * 0.8 -200);
                    lvalue1 = (gvalue1 < gvalue2) ? gvalue1 + 15 : gvalue2 + 15;
                    OnPropertyChanged("Lvalue1");
                    OnPropertyChanged("Value1");
                    OnPropertyChanged("Gvalue1");
                }
            }
        }

        public int Value2
        {
            get { return value2; }
            set
            {
                if (value != value2)
                {
                    value2 = value;
                    gvalue2 = (int)(value * 0.8 - 200);
                    lvalue2 = (gvalue2 < gvalue3) ? gvalue2 + 15 : gvalue3 + 15;
                    OnPropertyChanged("Lvalue2");
                    OnPropertyChanged("Value2");
                    OnPropertyChanged("Gvalue2");
                }
            }
        }

        public int Value3
        {
            get { return value3; }
            set
            {
                if (value != value3)
                {
                    value3 = value;
                    gvalue3 = (int)(value * 0.8 - 200);
                    lvalue3 = (gvalue3 < gvalue4) ? gvalue3 + 15 : gvalue4 + 15;
                    OnPropertyChanged("Value3");
                    OnPropertyChanged("Lvalue3");
                    OnPropertyChanged("Gvalue3");
                }
            }
        }

        public int Value4
        {
            get { return value4; }
            set
            {
                if (value != value4)
                {

                    value4 = value;
                    gvalue4 = (int)(value * 0.8 - 200);
                    OnPropertyChanged("Value4");
                    OnPropertyChanged("Gvalue4");
                }
            }
        }



        private string selectedValue;
        public string SelectedValue
        {
            get { return selectedValue; }
            set
            {
                if(value != selectedValue)
                {
                    selectedValue = value;
                    OnPropertyChanged("SelectedValue");
                }
            }
        }

        private string FilterValue = null;

        public MyICommand ShowFilter { get; set; }
        public int Gvalue0 { get => (int)(value0 * 0.8 -200); set => gvalue0 = value; }
        public int Gvalue1 { get => (int)(value1 * 0.8 -200); set => gvalue1 = value; }
        public int Gvalue2 { get => (int)(value2 * 0.8 - 200); set => gvalue2 = value; }
        public int Gvalue3 { get => (int)(value3 * 0.8 - 200); set => gvalue3 = value; }
        public int Gvalue4 { get => (int)(value4 * 0.8 - 200); set => gvalue4 = value; }
        public int Lvalue0 { get => (gvalue0 < gvalue1) ? gvalue0 + 15 : gvalue1 + 15; set => lvalue0 = value; }
        public int Lvalue1 { get => (gvalue1 < gvalue2) ? gvalue1 + 15 : gvalue2 + 15; set => lvalue1 = value; }
        public int Lvalue2 { get => (gvalue2 < gvalue3) ? gvalue2 + 15 : gvalue3 + 15; set => lvalue2 = value; }
        public int Lvalue3 { get => (gvalue3 < gvalue4) ? gvalue3 + 15 : gvalue4 + 15; set => lvalue3 = value; }

        public NetworkGraphViewModel()
        {
            for(int i=0; i<MainWindowViewModel.Entities.Count; i++)
            {
                Ids.Add(i.ToString());
            }
            value0 = 0;
            value1 = 0;
            value2 = 0;
            value3 = 0;
            value4 = 0;


            ShowFilter = new MyICommand(OnShowFilter);

            ReadNewEntities(MainWindowViewModel.Path);
        }

        public void ReadNewEntities(string s)
        {
            var readThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        if (File.Exists(s))
                        {
                            string str;
                            string[] value;
                            double num;
                            string index;
                            using (StreamReader reader = new StreamReader(s))
                            {
                                Entities.Clear();
                                while (reader.Peek() >= 0)
                                {
                                    str = reader.ReadLine();
                                    value = str.Split(':', '_');
                                    num = int.Parse(value[5]);
                                    //num = 550 - num * 4;
                                    index = value[4];

                                    Entity entity = new Entity();
                                    entity.EntityValue = num;
                                    entity.Id = index;

                                    if (FilterValue == null)
                                    {
                                        Entities.Insert(0, entity);
                                    }
                                    else
                                    {
                                        if (FilterValue.Equals(entity.Id))
                                            Entities.Insert(0, entity);
                                    }

                                }

                            }


                            Value0 = (int)Entities[0].EntityValue;
                            Value1 = (int)Entities[1].EntityValue;
                            Value2 = (int)Entities[2].EntityValue;
                            Value3 = (int)Entities[3].EntityValue;
                            Value4 = (int)Entities[4].EntityValue;


                        }
                        
                    }catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                    Thread.Sleep(1000);
                }
            });
            readThread.IsBackground = true;
            readThread.Start();
            
        }

       private void OnShowFilter()
        {
            FilterValue = SelectedValue;
            Console.WriteLine("FilterValue = ");
            Console.WriteLine(FilterValue);
        }
      

    }
}
