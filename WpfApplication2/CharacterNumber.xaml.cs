using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for CharacterNumber.xaml
    /// </summary>
    public partial class CharacterNumber : UserControl
    {
        public CharacterNumber()
        {
            InitializeComponent();
        }

        public CharacterNumber(String character, int number)
        {
            InitializeComponent();
            setContent(character, number);
        }

        public void setContent(String character, int number) {

            this.characterLabel.Content = character;
            this.numberLabel.Content = number;
        }
    }
}
