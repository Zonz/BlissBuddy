using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace BlissBuddy
{
    /// <summary>
    /// Interaction logic for EquipmentView.xaml
    /// </summary>
    public partial class EquipmentView : UserControl
    {
        private Item[] helmets;
        private Item[] capes;
        private Item[] amulets;
        private Item[] ammo;
        private Item[] mainHands;
        private Item[] offHands;
        private Item[] torsos;
        private Item[] legs;
        private Item[] gloves;
        private Item[] boots;
        private Item[] rings;

        private Item currentHelm;
        private Item currentCape;
        private Item currentAmulet;
        private Item currentAmmo;
        private Item currentMainHand;
        private Item currentOffHand;
        private Item currentTorso;
        private Item currentLegs;
        private Item currentGloves;
        private Item currentBoots;
        private Item currentRing;

        private Item currentStats;

        public EquipmentView()
        {
            InitializeComponent();
            helmets = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/Helmets.json"));
            capes = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/Capes.json"));
            amulets = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/Amulets.json"));
            ammo = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/Ammo.json"));
            mainHands = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/MainHands.json"));
            offHands = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/OffHands.json"));
            torsos = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/Torsos.json"));
            legs = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/Legs.json"));
            gloves = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/Gloves.json"));
            boots = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/Boots.json"));
            rings = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText("Data/Rings.json"));
        }

        private void OpenContextMenu(object sender, Item[] items, RoutedEventHandler handler)
        {
            ContextMenu menu = new ContextMenu();
            foreach (Item item in items)
            {
                MenuItem menuItem = new MenuItem
                {
                    Header = item.Name,
                    Tag = item,
                };
                menuItem.Click += handler;
                menu.Items.Add(menuItem);
            }
            menu.PlacementTarget = sender as Image;
            menu.IsOpen = true;
        }

        private Item SelectItem(CachedImage.Image image, RoutedEventArgs e)
        {
            Item item = (Item)((MenuItem)e.OriginalSource).Tag;
            image.ImageUrl = item.ImageUrl;
            image.ToolTip = item.Name;
            image.Visibility = Visibility.Visible;
            return item;
        }

        private void HelmetSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentHelm);
            currentHelm = SelectItem(HelmImage, e);
            AddItem(currentHelm);
        }

        private void HelmLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, helmets, HelmetSelected);
        }

        private void HelmRightClick(object sender, MouseButtonEventArgs e)
        {
            HelmImage.Visibility = Visibility.Hidden;
            RemoveItem(currentHelm);
            currentHelm = new Item();
        }

        private void CapeSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentCape);
            currentCape = SelectItem(CapeImage, e);
            AddItem(currentCape);
        }

        private void CapeLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, capes, CapeSelected);
        }

        private void CapeRightClick(object sender, MouseButtonEventArgs e)
        {
            CapeImage.Visibility = Visibility.Hidden;
            RemoveItem(currentCape);
            currentCape = new Item();
        }

        private void AmuletSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentAmulet);
            currentAmulet = SelectItem(AmuletImage, e);
            AddItem(currentAmulet);
        }

        private void AmuletLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, amulets, AmuletSelected);
        }

        private void AmuletRightClick(object sender, MouseButtonEventArgs e)
        {
            AmuletImage.Visibility = Visibility.Hidden;
            RemoveItem(currentAmulet);
            currentAmulet = new Item();
        }

        private void AmmoSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentAmmo);
            currentAmmo = SelectItem(AmmoImage, e);
            AddItem(currentAmmo);
        }

        private void AmmoLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, ammo, AmmoSelected);
        }

        private void AmmoRightClick(object sender, MouseButtonEventArgs e)
        {
            AmmoImage.Visibility = Visibility.Hidden;
            RemoveItem(currentAmmo);
            currentAmmo = new Item();
        }

        private void MainHandSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentMainHand);
            currentMainHand = SelectItem(MainHandImage, e);
            AddItem(currentMainHand);
            if (currentMainHand.TwoHandWeapon)
            {
                RemoveItem(currentOffHand);
                currentOffHand = new Item();
                OffHandImage.Visibility = Visibility.Hidden;
            }
        }

        private void MainHandLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, mainHands, MainHandSelected);
        }

        private void MainHandRightClick(object sender, MouseButtonEventArgs e)
        {
            MainHandImage.Visibility = Visibility.Hidden;
            RemoveItem(currentMainHand);
            currentMainHand = new Item();
        }

        private void OffHandSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentOffHand);
            currentOffHand = SelectItem(OffHandImage, e);
            AddItem(currentOffHand);
            if (currentMainHand.TwoHandWeapon)
            {
                RemoveItem(currentMainHand);
                currentMainHand = new Item();
                MainHandImage.Visibility = Visibility.Hidden;
            }
        }

        private void OffHandLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, offHands, OffHandSelected);
        }

        private void OffHandRightClick(object sender, MouseButtonEventArgs e)
        {
            OffHandImage.Visibility = Visibility.Hidden;
            RemoveItem(currentOffHand);
            currentOffHand = new Item();
        }

        private void TorsoSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentTorso);
            currentTorso = SelectItem(TorsoImage, e);
            AddItem(currentTorso);
        }

        private void TorsoLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, torsos, TorsoSelected);
        }

        private void TorsoRightClick(object sender, MouseButtonEventArgs e)
        {
            TorsoImage.Visibility = Visibility.Hidden;
            RemoveItem(currentTorso);
            currentTorso = new Item();
        }

        private void LegsSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentLegs);
            currentLegs = SelectItem(LegsImage, e);
            AddItem(currentLegs);
        }

        private void LegsLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, legs, LegsSelected);
        }

        private void LegsRightClick(object sender, MouseButtonEventArgs e)
        {
            LegsImage.Visibility = Visibility.Hidden;
            RemoveItem(currentLegs);
            currentLegs = new Item();
        }

        private void GlovesSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentGloves);
            currentGloves = SelectItem(GlovesImage, e);
            AddItem(currentGloves);
        }

        private void GlovesLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, gloves, GlovesSelected);
        }

        private void GlovesRightClick(object sender, MouseButtonEventArgs e)
        {
            GlovesImage.Visibility = Visibility.Hidden;
            RemoveItem(currentGloves);
            currentGloves = new Item();
        }

        private void BootsSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentBoots);
            currentBoots = SelectItem(BootsImage, e);
            AddItem(currentBoots);
        }

        private void BootsLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, boots, BootsSelected);
        }

        private void BootsRightClick(object sender, MouseButtonEventArgs e)
        {
            BootsImage.Visibility = Visibility.Hidden;
            RemoveItem(currentBoots);
            currentBoots = new Item();
        }

        private void RingSelected(object sender, RoutedEventArgs e)
        {
            RemoveItem(currentRing);
            currentRing = SelectItem(RingImage, e);
            AddItem(currentRing);
        }

        private void RingLeftClick(object sender, MouseButtonEventArgs e)
        {
            OpenContextMenu(sender, rings, RingSelected);
        }

        private void RingRightClick(object sender, MouseButtonEventArgs e)
        {
            RingImage.Visibility = Visibility.Hidden;
            RemoveItem(currentRing);
            currentRing = new Item();
        }

        private void AddItem(Item item)
        {
            currentStats.StabAttack += item.StabAttack;
            currentStats.SlashAttack += item.SlashAttack;
            currentStats.CrushAttack += item.CrushAttack;
            currentStats.MagicAttack += item.MagicAttack;
            currentStats.RangeAttack += item.RangeAttack;
            currentStats.StabDefence += item.StabDefence;
            currentStats.SlashDefence += item.SlashDefence;
            currentStats.CrushDefence += item.CrushDefence;
            currentStats.MagicDefence += item.MagicDefence;
            currentStats.RangeDefence += item.RangeDefence;
            currentStats.SummoningDefence += item.SummoningDefence;
            currentStats.AbsorbMelee += item.AbsorbMelee;
            currentStats.AbsorbMagic += item.AbsorbMagic;
            currentStats.AbsorbRanged += item.AbsorbRanged;
            currentStats.Strength += item.Strength;
            currentStats.RangedStrength += item.RangedStrength;
            currentStats.Prayer += item.Prayer;
            currentStats.MagicDamage += item.MagicDamage;
            currentStats.HealthBonus += item.HealthBonus;
            UpdateStats();
        }

        private void RemoveItem(Item item)
        {
            currentStats.StabAttack -= item.StabAttack;
            currentStats.SlashAttack -= item.SlashAttack;
            currentStats.CrushAttack -= item.CrushAttack;
            currentStats.MagicAttack -= item.MagicAttack;
            currentStats.RangeAttack -= item.RangeAttack;
            currentStats.StabDefence -= item.StabDefence;
            currentStats.SlashDefence -= item.SlashDefence;
            currentStats.CrushDefence -= item.CrushDefence;
            currentStats.MagicDefence -= item.MagicDefence;
            currentStats.RangeDefence -= item.RangeDefence;
            currentStats.SummoningDefence -= item.SummoningDefence;
            currentStats.AbsorbMelee -= item.AbsorbMelee;
            currentStats.AbsorbMagic -= item.AbsorbMagic;
            currentStats.AbsorbRanged -= item.AbsorbRanged;
            currentStats.Strength -= item.Strength;
            currentStats.RangedStrength -= item.RangedStrength;
            currentStats.Prayer -= item.Prayer;
            currentStats.MagicDamage -= item.MagicDamage;
            currentStats.HealthBonus -= item.HealthBonus;
            UpdateStats();
        }

        private void UpdateStats()
        {
            StabAttack.Text = addPlus(currentStats.StabAttack);
            SlashAttack.Text = addPlus(currentStats.SlashAttack);
            CrushAttack.Text = addPlus(currentStats.CrushAttack);
            MagicAttack.Text = addPlus(currentStats.MagicAttack);
            RangeAttack.Text = addPlus(currentStats.RangeAttack);
            StabDefence.Text = addPlus(currentStats.StabDefence);
            SlashDefence.Text = addPlus(currentStats.SlashDefence);
            CrushDefence.Text = addPlus(currentStats.CrushDefence);
            MagicDefence.Text = addPlus(currentStats.MagicDefence);
            RangeDefence.Text = addPlus(currentStats.RangeDefence);
            SummoningDefence.Text = addPlus(currentStats.SummoningDefence);
            AbsorbMelee.Text = addPlus(currentStats.AbsorbMelee);
            AbsorbMagic.Text = addPlus(currentStats.AbsorbMagic);
            AbsorbRanged.Text = addPlus(currentStats.AbsorbRanged);
            Strength.Text = addPlus(currentStats.Strength);
            RangedStr.Text = addPlus(currentStats.RangedStrength);
            Prayer.Text = addPlus(currentStats.Prayer);
            MagicDamage.Text = addPlus(currentStats.MagicDamage);
            HealthText.Text = (990 + currentStats.HealthBonus).ToString();

            string addPlus(int value)
            {
                if (value >= 0)
                    return "+" + value;
                return value.ToString();
            }
        }
    }
}
