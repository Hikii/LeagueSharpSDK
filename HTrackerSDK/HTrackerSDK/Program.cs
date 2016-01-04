using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.SDK.Core.Events;
using LeagueSharp.SDK.Core.UI.IMenu;

namespace HTrackerSDK
{
    class Program
    {
        static void Main(string[] args)
        {
            Load.OnLoad += OnLoad;
        }
        private static void OnLoad(object sender, EventArgs e)
        {
            Tracker.OnLoad();
        }
    }
}
