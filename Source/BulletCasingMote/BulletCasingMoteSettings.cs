using Verse;

namespace BulletCasingMote
{
    public class BulletCasingMoteSettings : ModSettings
    {
        public static bool filth;
        public static bool useWeaponRotation;
        public static IntRange velocityFactor = new IntRange (2, 3);
        public static bool uncapCasingSize;
        public static float var1;
        public static float var2;
        public static float var3;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref filth, "BulletCasingMote_filth", true, true);
            Scribe_Values.Look<bool>(ref useWeaponRotation, "BulletCasingMote_useWeaponRotation", true, true);
            Scribe_Values.Look<IntRange>(ref velocityFactor, "BulletCasingMote_velocityFactor", new IntRange(2, 3), true);
            Scribe_Values.Look<bool>(ref uncapCasingSize, "BulletCasingMote_uncapCasingSize", false, true);
            Scribe_Values.Look<float>(ref var1, "BulletCasingMote_var1", 3f, true);
            Scribe_Values.Look<float>(ref var2, "BulletCasingMote_var2", 0f, true);
            Scribe_Values.Look<float>(ref var3, "BulletCasingMote_var3", 10f, true);
        }
    }
}
