using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppVisuals;
using ConsoleAppVisuals.AnimatedElements;
using ConsoleAppVisuals.Enums;
using ConsoleAppVisuals.InteractiveElements;
using ConsoleAppVisuals.PassiveElements;
using Projet;

namespace Projet_SouidiCazac
{
    class Program
    {
        static void Main(string[] args)
        {
            Window.Open();

            Header header = new Header();
            Title title = new Title("VeloMax");
            Footer footer = new Footer();
            Window.AddElement(header, footer, title);
            Window.Render();

            DataAccess velomax = Interface.dbConnection();

            Interface.mainMenu(velomax);

            Window.Close();
        }
    }
}
