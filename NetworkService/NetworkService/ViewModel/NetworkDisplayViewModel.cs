using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        private string Slika = "";
        private Entity SelectedEntity { get; set; } = new Entity();
        private DisplayEntity displayEntity = new DisplayEntity();
        private bool dragging = false;
        public static ObservableCollection<DisplayEntity> entities = new ObservableCollection<DisplayEntity>();
        public Entity SelectedItem { get; set; }
        private Canvas tempCanvas;
        public static ObservableCollection<Images> canvasImages { get; set; } = new ObservableCollection<Images>();

        public static ObservableCollection<Entity> DisplayEntities { get; private set; } = new ObservableCollection<Entity>();

        public MyICommand<ListView> ListViewSelectionChanged { get; set; }
        public MyICommand ListViewMouseLeftButtonUp { get; set; }
        public MyICommand<Canvas> drop { get; set; }
        public MyICommand<Canvas> mouseDown { get; set; }
        public MyICommand<Canvas> buttonClick { get; set; }
        public MyICommand refreshClick { get; set; }


        public NetworkDisplayViewModel()
        {
            if(entities.Count > 0)
            {
                foreach(DisplayEntity entity in entities)
                {
                    SetCanvasBackground(getCanvasIdFromName(entity.Id), entity.Entity.Type.ImageSource);
                }
            }
            else
            {
                SetCanvasBackgroundsToWhite();

                DisplayEntities.Clear();
                foreach (Entity entity in MainWindowViewModel.Entities)
                {
                    DisplayEntities.Add(entity);
                }
            }
            
            ListViewSelectionChanged = new MyICommand<ListView>(OnListViewSelectionChanged);
            ListViewMouseLeftButtonUp = new MyICommand(OnListViewMouseLeftButtonUp);
            drop = new MyICommand<Canvas>(Ondrop);
            mouseDown = new MyICommand<Canvas>(OnmouseDown);
            buttonClick = new MyICommand<Canvas>(OnbuttonClick);
        }


        private void SetCanvasBackgroundsToWhite()
        {
            BitmapImage background = new BitmapImage();
            background.BeginInit();
            background.UriSource = new Uri("pack://application:,,,/ViewModel/Images/white.png");
            background.EndInit();

            for (int i = 0; i < 12; i++)
            {
                canvasImages.Add(new Images() { Image = new ImageBrush(background), Used=false });
            }
        }

        private void SetCanvasBackground(int id, string path)
        {
            BitmapImage background = new BitmapImage();
            background.BeginInit();
            background.UriSource = new Uri(path);
            background.EndInit();

            canvasImages[id].Image = new ImageBrush(background);
            if (path.Equals("pack://application:,,,/ViewModel/Images/white.png"))
            {
                canvasImages[id].Used = false;
            }
            else
            {
                canvasImages[id].Used = true;
            }
            
        }


        private void OnListViewSelectionChanged(ListView listView)
        {
            if (!dragging && SelectedItem != null)
            {
                dragging = true;
                SelectedEntity = SelectedItem;
                Slika = SelectedEntity.Type.ImageSource;

                DragDrop.DoDragDrop(listView, Slika, DragDropEffects.Copy);
            }
        }

        private void OnListViewMouseLeftButtonUp()
        {
            dragging = false;
            Slika = "";
        }

        private void Ondrop(Canvas canvas)
        {
            dragging = false;
            if (SelectedEntity != null)
            {
                if (!canvasImages[getCanvasIdFromName(canvas.Name)].Used)
                {
                    if (SelectedEntity.EntityValue < 670 || SelectedEntity.EntityValue > 735)
                    {
                        Slika = "pack://application:,,,/ViewModel/Images/error.png";
                    }
                    SetCanvasBackground(getCanvasIdFromName(canvas.Name), Slika);

                    DisplayEntity de = new DisplayEntity();
                    de.Entity = SelectedEntity;
                    de.Id = canvas.Name;

                    entities.Add(de);

                    DisplayEntity temp = new DisplayEntity();
                    if (tempCanvas != null && !canvas.Name.Equals(tempCanvas.Name))
                    {
                        foreach (DisplayEntity displayEntity in entities)
                        {
                            if (tempCanvas.Name.Equals(displayEntity.Id))
                            {
                                temp = displayEntity;
                            }
                        }

                        SetCanvasBackground(getCanvasIdFromName(tempCanvas.Name), "pack://application:,,,/ViewModel/Images/white.png");
                        tempCanvas = null;
                        if (temp != null)
                        {
                            entities.Remove(temp);
                        }
                    }
                    DisplayEntities.Remove(SelectedEntity);
                }
            }
        }

       

        private int getCanvasIdFromName(string name)
        {
            try
            {
                int id;
                string value = name.Substring(10);
                id = int.Parse(value);
                return --id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        private void OnmouseDown(Canvas canvas)
        {
            if (!dragging)
            {
                foreach (DisplayEntity entity in entities)
                {
                    if (entity.Id.Equals(canvas.Name))
                    {
                        dragging = true;
                        tempCanvas = canvas;
                        SelectedEntity = entity.Entity;
                        Slika = SelectedEntity.Type.ImageSource;
                        DragDrop.DoDragDrop(canvas, Slika, DragDropEffects.Copy);
                        break;
                    }
                }

            }
        }

        private void OnbuttonClick(Canvas canvas)
        {
            bool done = false;
            DisplayEntity entity1 = new DisplayEntity();
            Canvas findCanvas = canvas;
            if (findCanvas != null)
            {
                foreach (DisplayEntity entity in entities)
                {
                    if (entity.Id.Equals(findCanvas.Name))
                    {
                        foreach (Entity entity2 in NetworkEntitiesViewModel.BackUpEntities)
                        {
                            if (entity2.Id.Equals(entity.Entity.Id))
                            {
                                SetCanvasBackground(getCanvasIdFromName(findCanvas.Name), "pack://application:,,,/ViewModel/Images/white.png");
                                NetworkDisplayViewModel.DisplayEntities.Add(entity2);
                                done = true;
                                entity1 = entity;
                                break;
                            }
                        }
                    }
                }
                if (done)
                {
                    entities.Remove(entity1);
                }
            }
        }

        public void UpdateCanvas()
        {
            Console.WriteLine("Update!!!");
            foreach (DisplayEntity entity in entities)
            {
                if (entity.Entity.EntityValue < 670 || entity.Entity.EntityValue > 735)
                {
                    SetCanvasBackground(getCanvasIdFromName(entity.Id), "pack://application:,,,/ViewModel/Images/error.png");                    
                    Console.WriteLine("Error out of limit " + entity.Id);
                    //Console.WriteLine("Ovde 111 " + entity.Id);
                }
                else
                {
                    SetCanvasBackground(getCanvasIdFromName(entity.Id), entity.Entity.Type.ImageSource);
                    Console.WriteLine("Valid value" + entity.Id);
                    //Console.WriteLine("Ovde 222 " + entity.Id);
                }
                Console.WriteLine("ovde 333");
            }
            Console.WriteLine("Kraj!");
        }


    }
}
