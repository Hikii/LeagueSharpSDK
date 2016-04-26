using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hJhin.Extensions;
using LeagueSharp;
using LeagueSharp.SDK;

namespace hJhin.Modes
{
    static class Ultimate
    {
        public static void Execute()
        {
            if (Config.Menu["ultimate.settings"]["combo.r"])
            {
                if (ObjectManager.Player.IsActive(Spells.R))
                {
                    if (Config.Menu["ultimate.settings"]["auto.shoot.bullets"])
                    {

                        var enemy = GameObjects.EnemyHeroes.Where(x => Config.Menu["ultimate.settings"]["r.combo." + x.ChampionName] && x.IsValidTarget(Spells.R.Range)).MinOrDefault(x => x.Health);
                        if (enemy != null)
                        {
                            var pred = Spells.R.GetPrediction(enemy);
                            if (pred.Hitchance >= Provider.HikiChance())
                            {
                                Spells.R.Cast(pred.CastPosition);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (Spells.R.IsReady() && Config.SemiManualUlt.Active)
                    {
                        var enemy = GameObjects.EnemyHeroes.Where(x => Config.Menu["ultimate.settings"]["r.combo." + x.ChampionName] && x.IsValidTarget(Spells.R.Range)).MinOrDefault(x => x.Health);
                        if (enemy != null)
                        {
                            var pred = Spells.R.GetPrediction(enemy);
                            if (pred.Hitchance >= Provider.HikiChance())
                            {
                                Spells.R.Cast(pred.CastPosition);
                                return;
                            }
                        }
                    }
                }
            }
            
        }
    }
}
