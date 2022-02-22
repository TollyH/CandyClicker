/*
 * Candy Clicker ver. 1.1.1
 * Copyright © 2021-2022  Ptolemy Hill
 */
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CandyClicker
{
    /// <summary>
    /// Interaction logic for ShopControl.xaml
    /// </summary>
    public partial class ShopControl : UserControl
    {

        public static readonly DependencyProperty ShopItemNameProperty = DependencyProperty.Register("ShopItemName", typeof(string), typeof(ShopControl));
        public string ShopItemName
        {
            get => (string)GetValue(ShopItemNameProperty);
            set => SetValue(ShopItemNameProperty, value);
        }
        public static readonly DependencyProperty ShopItemPriceProperty = DependencyProperty.Register("ShopItemPrice", typeof(ulong), typeof(ShopControl));
        public ulong ShopItemPrice
        {
            get => (ulong)GetValue(ShopItemPriceProperty);
            set => SetValue(ShopItemPriceProperty, value);
        }
        public static readonly DependencyProperty PriceForegroundProperty = DependencyProperty.Register("PriceForeground", typeof(Brush), typeof(ShopControl));
        public Brush PriceForeground
        {
            get => (Brush)GetValue(PriceForegroundProperty);
            set => SetValue(PriceForegroundProperty, value);
        }

        public ShopControl()
        {
            InitializeComponent();
        }

        private void UserControlShopItem_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            DoubleAnimation slightWhiteFade = new()
            {
                From = 0,
                To = 0.4,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(slightWhiteFade, rectangleFlasher);
            Storyboard.SetTargetProperty(slightWhiteFade, new PropertyPath(OpacityProperty));
            sb.Children.Add(slightWhiteFade);

            sb.Begin();
        }

        private void UserControlShopItem_MouseLeave(object sender, MouseEventArgs e)
        {
            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            DoubleAnimation slightWhiteFade = new()
            {
                From = 0.4,
                To = 0,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(slightWhiteFade, rectangleFlasher);
            Storyboard.SetTargetProperty(slightWhiteFade, new PropertyPath(OpacityProperty));
            sb.Children.Add(slightWhiteFade);

            sb.Begin();
        }

        private void UserControlShopItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Storyboard sb = new()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };

                DoubleAnimation slightWhiteFadeIn = new()
                {
                    From = 0.4,
                    To = 1,
                    Duration = sb.Duration.TimeSpan / 2
                };
                Storyboard.SetTarget(slightWhiteFadeIn, rectangleFlasher);
                Storyboard.SetTargetProperty(slightWhiteFadeIn, new PropertyPath(OpacityProperty));
                sb.Children.Add(slightWhiteFadeIn);

                DoubleAnimation slightWhiteFadeOut = new()
                {
                    From = 1,
                    To = 0.4,
                    Duration = sb.Duration.TimeSpan / 2,
                    BeginTime = sb.Duration.TimeSpan / 2
                };
                Storyboard.SetTarget(slightWhiteFadeOut, rectangleFlasher);
                Storyboard.SetTargetProperty(slightWhiteFadeOut, new PropertyPath(OpacityProperty));
                sb.Children.Add(slightWhiteFadeOut);

                sb.Begin();
            }
        }
    }
}
