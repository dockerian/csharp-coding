﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coding.AppWpf.Controls
{
    public class ImageButton : Button
    {
        [Category("Common Properties")]
        public ImageSource ImageSourceActive
        {
            get { return (ImageSource)GetValue(ImageSourceActiveProperty); }
            set { this.SetValue(ImageSourceActiveProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceActiveProperty =
            DependencyProperty.Register("ImageSourceActive", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public ImageSource ImageSourceDisabled
        {
            get { return (ImageSource)GetValue(ImageSourceDisabledProperty); }
            set { this.SetValue(ImageSourceDisabledProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceDisabledProperty =
            DependencyProperty.Register("ImageSourceDisabled", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public ImageSource ImageSourceIdle
        {
            get { return (ImageSource)GetValue(ImageSourceIdleProperty); }
            set { this.SetValue(ImageSourceIdleProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceIdleProperty =
            DependencyProperty.Register("ImageSourceIdle", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public ImageSource ImageSourceHover
        {
            get { return (ImageSource)GetValue(ImageSourceHoverProperty); }
            set { this.SetValue(ImageSourceHoverProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceHoverProperty =
            DependencyProperty.Register("ImageSourceHover", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));


        [Category("Common Properties")]
        public ImageSource ImageSourcePressed
        {
            get { return (ImageSource)GetValue(ImageSourcePressedProperty); }
            set { this.SetValue(ImageSourcePressedProperty, value); }
        }

        public static readonly DependencyProperty ImageSourcePressedProperty =
            DependencyProperty.Register("ImageSourcePressed", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ImageButton), new UIPropertyMetadata(false));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { this.SetValue(IsActiveProperty, value); }
        }

    }
}
