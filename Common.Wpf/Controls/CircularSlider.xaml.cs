using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Common.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for CircularSlider.xaml
    /// </summary>
    public partial class CircularSlider : UserControl
    {
        #region Static

        static CircularSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularSlider), new FrameworkPropertyMetadata(typeof(CircularSlider)));
        }

        #region DependencyProperties

        // Using a DependencyProperty as the backing store for Angle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(CircularSlider), new UIPropertyMetadata(30.0, OnAngleUpdate));

        // Using a DependencyProperty as the backing store for MaximumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register("MaximumValue", typeof(double), typeof(CircularSlider), new UIPropertyMetadata(10.0, OnMaxValueUpdate));

        // Using a DependencyProperty as the backing store for MinimumAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumAngleProperty =
            DependencyProperty.Register("MinimumAngle", typeof(double), typeof(CircularSlider), new UIPropertyMetadata(30.0, OnMinAngleUpdate));

        // Using a DependencyProperty as the backing store for MaximumAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumAngleProperty =
            DependencyProperty.Register("MaximumAngle", typeof(double), typeof(CircularSlider), new UIPropertyMetadata(330.0, OnMaxAngleUpdate));

        // Using a DependencyProperty as the backing store for MinimumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register("MinimumValue", typeof(double), typeof(CircularSlider), new UIPropertyMetadata(0.0, OnMinValueUpdate));

        // Using a DependencyProperty as the backing store for RoundValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RoundValueProperty =
            DependencyProperty.Register("RoundValue", typeof(bool), typeof(CircularSlider), new UIPropertyMetadata(false));

        // Using a DependencyProperty as the backing store for ShowValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowValueProperty =
            DependencyProperty.Register("ShowValue", typeof(bool), typeof(CircularSlider), new UIPropertyMetadata(true));

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", 
                typeof(double), typeof(CircularSlider), 
                new UIPropertyMetadata(0.0, 
                    new PropertyChangedCallback(OnValueUpdate), 
                    new CoerceValueCallback(OnCoerceValue)));

        #endregion

        #region Static Functions

        private static object OnCoerceValue(DependencyObject obj, object value)
        {
            double current = (double)value;

            CircularSlider slider = obj as CircularSlider;

            if (obj != null)
            {
                if (current < slider.MinimumValue) current = slider.MinimumValue;
                if (current > slider.MaximumValue) current = slider.MaximumValue;
            }

            return current;
        }

        private static void OnAngleUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CircularSlider slider = obj as CircularSlider;

            if (slider != null)
            {
                if (slider.Angle > slider.MaximumAngle)
                {
                    slider.Angle = slider.MaximumAngle;
                }
                if (slider.Angle < slider.MinimumAngle)
                {
                    slider.Angle = slider.MinimumAngle;
                }
            }
        }

        private static void OnMaxAngleUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CircularSlider slider = obj as CircularSlider;

            if (slider != null)
            {
                if (slider.MaximumAngle < slider.MinimumAngle)
                {
                    slider.MaximumAngle = slider.MinimumAngle + 10;
                }
            }
        }

        private static void OnMinAngleUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CircularSlider slider = obj as CircularSlider;

            if (slider != null)
            {
                if (slider.MinimumAngle > slider.MaximumAngle)
                {
                    slider.MinimumAngle = slider.MaximumAngle - 10;
                }
            }
        }

        private static void OnMaxValueUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CircularSlider slider = obj as CircularSlider;

            if (slider != null)
            {
                slider.Update();
            }
        }

        private static void OnMinValueUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CircularSlider slider = obj as CircularSlider;

            if (slider != null)
            {
                slider.Update();
            }
        }

        private static void OnValueUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CircularSlider slider = obj as CircularSlider;

            if (slider != null)
            {
                if (slider.Value > slider.MaximumValue)
                {
                    slider.Value = slider.MaximumValue;
                }
                if (slider.Value < slider.MinimumValue)
                {
                    slider.Value = slider.MinimumValue;
                }
                slider.Update();
            }
        }

        #endregion

        #endregion

        public CircularSlider()
        {
            InitializeComponent();
        }

        #region Fields

        Point prevPoint;
        double prevValue;

        #endregion

        #region Functions

        private void CircularSlider_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ReleaseMouseCapture();
            prevPoint = new Point(0, 0);
        }

        private void CircularSlider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.CaptureMouse();
            prevPoint = Mouse.GetPosition(this);
        }

        private void CircularSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            prevPoint = new Point(0, 0);
        }

        private void CircularSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                Point currentLocation = Mouse.GetPosition(this);
                Point dialCenter = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
                if (prevPoint.X != 0 && prevPoint.Y != 0)
                {
                    // Calculate an angle 
                    double angle = (GetAngle(currentLocation.X, currentLocation.Y, dialCenter.X, dialCenter.Y)) - (GetAngle(prevPoint.X, prevPoint.Y, dialCenter.X, dialCenter.Y));

                    double degreesPerVolumeTick = (MaximumAngle - MinimumAngle) / (double)(MaximumValue - MinimumValue);
                    double value = angle / degreesPerVolumeTick;
                    if (value - prevValue < (MaximumValue / 2) && value - prevValue > -(MaximumValue / 2))
                    {
                        if (RoundValue)
                        {
                            int newValue = Convert.ToInt32(Value + value);
                            Value = newValue;
                        }
                        else
                        {
                            Value = Math.Round(Value + value, 1);
                        }
                        prevValue = value;
                    }
                    else
                    {
                        prevPoint = new Point(0, 0);
                    }
                }
                prevPoint = currentLocation;
            }
        }

        private void CircularSlider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Value += e.Delta / Mouse.MouseWheelDeltaForOneLine;
        }

        private double GetAngle(double px1, double py1, double px2, double py2)
        {
            // Calculate the angle 
            double angle = System.Math.Atan((py2 - py1) / (px2 - px1));

            // Convert to degrees 
            angle = angle * 180 / System.Math.PI;
            return angle;
        }

        private void Update()
        {
            double degreesPerVolumeTick = (MaximumAngle - MinimumAngle) / (double)(MaximumValue - MinimumValue);

            Angle = MinimumAngle + degreesPerVolumeTick * Value;
        }

        #endregion

        #region Properties

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public double MaximumAngle
        {
            get { return (double)GetValue(MaximumAngleProperty); }
            set { SetValue(MaximumAngleProperty, value); }
        }

        public double MaximumValue
        {
            get { return (double)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        public double MinimumAngle
        {
            get { return (double)GetValue(MinimumAngleProperty); }
            set { SetValue(MinimumAngleProperty, value); }
        }

        public double MinimumValue
        {
            get { return (double)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        public bool RoundValue
        {
            get { return (bool)GetValue(RoundValueProperty); }
            set { SetValue(RoundValueProperty, value); }
        }

        public bool ShowValue
        {
            get { return (bool)GetValue(ShowValueProperty); }
            set { SetValue(ShowValueProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.MouseWheel += new MouseWheelEventHandler(CircularSlider_MouseWheel);
            this.MouseMove += new MouseEventHandler(CircularSlider_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(CircularSlider_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CircularSlider_MouseLeftButtonDown);
            this.MouseLeave += new MouseEventHandler(CircularSlider_MouseLeave);

            Update();
        }

        #endregion

    }
}
