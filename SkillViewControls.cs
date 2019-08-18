using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BlissBuddy.Views
{
    public partial class SkillView : UserControl
    {
        public Skill Skill;

        private readonly CheckBox dxpCheckbox;
        private readonly CheckBox brawlingCheckbox;
        private readonly CheckBox wildernessCheckbox;

        private bool dxpChecked => dxpCheckbox.IsChecked.HasValue && dxpCheckbox.IsChecked.Value;
        private bool brawlingChecked => brawlingCheckbox.IsChecked.HasValue && brawlingCheckbox.IsChecked.Value;
        private bool wildernessChecked => wildernessCheckbox.IsChecked.HasValue && wildernessCheckbox.IsChecked.Value;

        public SkillView(SkillViewWindow parent)
        {
            if (parent != null)
            {
                dxpCheckbox = parent.DXPCheckBox;
                brawlingCheckbox = parent.BrawlingGlovesCheckBox;
                wildernessCheckbox = parent.WildernessCheckBox;

                dxpCheckbox.Checked += CheckboxStateChanged;
                dxpCheckbox.Unchecked += CheckboxStateChanged;
                brawlingCheckbox.Checked += CheckboxStateChanged;
                brawlingCheckbox.Unchecked += CheckboxStateChanged;
                wildernessCheckbox.Checked += CheckboxStateChanged;
                wildernessCheckbox.Unchecked += CheckboxStateChanged;
            }
        }

        private void CheckboxStateChanged(object sender, RoutedEventArgs e)
        {
            InputChanged();
        }

        private void NumberInputValidation(object sender, TextCompositionEventArgs e)
        {
            TextBox source = (TextBox)sender;
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberFieldLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox source = (TextBox)sender;
            Regex regex = new Regex("[^0-9]+");
            source.Text = regex.Replace(source.Text, ""); // Prevents pasting of invalid characters
            if (source.Text == "")
                source.Text = "0";
            if (source.Text.Length == 9 && source.Text[0] >= '2')
                source.Text = "200000000";
            InputChanged();
        }

        private void CurrentUpButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.Prestige++;
            if (Skill.Prestige > 99)
                Skill.Prestige = 99;
            InputChanged();
        }

        private void CurrentDownButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.Prestige--;
            if (Skill.Prestige < 0)
                Skill.Prestige = 0;
            InputChanged();
        }

        private void TargetUpButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.TargetPrestige++;
            if (Skill.TargetPrestige > 99)
                Skill.TargetPrestige = 99;
            InputChanged();
        }

        private void TargetDownButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.TargetPrestige--;
            if (Skill.TargetPrestige < 0)
                Skill.TargetPrestige = 0;
            InputChanged();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                Keyboard.ClearFocus();
                NumberFieldLostFocus(sender, null);
            }
        }

        private T FindAncestor<T>(FrameworkElement current) where T : FrameworkElement
        {
            do
            {
                if (current is T)
                    return (T)current;
                current = (FrameworkElement)current.Parent;
            }
            while (current != null);
            return null;
        }

        private void Level_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = FindAncestor<ContextMenu>((FrameworkElement)sender);
            if (menu != null)
            {
                TextBox source = (TextBox)menu.PlacementTarget;
                source.Text = Experience.FromLevel(int.Parse((string)((MenuItem)sender).Tag)).ToString();
                Keyboard.ClearFocus();
                InputChanged();
            }
        }

        private void MaxExp_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = FindAncestor<ContextMenu>((FrameworkElement)sender);
            if (menu != null)
            {
                TextBox source = (TextBox)menu.PlacementTarget;
                source.Text = "200000000";
                Keyboard.ClearFocus();
                InputChanged();
            }
        }

        protected void SetExperienceValues()
        {
            if (int.TryParse(ExperienceBox.Text, out int i)) Skill.Experience = i;
            if (int.TryParse(PrestigeBox.Text, out i)) Skill.Prestige = i;
            if (int.TryParse(TargetExperienceBox.Text, out i)) Skill.TargetExperience = i;
            if (int.TryParse(TargetPrestigeBox.Text, out i)) Skill.TargetPrestige = i;
        }
    }
}
