using BlissBuddy.Views;
using CachedImage;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BlissBuddy
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            try
            {
                Directory.CreateDirectory("Images");
                FileCache.AppCacheMode = FileCache.CacheMode.Dedicated;
                FileCache.AppCacheDirectory = "Images";

                InitializeComponent();
                DataContext = this;

                AddView("Skills", new SkillViewWindow());
                AddView("Equipment", new EquipmentView());
                AddView("Lamps", new LampView());

                void AddView(string header, ContentControl content)
                {
                    Tabs.Items.Add(new TabItem { Header = header, Content = content });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
