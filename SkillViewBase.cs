using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BlissBuddy.Views
{
    public abstract class SkillViewBase : UserControl
    {
        public readonly Skill Skill;

        public abstract void InputChanged();

        private readonly CheckBox dxpCheckbox;
        private readonly CheckBox brawlingCheckbox;
        private readonly CheckBox wildernessCheckbox;

        protected bool dxpChecked => dxpCheckbox.IsChecked.HasValue && dxpCheckbox.IsChecked.Value;
        protected bool brawlingChecked => brawlingCheckbox.IsChecked.HasValue && brawlingCheckbox.IsChecked.Value;
        protected bool wildernessChecked => wildernessCheckbox.IsChecked.HasValue && wildernessCheckbox.IsChecked.Value;

        public SkillViewBase() : this(null, null) { }

        public SkillViewBase(Skill skill, SkillViewWindow parent)
        {
            Skill = skill;

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

        protected void CheckboxStateChanged(object sender, RoutedEventArgs e)
        {
            InputChanged();
        }

        protected void NumberInputValidation(object sender, TextCompositionEventArgs e)
        {
            TextBox source = (TextBox)sender;
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        protected void NumberFieldLostFocus(object sender, RoutedEventArgs e)
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

        protected void CurrentUpButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.Prestige++;
            if (Skill.Prestige > 99)
                Skill.Prestige = 99;
            InputChanged();
        }

        protected void CurrentDownButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.Prestige--;
            if (Skill.Prestige < 0)
                Skill.Prestige = 0;
            InputChanged();
        }

        protected void TargetUpButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.TargetPrestige++;
            if (Skill.TargetPrestige > 99)
                Skill.TargetPrestige = 99;
            InputChanged();
        }

        protected void TargetDownButton_Click(object sender, RoutedEventArgs e)
        {
            Skill.TargetPrestige--;
            if (Skill.TargetPrestige < 0)
                Skill.TargetPrestige = 0;
            InputChanged();
        }

        protected void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                Keyboard.ClearFocus();
                NumberFieldLostFocus(sender, null);
            }
        }

        protected T FindAncestor<T>(FrameworkElement current) where T : FrameworkElement
        {
            do
            {
                if (current is T ancestor)
                    return ancestor;
                current = (FrameworkElement)current.Parent;
            }
            while (current != null);
            return null;
        }

        protected void Level_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = FindAncestor<ContextMenu>((FrameworkElement)sender);
            if (menu != null && menu.PlacementTarget is TextBox source)
            {
                source.Text = Experience.FromLevel(int.Parse((string)((MenuItem)sender).Tag)).ToString();
                Keyboard.ClearFocus();
                InputChanged();
            }
        }

        protected void MaxExp_Click(object sender, RoutedEventArgs e)
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
    }
}
