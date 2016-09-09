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
using Common.Wpf.Controls;

namespace Coding.AppWpf.Controls
{
    /// <summary>
    /// Interaction logic for CircularVolumes.xaml
    /// </summary>
    public partial class CircularVolumes : UserControl
    {
        #region Dependency Properties
        // Using a DependencyProperty as the backing store for RingMaximumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RingMaximumValueProperty =
            DependencyProperty.Register("RingMaximumValue", typeof(double), typeof(CircularVolumes), new UIPropertyMetadata(10.0));

        // Using a DependencyProperty as the backing store for RingMinimumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RingMinimumValueProperty =
            DependencyProperty.Register("RingMinimumValue", typeof(double), typeof(CircularVolumes), new UIPropertyMetadata(0.0));

        // Using a DependencyProperty as the backing store for RingValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RingValueProperty =
            DependencyProperty.Register("RingValue", typeof(double), typeof(CircularVolumes), new UIPropertyMetadata(0.0));

        // Using a DependencyProperty as the backing store for RxMaximumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RxMaximumValueProperty =
            DependencyProperty.Register("RxMaximumValue", typeof(double), typeof(CircularVolumes), new UIPropertyMetadata(10.0));

        // Using a DependencyProperty as the backing store for RxMinimumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RxMinimumValueProperty =
            DependencyProperty.Register("RxMinimumValue", typeof(double), typeof(CircularVolumes), new UIPropertyMetadata(0.0));

        // Using a DependencyProperty as the backing store for RxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RxValueProperty =
            DependencyProperty.Register("RxValue", typeof(double), typeof(CircularVolumes), new UIPropertyMetadata(0.0));

        // Using a DependencyProperty as the backing store for TxMaximumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TxMaximumValueProperty =
            DependencyProperty.Register("TxMaximumValue", typeof(double), typeof(CircularVolumes), new UIPropertyMetadata(10.0));

        // Using a DependencyProperty as the backing store for TxMinimumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TxMinimumValueProperty =
            DependencyProperty.Register("TxMinimumValue", typeof(double), typeof(CircularVolumes), new UIPropertyMetadata(0.0));

        // Using a DependencyProperty as the backing store for TxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TxValueProperty =
            DependencyProperty.Register("TxValue", typeof(double), typeof(CircularVolumes), new UIPropertyMetadata(0.0));
        #endregion

        #region Properties
        public double RingMaximumValue
        {
            get { return (double)GetValue(RingMaximumValueProperty); }
            set { SetValue(RingMaximumValueProperty, value); }
        }

        public double RingMinimumValue
        {
            get { return (double)GetValue(RingMinimumValueProperty); }
            set { SetValue(RingMinimumValueProperty, value); }
        }

        public double RingValue
        {
            get { return (double)GetValue(RingValueProperty); }
            set { SetValue(RingValueProperty, value); }
        }

        public double RxMaximumValue
        {
            get { return (double)GetValue(RxMaximumValueProperty); }
            set { SetValue(RxMaximumValueProperty, value); }
        }

        public double RxMinimumValue
        {
            get { return (double)GetValue(RxMinimumValueProperty); }
            set { SetValue(RxMinimumValueProperty, value); }
        }

        public double RxValue
        {
            get { return (double)GetValue(RxValueProperty); }
            set { SetValue(RxValueProperty, value); }
        }

        public double TxMaximumValue
        {
            get { return (double)GetValue(TxMaximumValueProperty); }
            set { SetValue(TxMaximumValueProperty, value); }
        }

        public double TxMinimumValue
        {
            get { return (double)GetValue(TxMinimumValueProperty); }
            set { SetValue(TxMinimumValueProperty, value); }
        }

        public double TxValue
        {
            get { return (double)GetValue(TxValueProperty); }
            set { SetValue(TxValueProperty, value); }
        }
        #endregion

        public CircularVolumes()
        {
            InitializeComponent();
        }

        #region Functions

        #endregion

    }
}
