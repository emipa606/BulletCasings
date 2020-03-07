using HarmonyLib;
using UnityEngine;
using Verse;
using System.Reflection;

namespace BulletCasingMote
{
    public class BulletCasingMote : Mod
    {
        public static BulletCasingMoteSettings settings;
        public BulletCasingMote(ModContentPack content) : base(content)
        {
            settings = GetSettings<BulletCasingMoteSettings>();
            var harmony = new Harmony("Syrchalis.Rimworld.BulletCasingMote");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public override string SettingsCategory() => "BulletCasingMoteSettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            checked
            {
                Listing_Syrchalis listing_Standard = new Listing_Syrchalis();
                listing_Standard.Begin(inRect);
                listing_Standard.CheckboxLabeled("BulletCasingfilth".Translate(), ref BulletCasingMoteSettings.filth, ("BulletCasingfilthTooltip".Translate()));
                listing_Standard.CheckboxLabeled("BulletCasingUseWeaponRotation".Translate(), ref BulletCasingMoteSettings.useWeaponRotation, ("BulletCasingUseWeaponRotationTooltip".Translate()));
                listing_Standard.LabelHighlight("BulletCasingVelocityFactor".Translate(), tooltip: "BulletCasingVelocityFactorTooltip".Translate());
                listing_Standard.IntRange(ref BulletCasingMoteSettings.velocityFactor, 1, 10);
                if(BulletCasingMoteSettings.velocityFactor.min < 1)
                {
                    BulletCasingMoteSettings.velocityFactor.min = 1;
                }
                listing_Standard.Gap(12f);
                listing_Standard.CheckboxLabeled("BulletCasingUncapCasingSize".Translate(), ref BulletCasingMoteSettings.uncapCasingSize, ("BulletCasingUncapCasingSizeTooltip".Translate()));
                /*listing_Standard.Label("X = " + BulletCasingMoteSettings.var1);
                BulletCasingMoteSettings.var1 = GenMath.RoundTo(listing_Standard.Slider(BulletCasingMoteSettings.var1, 0f, 10f), 0.1f);
                listing_Standard.Label("Y = " + BulletCasingMoteSettings.var2);
                BulletCasingMoteSettings.var2 = GenMath.RoundTo(listing_Standard.Slider(BulletCasingMoteSettings.var2, 0f, 10f), 0.1f);
                listing_Standard.Label("AgeSecsFactor = " + BulletCasingMoteSettings.var3);
                BulletCasingMoteSettings.var3 = GenMath.RoundTo(listing_Standard.Slider(BulletCasingMoteSettings.var3, 0f, 10f), 0.1f);*/
                listing_Standard.Gap(12f);
                if (listing_Standard.ButtonText("BulletCasingDefaultSettings".Translate(), "BulletCasingDefaultSettingsTooltip".Translate()))
                {
                    BulletCasingMoteSettings.useWeaponRotation = true;
                    BulletCasingMoteSettings.velocityFactor.min = 2;
                    BulletCasingMoteSettings.velocityFactor.max = 3;
                    BulletCasingMoteSettings.uncapCasingSize = false;
                }
                listing_Standard.End();
                settings.Write();
            }
        }
        public override void WriteSettings()
        {
            base.WriteSettings();
        }
    }
}
