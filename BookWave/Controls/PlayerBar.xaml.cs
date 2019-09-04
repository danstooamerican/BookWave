﻿using BookWave.Desktop;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BookWave.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for PlayerBar.xaml
    /// </summary>
    public partial class PlayerBar : UserControl
    {
        #region Public Properties

        private double mLastVolume;

        public double LastVolume
        {
            get { return mLastVolume; }
            set { mLastVolume = value; }
        }

        #endregion

        public PlayerBar()
        {
            InitializeComponent();           
        }

        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue, UriKind.RelativeOrAbsolute));
        }

        private void BtnToggleVolume_Click(object sender, RoutedEventArgs e)
        {
            if (sldVolume.Value == 0)
            {
                sldVolume.Value = LastVolume;
            }
            else
            {
                LastVolume = sldVolume.Value;
                sldVolume.Value = 0;
            }
        }
    }
}
