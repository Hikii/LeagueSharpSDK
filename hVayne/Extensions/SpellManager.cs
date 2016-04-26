﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hVayne.Champions;
using LeagueSharp;
using LeagueSharp.SDK;
using SharpDX;

namespace hVayne.Extensions
{
    class SpellManager
    {

        public static void SafePosition(Obj_AI_Base enemy)
        {
            var range = enemy.AttackRange;
            var path = CircleCircleIntersection(ObjectManager.Player.ServerPosition.ToVector2(),
                enemy.Position.ToVector2(), Spells.Q.Range, range);
            
            
            if (path.Count() > 0)
            {
                var epos = path.MinOrDefault(x => x.Distance(Game.CursorPos));
                if (epos.ToVector3().IsUnderEnemyTurret() || epos.ToVector3().IsWall())
                {
                    return;
                }

                if (epos.ToVector3().CountEnemyHeroesInRange(Spells.Q.Range - 100) > 0)
                {
                    return;
                }
                Spells.Q.Cast(epos);
            }
            if (path.Count() == 0)
            {
                var epos = ObjectManager.Player.ServerPosition.Extend(enemy.ServerPosition, -Spells.Q.Range);
                if (epos.IsUnderEnemyTurret() || epos.IsWall())
                {
                    return;
                }

                // no intersection or target to close
                Spells.Q.Cast(ObjectManager.Player.ServerPosition.Extend(enemy.ServerPosition, -Spells.Q.Range));
            }
        }
        public static void ExecuteQ(Obj_AI_Hero enemy)
        {
            //q.style

            switch (Config.MethodQ.SelectedValue)
            {
                case "Safe Position":
                    SafePosition(enemy);
                    break;
                case "Cursor Position":
                    Spells.Q.Cast(Game.CursorPos);
                    break;
            }
        }

        public static void ExecuteE(Obj_AI_Hero enemy)
        {
            switch (Config.CondemnMethod.SelectedValue)
            {
                case "Shine":
                    Condemn.ShineCondemn(enemy, Config.PushDistance.Value);
                    break;
                case "Asuna":
                    Condemn.AsunaCondemn(enemy, Config.PushDistance.Value);
                    break;
                case "360":
                    if (Condemn.Condemn360(enemy, Config.PushDistance.Value))
                        Spells.E.Cast(enemy);
                    break;
            }
        }

        private static Vector2[] CircleCircleIntersection(Vector2 center1, Vector2 center2, float radius1, float radius2)
        {
            var d = center1.Distance(center2);
            //The Circles dont intersect:
            if (d > radius1 + radius2 || (d <= Math.Abs(radius1 - radius2)))
            {
                return new Vector2[] { };
            }

            var a = (radius1 * radius1 - radius2 * radius2 + d * d) / (2 * d);
            var h = (float)Math.Sqrt(radius1 * radius1 - a * a);
            var direction = (center2 - center1).Normalized();
            var pa = center1 + a * direction;
            var s1 = pa + h * direction.Perpendicular();
            var s2 = pa - h * direction.Perpendicular();
            return new[] { s1, s2 };
        }
    }
}
