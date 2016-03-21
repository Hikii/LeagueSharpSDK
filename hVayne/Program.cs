using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hVayne.Champions;
using LeagueSharp;
using LeagueSharp.SDK;

namespace hVayne
{
    class Program
    {
        static void Main(string[] args)
        {
            Events.OnLoad += Load;
        }

        private static void Load(object sender, EventArgs e)
        {
            if (ObjectManager.Player.ChampionName == "Vayne")
            {
                // ReSharper disable once ObjectCreationAsStatement
                new Vayne();
            }
            else
            {
                Console.WriteLine("{0} not supported :roto2: ",ObjectManager.Player.ChampionName);
            }
        }
    }
}
