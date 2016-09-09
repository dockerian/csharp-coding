using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Coding.AppWpf.Controls
{
    public class ImageToggleButton : ToggleButton
    {
        [Category("Common Properties")]
        public ImageSource ImageSourceActive
        {
            get { return (ImageSource)GetValue(ImageSourceActiveProperty); }
            set { this.SetValue(ImageSourceActiveProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceActiveProperty =
            DependencyProperty.Register("ImageSourceActive", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public ImageSource ImageSourceActiveHover
        {
            get { return (ImageSource)GetValue(ImageSourceActiveHoverProperty); }
            set { this.SetValue(ImageSourceActiveHoverProperty, value); }
        }

        [Category("Common Properties")]
        public ImageSource ImageSourceActiveDisabled
        {
            get { return (ImageSource)GetValue(ImageSourceActiveDisabledProperty); }
            set { this.SetValue(ImageSourceActiveDisabledProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceActiveDisabledProperty =
            DependencyProperty.Register("ImageSourceActiveDisabled", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ImageSourceActiveHoverProperty =
            DependencyProperty.Register("ImageSourceActiveHover", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public ImageSource ImageSourceActivePressed
        {
            get { return (ImageSource)GetValue(ImageSourceActivePressedProperty); }
            set { this.SetValue(ImageSourceActivePressedProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceActivePressedProperty =
            DependencyProperty.Register("ImageSourceActivePressed", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public int ImageSourceHeight
        {
            get { return (int)GetValue(ImageSourceHeightProperty); }
            set { this.SetValue(ImageSourceHeightProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceHeightProperty =
            DependencyProperty.Register("ImageSourceHeight", typeof(int), typeof(ImageToggleButton), new UIPropertyMetadata(50, OnImageSourceHeightChanged));

        protected static void OnImageSourceHeightChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (int)e.NewValue;
            var control = source as ImageToggleButton;

            if (control != null)
            {
                control.ImageSourceHeight = newValue;
            }
        }

        [Category("Common Properties")]
        public ImageSource ImageSourceIdle
        {
            get { return (ImageSource)GetValue(ImageSourceIdleProperty); }
            set { this.SetValue(ImageSourceIdleProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceIdleProperty =
            DependencyProperty.Register("ImageSourceIdle", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public ImageSource ImageSourceIdleDisabled
        {
            get { return (ImageSource)GetValue(ImageSourceIdleDisabledProperty); }
            set { this.SetValue(ImageSourceIdleDisabledProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceIdleDisabledProperty =
            DependencyProperty.Register("ImageSourceIdleDisabled", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public ImageSource ImageSourceIdleHover
        {
            get { return (ImageSource)GetValue(ImageSourceIdleHoverProperty); }
            set { this.SetValue(ImageSourceIdleHoverProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceIdleHoverProperty =
            DependencyProperty.Register("ImageSourceIdleHover", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public ImageSource ImageSourceIdlePressed
        {
            get { return (ImageSource)GetValue(ImageSourceIdlePressedProperty); }
            set { this.SetValue(ImageSourceIdlePressedProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceIdlePressedProperty =
            DependencyProperty.Register("ImageSourceIdlePressed", typeof(ImageSource), typeof(ImageToggleButton), new UIPropertyMetadata(null));

        [Category("Common Properties")]
        public int ImageSourceWidth
        {
            get { return (int)GetValue(ImageSourceWidthProperty); }
            set { this.SetValue(ImageSourceWidthProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceWidthProperty =
            DependencyProperty.Register("ImageSourceWidth", typeof(int), typeof(ImageToggleButton), new UIPropertyMetadata(50, OnImageSourceWidthChanged));

        protected static void OnImageSourceWidthChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (int)e.NewValue;
            var control = source as ImageToggleButton;

            if (control != null)
            {
                control.ImageSourceWidth = newValue;
            }
        }

    }
}
