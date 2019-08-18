using BlissBuddy.Views;
using CachedImage;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace BlissBuddy
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, SkillView> skillViews;

        public MainWindow()
        {
            try
            {
                Directory.CreateDirectory("Images");
                FileCache.AppCacheMode = FileCache.CacheMode.Dedicated;
                FileCache.AppCacheDirectory = "Images";

                InitializeComponent();
                DataContext = this;

                skillViews = new Dictionary<string, SkillView>();
                Skill[] skills = JsonConvert.DeserializeObject<Skill[]>(File.ReadAllText("Data/Skills.json"));
                foreach (Skill skill in skills)
                {
                    SkillView view = new SkillView(skill, this);
                    skillViews.Add(skill.Name, view);
                    SkillViews.Items.Add(new TabItem
                    {
                        Header = skill.Name,
                        Content = view,
                    });
                }

                EquipmentTab.Content = new EquipmentView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
