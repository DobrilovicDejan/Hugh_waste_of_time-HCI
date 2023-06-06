using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NetworkService.ViewModel
{
    public class NetworkEntitiesViewModel : BindableBase
    {
        public ObservableCollection<Entity> NetworkEntities { get; private set; } = new ObservableCollection<Entity>();
        public static ObservableCollection<Entity> BackUpEntities { get; private set; } = new ObservableCollection<Entity>();

        public MyICommand Refresh { get; set; }
        public MyICommand Add { get; set; }
        public MyICommand Delete { get; set; }
        public MyICommand Filter { get; set; }
        public MyICommand<TextBox> gotFocusId { get; set; }
        public MyICommand<TextBox> gotFocusFilter { get; set; }
        public MyICommand<TextBox> gotFocusName { get; set; }

        public ObservableCollection<ProtokType> protokTypes { get; private set; } = new ObservableCollection<ProtokType>();

        public Entity newEntity { get; set; } = new Entity();

        private Entity selectedEntity;
        public Entity SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                selectedEntity = value;
                Delete.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedEntity");
            }
        }

        private string entityprotokType;

        public string EntityprotokType
        {
            get { return entityprotokType; }
            set
            {
                if(entityprotokType != value)
                {
                    entityprotokType = value;
                    newEntity.Type.Protok = entityprotokType;
                    OnPropertyChanged("EntityprotokType");
                    Add.RaiseCanExecuteChanged();
                }
            }
        }

        public Filter FilterEntities { get; set; } = new Filter();
        public static TextBox WriteTextBox { get; set; } = new TextBox();

        private Visibility keyboardVisible;
        private string valueChanged;

        public Visibility KeyboardVisible
        {
            get { return keyboardVisible; }
            set
            {
                if(keyboardVisible != value)
                {
                    keyboardVisible = value;
                    OnPropertyChanged("KeyboardVisible");
                }
            }
        }



        public NetworkEntitiesViewModel()
        {
            newEntity.Type = new ProtokType();
            protokTypes.Add(new ProtokType() { ImageSource = "Images/zapreminski.jpg", Protok = "Zapreminski" });
            protokTypes.Add(new ProtokType() { ImageSource = "Images/turbinski.jpg", Protok = "Turbinski" });
            protokTypes.Add(new ProtokType() { ImageSource = "Images/elektronski.jpg", Protok = "Elektronski" });
            Refresh = new MyICommand(OnRefresh);
            Add = new MyICommand(OnAdd, CanAdd);
            Delete = new MyICommand(OnDelete);
            Filter = new MyICommand(OnFilter, CanFilter);
            gotFocusId = new MyICommand<TextBox>(OngotFocusId);
            gotFocusName = new MyICommand<TextBox>(OngotFocusName);
            gotFocusFilter = new MyICommand<TextBox>(OngotFocusFilter);
            KeyboardVisible = Visibility.Collapsed;
            OnRefresh();

            buttonPress = new MyICommand<string>(OnbuttonPress);
            buttonDelete = new MyICommand(OnbuttonDelete);
            buttonDeleteAll = new MyICommand(OnbuttonDeleteAll);
            buttonEnter = new MyICommand(OnbuttonEnter);
            // newEntity.Type.ImageSource = "hello";
        }

        private void OnRefresh()
        {
            NetworkEntities.Clear();
            foreach (Entity entity1 in BackUpEntities)
            {
                NetworkEntities.Add(entity1);
            }

        }

        private void OngotFocusId(TextBox textBox)
        {
            KeyboardVisible = Visibility.Visible;
            WriteTextBox = textBox;
            Input = textBox.Text;
            valueChanged = "Id";
            
            
        }

        private void OngotFocusName(TextBox textBox)
        {
            KeyboardVisible = Visibility.Visible;
            WriteTextBox = textBox;
            Input = textBox.Text;
            valueChanged = "Name";
        }
        private void OngotFocusFilter(TextBox textBox)
        {
            KeyboardVisible = Visibility.Visible;
            WriteTextBox = textBox;
            Input = textBox.Text;
            valueChanged = "Filter";
        }

        private void OnAdd()
        {
            foreach(Entity entity in BackUpEntities)
            {
                if (entity.Id.Equals(newEntity.Id))
                {
                    entity.Name = newEntity.Name;
                    entity.Type.Protok = newEntity.Type.Protok;
                    entity.Type.ImageSource = newEntity.Type.ImageSource;
                    entity.EntityValue = newEntity.EntityValue;
                    return;
                }
            }
            Entity entity2 = CreateNewEntity(newEntity);

            MainWindowViewModel.LastAction = entity2;
            MainWindowViewModel.LastActionId = Actions.ADD;
            NetworkEntities.Add(entity2);
            BackUpEntities.Add(entity2);
            MainWindowViewModel.Entities.Add(entity2);
            NetworkDisplayViewModel.DisplayEntities.Add(entity2);
        }

        public void UndoAdd(Entity entity)
        {
            BackUpEntities.Remove(entity);
            MainWindowViewModel.Entities.Remove(entity);
            NetworkEntities.Remove(entity);
            NetworkDisplayViewModel.DisplayEntities.Remove(entity);

            OnRefresh();
            
        }

        private void OnDelete()
        {
            MainWindowViewModel.LastAction = selectedEntity;
            MainWindowViewModel.LastActionId = Actions.DELETE;
            BackUpEntities.Remove(SelectedEntity);
            MainWindowViewModel.Entities.Remove(selectedEntity);
            NetworkEntities.Remove(SelectedEntity);
            NetworkDisplayViewModel.DisplayEntities.Remove(selectedEntity);
            OnRefresh();
        }

        public void UndoDelete(Entity entity)
        {
            NetworkEntities.Add(entity);
            BackUpEntities.Add(entity);
            MainWindowViewModel.Entities.Add(entity);
            NetworkDisplayViewModel.DisplayEntities.Add(entity);
            OnRefresh();
        }

        private bool CanDelete()
        {
            if (SelectedEntity != null)
                return true;
            return false;  
        }

        private void OnFilter()
        {
            NetworkEntities.Clear();

            try
            {
                if(int.Parse(FilterEntities.FilterValue) != 0 && (FilterEntities.LessThan || FilterEntities.GreaterThan || FilterEntities.EqualAs))
                {
                    LessOrGreaterFilter();
                }
                else
                {
                    protokFilter();
                }
            }
            catch (Exception)
            {
                protokFilter();
            }
        }

        private bool CanFilter()
        {
            try
            {
                int.Parse(FilterEntities.FilterValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool CanAdd()
        {
            try
            {
                int.Parse(newEntity.Id);
                if(newEntity.Name != null && newEntity.Name.Trim().Length > 0 && newEntity.Type.Protok != null)
                    return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void protokFilter()
        {
            try
            {
                if(FilterEntities.Protok != null)
                {
                    if (FilterEntities.Protok.Equals("Zapraminski"))
                    {
                        foreach (Entity entity in BackUpEntities)
                        {
                            if (entity.Type.Protok.Equals("Zapreminski"))
                            {
                                NetworkEntities.Add(entity);
                            }
                        }
                    }
                    else if (FilterEntities.Protok.Equals("Turbinski"))
                    {
                        foreach (Entity entity in BackUpEntities)
                        {
                            if (entity.Type.Protok.Equals("Turbinski"))
                            {
                                NetworkEntities.Add(entity);
                            }
                        }
                    }
                    else if (FilterEntities.Protok.Equals("Elektronski"))
                    {
                        foreach (Entity entity in BackUpEntities)
                        {
                            if (entity.Type.Protok.Equals("Elektronski"))
                            {
                                NetworkEntities.Add(entity);
                            }
                        }
                    }
                }
                
            }catch (Exception)
            {

            }
            
        }

        public void LessOrGreaterFilter()
        {
            int fvalue;
            try
            {
                fvalue = int.Parse(FilterEntities.FilterValue);
            }
            catch (Exception)
            {
                fvalue = -1;
            }

            if (FilterEntities.Protok.Equals("Zapreminski"))
            {
                foreach (Entity entity in BackUpEntities)
                {
                    if (entity.Type.Protok.Equals("Zapreminski"))
                    {
                        if (FilterEntities.GreaterThan == true)
                        {
                            if (int.Parse(entity.Id) > fvalue)
                            {
                                NetworkEntities.Add(entity);
                            }
                        }

                        else if (FilterEntities.EqualAs == true)
                        {
                            if (int.Parse(entity.Id) == fvalue)
                            {
                                NetworkEntities.Add(entity);
                            }
                        }
                        else if (FilterEntities.LessThan == true)
                        {
                            if (int.Parse(entity.Id) < fvalue)
                            {
                                NetworkEntities.Add(entity);
                            }
                        }

                    }
                }
            }
            else if (FilterEntities.Protok.Equals("Turbinski"))
            {
                foreach (Entity entity in BackUpEntities)
                {
                    if (entity.Type.Protok.Equals("Turbinski"))
                    {
                        if (FilterEntities.GreaterThan == true)
                        {
                            if (int.Parse(entity.Id) > fvalue)
                            {
                                NetworkEntities.Add(entity);
                            }
                        }

                        else if (FilterEntities.EqualAs == true)
                        {
                            if (int.Parse(entity.Id) == fvalue)
                            {
                                NetworkEntities.Add(entity);
                            }
                        }
                        else if (FilterEntities.LessThan == true)
                        {
                            if (int.Parse(entity.Id) < fvalue)
                            {
                                NetworkEntities.Add(entity);
                            }
                        }

                    }
                }
            }
            else if (FilterEntities.Protok.Equals("Elektronski"))
            {
                foreach (Entity entity in BackUpEntities)
                {
                    if (entity.Type.Protok.Equals("Elektronski"))
                    {
                        if (FilterEntities.GreaterThan == true)
                        {
                            if (int.Parse(entity.Id) > fvalue)
                            {
                                NetworkEntities.Add(entity);
                            }
                        }

                        else if (FilterEntities.EqualAs == true)
                        {
                            if (int.Parse(entity.Id) == fvalue)
                            {
                                NetworkEntities.Add(entity);
                            }
                        }
                        else if (FilterEntities.LessThan == true)
                        {
                            if (int.Parse(entity.Id) < fvalue)
                            {
                                NetworkEntities.Add(entity);
                            }
                        }

                    }
                }
            }
        }

        public static Entity CreateNewEntity(Entity entity)
        {
            try
            {
                Entity entity2 = new Entity();
                entity2.Id = entity.Id;
                entity2.Name = entity.Name;
                entity2.Type = new ProtokType();
                entity2.Type.Protok = entity.Type.Protok;
                entity2.Type.ImageSource = entity.Type.ImageSource;
                entity2.EntityValue = entity.EntityValue;
                return entity2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        public static string InputText { get; set; }

        private string input;
        public string Input
        {
            get { return input; }
            set
            {
                if (input != value)
                {
                    input = value;
                    OnPropertyChanged("Input");
                }
            }
        }
        public MyICommand<string> buttonPress { get; set; }
        public MyICommand buttonDelete { get; set; }
        public MyICommand buttonDeleteAll { get; set; }
        public MyICommand buttonEnter { get; set; }

    

        private void OnbuttonPress(string button)
        {
            Input += button;
        }

        private void OnbuttonDelete()
        {
            if (Input.Count() > 0)
            {
                Input = Input.Remove(Input.Length - 1);
            }

        }

        private void OnbuttonDeleteAll()
        {
            Input = "";
        }

        private void OnbuttonEnter()
        {
            if(valueChanged != null)
            {
                if (valueChanged.Equals("Id"))
                {
                    newEntity.Id = Input;
                    Add.RaiseCanExecuteChanged();
                }
                else if(valueChanged.Equals("Name"))
                {
                    newEntity.Name = Input;
                    Add.RaiseCanExecuteChanged();
                }
                else if (valueChanged.Equals("Filter"))
                {
                    FilterEntities.FilterValue = Input;
                    Filter.RaiseCanExecuteChanged();
                }
            }
            WriteTextBox.Text = Input;
            Input = "";
            KeyboardVisible = Visibility.Collapsed;
        }
    }
}
