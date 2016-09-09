using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Common.Wpf.Extensions
{
    public class AutoScrollHandler : DependencyObject, IDisposable
    {

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable),
            typeof(AutoScrollHandler), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(ItemsSourcePropertyChanged)));

        private System.Windows.Controls.ListBox Target;

        public AutoScrollHandler(System.Windows.Controls.ListBox target)
        {
            Target = target;
            Binding B = new Binding("ItemsSource");
            B.Source = Target;
            BindingOperations.SetBinding(this, ItemsSourceProperty, B);

            Target.Loaded += new RoutedEventHandler(Target_Loaded); //ToDo: Need to unsuscribe this?
            Target.Unloaded += new RoutedEventHandler(Target_Unloaded);
        }

        void Target_Loaded(object sender, RoutedEventArgs e)
        {
            if (Target.Items.Count > 0)
            {
                Target.ScrollIntoView(Target.Items[Target.Items.Count - 1]);
            }

            Target.IsVisibleChanged += new DependencyPropertyChangedEventHandler(Target_IsVisibleChanged);
        }

        void Target_Unloaded(object sender, RoutedEventArgs e)
        {
            Target.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(Target_IsVisibleChanged);
        }

        void Target_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (Target.Items.Count > 0)
                {
                    Target.ScrollIntoView(Target.Items[Target.Items.Count - 1]);
                }
            }
        }

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, ItemsSourceProperty);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        static void ItemsSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((AutoScrollHandler)o).ItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        void ItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            INotifyCollectionChanged Collection = oldValue as INotifyCollectionChanged;
            if (Collection != null)
                Collection.CollectionChanged -= new NotifyCollectionChangedEventHandler(Collection_CollectionChanged);
            Collection = newValue as INotifyCollectionChanged;
            if (Collection != null)
                Collection.CollectionChanged += new NotifyCollectionChangedEventHandler(Collection_CollectionChanged);
        }

        void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add || e.NewItems == null || e.NewItems.Count < 1)
                return;

            Target.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);
        }

    }

    public static class ListBoxExtension
    {
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(ListBoxExtension),
            new UIPropertyMetadata(false, OnAutoScrollPropertyChanged));

        public static readonly DependencyProperty AutoScrollHandlerProperty =
            DependencyProperty.RegisterAttached("AutoScrollHandler", typeof(AutoScrollHandler), typeof(ListBoxExtension));

        public static bool GetAutoScroll(DependencyObject d)
        {

            return (bool)d.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(DependencyObject d, bool value)
        {
            d.SetValue(AutoScrollProperty, value);
        }

        private static void OnAutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBox instance = d as ListBox;
            bool value = (bool)e.NewValue;
            AutoScrollHandler OldHandler = (AutoScrollHandler)instance.GetValue(AutoScrollHandlerProperty);
            if (OldHandler != null)
            {
                OldHandler.Dispose();
                instance.SetValue(AutoScrollHandlerProperty, null);
            }
            if (value)
                instance.SetValue(AutoScrollHandlerProperty, new AutoScrollHandler(instance));
        }

        // ---------------------- SelectedItems Binding Code --------------------------------------------
        // Credits: Alex's Shed Blog
        // http://alexshed.spaces.live.com/blog/cns!71C72270309CE838!149.entry
        #region MultiSelect Items Binding

        /// <summary>
        /// Identifies the ListBoxExtension.SelectedItemsSource attached property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsSourceProperty =
           DependencyProperty.RegisterAttached(
              "SelectedItemsSource",
              typeof(IList),
              typeof(ListBoxExtension),
              new PropertyMetadata(
                  null,
                  new PropertyChangedCallback(OnSelectedItemsSourceChanged), ExampleCoerceValueCallBack));

        /// <summary>
        /// Gets the IList that contains the values that should be selected.
        /// </summary>
        /// <param name="element">The ListBox to check.</param>
        public static IList GetSelectedItemsSource(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (IList)element.GetValue(SelectedItemsSourceProperty);
        }

        /// <summary>
        /// Sets the IList that contains the values that should be selected.
        /// </summary>
        /// <param name="element">The ListBox being set.</param>
        /// <param name="value">The value of the property.</param>
        public static void SetSelectedItemsSource(DependencyObject element, IList value)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.SetValue(SelectedItemsSourceProperty, value);
        }

        // Used to set a flag on the ListBox to avoid reentry of SelectionChanged due to
        // a full syncronisation pass
        private static readonly DependencyPropertyKey IsResynchingPropertyKey =
           DependencyProperty.RegisterAttachedReadOnly(
                "IsResynching",
                typeof(bool),
                typeof(ListBoxExtension),
                new PropertyMetadata(false));

        private static void OnSelectedItemsSourceChanged(DependencyObject d,
                            DependencyPropertyChangedEventArgs e)
        {
            ListBox listBox = d as ListBox;
            if (listBox == null)
                throw new InvalidOperationException("The ListBoxExtension.SelectedItemsSource attached " +
                   "property can only be applied to ListBox controls.");

            listBox.SelectionChanged -= new SelectionChangedEventHandler(OnListBoxSelectionChanged);


            if (e.NewValue != null)
            {
                ListenForChanges(listBox);
            }
        }

        private static void ListenForChanges(ListBox listBox)
        {
            // Wait until the element is initialised
            if (!listBox.IsInitialized)
            {
                EventHandler callback = null;
                callback = delegate
                {
                    listBox.Initialized -= callback;
                    ListenForChanges(listBox);
                };
                listBox.Initialized += callback;
                return;
            }
            
            listBox.SelectionChanged += new SelectionChangedEventHandler(OnListBoxSelectionChanged);
            ResynchList(listBox);
        }

        private static object ExampleCoerceValueCallBack(DependencyObject d, object value)
        {
            ListBox listBox = d as ListBox;
            if (listBox == null)
                throw new InvalidOperationException("The ListBoxExtension.SelectedItemsSource attached " +
                   "property can only be applied to ListBox controls.");
            
            ResynchList(listBox);
            return value;
        }

        private static void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                bool isResynching = (bool)listBox.GetValue(IsResynchingPropertyKey.DependencyProperty);
                if (isResynching)
                    return;

                IList list = GetSelectedItemsSource(listBox);
                if (list != null)
                {
                    foreach(object obj in e.RemovedItems)
                    {
                        if (list.Contains(obj))
                            list.Remove(obj);
                    }
                    foreach(object obj in e.AddedItems)
                    {
                        if (!list.Contains(obj))
                            list.Add(obj);
                    }
                }
            }
        }

        private static void ResynchList(ListBox listBox)
        {
            if (listBox != null)
            {
                listBox.SetValue(IsResynchingPropertyKey, true);
                IList list = GetSelectedItemsSource(listBox);

                if (listBox.SelectionMode == SelectionMode.Single)
                {
                    listBox.SelectedItem = null;
                    if (list != null)
                    {
                        if (list.Count > 1)
                        {
                            // There is more than one item selected, but the listbox is in Single selection mode
                            throw new InvalidOperationException("ListBox is in Single selection mode, but was given more than one selected value.");
                        }

                        if (list.Count == 1)
                            listBox.SelectedItem = list[0];
                    }
                }
                else
                {
                    listBox.SelectedItems.Clear();
                    if (list != null)
                    {
                        foreach(object obj in listBox.Items)
                            if (list.Contains(obj))
                                listBox.SelectedItems.Add(obj);
                    }
                }

                listBox.SetValue(IsResynchingPropertyKey, false);
            }
        }

        #endregion
    }

}