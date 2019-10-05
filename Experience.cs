using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlissBuddy
{
    public static class Experience
    {
        private static readonly Dictionary<int, int> experienceChart;
        private static readonly int[] lamps;
        private const int THM = 200000000;

        static Experience()
        {
            experienceChart = new Dictionary<int, int>();

            double current = 0;
            for (int i = 1; i < 126; i++)
            {
                current += Math.Floor(i + 300 * Math.Pow(2, i / 7d));
                experienceChart[i + 1] = (int)Math.Floor(current / 4);
            }

            lamps = JsonConvert.DeserializeObject<int[]>(File.ReadAllText("Data/Lamps.json"));
        }

        public static int FromLevel(int level)
        {
            if (level <= 1) return 0;
            if (level >= 127) return THM;
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

        public static double Lamp(int level, int prestige)
        {
            int lamp;
            if (level <= 1) lamp = lamps[0];
            else if (level >= 99) lamp = lamps[97];
            else lamp = lamps[level - 1];
            return lamp / Math.Pow(2, prestige);
        }

        public static long ToRaw(int experience, int prestige)
        {
            long modifier = (long)Math.Pow(2, prestige);
            return experience * modifier + THM * (modifier - 1);
        }

        public static (int Experience, int Prestige) FromRaw(long rawExperience)
        {
            long experience;
            int prestige = 0;
            long modifier = 1;

            while ((experience = rawExperience - modifier * THM) >= 0)
            {
                rawExperience = experience;
                modifier *= 2;
                prestige++;
            }

            return ((int)(rawExperience / modifier), prestige);
        }
    }
}
