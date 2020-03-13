using HarmonyLib;
using System;
using UnityEngine;
using RimWorld;
using Verse;

namespace BulletCasingMote
{
    [HarmonyPatch(typeof(Verb), "TryCastNextBurstShot")]
    public static class TryCastNextBurstShotPatch
    {
        [HarmonyPostfix]
        public static void TryCastNextBurstShot_Postfix(Verb __instance)
        {
            if (__instance.verbProps.muzzleFlashScale > 0.01f && __instance.verbProps.verbClass != typeof(Verb_ShootOneUse))
            {
                if (__instance.CasterIsPawn)
                {
                    ThingDef filth = BulletCasingMoteDefOf.Filth_BulletCasingsCharge;
                    if (__instance.CasterPawn.equipment.Primary.def.defName.Contains("Charge", StringComparison.OrdinalIgnoreCase))
                    {
                        ThrowCasing(__instance.CasterPawn, __instance.caster.Map, __instance.GetProjectile().projectile.GetDamageAmount(1f), BulletCasingMoteDefOf.Mote_BulletCasing_Charge);
                    }
                    else if (__instance.CasterPawn.equipment.Primary.def.defName.Contains("Shotgun", StringComparison.OrdinalIgnoreCase))
                    {
                        ThrowCasing(__instance.CasterPawn, __instance.caster.Map, __instance.GetProjectile().projectile.GetDamageAmount(1f), BulletCasingMoteDefOf.Mote_BulletCasing_Shotgun);
                        filth = BulletCasingMoteDefOf.Filth_BulletCasingsShotgun;
                    }
                    else
                    {
                        ThrowCasing(__instance.CasterPawn, __instance.caster.Map, __instance.GetProjectile().projectile.GetDamageAmount(1f), BulletCasingMoteDefOf.Mote_BulletCasing);
                        filth = BulletCasingMoteDefOf.Filth_BulletCasingsRifle;
                    }
                    if (Rand.Value > 0.9f && BulletCasingMoteSettings.filth)
                    {
                        IntVec3 randomCell = new IntVec3(Rand.Range(__instance.caster.Position.x - 1, __instance.caster.Position.x + 1), 0, Rand.Range(__instance.caster.Position.z - 1, __instance.caster.Position.z + 1));
                        FilthMaker.TryMakeFilth(randomCell, __instance.caster.Map, filth);
                    }
                }
                else if (__instance.caster.def != ThingDefOf.Turret_Mortar)
                {
                    ThrowCasingTurret(__instance.caster, __instance.caster.Map, __instance.GetProjectile().projectile.GetDamageAmount(1f), BulletCasingMoteDefOf.Mote_BulletCasing);
                }
            } 
        }

        public static Mote ThrowCasing(Pawn caster, Map map, int weaponDamage, ThingDef moteDef)
        {
            if (!caster.Position.ShouldSpawnMotesAt(map) || map.moteCounter.Saturated)
            {
                return null;
            }
            float angle = (caster.TargetCurrentlyAimingAt.CenterVector3 - caster.DrawPos).AngleFlat();
            MoteThrownCasing moteThrown = (MoteThrownCasing)ThingMaker.MakeThing(moteDef, null);
            if (BulletCasingMoteSettings.uncapCasingSize)
            {
                moteThrown.Scale = GenMath.LerpDouble(5f, 30f, 0.2f, 0.4f, weaponDamage);
            }
            else
            {
                moteThrown.Scale = GenMath.LerpDoubleClamped(5f, 30f, 0.2f, 0.4f, weaponDamage);
            } 
            moteThrown.exactPosition = caster.Position.ToVector3Shifted();
            moteThrown.exactPosition += Quaternion.AngleAxis(angle, Vector3.up) * new Vector3(0f, 0f, 0.3f); //puts the casing slightly infront of the pawn
            moteThrown.rotationRate = Rand.Range(-360f, 360f);
            moteThrown.speed = Rand.Range(BulletCasingMoteSettings.velocityFactor.min, BulletCasingMoteSettings.velocityFactor.max);
            moteThrown.rotation = angle;
            moteThrown.velocityRandom = new Vector3(Rand.Range(-3f, 3f), 0f, Rand.Range(-3f, 3f));
            GenSpawn.Spawn(moteThrown, caster.Position, map, WipeMode.Vanish);
            return moteThrown;
        }

        public static Mote ThrowCasingTurret(Thing caster, Map map, int weaponDamage, ThingDef moteDef)
        {
            if (!caster.Position.ShouldSpawnMotesAt(map) || map.moteCounter.Saturated)
            {
                return null;
            }
            Building_TurretGun turret = caster as Building_TurretGun;
            TurretTop turrettop = Traverse.Create(turret).Field("top").GetValue<TurretTop>();
            
            float angle = Traverse.Create(turrettop).Field("curRotationInt").GetValue<float>();
            MoteThrownCasing moteThrown = (MoteThrownCasing)ThingMaker.MakeThing(moteDef, null);
            if (BulletCasingMoteSettings.uncapCasingSize)
            {
                moteThrown.Scale = GenMath.LerpDouble(5f, 50f, 0.2f, 0.5f, weaponDamage);
            }
            else
            {
                moteThrown.Scale = GenMath.LerpDoubleClamped(5f, 75f, 0.2f, 0.5f, weaponDamage);
            }
            moteThrown.exactPosition = caster.TrueCenter();
            moteThrown.rotationRate = Rand.Range(-360f, 360f);
            moteThrown.speed = Rand.Range(BulletCasingMoteSettings.velocityFactor.min, BulletCasingMoteSettings.velocityFactor.max);
            moteThrown.rotation = angle;
            moteThrown.velocityRandom = new Vector3(Rand.Range(-4f, 4f), 0f, Rand.Range(-4f, 4f));
            GenSpawn.Spawn(moteThrown, caster.Position, map, WipeMode.Vanish);
            return moteThrown;
        }
    }
    public class MoteThrownCasing : MoteThrown
    {
        public float speed;
        public float rotation;
        public Vector3 velocityRandom;
        protected override Vector3 NextExactPosition(float deltaTime)
        {
            Vector3 velocity = (Quaternion.AngleAxis((-Mathf.Rad2Deg * Mathf.Atan(1f - AgeSecs * BulletCasingMoteSettings.var3)) % 360f, Vector3.up) * new Vector3(BulletCasingMoteSettings.var1, 0f, BulletCasingMoteSettings.var2) * speed);
            if (BulletCasingMoteSettings.useWeaponRotation)
            {
                velocity = velocity.RotatedBy(rotation);
            }
            if (AgeSecs > (0.4f / (1f + BulletCasingMoteSettings.velocityFactor.Average / 10f)))
            {
                velocity = velocityRandom / Mathf.Pow((1 + AgeSecs), 2);
                rotationRate = Mathf.Clamp(rotationRate, -180f, 180);
            }
            return exactPosition + velocity * deltaTime;
        }
    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}