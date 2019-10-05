using System;
using System.Windows;

namespace BlissBuddy.Views
{
    class LampView : SkillView
    {
        public LampView() : base(new Skill(), null)
        {
            MainGrid.ColumnDefinitions.RemoveAt(2);
            IterationsTitle.Text = "Lamps needed:";
            ExperienceTitle.Text = "Experience per lamp at current level:";
            ExperiencePerItem.Content = 2356;
        }

        public override void InputChanged()
        {
            try
            {
                SetExperienceValues();
                double currentExp = Skill.Experience;
                int currentPrestige = Skill.Prestige;
                int lamps = 0;
                long rawTarget = Skill.RawTargetExperience;
                while (Experience.ToRaw((int)currentExp, currentPrestige) < rawTarget)
                {
                    currentExp += Experience.Lamp(Experience.ToLevel((int)currentExp), currentPrestige);
                    if (currentExp >= 200000000)
                    {
                        currentExp = 1;
                        currentPrestige++;
                    }
                    lamps++;
                }
                IterationsNeeded.Content = lamps;
                ExperiencePerItem.Content = Experience.Lamp(Experience.ToLevel(Skill.Experience), Skill.Prestige);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
