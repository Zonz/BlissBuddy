using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// Interaction logic for ItemQuantityPanel.xaml
    /// </summary>
    public partial class ItemQuantityPanel : SkillViewBase
    {
        private class ItemQuantity : BindableBase
        {
            private string name;
            public string Name
            {
                get => name;
                set => SetProperty(ref name, value);
            }

            private int quantity;
            public int Quantity
            {
                get => quantity;
                set => SetProperty(ref quantity, value);
            }

            private float experience;
            public float Experience
            {
                get => experience;
                set => SetProperty(ref experience, value);
            }

            public ItemQuantity(string name, float experience)
            {
                Name = name;
                Quantity = 0;
                Experience = experience;
            }
        }

        public float TotalExp => item.Quantity * item.Experience;

        private ItemQuantity item;
        private PotentialExpView parent;

        public ItemQuantityPanel(string itemName, float experience, PotentialExpView parent) : base()
        {
            InitializeComponent();
            item = new ItemQuantity(itemName + " (" + experience + ")", experience);
            DataContext = item;
            this.parent = parent;
        }

        public override void InputChanged()
        {
            if (int.TryParse(QuantityBox.Text, out int i)) item.Quantity = i;
            parent.InputChanged();
        }

        public void Clear()
        {
            item.Quantity = 0;
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            item.Quantity++;
            InputChanged();
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (item.Quantity > 0)
            {
                item.Quantity--;
                InputChanged();
            }
        }
    }
}
