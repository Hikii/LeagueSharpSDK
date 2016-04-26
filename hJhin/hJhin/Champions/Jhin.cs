using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hJhin.Extensions;
using hJhin.Modes;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;

namespace hJhin.Champions
{
    public class Jhin
    {
        public Jhin()
        {
            JhinOnLoad();
        }

        private static Orbwalker Orbwalker
        {
            get { return Variables.Orbwalker; }
        }

        private static void JhinOnLoad()
        {
            Notifications.Add(new Notification("hJhin - (click and read)", 
                "Jhin is well syncronized with scripting mechanisms. I developed this assembly to increase your ingame " +
                "performance with Jhin. With this assembly taking a control of your game is inevitable." +
                " Take a step in enjoy the smooth work. Thanks @Southpaw"));

            Spells.Initialize();
            Config.ExecuteMenu();

            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveMode)
            {
                case OrbwalkingMode.Combo:
                    Combo.Execute();
                    break;

                case OrbwalkingMode.LaneClear:
                    Jungle.Execute();
                    Clear.Execute();
                    break;

                case OrbwalkingMode.Hybrid:
                    Harass.Execute();
                    break;

                    case OrbwalkingMode.None:
                    Ultimate.Execute();
                    break;
            }

            if (ObjectManager.Player.IsActive(Spells.R))
            {
                Orbwalker.SetAttackState(false);
                Orbwalker.SetMovementState(false);
            }
            else
            {
                Orbwalker.SetAttackState(true);
                Orbwalker.SetMovementState(true);
            }

            if (Config.Menu["activator.settings"]["use.qss"] && (Items.HasItem((int)ItemId.Quicksilver_Sash) && Items.CanUseItem((int)ItemId.Quicksilver_Sash) ||
                Items.CanUseItem(3139) && Items.HasItem(3137)))
            {
                Qss.ExecuteQss();
            }

            if (Config.Menu["misc.settings"]["auto.orb.buy"] && ObjectManager.Player.Level >= Config.Menu["misc.settings"]["orb.level"]
                && !Items.HasItem((int)ItemId.Farsight_Orb_Trinket))
            {
                ObjectManager.Player.BuyItem(ItemId.Farsight_Orb_Trinket);
            }
        }
    }
}
