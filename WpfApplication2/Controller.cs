using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace WpfApplication2
{
    class Controller
    {

        private HoldKeyDetector h;
        private MainWindow mw;
        private List<string> currentlyShowingSpecialChars = null;
        
        private const int ASCII_A = 65;

        private const int ASCII_0 = 48;
        private const int ASCII_6 = 54;

        Dictionary<string, List<string>> mapping = new Dictionary<string, List<string>>();


        public Controller()
        {
            mw = new WpfApplication2.MainWindow();
            mw.controller = this;
            ReadMappingFile();
        }

        public void StartKeyboardListening()
        {
            h = HoldKeyDetector.Instance;
            h.longPressCallback = LongPressCallback;
            h.shortPressCallback = ShortPressCallback;
        }

        private List<string> specialCharsOf(int code)
        {

            char c = (char)code;
            string s = c.ToString().ToLower();
            List<string> list = null;
            mapping.TryGetValue(s, out list);
            return list;
        }

        private void ShortPressCallback(int code)
        {
            if (this.currentlyShowingSpecialChars == null)
                return;

            if (code <= ASCII_0 || code > ASCII_6)
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

        private void ReadMappingFile()
        {
            string path = @"..\..\mapping.json";

            // This text is added only once to the file.
            if (!File.Exists(path))
                return;
            string json = File.ReadAllText(path);
            Console.WriteLine(json);

            var serializer = new JavaScriptSerializer();
            Dictionary<string, object> result = (Dictionary<string, object>)serializer.DeserializeObject(json);
            foreach (KeyValuePair<string, object> entry in result)
            {
                string from = entry.Key;
                object[] to = (object[])entry.Value;

                List<string> list = new List<string>();
                mapping.Add(from, list);
                foreach (object o in to) {
                    string s = (string)o;
                    list.Add(s);
                }                
            }
        }
    }
}
