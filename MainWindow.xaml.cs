/*
 * Candy Clicker ver. 1.1.0
 * Copyright © 2021-2022  Ptolemy Hill
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace CandyClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty CandyScoreProperty = DependencyProperty.Register("CandyScore", typeof(ulong), typeof(MainWindow));
        public ulong CandyScore
        {
            get => (ulong)GetValue(CandyScoreProperty);
            set => SetValue(CandyScoreProperty, value);
        }
        public static readonly DependencyProperty CandyPerClickProperty = DependencyProperty.Register("CandyPerClick", typeof(ulong), typeof(MainWindow));
        public ulong CandyPerClick
        {
            get => (ulong)GetValue(CandyPerClickProperty);
            set => SetValue(CandyPerClickProperty, value);
        }
        public static readonly DependencyProperty CandyPerSecondProperty = DependencyProperty.Register("CandyPerSecond", typeof(ulong), typeof(MainWindow));
        public ulong CandyPerSecond
        {
            get => (ulong)GetValue(CandyPerSecondProperty);
            set => SetValue(CandyPerSecondProperty, value);
        }
        public static readonly DependencyProperty CandyPSReincarnationMultiplierProperty = DependencyProperty.Register("CandyPSReincarnationMultiplier", typeof(ulong), typeof(MainWindow));
        public ulong CandyPSReincarnationMultiplier
        {
            get => (ulong)GetValue(CandyPSReincarnationMultiplierProperty);
            set
            {
                SetValue(CandyPSReincarnationMultiplierProperty, value);
                textBlockReincarnated.Visibility = value is <= 1 or ulong.MaxValue ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public ulong[] ShopPurchasedCount { get; set; }

        public ulong ReincarnateCounter { get; set; }
        public ulong OverflowCounter { get; set; }

        private readonly string savePath = AppDomain.CurrentDomain.BaseDirectory + @"\save_data.dat";

        private uint clicksTowardSpecial = 0;
        private bool isSpecialActive = false;
        private bool isEndGameVisualActive = false;

        private bool doSaveIntegrityChecks = true;
        private bool doAutoClickPrevention = true;
        private bool doCandyRain = true;
        private byte cheatMenuKeyProgression = 0;
        // IAMALAZYCHEATER ;)
        private readonly Key[] cheatMenuKeys = new Key[15] { Key.I, Key.A, Key.M, Key.A, Key.L, Key.A, Key.Z, Key.Y, Key.C, Key.H, Key.E, Key.A, Key.T, Key.E, Key.R };
        private byte spawnRareCandyKeyProgression = 0;
        // EDIBLEGOLD
        private readonly Key[] spawnRareCandyKeys = new Key[] { Key.E, Key.D, Key.I, Key.B, Key.L, Key.E, Key.G, Key.O, Key.L, Key.D };

        private enum EasterEggState
        {
            Normal,
            Tolly,
            Fin,
            Riley,
            Tom
        }
        private readonly Dictionary<EasterEggState, Key[]> easterEggKeys = new()
        {
            {
                EasterEggState.Normal,
                new Key[20]
                {
                    // SAVEMEFROMTHISHORROR
                    Key.S, Key.A, Key.V, Key.E, Key.M, Key.E, Key.F, Key.R, Key.O, Key.M, Key.T, Key.H, Key.I, Key.S, Key.H, Key.O, Key.R, Key.R, Key.O, Key.R
                }
            },
            {
                EasterEggState.Tolly,
                new Key[16]
                {
                    // IAMTHEPROGRAMMER
                    Key.I, Key.A, Key.M, Key.T, Key.H, Key.E, Key.P, Key.R, Key.O, Key.G, Key.R, Key.A, Key.M, Key.M, Key.E, Key.R
                }
            },
            {
                EasterEggState.Fin,
                new Key[12]
                {
                    // INSANITYISME
                    Key.I, Key.N, Key.S, Key.A, Key.N, Key.I, Key.T, Key.Y, Key.I, Key.S, Key.M, Key.E
                }
            },
            {
                EasterEggState.Riley,
                new Key[16]
                {
                    // PLEASELETMESLEEP
                    Key.P, Key.L, Key.E, Key.A, Key.S, Key.E, Key.L, Key.E, Key.T, Key.M, Key.E, Key.S, Key.L, Key.E, Key.E, Key.P
                }
            },
            {
                EasterEggState.Tom,
                new Key[15]
                {
                    // TOOHIGHTOATTEND
                    Key.T, Key.O, Key.O, Key.H, Key.I, Key.G, Key.H, Key.T, Key.O, Key.A, Key.T, Key.T, Key.E, Key.N, Key.D
                }
            }
        };
        private readonly Dictionary<EasterEggState, BitmapImage[]> easterEggImages = new()
        {
            {
                EasterEggState.Normal,
                new BitmapImage[1]
                {
                    new BitmapImage(new Uri("pack://application:,,,/Images/candy.png"))
                }
            },
            {
                EasterEggState.Tolly,
                new BitmapImage[2]
                {
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-tolly-1.png")),
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-tolly-2.png"))
                }
            },
            {
                EasterEggState.Fin,
                new BitmapImage[5]
                {
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-fin-1.png")),
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-fin-2.png")),
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-fin-3.png")),
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-fin-4.png")),
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-fin-5.png"))
                }
            },
            {
                EasterEggState.Riley,
                new BitmapImage[2]
                {
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-riley-1.png")),
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-riley-2.png"))
                }
            },
            {
                EasterEggState.Tom,
                new BitmapImage[2]
                {
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-tom-1.png")),
                    new BitmapImage(new Uri("pack://application:,,,/Images/easter-tom-2.png"))
                }
            }
        };
        private readonly Dictionary<EasterEggState, Color> easterEggBackgroundColors = new()
        {
            { EasterEggState.Normal, new Color() { R = 0x86, G = 0xFF, B = 0xFA, A = 0xFF } },
            { EasterEggState.Tolly, new Color() { R = 0x46, G = 0x46, B = 0x46, A = 0xFF } },
            { EasterEggState.Fin, new Color() { R = 0x94, G = 0x00, B = 0x00, A = 0xFF } },
            { EasterEggState.Riley, new Color() { R = 0x00, G = 0x5C, B = 0x1D, A = 0xFF } },
            { EasterEggState.Tom, new Color() { R = 0xFF, G = 0x82, B = 0x9E, A = 0xFF } }
        };
        private readonly Dictionary<EasterEggState, byte> easterEggKeyProgression = new()
        {
            { EasterEggState.Normal, 0 },
            { EasterEggState.Tolly, 0 },
            { EasterEggState.Fin, 0 },
            { EasterEggState.Riley, 0 },
            { EasterEggState.Tom, 0 }
        };
        private EasterEggState currentEasterEggState = EasterEggState.Normal;

        // Used to calculate CPS and prevent autoclicker usage
        private DateTime lastClickTime = DateTime.MinValue;
        private int clicksThisSecond = 0;
        private readonly List<int> previousClicksPerSecond = new();

        private readonly System.Timers.Timer timerPerSecond = new(1000);
        private readonly System.Timers.Timer timerCandyPerSecond = new(50);
        private readonly System.Timers.Timer timerCandyRain = new();
        private readonly System.Timers.Timer timerAutoSave = new(10000);
        private readonly Random rng = new();

        private uint candyPerSecondCycleCount = 0;

        private static readonly byte[] saveHeader = new byte[8] { 0x43, 0x6E, 0x64, 0x79, 0x43, 0x6C, 0x63, 0x6B };  // "CndyClck"

        public MainWindow()
        {
            ShowInTaskbar = !File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\no-taskbar");
            doSaveIntegrityChecks = !File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\no-save-checking");
            doAutoClickPrevention = !File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\no-cps-checking");
            doCandyRain = !File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\no-candy-rain");

            InitializeComponent();

            _ = flowDocumentCandiesPerSecond.Blocks.Remove(paragraphEndGamePerSecond);

            LoadSaveData();
            ReloadShop();
            UpdateOverflowBanner();
            SeasonThemeCheck();

            timerPerSecond.Elapsed += TimerPerSecond_Elapsed;
            timerPerSecond.Start();

            timerCandyPerSecond.Elapsed += TimerCandyPerSecond_Elapsed;
            timerCandyPerSecond.Start();

            timerCandyRain.Elapsed += TimerCandyRain_Elapsed;
            timerCandyRain.Start();

            timerAutoSave.Elapsed += TimerAutoSave_Elapsed;
            timerAutoSave.Start();
        }

        private void LoadSaveData()
        {
            if (File.Exists(savePath))
            {
                try
                {
                    byte[] allBytes = File.ReadAllBytes(savePath);
                    byte[] header = allBytes[..8];
                    if (!header.SequenceEqual(saveHeader) && doSaveIntegrityChecks)
                    {
                        throw new ApplicationException("Invalid save file");
                    }
                    byte[] saveBytes = allBytes[8..];
                    byte[] hashBytes = saveBytes[^16..];
                    using MD5 md5 = MD5.Create();
                    if (!md5.ComputeHash(saveBytes[..^16]).SequenceEqual(hashBytes) && doSaveIntegrityChecks)
                    {
                        _ = MessageBox.Show("Modified save file detected", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Environment.Exit(1);
                    }

                    CandyScore = BitConverter.ToUInt64(saveBytes, 0);
                    CandyPerClick = BitConverter.ToUInt64(saveBytes, 8);
                    CandyPerSecond = BitConverter.ToUInt64(saveBytes, 16);
                    CandyPSReincarnationMultiplier = BitConverter.ToUInt64(saveBytes, 24);
                    ReincarnateCounter = BitConverter.ToUInt64(saveBytes, 32);
                    OverflowCounter = BitConverter.ToUInt64(saveBytes, 40);

                    UpdateRainDuration();

                    ShopPurchasedCount = new ulong[Shop.CandyShop.Length];
                    for (int i = 0; i < (saveBytes.Length - 64) / 8 && i < ShopPurchasedCount.Length; i++)
                    {
                        ShopPurchasedCount[i] = BitConverter.ToUInt64(saveBytes, (i * 8) + 48);
                    }
                }
                catch
                {
                    _ = MessageBox.Show("Save file is corrupt", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(1);
                }
            }
            else
            {
                CandyScore = 0;
                CandyPerClick = 1;
                CandyPerSecond = 0;
                CandyPSReincarnationMultiplier = 1;
                ReincarnateCounter = 0;
                OverflowCounter = 0;
                ShopPurchasedCount = new ulong[Shop.CandyShop.Length];
                timerCandyRain.Interval = 10000;

                OpenHelpPopUp();
            }
        }

        private void SaveData()
        {
            List<byte> candyScoreBytes = new();
            candyScoreBytes.AddRange(BitConverter.GetBytes(CandyScore));
            candyScoreBytes.AddRange(BitConverter.GetBytes(CandyPerClick));
            candyScoreBytes.AddRange(BitConverter.GetBytes(CandyPerSecond));
            candyScoreBytes.AddRange(BitConverter.GetBytes(CandyPSReincarnationMultiplier));
            candyScoreBytes.AddRange(BitConverter.GetBytes(ReincarnateCounter));
            candyScoreBytes.AddRange(BitConverter.GetBytes(OverflowCounter));

            List<byte> purchasedItemsBytes = new();
            foreach (ulong count in ShopPurchasedCount)
            {
                purchasedItemsBytes.AddRange(BitConverter.GetBytes(count));
            }

            byte[] saveBytes = candyScoreBytes.Concat(purchasedItemsBytes).ToArray();
            using MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(saveBytes);

            byte[] combinedBytes = saveHeader.Concat(saveBytes.Concat(hashBytes)).ToArray();
            File.WriteAllBytes(savePath, combinedBytes);
        }

        private static ulong CalculateInflatedCost(ulong originalPrice, ulong purchasedCount)
        {
            return (ulong)(originalPrice + (purchasedCount * originalPrice * 0.25));
        }

        private void ReloadShop()
        {
            stackPanelShop.Children.Clear();
            for (int i = 0; i < Shop.CandyShop.Length; i++)
            {
                Shop.ShopItem item = Shop.CandyShop[i];
                ShopControl newControl = new()
                {
                    ShopItemName = item.Name,
                    ShopItemPrice = CalculateInflatedCost(item.Price, ShopPurchasedCount[i]),
                    ToolTip = $"+{item.AdditionalPerClick} per click, +{item.AdditionalPerSecond} per second, {ShopPurchasedCount[i]} already purchased"
                };
                newControl.MouseDown += ShopItem_MouseDown;
                _ = stackPanelShop.Children.Add(newControl);
            }
            UpdateShopPriceColours();
        }

        private void UpdateShopPriceColours()
        {
            foreach (ShopControl control in stackPanelShop.Children)
            {
                control.PriceForeground = control.ShopItemPrice <= CandyScore ? textBlockScore.Foreground : Brushes.Gray;
            }
        }

        private void GiveCandy(ulong amount)
        {
            if (amount == 0)
            {
                return;
            }
            ulong oldCandyScore = CandyScore;
            CandyScore += amount;
            if (oldCandyScore > CandyScore)
            {
                OverflowCounter++;
                UpdateOverflowBanner();
            }
            UpdateShopPriceColours();
        }

        private async void SpawnRareCandy()
        {
            Image rareCandy = new()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Images/candy-special.png")),
                Height = 45,
                Width = 45,
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.Both
            };

            double xCoord = rng.Next((int)canvasCandyRain.ActualWidth - (int)rareCandy.Width);
            Border alert = new()
            {
                Child = new TextBlock()
                {
                    Text = "!",
                    FontFamily = textBlockScore.FontFamily,
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    Foreground = (Brush)new BrushConverter().ConvertFromString("#ebba0c"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
                Height = 30,
                Width = 30,
                BorderThickness = new Thickness(0),
                Background = Brushes.Blue,
                CornerRadius = new CornerRadius(30)
            };
            _ = canvasRareCandy.Children.Add(alert);
            Canvas.SetLeft(alert, xCoord);
            Canvas.SetTop(alert, 0);
            for (int i = 0; i < 5; i++)
            {
                alert.Visibility = Visibility.Visible;
                await Task.Delay(100);
                alert.Visibility = Visibility.Hidden;
                await Task.Delay(100);
            }
            canvasRareCandy.Children.Remove(alert);

            rareCandy.MouseDown += RareCandy_MouseDown;
            _ = canvasRareCandy.Children.Add(rareCandy);
            Canvas.SetLeft(rareCandy, xCoord);
            Canvas.SetTop(rareCandy, -rareCandy.Height);

            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(1.5))
            };

            DoubleAnimation moveDown = new()
            {
                From = -rareCandy.Height,
                To = canvasCandyRain.ActualHeight,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(moveDown, rareCandy);
            Storyboard.SetTargetProperty(moveDown, new PropertyPath(Canvas.TopProperty));
            sb.Children.Add(moveDown);

            sb.Completed += RareCandy_Completed;

            sb.Begin();
        }

        private void UpdateRainDuration()
        {
            timerCandyRain.Interval = CandyPerSecond / 10d <= 1 ? 10000 : (10000d / (CandyPerSecond / 10d)) + 1d;
        }

        private void UpdateSpecialProgress(bool doAnimation)
        {
            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(doAnimation ? 0.25 : 0))
            };

            DoubleAnimation changeWidth = new()
            {
                From = rectangleBonusProgress.Width,
                To = clicksTowardSpecial / 1000d * rectangleBonusBackground.ActualWidth,
                Duration = sb.Duration.TimeSpan
            };
            Storyboard.SetTarget(changeWidth, rectangleBonusProgress);
            Storyboard.SetTargetProperty(changeWidth, new PropertyPath(WidthProperty));
            sb.Children.Add(changeWidth);

            sb.Begin();
        }

        private async void InitiateSpecial()
        {
            isSpecialActive = true;
            ImageSource regularSource = new BitmapImage(new Uri("pack://application:,,,/Images/candy.png"));
            if (currentEasterEggState == EasterEggState.Normal)
            {
                imageCandy.Source = new BitmapImage(new Uri("pack://application:,,,/Images/candy-special.png"));
            }
            textBlockSpecial.Visibility = Visibility.Visible;
            rectangleBonusProgress.Fill = textBlockSpecial.Foreground;

            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(5))
            };
            DoubleAnimation changeWidth = new()
            {
                From = rectangleBonusProgress.ActualWidth,
                To = 0,
                Duration = sb.Duration.TimeSpan
            };
            Storyboard.SetTarget(changeWidth, rectangleBonusProgress);
            Storyboard.SetTargetProperty(changeWidth, new PropertyPath(WidthProperty));
            sb.Children.Add(changeWidth);
            sb.Begin();

            await Task.Delay(5000);

            rectangleBonusProgress.Fill = textBlockScore.Foreground;
            textBlockSpecial.Visibility = Visibility.Hidden;
            if (currentEasterEggState == EasterEggState.Normal)
            {
                imageCandy.Source = regularSource;
                SeasonThemeCheck();
            }
            clicksTowardSpecial = 0;
            isSpecialActive = false;
        }

        private void FadeBackgroundColor(Color targetColor)
        {
            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            ColorAnimation fade = new()
            {
                From = ((SolidColorBrush)Background).Color,
                To = targetColor,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(fade, windowCandyClicker);
            Storyboard.SetTargetProperty(fade, new PropertyPath("(Window.Background).(SolidColorBrush.Color)"));
            sb.Children.Add(fade);

            sb.Begin();
        }

        private void EndGameVisualUpdate()
        {
            isEndGameVisualActive = true;
            _ = flowDocumentCandiesPerSecond.Blocks.Remove(paragraphCandiesPerSecond);
            flowDocumentCandiesPerSecond.Blocks.Add(paragraphEndGamePerSecond);
            Icon = new BitmapImage(new Uri("pack://application:,,,/Icons/candy_special_dzI_icon.ico"));
            FadeBackgroundColor(new Color() { R = 0xE5, G = 0xF1, B = 0x9E, A = 0xFF });
        }

        private void UndoEndGameVisualUpdate()
        {
            isEndGameVisualActive = false;
            _ = flowDocumentCandiesPerSecond.Blocks.Remove(paragraphEndGamePerSecond);
            flowDocumentCandiesPerSecond.Blocks.Add(paragraphCandiesPerSecond);
            Icon = new BitmapImage(new Uri("pack://application:,,,/Icons/candy_xnp_icon.ico"));
            FadeBackgroundColor(new Color() { R = 0x86, G = 0xF1, B = 0xE6, A = 0xFF });
            SeasonThemeCheck();
        }

        private void SeasonThemeCheck()
        {
            DateTime now = DateTime.Now;
            // Christmas
            if (now.Month == 12 || (now.Month == 1 && now.Day <= 6))
            {
                imageCandy.Source = new BitmapImage(new Uri("pack://application:,,,/Images/candy-season-christmas.png"));
                FadeBackgroundColor(new Color() { R = 0x64, G = 0xF5, B = 0x5F, A = 0xFF });
            }
            // Halloween
            else if (now.Month == 10)
            {
                imageCandy.Source = new BitmapImage(new Uri("pack://application:,,,/Images/candy-season-halloween.png"));
                Application.Current.Resources["MainForeground"] = Brushes.OrangeRed;
                FadeBackgroundColor(new Color() { R = 0x26, G = 0x06, B = 0x61, A = 0xFF });
            }
        }

        private void OpenHelpPopUp()
        {
            timerPerSecond.Stop();
            timerCandyPerSecond.Stop();
            gridHelp.Visibility = Visibility.Visible;

            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            DoubleAnimation fadeIn = new()
            {
                From = 0,
                To = 1,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(fadeIn, gridHelp);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));
            sb.Children.Add(fadeIn);

            sb.Begin();
        }

        private void CloseHelpPopUp()
        {
            timerPerSecond.Start();
            timerCandyPerSecond.Start();
            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            DoubleAnimation fadeOut = new()
            {
                From = 1,
                To = 0,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(fadeOut, gridHelp);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(OpacityProperty));
            sb.Children.Add(fadeOut);

            sb.Completed += HelpFadeOut_Completed;

            sb.Begin();
        }

        private void OpenCheatPopUp()
        {
            timerPerSecond.Stop();
            timerCandyPerSecond.Stop();

            textBoxCheatCandies.Text = CandyScore.ToString();
            textBoxCheatPerClick.Text = CandyPerClick.ToString();
            textBoxCheatPerSecond.Text = CandyPerSecond.ToString();
            textBoxCheatPSMultiplier.Text = CandyPSReincarnationMultiplier.ToString();
            textBoxCheatReincarnationCount.Text = ReincarnateCounter.ToString();
            textBoxCheatOverflowCount.Text = OverflowCounter.ToString();
            textBoxCheatSpecialProgress.Text = clicksTowardSpecial.ToString();
            checkBoxCheatTaskbarHide.IsChecked = !ShowInTaskbar;
            checkBoxCheatDisableSaveChecks.IsChecked = !doSaveIntegrityChecks;
            checkBoxCheatDisableAutoclickerCheck.IsChecked = !doAutoClickPrevention;
            checkBoxCheatStopCandyRain.IsChecked = !doCandyRain;

            gridCheat.Visibility = Visibility.Visible;

            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            DoubleAnimation fadeIn = new()
            {
                From = 0,
                To = 1,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(fadeIn, gridCheat);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));
            sb.Children.Add(fadeIn);

            sb.Begin();
        }

        private void CloseCheatPopUp()
        {
            timerPerSecond.Start();
            timerCandyPerSecond.Start();
            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            DoubleAnimation fadeOut = new()
            {
                From = 1,
                To = 0,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(fadeOut, gridCheat);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(OpacityProperty));
            sb.Children.Add(fadeOut);

            sb.Completed += CheatFadeOut_Completed;

            sb.Begin();
        }

        private ulong CalculateReincarnationCost()
        {
            return 100000000 * (CandyPSReincarnationMultiplier + 1) * (ReincarnateCounter + 1);
        }

        private ulong CalculateReincarnationResult()
        {
            ulong result = CandyScore / 100000000 / (ReincarnateCounter + 1);
            return result >= CandyPSReincarnationMultiplier ? result : ulong.MaxValue;
        }

        private void OpenReincarnationPopUp()
        {
            timerPerSecond.Stop();
            timerCandyPerSecond.Stop();
            ulong reincarnationCost = CalculateReincarnationCost();
            textBlockReincarnateDescription.Text = $"Reincarnation allows you to restart the game with a permanent multiplier added to your candies per second. For every {100000000 * (ReincarnateCounter + 1):N0} candies you currently have, you will get an additional multiplier on your candies per second after reincarnation. Your current candies, candies per second, candies per click, shop prices, and previous reincarnations will all be reset. You must have at least {reincarnationCost:N0} candies to reincarnate.";
            if (CandyScore >= reincarnationCost)
            {
                if (CandyPSReincarnationMultiplier == ulong.MaxValue)
                {
                    buttonAgreeToReincarnate.IsEnabled = false;
                    buttonAgreeToReincarnate.Content = "Max Limit Reached";
                    buttonAgreeToReincarnate.Foreground = Brushes.Gray;
                }
                else
                {
                    buttonAgreeToReincarnate.IsEnabled = true;
                    buttonAgreeToReincarnate.Content = "Reincarnate";
                    buttonAgreeToReincarnate.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFF9F9F9");
                    textBlockReincarnateDescription.Text += $" Reincarnating now will give you a {CalculateReincarnationResult():N0}x candy per second multiplier.";
                }
            }
            else
            {
                buttonAgreeToReincarnate.IsEnabled = false;
                buttonAgreeToReincarnate.Content = "Not Enough Candy";
                buttonAgreeToReincarnate.Foreground = Brushes.Gray;
            }
            gridReincarnate.Visibility = Visibility.Visible;

            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            DoubleAnimation fadeIn = new()
            {
                From = 0,
                To = 1,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(fadeIn, gridReincarnate);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));
            sb.Children.Add(fadeIn);

            sb.Begin();
        }

        private void CloseReincarnationPopUp()
        {
            timerPerSecond.Start();
            timerCandyPerSecond.Start();
            Storyboard sb = new()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            DoubleAnimation fadeOut = new()
            {
                From = 1,
                To = 0,
                Duration = sb.Duration
            };
            Storyboard.SetTarget(fadeOut, gridReincarnate);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(OpacityProperty));
            sb.Children.Add(fadeOut);

            sb.Completed += ReincarnationFadeOut_Completed;

            sb.Begin();
        }

        private void UpdateOverflowBanner()
        {
            if (OverflowCounter < 1000)
            {
                // Check if the target number of stars will actually fit on-screen
                // If not, replace it with a numeric value instead
                string targetNewString = new('★', (int)OverflowCounter);
                Typeface typeface = new(textBlockOverflow.FontFamily, textBlockOverflow.FontStyle, textBlockOverflow.FontWeight, textBlockOverflow.FontStretch);
                FormattedText format = new(targetNewString, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, textBlockOverflow.FontSize,
                    textBlockOverflow.Foreground, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                textBlockOverflow.Text = format.Width > gridCandyClicker.ActualWidth - (textBlockOverflow.Margin.Left + textBlockOverflow.Margin.Right)
                    ? $"★ x {OverflowCounter}"
                    : targetNewString;
            }
            else
            {
                textBlockOverflow.Text = $"★ x {OverflowCounter}";
            }
        }

        private void ShopItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                int selectedIndex = stackPanelShop.Children.IndexOf((UIElement)sender);
                Shop.ShopItem purchasedItem = Shop.CandyShop[selectedIndex];

                ulong adjustedPrice = CalculateInflatedCost(purchasedItem.Price, ShopPurchasedCount[selectedIndex]);
                if (adjustedPrice <= CandyScore)
                {
                    CandyScore -= adjustedPrice;
                    CandyPerClick += purchasedItem.AdditionalPerClick;
                    CandyPerSecond += purchasedItem.AdditionalPerSecond;
                    ShopPurchasedCount[selectedIndex]++;
                    UpdateRainDuration();
                    ((ShopControl)stackPanelShop.Children[selectedIndex]).ShopItemPrice += (ulong)(purchasedItem.Price * 0.25);
                    ((ShopControl)stackPanelShop.Children[selectedIndex]).ToolTip =
                        $"+{purchasedItem.AdditionalPerClick} per click, +{purchasedItem.AdditionalPerSecond} per second, {ShopPurchasedCount[selectedIndex]} already purchased";
                    UpdateShopPriceColours();
                }
                else
                {
                    Storyboard sb = new()
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(0.25))
                    };

                    DoubleAnimation pulseScore = new()
                    {
                        From = 36,
                        To = 42,
                        Duration = sb.Duration.TimeSpan / 2
                    };
                    Storyboard.SetTarget(pulseScore, textBlockScore);
                    Storyboard.SetTargetProperty(pulseScore, new PropertyPath(FontSizeProperty));
                    sb.Children.Add(pulseScore);

                    DoubleAnimation resetScore = new()
                    {
                        From = 42,
                        To = 36,
                        Duration = sb.Duration.TimeSpan / 2,
                        BeginTime = sb.Duration.TimeSpan / 2
                    };
                    Storyboard.SetTarget(resetScore, textBlockScore);
                    Storyboard.SetTargetProperty(resetScore, new PropertyPath(FontSizeProperty));
                    sb.Children.Add(resetScore);

                    sb.Begin();
                }
            }
        }

        private void ImageCandy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && (DateTime.Now > lastClickTime + new TimeSpan((long)(TimeSpan.TicksPerSecond * 0.04)) || !doAutoClickPrevention))
            {
                GiveCandy(isSpecialActive ? CandyPerClick * 10 : CandyPerClick);
                if (!isSpecialActive)
                {
                    clicksTowardSpecial++;
                    if (clicksTowardSpecial == 1000)
                    {
                        InitiateSpecial();
                    }
                    else
                    {
                        UpdateSpecialProgress(true);
                    }
                }

                lastClickTime = DateTime.Now;
                clicksThisSecond++;

                if (!isSpecialActive || currentEasterEggState != EasterEggState.Normal)
                {
                    imageCandy.Source = easterEggImages[currentEasterEggState][rng.Next(easterEggImages[currentEasterEggState].Length)];
                    if (currentEasterEggState == EasterEggState.Normal)
                    {
                        SeasonThemeCheck();
                    }
                }

                if (rng.Next(10) == 0)
                {
                    Storyboard sb = new()
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(0.25))
                    };

                    DoubleAnimation rotateCandy = new()
                    {
                        From = 0,
                        To = 360,
                        Duration = sb.Duration
                    };
                    Storyboard.SetTarget(rotateCandy, imageCandy);
                    Storyboard.SetTargetProperty(rotateCandy, new PropertyPath("RenderTransform.(TransformGroup.Children)[0].Angle"));
                    sb.Children.Add(rotateCandy);

                    sb.Begin();
                }
                else
                {
                    Storyboard sb = new()
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(0.25))
                    };

                    DoubleAnimation shrinkX = new()
                    {
                        From = 1,
                        To = 0.9,
                        Duration = sb.Duration.TimeSpan / 2
                    };
                    Storyboard.SetTarget(shrinkX, imageCandy);
                    Storyboard.SetTargetProperty(shrinkX, new PropertyPath("RenderTransform.(TransformGroup.Children)[1].ScaleX"));
                    sb.Children.Add(shrinkX);

                    DoubleAnimation growX = new()
                    {
                        From = 0.9,
                        To = 1,
                        Duration = sb.Duration.TimeSpan / 2,
                        BeginTime = sb.Duration.TimeSpan / 2
                    };
                    Storyboard.SetTarget(growX, imageCandy);
                    Storyboard.SetTargetProperty(growX, new PropertyPath("RenderTransform.(TransformGroup.Children)[1].ScaleX"));
                    sb.Children.Add(growX);

                    sb.Begin();
                }
            }
        }

        private void TimerCandyPerSecond_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ulong amount = Dispatcher.Invoke(() => CandyPSReincarnationMultiplier <= 1
                ? CandyPerSecond
                : CandyPerSecond <= CandyPerSecond * CandyPSReincarnationMultiplier
                    ? CandyPerSecond * CandyPSReincarnationMultiplier
                    : ulong.MaxValue);
            uint executionsPerSecond = 1000 / (uint)timerCandyPerSecond.Interval;
            if (amount == 0)
            {
                return;
            }
            ulong perTick = amount / executionsPerSecond;
            ulong remainder = amount % executionsPerSecond;
            ulong toGive = perTick;
            if (remainder != 0 && candyPerSecondCycleCount % (executionsPerSecond / remainder) == 0)
            {
                toGive++;
            }
            Dispatcher.Invoke(() => GiveCandy(toGive));
            candyPerSecondCycleCount++;
            if (candyPerSecondCycleCount == executionsPerSecond)
            {
                candyPerSecondCycleCount = 0;
            }
        }

        private void TimerPerSecond_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                _ = gridCandyClicker.Focus();
                if (!isEndGameVisualActive && CandyPSReincarnationMultiplier == ulong.MaxValue)
                {
                    EndGameVisualUpdate();
                }
                else if (isEndGameVisualActive && CandyPSReincarnationMultiplier != ulong.MaxValue)
                {
                    UndoEndGameVisualUpdate();
                }
                richTextBoxCandiesPerSecond.Visibility = isEndGameVisualActive && CandyPerSecond == 0 ? Visibility.Collapsed : Visibility.Visible;
                if ((DateTime.Now - lastClickTime).TotalSeconds >= 5)
                {
                    previousClicksPerSecond.Clear();
                    textBlockClicksPerSecond.Text = "0.0 CPS";
                }
                else
                {
                    previousClicksPerSecond.Add(clicksThisSecond);
                    clicksThisSecond = 0;
                    // First value is skipped as it will most likely be lower than expected
                    if (previousClicksPerSecond.Count >= 2)
                    {
                        textBlockClicksPerSecond.Text = $"{(double)previousClicksPerSecond.Skip(1).Sum() / (previousClicksPerSecond.Count - 1):0.0} CPS";
                    }
                }
                if (!isEndGameVisualActive)
                {
                    ulong perSecond = CandyPSReincarnationMultiplier <= 1 ? CandyPerSecond : CandyPerSecond * CandyPSReincarnationMultiplier;
                    double clicksPerSecond = previousClicksPerSecond.Count >= 2 ? (double)previousClicksPerSecond.Skip(1).Sum() / (previousClicksPerSecond.Count - 1) : 0;
                    ulong perClick = isSpecialActive ? CandyPerClick * 10 : CandyPerClick;
                    runCandiesPerSecond.Text = $"{Math.Round(perClick * clicksPerSecond) + perSecond:N0}";
                }
                if (rng.Next(60) == 0)
                {
                    SpawnRareCandy();
                }
            });
        }

        private void RareCandy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Image completed = (Image)sender;
                canvasCandyRain.Children.Remove(completed);
                completed.BeginAnimation(Canvas.TopProperty, null);
                completed.Source = null;
                GiveCandy(CandyPerClick * 500);
            }
        }

        private void TimerCandyRain_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (doCandyRain)
            {
                Dispatcher.Invoke(() =>
                {
                    double speedModifier = rng.Next(2, 4) + rng.NextDouble();
                    Image newRain = new()
                    {
                        Source = imageCandy.Source,
                        Width = 43 - (speedModifier * speedModifier),
                        Height = 43 - (speedModifier * speedModifier),
                        Stretch = Stretch.Uniform,
                        StretchDirection = StretchDirection.Both,
                        Opacity = 0.5
                    };
                    _ = canvasCandyRain.Children.Add(newRain);
                    double xCoord = rng.Next((int)canvasCandyRain.ActualWidth - (int)newRain.Width);
                    Canvas.SetLeft(newRain, xCoord);
                    Canvas.SetTop(newRain, -newRain.Height);

                    Storyboard sb = new()
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(speedModifier))
                    };

                    DoubleAnimation moveDown = new()
                    {
                        From = -newRain.Height,
                        To = canvasCandyRain.ActualHeight,
                        Duration = sb.Duration
                    };
                    Storyboard.SetTarget(moveDown, newRain);
                    Storyboard.SetTargetProperty(moveDown, new PropertyPath(Canvas.TopProperty));
                    sb.Children.Add(moveDown);

                    sb.Completed += Rain_Completed;

                    sb.Begin();
                });
            }
        }

        private void Rain_Completed(object sender, EventArgs e)
        {
            Image completed = (Image)Storyboard.GetTarget(((ClockGroup)sender).Children[0].Timeline);
            completed.BeginAnimation(Canvas.TopProperty, null);
            completed.Source = null;
            Dispatcher.Invoke(() => canvasCandyRain.Children.Remove(completed));
        }

        private void RareCandy_Completed(object sender, EventArgs e)
        {
            Image completed = (Image)Storyboard.GetTarget(((ClockGroup)sender).Children[0].Timeline);
            completed.BeginAnimation(Canvas.TopProperty, null);
            completed.Source = null;
            Dispatcher.Invoke(() => canvasRareCandy.Children.Remove(completed));
        }

        private void TimerAutoSave_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => SaveData());
        }

        private void WindowCandyClicker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveData();
        }

        private void WindowCandyClicker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateOverflowBanner();
            if (!isSpecialActive)
            {
                UpdateSpecialProgress(false);
            }
        }

        private void ButtonReincarnate_Click(object sender, RoutedEventArgs e)
        {
            OpenReincarnationPopUp();
        }

        private void ButtonAgreeToReincarnate_Click(object sender, RoutedEventArgs e)
        {
            if (CandyScore >= CalculateReincarnationCost())
            {
                CandyPSReincarnationMultiplier = CalculateReincarnationResult();
                ReincarnateCounter++;
                CandyScore = 0;
                CandyPerSecond = 0;
                UpdateRainDuration();
                CandyPerClick = 1;
                clicksTowardSpecial = 0;
                UpdateSpecialProgress(false);
                OverflowCounter = 0;
                UpdateOverflowBanner();
                ShopPurchasedCount = new ulong[Shop.CandyShop.Length];
                ReloadShop();

                CloseReincarnationPopUp();
            }
        }

        private void TextBlockCancelReincarnate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                CloseReincarnationPopUp();
            }
        }

        private void ReincarnationFadeOut_Completed(object sender, EventArgs e)
        {
            gridReincarnate.Visibility = Visibility.Collapsed;
        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            OpenHelpPopUp();
        }

        private void ButtonCloseHelp_Click(object sender, RoutedEventArgs e)
        {
            CloseHelpPopUp();
        }

        private void HelpFadeOut_Completed(object sender, EventArgs e)
        {
            gridHelp.Visibility = Visibility.Collapsed;
        }

        private void CheatFadeOut_Completed(object sender, EventArgs e)
        {
            gridCheat.Visibility = Visibility.Collapsed;
        }

        private void GridCandyClicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == cheatMenuKeys[cheatMenuKeyProgression])
            {
                cheatMenuKeyProgression++;
                if (cheatMenuKeyProgression == cheatMenuKeys.Length)
                {
                    cheatMenuKeyProgression = 0;
                    OpenCheatPopUp();
                }
            }
            else
            {
                cheatMenuKeyProgression = 0;
            }

            if (e.Key == spawnRareCandyKeys[spawnRareCandyKeyProgression])
            {
                spawnRareCandyKeyProgression++;
                if (spawnRareCandyKeyProgression == spawnRareCandyKeys.Length)
                {
                    spawnRareCandyKeyProgression = 0;
                    SpawnRareCandy();
                }
            }
            else
            {
                spawnRareCandyKeyProgression = 0;
            }

            foreach (EasterEggState state in Enum.GetValues(typeof(EasterEggState)))
            {
                if (e.Key == easterEggKeys[state][easterEggKeyProgression[state]])
                {
                    easterEggKeyProgression[state]++;
                    if (easterEggKeyProgression[state] == easterEggKeys[state].Length)
                    {
                        easterEggKeyProgression[state] = 0;
                        currentEasterEggState = state;

                        Color targetColor = state == EasterEggState.Normal && isEndGameVisualActive
                            ? (new() { R = 0xE5, G = 0xF1, B = 0x9E, A = 0xFF })
                            : easterEggBackgroundColors[state];

                        imageCandy.Source = state == EasterEggState.Normal && isSpecialActive
                            ? new BitmapImage(new Uri("pack://application:,,,/Images/candy-special.png"))
                            : easterEggImages[state][rng.Next(easterEggImages[currentEasterEggState].Length)];
                        FadeBackgroundColor(targetColor);
                        if (state == EasterEggState.Normal)
                        {
                            SeasonThemeCheck();
                        }
                    }
                }
                else
                {
                    easterEggKeyProgression[state] = 0;
                }
            }
        }

        private void ButtonCommitCheat_Click(object sender, RoutedEventArgs e)
        {
            string failed = "";
            if (ulong.TryParse(textBoxCheatCandies.Text, out _))
            {
                CandyScore = ulong.Parse(textBoxCheatCandies.Text);
            }
            else
            {
                failed += "Candy Score, ";
            }
            if (ulong.TryParse(textBoxCheatPerClick.Text, out _))
            {
                CandyPerClick = ulong.Parse(textBoxCheatPerClick.Text);
            }
            else
            {
                failed += "Candy Per Click, ";
            }
            if (ulong.TryParse(textBoxCheatPerSecond.Text, out _))
            {
                CandyPerSecond = ulong.Parse(textBoxCheatPerSecond.Text);
                UpdateRainDuration();
            }
            else
            {
                failed += "Candy Per Second, ";
            }
            if (ulong.TryParse(textBoxCheatPSMultiplier.Text, out _))
            {
                CandyPSReincarnationMultiplier = ulong.Parse(textBoxCheatPSMultiplier.Text);
            }
            else
            {
                failed += "Reincarnation Multiplier, ";
            }
            if (ulong.TryParse(textBoxCheatReincarnationCount.Text, out _))
            {
                ReincarnateCounter = ulong.Parse(textBoxCheatReincarnationCount.Text);
            }
            else
            {
                failed += "Reincarnation Counter, ";
            }
            if (ulong.TryParse(textBoxCheatOverflowCount.Text, out _))
            {
                OverflowCounter = ulong.Parse(textBoxCheatOverflowCount.Text);
                UpdateOverflowBanner();
            }
            else
            {
                failed += "Overflow Counter, ";
            }
            if (uint.TryParse(textBoxCheatSpecialProgress.Text, out _) && uint.Parse(textBoxCheatSpecialProgress.Text) <= 999)
            {
                clicksTowardSpecial = uint.Parse(textBoxCheatSpecialProgress.Text);
                UpdateSpecialProgress(true);
            }
            else
            {
                failed += "Candy Meter Progress, ";
            }
            if (checkBoxCheatTaskbarHide.IsChecked.Value)
            {
                ShowInTaskbar = false;
                File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\no-taskbar").Close();
            }
            else
            {
                ShowInTaskbar = true;
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\no-taskbar");
            }
            if (checkBoxCheatDisableSaveChecks.IsChecked.Value)
            {
                doSaveIntegrityChecks = false;
                File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\no-save-checking").Close();
            }
            else
            {
                doSaveIntegrityChecks = true;
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\no-save-checking");
            }
            if (checkBoxCheatDisableAutoclickerCheck.IsChecked.Value)
            {
                doAutoClickPrevention = false;
                File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\no-cps-checking").Close();
            }
            else
            {
                doAutoClickPrevention = true;
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\no-cps-checking");
            }
            if (checkBoxCheatStopCandyRain.IsChecked.Value)
            {
                doCandyRain = false;
                File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\no-candy-rain").Close();
            }
            else
            {
                doCandyRain = true;
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\no-candy-rain");
            }
            CloseCheatPopUp();
            if (failed != "")
            {
                _ = MessageBox.Show($"The following value(s) failed to parse: {failed}these values have been ignored.", "Parsing Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCloseCheat_Click(object sender, RoutedEventArgs e)
        {
            CloseCheatPopUp();
        }
    }
}
