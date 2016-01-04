using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.SDK.Core;
using SharpDX.Direct3D9;

namespace HTrackerSDK
{
    class SpellTracker
    {
        private static readonly SpellSlot[] Spells =
        {
            SpellSlot.Q,
            SpellSlot.W,
            SpellSlot.E,
            SpellSlot.R
        };
        private static readonly SpellSlot[] Summoners =
        {
            SpellSlot.Summoner1,
            SpellSlot.Summoner2,
        };

        public static string[] SummonersNames =
        {
            "summonerbarrier", "summonerboost", "summonerdot", "summonerexhaust",
            "summonerflash", "summonerhaste", "summonerheal", "summonerodinGarrison",
            "summonerteleport", 
        };

        public static Font HikiFont = new Font(Drawing.Direct3DDevice, new FontDescription
        {
            FaceName = "Tahoma",
            Height = 15,
            Weight = FontWeight.Bold,
            OutputPrecision = FontPrecision.Default,
            Quality = FontQuality.ClearTypeNatural,
        });
        private static int _x, _y;

        private static int GetCooldownExpires(Obj_AI_Base hero, SpellSlot slot)
        {
            return (int)hero.Spellbook.GetSpell(slot).CooldownExpires;
        }

        public static string GetSummonerName(Obj_AI_Base hero, SpellSlot slot)
        {
            return hero.Spellbook.GetSpell(slot).SData.Name;
        }

        public static int GetSummonerExpires(Obj_AI_Base hero, SpellSlot slot)
        {
            return (int)hero.Spellbook.GetSpell(slot).CooldownExpires;
        }

        public static int GetSpellLevel(Obj_AI_Base hero, SpellSlot slot)
        {
            return hero.Spellbook.GetSpell(slot).Level;
        }

        public static string GetSName(Obj_AI_Base hero, SpellSlot slot)
        {
            switch (hero.Spellbook.GetSpell(slot).SData.Name)
            {
                case "summonerflash":
                    return "F:";
                case "summonerheal":
                    return "H:";
                case "summonerbarrier":
                    return "B: ";
                case "summonerexhaust":
                    return "E: ";
                case "summonerdot":
                    return "I: ";
                case "summonerteleport: ":
                    return "TP: ";
                case "s5_summonersmiteduel":
                    return "S: ";
                case "s5_summonersmiteplayerganker":
                    return "S: ";
                case "s5_summonersmitequick":
                    return "S: ";
                case "itemsmiteaoe":
                    return "S: ";
            }
            return "U: ";
        }

        public static void PlayerTracker()
        {
            foreach (var ally in GameObjects.AllyHeroes.Where(x => x.IsVisible && x.IsValid && !x.IsDead && x.IsMe))
            {
                for (var i = 0; i < Spells.Length; i++)
                {
                    _x = (int)ally.HPBarPosition.X + 30 * i;
                    _y = (int)ally.HPBarPosition.Y + 50;
                    HikiFont.DrawText(null, Spells[i].ToString(), _x + 36, _y - 25, SharpDX.Color.Gold);

                    var cooldown = Convert.ToInt32(GetCooldownExpires(ally, Spells[i]) - Game.Time - 1);
                    if (cooldown > 0)
                    {
                        HikiFont.DrawText(null, "" + cooldown, _x + 36, _y - 10, SharpDX.Color.Gold);
                    }
                    else
                    {
                        HikiFont.DrawText(null, "0", _x + 36, _y - 10, SharpDX.Color.White);
                    }
                }
                for (var i = 0; i < Summoners.Length; i++)
                {
                    _x = (int)ally.HPBarPosition.X + 80 * i;
                    _y = (int)ally.HPBarPosition.Y + 50;
                    var cooldown = Convert.ToInt32(GetCooldownExpires(ally, Summoners[i]) - Game.Time - 1);
                    HikiFont.DrawText(null, "" + GetSName(ally, Summoners[i]), _x + 23, _y - 60, SharpDX.Color.White);
                    if (cooldown > 0)
                    {
                        HikiFont.DrawText(null, "" + cooldown, _x + 40, _y + -60, SharpDX.Color.White);
                    }
                    else
                    {
                        HikiFont.DrawText(null, "0", _x + 40, _y + -60, SharpDX.Color.White);
                    }
                }
            }
        }

        public static void EnemyTracker()
        {
            foreach (var ally in GameObjects.EnemyHeroes.Where(x => x.IsVisible && x.IsValid && !x.IsDead))
            {
                for (var i = 0; i < Spells.Length; i++)
                {
                    _x = (int)ally.HPBarPosition.X + 30 * i;
                    _y = (int)ally.HPBarPosition.Y + 50;
                    HikiFont.DrawText(null, "" + Spells[i], _x + 10, _y - 15, SharpDX.Color.Gold);

                    var cooldown = Convert.ToInt32(GetCooldownExpires(ally, Spells[i]) - Game.Time - 1);
                    if (cooldown > 0)
                    {
                        HikiFont.DrawText(null, "" + cooldown, _x + 10, _y, SharpDX.Color.Gold);
                    }
                    else
                    {
                        HikiFont.DrawText(null, "0", _x + 10, _y, SharpDX.Color.White);
                    }
                }
                for (var i = 0; i < Summoners.Length; i++)
                {
                    _x = (int)ally.HPBarPosition.X + 80 * i;
                    _y = (int)ally.HPBarPosition.Y + 50;
                    var cooldown = Convert.ToInt32(GetCooldownExpires(ally, Summoners[i]) - Game.Time - 1);
                    HikiFont.DrawText(null, "" + GetSName(ally, Summoners[i]), _x + 23, _y - 50, SharpDX.Color.Gold);
                    if (cooldown > 0)
                    {
                        HikiFont.DrawText(null, "" + cooldown, _x + 40, _y + -50, SharpDX.Color.White);
                    }
                    else
                    {
                        HikiFont.DrawText(null, "0", _x + 40, _y + -50, SharpDX.Color.White);
                    }
                }
            }
        }

        public static void AllyTracker()
        {
            foreach (var ally in GameObjects.AllyHeroes.Where(x => x.IsVisible && x.IsValid && !x.IsDead && !x.IsMe))
            {
                for (var i = 0; i < Spells.Length; i++)
                {
                    _x = (int)ally.HPBarPosition.X + 30 * i;
                    _y = (int)ally.HPBarPosition.Y + 50;
                    HikiFont.DrawText(null, "" + Spells[i], _x + 10, _y - 15, SharpDX.Color.Gold);

                    var cooldown = Convert.ToInt32(GetCooldownExpires(ally, Spells[i]) - Game.Time - 1);
                    if (cooldown > 0)
                    {
                        HikiFont.DrawText(null, "" + cooldown, _x + 10, _y, SharpDX.Color.Gold);
                    }
                    else
                    {
                        HikiFont.DrawText(null, "0", _x + 10, _y, SharpDX.Color.White);
                    }
                }
                for (var i = 0; i < Summoners.Length; i++)
                {
                    _x = (int)ally.HPBarPosition.X + 80 * i;
                    _y = (int)ally.HPBarPosition.Y + 50;
                    var cooldown = Convert.ToInt32(GetCooldownExpires(ally, Summoners[i]) - Game.Time - 1);
                    HikiFont.DrawText(null, "" + GetSName(ally, Summoners[i]), _x + 23, _y - 50, SharpDX.Color.Gold);
                    if (cooldown > 0)
                    {
                        HikiFont.DrawText(null, "" + cooldown, _x + 30, _y + -50, SharpDX.Color.White);
                    }
                    else
                    {
                        HikiFont.DrawText(null, "0", _x + 40, _y + -50, SharpDX.Color.White);
                    }
                }
            }
        }
    }
}
