using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.SDK.Core;
using LeagueSharp.SDK.Core.UI.IMenu;
using LeagueSharp.SDK.Core.UI.IMenu.Values;
using SharpDX.Direct3D9;

namespace HTrackerSDK
{
    class Tracker
    {
        public static Menu Menu;
        public static void OnLoad()
        {
            Menu = new Menu("HTracker", "HTracker", true);
            {
                var spellMenu = Menu.Add(new Menu("spell.track", "Spell Tracker"));
                {
                    spellMenu.Add(new MenuBool("track.ally.skill", "Track Ally Spells", false));
                    spellMenu.Add(new MenuBool("track.my.skill", "Track My Spells", false));
                    spellMenu.Add(new MenuBool("track.enemy.skill", "Track Enemy Spells", true));
                }
                Menu.Attach();
            }
            Game.PrintChat("<font color='#ff3232'>HTracker: </font> <font color='#d4d4d4'>If you like this assembly feel free to upvote on Assembly Database</font>");
            Drawing.OnDraw += OnDraw;
           
        }

        private static void OnDraw(EventArgs args)
        {
            if (Menu["spell.track"]["track.my.skill"])
            {
                SpellTracker.PlayerTracker();
            }
            if (Menu["spell.track"]["track.ally.skill"])
            {
                SpellTracker.AllyTracker();
            }
            if (Menu["spell.track"]["track.enemy.skill"])
            {
                SpellTracker.EnemyTracker();
            }
        }
    }
}
