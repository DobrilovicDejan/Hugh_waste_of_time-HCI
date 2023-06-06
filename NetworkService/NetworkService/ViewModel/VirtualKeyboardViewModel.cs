using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class VirtualKeyboardViewModel:BindableBase
    {
        public static string InputText { get; set; }

        private string input;
        public string Input
        {
            get { return input; }
            set
            {
                if(input != value)
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

        public VirtualKeyboardViewModel()
        {
            buttonPress = new MyICommand<string>(OnbuttonPress);
            buttonDelete = new MyICommand(OnbuttonDelete);
            buttonDeleteAll = new MyICommand(OnbuttonDeleteAll);
            buttonEnter = new MyICommand(OnbuttonEnter);
        }

        private void OnbuttonPress(string button)
        {
            Input += button;
        }

        private void OnbuttonDelete()
        {
            if(Input.Count() > 0)
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
            Console.WriteLine("enter");
            NetworkEntitiesViewModel.WriteTextBox.Text = Input;
            Input = "";
        }
    }
}
