﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Wrappers.Damages;

namespace hJhin.Extensions
{
    static class Provider
    {
        /// <summary>
        /// Thats gives if enemy stunnable with W or not
        /// </summary>
        /// <param name="unit">Target</param>
        /// <returns></returns>
        public static bool IsStunnable(this Obj_AI_Hero unit)
        {
            return unit.HasBuff("jhinespotteddebuff");
        }
        /// <summary>
        /// Thats check if spell name equals to value
        /// </summary>
        /// <param name="unit">Player</param>
        /// <param name="spell">Spell</param>
        /// <returns></returns>
        public static bool IsActive(this Obj_AI_Hero unit, Spell spell)
        {
            return spell.Instance.Name == "JhinRShot";
        }

        /// <summary>
        /// Thats gives me if enemy immobile or not for e stun root
        /// </summary>
        /// <param name="unit">Target</param>
        /// <returns></returns>
        public static bool IsEnemyImmobile(this Obj_AI_Hero unit)
        {
            return unit.HasBuffOfType(BuffType.Stun) || unit.HasBuffOfType(BuffType.Snare) ||
                   unit.HasBuffOfType(BuffType.Knockup) ||
                   unit.HasBuffOfType(BuffType.Charm) || unit.HasBuffOfType(BuffType.Fear) ||
                   unit.HasBuffOfType(BuffType.Knockback) ||
                   unit.HasBuffOfType(BuffType.Taunt) || unit.HasBuffOfType(BuffType.Suppression) ||
                   unit.IsStunned;
        }

        /// <summary>
        /// Thats gives me if unit has teleport buff
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <returns></returns>
        public static bool HasTeleportBuff(this Obj_AI_Base unit)
        {
            return unit.HasBuff("teleport_target") && unit.Team != ObjectManager.Player.Team;
        }

        /// <summary>
        /// Gives me Jhin is reloading or not 
        /// </summary>
        /// <param name="unit">Target</param>
        /// <returns></returns>
        public static bool IsReloading(this Obj_AI_Hero unit)
        {
            return unit.HasBuff("JhinPassiveReload");
        }


        /// <summary>
        /// Basic Attack Indicator
        /// </summary>
        /// <param name="enemy">Target</param>
        /// <returns></returns>
        public static int BasicAttackIndicator(Obj_AI_Hero enemy)
        {
            var aCalculator = ObjectManager.Player.CalculateDamage(enemy, DamageType.Physical, ObjectManager.Player.TotalAttackDamage);
            var killableAaCount = enemy.Health / aCalculator;
            var totalAa = (int)Math.Ceiling(killableAaCount);
            return totalAa;
        }

        public static HitChance HikiChance()
        {
            if (Config.HitChance.SelectedValue == "Medium")
            {
                return HitChance.Medium;
            }

            if (Config.HitChance.SelectedValue == "High")
            {
                return HitChance.High;
            }

            if (Config.HitChance.SelectedValue == "Very High")
            {
                return HitChance.VeryHigh;
            }
            return HitChance.Low;
        }
    }
}
