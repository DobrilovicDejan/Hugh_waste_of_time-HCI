using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public enum Actions { NO_ACTION, ADD, DELETE, WINDOW_CHANGED};
    public class MainWindowViewModel:BindableBase
    {

        public static ObservableCollection<Entity> Entities { get; set; } = new ObservableCollection<Entity>();
        private Entity entity;
        public ObservableCollection<string> menuItems { get; set; } = new ObservableCollection<string>();
        public MyICommand AddEntity { get; set; }
        public MyICommand ShowMenu { get; set; }
        public MyICommand SetMenuEntities { get; set; }
        public MyICommand SetMenuDisplay { get; set; }
        public MyICommand SetMenuGraph { get; set; }
        public MyICommand UndoAction { get; set; }
        public static Object LastAction { get; set; }
        public static Actions LastActionId { get; set; }

        public static string Path = AppDomain.CurrentDomain.BaseDirectory + "log.txt";

        private NetworkEntitiesViewModel networkEntitiesViewModel = new NetworkEntitiesViewModel();
        private NetworkDisplayViewModel networkDisplayViewModel = new NetworkDisplayViewModel();
        private NetworkGraphViewModel networkGraphViewModel = new NetworkGraphViewModel();
        private BindableBase currentViewModel;


        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }


        public string SelectedMenu
        {
            get
            {
                return SelectedMenu;
            }
            set
            {
                if(value != SelectedMenu)
                {
                    SelectedMenu = value;
                    OnPropertyChanged("SelectedMenu");
                }
            }
        }

        public MainWindowViewModel()
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
            menuItems.Add("Entities");
            menuItems.Add("Display");
            menuItems.Add("Graph");
            CurrentViewModel = networkEntitiesViewModel;
            //LoadEntities();

            AddEntity = new MyICommand(OnAddEntity);
            SetMenuEntities = new MyICommand(OnSetMenuEntities);
            SetMenuDisplay = new MyICommand(OnSetMenuDisplay);
            SetMenuGraph = new MyICommand(OnSetMenuGraph);
            UndoAction = new MyICommand(OnUndoAction);

            createListener(); //Povezivanje sa serverskom aplikacijom
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Entities.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji

                            SaveToFile(incomming); 
                            string[] str = incomming.Split('_', ':');
                            entity = new Entity() { Id = str[1], EntityValue = int.Parse(str[2]) };
                            AddNewEntity(entity);

                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }


        private void SaveToFile(string s)
        {
            using (StreamWriter writer = File.AppendText(Path))
            {
                writer.WriteLine(DateTime.Now + ":" + s);
            }
        }

        public void LoadEntities()
        {
            ObservableCollection<Entity> entities = new ObservableCollection<Entity>();
            ProtokType protoci = new ProtokType();
            protoci.Protok = "Zapreminski";
            protoci.ImageSource = "fas";
        }

        private void OnAddEntity()
        {
            if (entity != null)
            {
                Entities.Add(entity);
                Console.WriteLine(entity.EntityValue);
            }
            
        }

        private void AddNewEntity(Entity entity)
        {
            try
            {
                foreach(Entity e in Entities)
                {
                    if (e.Id.Equals(entity.Id))
                    {
                        Entities[int.Parse(entity.Id)].EntityValue = entity.EntityValue;

                        //networkDisplayViewModel.UpdateCanvas();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnUndoAction()
        {
            if(LastAction != null && LastActionId != Actions.NO_ACTION)
            {
                if (LastActionId == Actions.ADD)
                {
                    networkEntitiesViewModel.UndoAdd((Entity)LastAction);
                }
                if(LastActionId == Actions.DELETE)
                {
                    networkEntitiesViewModel.UndoDelete((Entity)LastAction);
                }
                if(LastActionId == Actions.WINDOW_CHANGED)
                {
                    CurrentViewModel = (BindableBase)LastAction;
                }
                
            }
            

            LastAction = null;
            LastActionId = Actions.NO_ACTION;
        }


        private void OnSetMenuEntities()
        {
            LastAction = CurrentViewModel;
            LastActionId = Actions.WINDOW_CHANGED;
            
            CurrentViewModel = networkEntitiesViewModel;
        }

        private void OnSetMenuDisplay()
        {
            LastAction = CurrentViewModel;
            LastActionId = Actions.WINDOW_CHANGED;
            CurrentViewModel = networkDisplayViewModel;
        }

        private void OnSetMenuGraph()
        {
            LastAction = CurrentViewModel;
            LastActionId = Actions.WINDOW_CHANGED;

            CurrentViewModel = networkGraphViewModel;
        }

    }
}
