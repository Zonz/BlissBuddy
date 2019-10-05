using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace BlissBuddy.Views
{
    /// <summary>
    /// Interaction logic for SkillViewWindow.xaml
    /// </summary>
    public partial class SkillViewWindow : UserControl
    {
        private Dictionary<string, SkillView> skillViews;
        private Dictionary<string, PotentialExpView> potentialExpViews;

        public SkillViewWindow()
        {
            InitializeComponent();

            Skill[] skills = JsonConvert.DeserializeObject<Skill[]>(File.ReadAllText("Data/Skills.json"));
            skillViews = new Dictionary<string, SkillView>();
            potentialExpViews = new Dictionary<string, PotentialExpView>();
            foreach (Skill skill in skills)
            {
                SkillView view = new SkillView(skill, this);
                skillViews.Add(skill.Name, view);
                SkillViews.Items.Add(new TabItem
                {
                    Header = skill.Name,
                    Content = view,
                });

                // Make a copy of the skill so we can modify its values independantly
                PotentialExpView pView = new PotentialExpView(new Skill { Name = skill.Name, ExperienceMethods = skill.ExperienceMethods }, this);
                potentialExpViews.Add(skill.Name, pView);
                PotentialExpViews.Items.Add(new TabItem
                {
                    Header = skill.Name,
                    Content = pView,
                });
            }
        }

        private async void LookupStats_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameTextBox.Text;
                if (name == "")
                {
                    MessageBox.Show("Please enter a name into the text box.");
                    return;
                }
                using (WebClient web = new WebClient())
                {
                    Stream stream = await web.OpenReadTaskAsync(new Uri("http://blissscape.net/highscores/?player=" + name.Replace(" ", "+")));
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        reader.ReadLines(23);
                        if (reader.ReadLine().Contains("alert"))
                        {
                            MessageBox.Show($"No highscore results were found for {name}.");
                            return;
                        }
                        NameTextBox.Text = reader.ReadName();
                        reader.ReadLines(13);
                        for (int i = 0; i < 25; i++)
                        {
                            var stats = reader.ReadSkill();
                            if (!skillViews.TryGetValue(stats.Name, out SkillView skillView))
                                continue;
                            skillView.Skill.Rank = stats.Rank;
                            skillView.Skill.Experience = stats.Experience;
                            skillView.Skill.Prestige = stats.Prestige;
                            skillView.InputChanged();

                            if (!potentialExpViews.TryGetValue(stats.Name, out PotentialExpView pView))
                                continue;
                            pView.Skill.Rank = stats.Rank;
                            pView.Skill.Experience = stats.Experience;
                            pView.Skill.Prestige = stats.Prestige;
                            pView.InputChanged();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
