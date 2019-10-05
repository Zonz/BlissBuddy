using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BlissBuddy.Views
{
    /// <summary>
    /// Interaction logic for PotentialExpView.xaml
    /// </summary>
    public partial class PotentialExpView : SkillViewBase
    {
        private StackPanel[] experienceMethods;

        private ItemQuantityPanel[] items;

        private int startExperience;

        private int startPrestige;

        public void SetStartPoint()
        {
            startExperience = Skill.Experience;
            startPrestige = Skill.Prestige;
        }

        public PotentialExpView(Skill skill, SkillViewWindow parent) : base(skill, parent)
        {
            InitializeComponent();
            DataContext = Skill;

            experienceMethods = new StackPanel[skill.ExperienceMethods.Length];
            List<ItemQuantityPanel> tmpItems = new List<ItemQuantityPanel>();

            for (int i = 0; i < skill.ExperienceMethods.Length; i++)
            {
                ExperienceMethod expMethod = skill.ExperienceMethods[i];
                ExperienceMethodBox.Items.Add(expMethod);

                StackPanel stackPanel = new StackPanel
                {
                    Margin = new Thickness(0, 0, 0, 1),
                };

                foreach (var pair in expMethod.ExperienceTable)
                {
                    ItemQuantityPanel item = new ItemQuantityPanel(pair.Key, pair.Value, this);
                    tmpItems.Add(item);
                    stackPanel.Children.Add(item);
                }

                experienceMethods[i] = stackPanel;
            }

            ScrollView.Content = experienceMethods[0];
            items = tmpItems.ToArray();

            if (skill.ExperienceMethods.Length > 1)
            {
                ExperienceMethodBox.Visibility = Visibility.Visible;
                ExperienceMethodBox.SelectionChanged += ComboBoxSelectionChanged;
                ExperienceMethodBox.SelectedIndex = 0;
            }
            InputChanged();
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScrollView.Content = experienceMethods[ExperienceMethodBox.SelectedIndex];
        }

        public override void InputChanged()
        {
            if (int.TryParse(ExperienceBox.Text, out int i)) Skill.Experience = i;
            if (int.TryParse(PrestigeBox.Text, out i)) Skill.Prestige = i;

            long total = items.Sum(item => (long)item.TotalExp);
            if (dxpChecked)
                total *= 2;
            else if (brawlingChecked)
                total = (long)(total * (wildernessChecked ? 2 : 1.25f));
            total += Skill.RawExperience;
            EndRawExperience.Content = total;
            var stats = Experience.FromRaw(total);
            EndLevel.Content = Experience.ToLevel(stats.Experience);
            EndExperience.Content = stats.Experience;
            EndPrestige.Content = stats.Prestige;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in items)
                item.Clear();
            InputChanged();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.Experience = int.Parse(EndExperience.Content.ToString());
            Skill.Prestige = int.Parse(EndPrestige.Content.ToString());
            foreach (var item in items)
                item.Clear();
            InputChanged();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.Experience = startExperience;
            Skill.Prestige = startPrestige;
            foreach (var item in items)
                item.Clear();
            InputChanged();
        }
    }
}
