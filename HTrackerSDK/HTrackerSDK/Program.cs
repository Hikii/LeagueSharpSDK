using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.UI.IMenu;

namespace HTrackerSDK
{
    class Program
    {
        static void Main(string[] args)
        {
            Events.OnLoad += OnLoad;
        }
        private static void OnLoad(object sender, EventArgs e)
        {
            Tracker.OnLoad();
            WardTracker.OnLoad();
            CloneTracker.OnLoad();
            PathTracker.OnLoad();
        }
    }
}
