using System;
using System.Collections.Specialized;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using LeagueSharp;
using LeagueSharp.SDK.Core;
using LeagueSharp.SDK.Core.Enumerations;
using LeagueSharp.SDK.Core.Extensions;
using LeagueSharp.SDK.Core.IDrawing;
using LeagueSharp.SDK.Core.UI.IMenu.Values;
using LeagueSharp.SDK.Core.Wrappers;
using LeagueSharp.SDK.Core.Events;
using LeagueSharp.SDK.Core.Extensions.SharpDX;
using LeagueSharp.SDK.Core.Utils;
using Color = System.Drawing.Color;
using Menu = LeagueSharp.SDK.Core.UI.IMenu.Menu;

namespace HikiCarry_Graves_SDK
{
    class Program
    {
        public static Menu Menu;
        public const string ChampionName = "Graves";

        public static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }
        public Orbwalker Orbwalker { get; set; }
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;


        static void Main(string[] args)
        {
            Load.OnLoad += Load_OnLoad;
        }

        private static void Load_OnLoad(object sender, EventArgs e)
        {
            if (Player.ChampionName != ChampionName)
                return;

            Q = new Spell(SpellSlot.Q, 720f);
            W = new Spell(SpellSlot.W, 850f);
            E = new Spell(SpellSlot.E, 425f);
            R = new Spell(SpellSlot.R, 1100f);

            Q.SetSkillshot(0.25f, 15f * (float)Math.PI / 180, 2000f, false, SkillshotType.SkillshotCone);
            W.SetSkillshot(0.25f, 250f, 1650f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.25f, 100f, 2100f, false, SkillshotType.SkillshotLine);


            Menu = new Menu("HikiCarry Graves", "HikiCarry Graves", true);
            Bootstrap.Init(null);

            var ComboMenu = Menu.Add(new Menu("Combo", "Combo Settings"));
            ComboMenu.Add(new MenuBool("qCombo", "Use Q", true));
            ComboMenu.Add(new MenuSlider("qComboRange", "Use Q Combo Range", value: 500, minValue: 0, maxValue: 720));
            ComboMenu.Add(new MenuBool("wCombo", "Use W", true));
            ComboMenu.Add(new MenuBool("eCombo", "Use E", true));
            ComboMenu.Add(new MenuBool("rCombo", "Use R", true));

            var HybridMenu = Menu.Add(new Menu("Harass", "Hybrid Settings"));
            HybridMenu.Add(new MenuBool("qHarass", "Use Q", true));


            //var MiscMenu = Menu.Add(new Menu("Misc Settings", "Misc Settings"));
           // MiscMenu.Add(new MenuBool("bT", "Auto Scrying Orb Buy!", true));
           // MiscMenu.Add(new MenuSlider("bluetrinketlevel", "Scrying Orb Buy Level", value: 6, minValue: 1, maxValue: 18));
            //MiscMenu.Add(new MenuBool("killstealQ", "KillSteal Q!", true));
            //MiscMenu.Add(new MenuBool("killstealR", "KillSteal R!", true));

            var DrawingMenu = Menu.Add(new Menu("Drawing", "Drawing Settings"));
            DrawingMenu.Add(new MenuBool("qDraw", "Use Q", true));
            DrawingMenu.Add(new MenuBool("wDraw", "Use W", true));
            DrawingMenu.Add(new MenuBool("eDraw", "Use E", true));
            DrawingMenu.Add(new MenuBool("rDraw", "Use R", true));

            Menu.Attach();
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnAction += Orbwalker_OnAction;
            Drawing.OnDraw += OnDraw;
        }

        private static void Orbwalker_OnAction(object sender, LeagueSharp.SDK.Core.Orbwalker.OrbwalkerActionArgs e)
        {
           // COMING SOON
        }

        

        private static void OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode == OrbwalkerMode.Orbwalk)
            {
                Combo();
            }
            if (Orbwalker.ActiveMode == OrbwalkerMode.LaneClear)
            {
                Clear();
            }
            if (Orbwalker.ActiveMode == OrbwalkerMode.Hybrid)
            {
                Harass();
            }
        }
        private static void Combo()
        {
            var qUse = Menu["Combo"]["qCombo"].GetValue<MenuBool>().Value;
            var qRange = Menu["Combo"]["qComboRange"].GetValue<MenuSlider>().Value;
            var eUse = Menu["Combo"]["eCombo"].GetValue<MenuBool>().Value;
            var wUse = Menu["Combo"]["wCombo"].GetValue<MenuBool>().Value;
            var rUse = Menu["Combo"]["rCombo"].GetValue<MenuBool>().Value;
            var targetQ = TargetSelector.GetTarget(Q.Range);
            var targetW = TargetSelector.GetTarget(W.Range);
            var targetE = TargetSelector.GetTarget(E.Range);
            var targetR = TargetSelector.GetTarget(R.Range);
            



            if (qUse && Q.IsReady())
            {

                foreach (
                       var enemyQ in
                          GameObjects.Enemy.Where(
                               hero =>
                                   hero.IsValidTarget(qRange)))
                {

                    if (Q.CastIfHitchanceEquals(enemyQ, HitChance.High) == CastStates.SuccessfullyCasted)
                    {
                        Q.Cast(enemyQ);
                    }


                }
            }

            if (wUse && W.IsReady())
            {

                foreach (
                       var enemyW in
                          GameObjects.Enemy.Where(
                               hero =>
                                   hero.IsValidTarget(W.Range)))
                {

                    if (W.CastIfHitchanceEquals(enemyW, HitChance.High) == CastStates.SuccessfullyCasted)
                    {
                        W.Cast(enemyW);
                    }


                }
            }

            if (eUse && E.IsReady())
            {

                foreach (
                       var enemyE in
                          GameObjects.Enemy.Where(
                               hero =>
                                   hero.IsValidTarget(Q.Range)))
                {

                    if (W.CastIfHitchanceEquals(enemyE, HitChance.High) == CastStates.SuccessfullyCasted)
                    {
                        E.Cast(Game.CursorPos);
                    }


                }
            }

           
            if (rUse && R.IsReady())
            {

                foreach (
                       var enemyR in
                         ObjectManager.Get<Obj_AI_Hero>().Where(
                               hero =>
                                   hero.IsValidTarget(R.Range) 
                                   ))
                {

                    if (R.Level ==1)
                    {
                        double damageR1 = 250 + 1.5 * enemyR.FlatPhysicalDamageMod;
                        foreach (var user1 in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(R.Range) && hero.Health < damageR1 ))
                        {
                            R.Cast(user1);
                        }
                        
                    }
                    if (R.Level == 2)
                    {
                        double damageR1 = 400 + 1.5 * enemyR.FlatPhysicalDamageMod;
                        foreach (var user1 in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(R.Range) && hero.Health < damageR1))
                        {
                            R.Cast(user1);
                        }

                    }
                    if (R.Level == 3)
                    {
                        double damageR1 = 550 + 1.5 * enemyR.FlatPhysicalDamageMod;
                        foreach (var user1 in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(R.Range) && hero.Health < damageR1))
                        {
                            R.Cast(user1);
                        }

                    }

                }
            }

        }
        private static void Clear()
        {

        }

        private static void Harass()
        {
            var qHarass = Menu["Combo"]["qHarass"].GetValue<MenuBool>().Value;

            if (Q.IsReady() && qHarass)
            {
                foreach (
                       var enemyQ in
                          GameObjects.Enemy.Where(
                               hero =>
                                   hero.IsValidTarget(Q.Range)))
                {

                    if (Q.CastIfHitchanceEquals(enemyQ, HitChance.High) == CastStates.SuccessfullyCasted)
                    {
                        Q.Cast(enemyQ);
                    }
                }
            }
        }
        private static void OnDraw(EventArgs args)
        {
            var qDraw = Menu["Drawing"]["qDraw"].GetValue<MenuBool>().Value;
            var wDraw = Menu["Drawing"]["wDraw"].GetValue<MenuBool>().Value;
            var eDraw = Menu["Drawing"]["eDraw"].GetValue<MenuBool>().Value;
            var rDraw = Menu["Drawing"]["rDraw"].GetValue<MenuBool>().Value;


            if (qDraw)
            {
                Drawing.DrawCircle(Player.Position, Q.Range, Color.Green);
            }

            if (wDraw)
            {
                Drawing.DrawCircle(Player.Position, W.Range, Color.Brown);
            }

            if (eDraw)
            {
                Drawing.DrawCircle(Player.Position, E.Range, Color.Yellow);
            }

            if (rDraw)
            {
                Drawing.DrawCircle(Player.Position, R.Range, Color.Azure);
            }
        }
    }
}
