using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlissBuddy
{
    public class Skill : BindableBase
    {
        private const bool FORCE_TARGET_TO_BE_HIGHER = false;

        public string Name;

        public ExperienceMethod[] ExperienceMethods;

        private int rank;
        public int Rank
        {
            get => rank;
            set => SetProperty(ref rank, value);
        }

        private int experience;
        public int Experience
        {
            get => experience;
            set
            {
                if (FORCE_TARGET_TO_BE_HIGHER && RawTargetExperience < BlissBuddy.Experience.Raw(value, Prestige))
                {
                    TargetExperience = value;
                    TargetPrestige = Prestige;
                }
                if (SetProperty(ref experience, value))
                {
                    OnPropertyChanged("RawExperience");
                    OnPropertyChanged("Level");
                }
            }
        }

        private int prestige;
        public int Prestige
        {
            get => prestige;
            set
            {
                if (FORCE_TARGET_TO_BE_HIGHER && RawTargetExperience < BlissBuddy.Experience.Raw(Experience, value))
                {
                    TargetExperience = Experience;
                    TargetPrestige = value;
                }
                if (SetProperty(ref prestige, value))
                {
                    OnPropertyChanged("RawExperience");
                    OnPropertyChanged("Level");
                }
            }
        }

        public int Level => BlissBuddy.Experience.ToLevel(Experience);

        private int targetExp;
        public int TargetExperience
        {
            get => targetExp;
            set
            {
                if (FORCE_TARGET_TO_BE_HIGHER && BlissBuddy.Experience.Raw(value, TargetPrestige) < RawExperience)
                {
                    value = Experience;
                    TargetPrestige = Prestige;
                }
                if (SetProperty(ref targetExp, value))
                {
                    OnPropertyChanged("RawTargetExperience");
                    OnPropertyChanged("TargetLevel");
                }
            }
        }

        private int targetPrestige;
        public int TargetPrestige
        {
            get => targetPrestige;
            set
            {
                if (FORCE_TARGET_TO_BE_HIGHER && BlissBuddy.Experience.Raw(TargetExperience, value) < RawExperience)
                {
                    TargetExperience = Experience;
                    value = Prestige;
                }
                if (SetProperty(ref targetPrestige, value))
                {
                    OnPropertyChanged("RawTargetExperience");
                    OnPropertyChanged("TargetLevel");
                }
            }
        }

        public int TargetLevel => BlissBuddy.Experience.ToLevel(TargetExperience);

        public long RawExperienceToTarget => RawTargetExperience - RawExperience;

        public long RawExperience => BlissBuddy.Experience.Raw(Experience, Prestige);

        public long RawTargetExperience => BlissBuddy.Experience.Raw(TargetExperience, TargetPrestige);
    }
}
