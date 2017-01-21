using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int OPTIONS_NUMBER = 6;
        private List<CharacterNumber> characterNumberComponents = new List<CharacterNumber>();

        internal Controller controller { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }        

        private void InitializeCustomComponents()
        {
            for (int i = 0; i < OPTIONS_NUMBER; i++)
            {
                CharacterNumber control = grid.Children.Cast<CharacterNumber>().First(e => Grid.GetRow(e) == 0 && Grid.GetColumn(e) == (i + 1));
                control.MouseDown += Control_MouseDown;
                characterNumberComponents.Add(control);
            }
        }        

        public void addCharacters(List<string> specialCharacters) {
            int count = specialCharacters.Count;
            for (int i = 0; i < count; i++) {
                CharacterNumber control = characterNumberComponents[i];
                control.setContent(specialCharacters[i], i + 1);
                control.Visibility = Visibility.Visible;                
            }
            for (int i = count; i < OPTIONS_NUMBER; i++) {
                CharacterNumber control = characterNumberComponents[i];
                control.Visibility = Visibility.Hidden;
            }
        }

        private void Control_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CharacterNumber senderControl = (CharacterNumber)sender;
            string content = (string)senderControl.characterLabel.Content;
            controller.SpecialCharacterClicked(content);
        }
        
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            controller.WindowDeactivated();            
        }        
    }
}
