using System;
using System.Collections.Generic;

namespace BlissBuddy
{
    public static class Experience
    {
        private static readonly Dictionary<int, int> experienceChart;

        static Experience()
        {
            experienceChart = new Dictionary<int, int>();

            double current = 0;
            for (int i = 1; i < 126; i++)
            {
                current += Math.Floor(i + 300 * Math.Pow(2, i / 7d));
                experienceChart[i + 1] = (int)Math.Floor(current / 4);
            }
        }

        public static int FromLevel(int level)
        {
            if (level <= 1) return 0;
            if (level >= 127) return 200000000;
            return experienceChart[level];
        }

        public static int ToLevel(int experience)
        {
            int level = 1;
            foreach (var pair in experienceChart)
            {
                if (pair.Value > experience) break;
                level = pair.Key;
            }
            return level;
        }
    }
}
