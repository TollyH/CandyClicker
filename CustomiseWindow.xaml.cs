/*
 * Candy Clicker ver. 1.1.1
 * Copyright © 2021-2022  Ptolemy Hill
 */
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace CandyClicker
{
    /// <summary>
    /// Interaction logic for CustomiseWindow.xaml
    /// </summary>
    public partial class CustomiseWindow : Window
    {
        string currentImagePath;

        public CustomiseWindow(MainWindow parentWindow)
        {
            Owner = parentWindow;
            InitializeComponent();
            (Color startingBackground, Color startingForeground, currentImagePath) = ((MainWindow)Owner).GetCustomisations();
            sliderBackgroundRed.Value = startingBackground.R;
            sliderBackgroundGreen.Value = startingBackground.G;
            sliderBackgroundBlue.Value = startingBackground.B;
            sliderForegroundRed.Value = startingForeground.R;
            sliderForegroundGreen.Value = startingForeground.G;
            sliderForegroundBlue.Value = startingForeground.B;
            if (currentImagePath != null)
            {
                textBlockImagePath.Text = currentImagePath;
            }
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            Color newBackground = new()
            {
                A = 0xFF,
                R = (byte)sliderBackgroundRed.Value,
                G = (byte)sliderBackgroundGreen.Value,
                B = (byte)sliderBackgroundBlue.Value
            };
            Color newForeground = new()
            {
                A = 0xFF,
                R = (byte)sliderForegroundRed.Value,
                G = (byte)sliderForegroundGreen.Value,
                B = (byte)sliderForegroundBlue.Value
            };
            stackPanelPreview.Background = new SolidColorBrush(newBackground);
            textBlockPreview.Foreground = new SolidColorBrush(newForeground);
            if (currentImagePath != null)
            {
                imagePreview.Source = new BitmapImage(new Uri(currentImagePath));
            }
        }

        private void ButtonImageChange_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new()
            {
                Title = "Select Image",
                Filter = "Images|*.png;*.jpg;*.jpeg"
            };
            bool? success = fileDialog.ShowDialog();
            if (success.HasValue && success.Value && fileDialog.FileName != string.Empty)
            {
                currentImagePath = fileDialog.FileName;
                textBlockImagePath.Text = currentImagePath;
                UpdatePreview();
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Color newBackground = new()
            {
                A = 0xFF,
                R = (byte)sliderBackgroundRed.Value,
                G = (byte)sliderBackgroundGreen.Value,
                B = (byte)sliderBackgroundBlue.Value
            };
            Color newForeground = new()
            {
                A = 0xFF,
                R = (byte)sliderForegroundRed.Value,
                G = (byte)sliderForegroundGreen.Value,
                B = (byte)sliderForegroundBlue.Value
            };
            textBlockBackgroundRed.Text = $"Red ({newBackground.R})";
            textBlockBackgroundGreen.Text = $"Green ({newBackground.G})";
            textBlockBackgroundBlue.Text = $"Blue ({newBackground.B})";
            textBlockForegroundRed.Text = $"Red ({newForeground.R})";
            textBlockForegroundGreen.Text = $"Green ({newForeground.G})";
            textBlockForegroundBlue.Text = $"Blue ({newForeground.B})";
            UpdatePreview();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Owner).ResetCustomisations();
            Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Color newBackground = new()
            {
                A = 0xFF,
                R = (byte)sliderBackgroundRed.Value,
                G = (byte)sliderBackgroundGreen.Value,
                B = (byte)sliderBackgroundBlue.Value
            };
            Color newForeground = new()
            {
                A = 0xFF,
                R = (byte)sliderForegroundRed.Value,
                G = (byte)sliderForegroundGreen.Value,
                B = (byte)sliderForegroundBlue.Value
            };
            ((MainWindow)Owner).SetCustomisations(newBackground, newForeground, currentImagePath);
            Close();
        }

        private void ImagePreview_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
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
                Storyboard.SetTarget(rotateCandy, imagePreview);
                Storyboard.SetTargetProperty(rotateCandy, new PropertyPath("RenderTransform.(TransformGroup.Children)[0].Angle"));
                sb.Children.Add(rotateCandy);

                sb.Begin();
            }
            else if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
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
                Storyboard.SetTarget(shrinkX, imagePreview);
                Storyboard.SetTargetProperty(shrinkX, new PropertyPath("RenderTransform.(TransformGroup.Children)[1].ScaleX"));
                sb.Children.Add(shrinkX);

                DoubleAnimation growX = new()
                {
                    From = 0.9,
                    To = 1,
                    Duration = sb.Duration.TimeSpan / 2,
                    BeginTime = sb.Duration.TimeSpan / 2
                };
                Storyboard.SetTarget(growX, imagePreview);
                Storyboard.SetTargetProperty(growX, new PropertyPath("RenderTransform.(TransformGroup.Children)[1].ScaleX"));
                sb.Children.Add(growX);

                sb.Begin();
            }
        }
    }
}
