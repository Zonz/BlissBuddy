using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlissBuddy.Views
{
    public partial class SkillView : UserControl
    {
        private Dictionary<string, float> database;

        private ListBox[] experienceMethodLists;

        private ListBox currentSelectedList => experienceMethodLists[ExperienceMethodBox.SelectedIndex];

        public SkillView(Skill skill, SkillViewWindow parent) : this(parent)
        {
            InitializeComponent();
            Skill = skill;
            DataContext = Skill;

            if (skill.ExperienceMethods == null || skill.ExperienceMethods.Length == 0)
                return;

            experienceMethodLists = new ListBox[skill.ExperienceMethods.Length];
            ExperienceMethodBox.SelectedIndex = 0;

            for (int i = 0; i < skill.ExperienceMethods.Length; i++)
            {
                ExperienceMethod expMethod = skill.ExperienceMethods[i];
                ExperienceMethodBox.Items.Add(expMethod);
                ListBox listBox = new ListBox
                {
                    ItemsSource = expMethod.ExperienceTable.Keys,
                    SelectedIndex = 0,
                    BorderBrush = Brushes.Black,
                    Background = Brushes.Gray,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Visibility = Visibility.Collapsed,
                };
                experienceMethodLists[i] = listBox;
                listBox.SelectionChanged += ListSelectionChanged;
                Grid.SetRow(listBox, 1);
                ExperienceMethodGrid.Children.Add(listBox);
            }

            experienceMethodLists[0].Visibility = Visibility.Visible;

            if (skill.ExperienceMethods.Length > 1)
            {
                ExperienceMethodBox.Visibility = Visibility.Visible;
                ExperienceMethodBox.SelectionChanged += ComboBoxSelectionChanged;
                ComboBoxSelectionChanged(null, null);
            }
            else
            {
                ExperienceMethod method = skill.ExperienceMethods[0];

                if (method.ExperienceTable != null && method.ExperienceTable.Count > 0)
                    database = method.ExperienceTable;
                else
                    database = null;

                if (!string.IsNullOrWhiteSpace(method.IterationText))
                    IterationsTitle.Text = method.IterationText + " needed:";
                else
                    IterationsNeeded.Content = "";

                if (!string.IsNullOrWhiteSpace(method.BaseExpText))
                    ExperienceTitle.Text = "Experience per " + method.BaseExpText + ":";
                else
                    ExperiencePerItem.Content = "";

                InputChanged();
            }
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExperienceMethod method = (ExperienceMethod)ExperienceMethodBox.SelectedItem;
            
            if (method.ExperienceTable != null && method.ExperienceTable.Count > 0)
                database = method.ExperienceTable;
            else
                database = null;
            
            if (!string.IsNullOrWhiteSpace(method.IterationText))
                IterationsTitle.Text = method.IterationText + " needed:";
            else
                IterationsNeeded.Content = "";
            
            if (!string.IsNullOrWhiteSpace(method.BaseExpText))
                ExperienceTitle.Text = "Experience per " + method.BaseExpText + ":";
            else
                ExperiencePerItem.Content = "";
            
            foreach (ListBox list in experienceMethodLists)
                list.Visibility = Visibility.Collapsed;

            experienceMethodLists[ExperienceMethodBox.SelectedIndex].Visibility = Visibility.Visible;

            InputChanged();
        }

        private void ListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputChanged();
        }

        public virtual void InputChanged()
        {
            try
            {
                SetExperienceValues();
                if (database == null)
                    return;
                string selection = (string)currentSelectedList.SelectedItem;
                float baseExp = database[selection];
                if (dxpChecked)
                    baseExp *= 2;
                else if (brawlingChecked)
                    baseExp *= wildernessChecked ? 2 : 1.25f;
                int iterationsNeeded = (int)Math.Ceiling(Skill.RawExperienceToTarget / baseExp);
                NameTextBox.Text = selection;
                IterationsNeeded.Content = Math.Max(iterationsNeeded, 0);
                ExperiencePerItem.Content = baseExp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
