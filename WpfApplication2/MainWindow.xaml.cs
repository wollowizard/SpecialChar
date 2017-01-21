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
        public MainWindow()
        {
            InitializeComponent();
            DrawRectangle();
            //grid.ShowGridLines = true;
            //grid.Background = new SolidColorBrush(Colors.LightSteelBlue);

        }

        public void addCharacters(List<string> specialCharacters) {
            int count = specialCharacters.Count;
            for (int i = 0; i < count; i++) {
                CharacterNumber control = grid.Children.Cast<CharacterNumber>().First(e => Grid.GetRow(e) == 0 && Grid.GetColumn(e) == (i + 1));
                control.setContent(specialCharacters[i], i + 1);
                control.Visibility = Visibility.Visible;
                control.MouseDown += Control_MouseDown;
            }

        }

        private void Control_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CharacterNumber senderControl = (CharacterNumber)sender;
            Console.WriteLine("clicked on " + senderControl.characterLabel.Content);
            string content = (string)senderControl.characterLabel.Content;
            hideAndType(content);
            
        }

        private void DrawRectangle() {
            /*System.Windows.Shapes.Rectangle rect;
            rect = new System.Windows.Shapes.Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Width = 200;
            rect.Height = 200;
            Canvas.SetLeft(rect, 0);
            Canvas.SetTop(rect, 0);
            rect.StrokeThickness = 2;
            
            this.AddChild(rect);*/
        }

        
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            this.Hide();            
        }

        private void hideAndType(string specialCharacter) {
            this.OnDeactivated(null);

        }
    }
}
