using UnityEngine;

namespace VFM_VanillaFireModes.Utilities
{
    internal static class Utils
    {
        public static int GetBurstShotCountByMultiplier(int burstShotCount, float multiplier)
        {
            if (burstShotCount <= 1) return burstShotCount;
            return Mathf.Max(1, Mathf.RoundToInt(burstShotCount * multiplier + 0.5f));
        }

        public static int GetBurstShotCountByBonus(int burstShotCount, float bonus)
        {
            if (burstShotCount <= 1) return burstShotCount;
            return Mathf.Max(1, burstShotCount + Mathf.RoundToInt(bonus + 0.5f));
        }


        // x=武器默认连射次数，k=玩家设置的额外子弹数量加成，p=玩家设置的加成峰值
        // f(x, p) = ( (x - 1) / (p - 1) ) * e ^ ( 1 - (x - 1) / (p - 1))
        // 最终结果 burstShotCount + Max(1, f(x,p))
        public static int GetBurstShotCountByMod(int burstShotCount, float extraShot, float peakOffSet)
        {

            if (burstShotCount <= 1) return burstShotCount;
            if (peakOffSet <= 2.0f) peakOffSet = 2f;

            // (x - 1) / (p - 1)
            float u = (burstShotCount - 1f) / (peakOffSet - 1f);

            // f(x, p) = u * e^(1 - u)
            float fB = u * Mathf.Exp(1.0f - u);

            // 是少多 1 发
            int bonus = Mathf.Max(1, Mathf.RoundToInt(extraShot * fB));

            return Mathf.Max(1, burstShotCount + bonus);

        }

        // x=武器默认连射次数，M=玩家设置最大倍率，k=玩家设置的增长/衰减率，p=玩家设置的加成峰值
        // f(x,M, k,p) = Max(0.5, M - k * |k-p|)
        public static int GetBurstShotCountByTentFunction(int burstShotCount,float maxMultiplier, float slopeK, float peakOffSet)
        {
            if (burstShotCount <= 1) return burstShotCount;

            float k = Mathf.Max(slopeK, 0);
            float p = Mathf.Max(peakOffSet, 1);

            float distance = Mathf.Abs(burstShotCount - p);
            float multiplier = maxMultiplier - (k * distance);

            // 0.5f 保底
            multiplier = Mathf.Max(0.5f, multiplier);

            return Mathf.Max(1, Mathf.RoundToInt(burstShotCount * multiplier));
        }

    }
}
