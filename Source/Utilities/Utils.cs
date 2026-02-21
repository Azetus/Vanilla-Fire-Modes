using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Settings;

namespace VFM_VanillaFireModes.Utilities
{
    internal static class Utils
    {
        private static readonly float MAX_MULTIPLIER = 100.0f;
        private static readonly float MAX_EXTRASHOT = 100.0f;
        private static readonly float MIN_EXTRASHOT = 0.0f;

        public static int GetBurstShotCountByMultiplier(int burstShotCount, float multiplier)
        {
            if (burstShotCount <= 1) return burstShotCount;
            float mult = Math.Min(multiplier, MAX_MULTIPLIER);
            return Mathf.Max(1, (int)Math.Round(burstShotCount * mult));
        }

        public static int GetBurstShotCountByBonus(int burstShotCount, float bonus)
        {
            if (burstShotCount <= 1) return burstShotCount;
            float extraShot = Mathf.Min(bonus, MAX_EXTRASHOT);
            return Mathf.Max(1, burstShotCount + (int)Math.Round(bonus));
        }


        // x=武器默认连射次数，k=玩家设置的额外子弹数量加成，p=玩家设置的加成峰值
        // f(x, p) = ( (x - 1) / (p - 1) ) * e ^ ( 1 - (x - 1) / (p - 1))
        // 最终结果 burstShotCount + Max(1, f(x,p))
        public static int GetBurstShotCountByMod(int burstShotCount, float extra, float peakOffSet)
        {

            if (burstShotCount <= 1) return burstShotCount;
            float peak = Mathf.Max(peakOffSet, 2.0f); // peak 必须大于等于2
            float extraShot = Mathf.Min(MAX_EXTRASHOT, Mathf.Max(extra, MIN_EXTRASHOT));

            // (x - 1) / (p - 1)
            float u = (burstShotCount - 1f) / (peak - 1f);

            // f(x, p) = u * e^(1 - u)
            float fB = u * Mathf.Exp(1.0f - u);

            // 至少多 1 发
            int bonus = Mathf.Max(1, (int)Mathf.Round(extraShot * fB));

            return Mathf.Max(1, burstShotCount + bonus);

        }

        // x=武器默认连射次数，M=玩家设置最大倍率，k=玩家设置的增长/衰减率，p=玩家设置的加成峰值
        // f(x,M, k,p) = Max(0.5, M - k * |k-p|)
        public static int GetBurstShotCountByTentFunction(int burstShotCount, float maxMultiplier, float slopeK, float peakOffSet)
        {
            if (burstShotCount <= 1) return burstShotCount;

            float k = Mathf.Max(slopeK, 0);
            float p = Mathf.Max(peakOffSet, 2); // peak 必须大于等于2
            float MaxMult = Mathf.Min(maxMultiplier, MAX_MULTIPLIER);

            float distance = Mathf.Abs(burstShotCount - p);
            float multiplier = MaxMult - (k * distance);

            // 0.5f 保底
            multiplier = Mathf.Max(0.5f, multiplier);

            return Mathf.Max(1, (int)Math.Round(burstShotCount * multiplier));
        }

        public static string GetFireModeLabelFor(FireMode mode)
        {
            switch (mode)
            {
                case FireMode.Precision: return "VFM_PrecisionMode".Translate();
                case FireMode.Burst: return "VFM_ShortBurstMode".Translate();
                case FireMode.Suppression: return "VFM_SuppressionMode".Translate();
                default: return "VFM_DefaultMode".Translate();
            }
        }

        public static string ToPercentString(float value)
        {
            return $"{value * 100f}%";
        }

        public static FireMode EvaluateByDistance(
            float distance,
            VanillaFireModesModSetting settings)
        {
            if (distance >= settings.precisionMinDistance)
                return FireMode.Precision;

            if (distance >= settings.burstMinDistance)
                return FireMode.Burst;

            return FireMode.Suppression;
        }
    }
}
