using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        HoldKeyDetector h;
        MainWindow mw = new WpfApplication2.MainWindow();


        private static List<string> a = new List<string>();
        private static int aCode = 65;

        static App() {
            a.Add("à");
            a.Add("å");
            a.Add("á");
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            h = new HoldKeyDetector(Method1);
        }

        private static List<String> specialCharsOf(int code){

            if (code == aCode)
                return a;
            return null;
        }
        

        public void Method1(int code, string input)
        {
            List<String> specialChars = specialCharsOf(code);
            if (specialChars == null)
                return;
            Console.WriteLine("Showing " + code + " " + string.Join(",", specialChars));
            mw.addCharacters(specialChars);
            mw.Show();
            mw.Activate();
        }
    }
}
