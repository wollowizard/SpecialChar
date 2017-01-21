using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    class Controller
    {

        private HoldKeyDetector h;
        private MainWindow mw;        
        private List<String> currentlyShowingSpecialChars = null;

        private static List<string> a = new List<string>();
        private static int aCode = 65;

        private const int ASCII_0 = 48;
        private const int ASCII_1 = 49;
        private const int ASCII_2 = 50;
        private const int ASCII_3 = 51;
        private const int ASCII_4 = 52;
        private const int ASCII_5 = 53;
        private const int ASCII_6 = 54;




        static Controller()
        {
            a.Add("à");
            a.Add("å");
            a.Add("á");
        }

        public Controller() {
            mw = new WpfApplication2.MainWindow();
            mw.controller = this;
        }

        public void StartKeyboardListening() {
            h = HoldKeyDetector.Instance;
            h.longPressCallback = LongPressCallback;
            h.shortPressCallback = ShortPressCallback;
        }

        private static List<String> specialCharsOf(int code)
        {
            if (code == aCode)
                return a;
            return null;
        }

        private void ShortPressCallback(int code)
        {
            Console.WriteLine("---------------------- " + code + "---------------------- ");
            if (this.currentlyShowingSpecialChars == null)
                return;

            if (code < ASCII_1 || code > ASCII_6)
                return;

            int number = code - ASCII_0;
            int index = number - 1;
            
            if (index >= currentlyShowingSpecialChars.Count)
                return;
            
            SpecialCharacterClicked(currentlyShowingSpecialChars[index]);
            
        }

        private void LongPressCallback(int code)
        {
            currentlyShowingSpecialChars = specialCharsOf(code);
            if (currentlyShowingSpecialChars == null)
                return;            
            mw.addCharacters(currentlyShowingSpecialChars);
            mw.Show();
            mw.Activate();
            h.listenForNumbersMode = true;
        }

        internal void WindowDeactivated()
        {
            h.listenForNumbersMode = false;
            mw.Hide();
            currentlyShowingSpecialChars = null;
        }

        internal void SpecialCharacterClicked(string specialCharacter)
        {
            WindowDeactivated();
            
            SendCharactersUtils.SendString(specialCharacter);
        }
    }
}
