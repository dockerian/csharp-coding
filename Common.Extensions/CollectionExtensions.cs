using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Common.Extensions
{
    public static class CollectionExtensions
    {
        private class Comparer<T> : IComparer<T>
        {
            private readonly Comparison<T> comparison;

            public Comparer(Comparison<T> comparison)
            {
                this.comparison = comparison;
            }

            #region IComparer<T> Members

            public int Compare(T x, T y)
            {
                return comparison.Invoke(x, y);
            }

            #endregion
        }

        // Extends ObservableCollection<T> class.
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                collection.Add(item);
            }
        }

        public static ObservableCollection<T> Remove<T>(this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach(var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }

            return coll;
        }

        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            if (collection.Count == 0) return;

            List<T> sorted = collection.OrderBy(x => x).ToList();

            for(int i = 0; i < sorted.Count(); i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
        }

        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            if (collection.Count == 0) return;

            var comparer = new Comparer<T>(comparison);

            List<T> sorted = collection.OrderBy(x => x, comparer).ToList();

            for(int i = 0; i < sorted.Count(); i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
        }

        public static void Sort<T>(this ObservableCollection<T> collection, IComparer<T> comparer)
        {
            for (int i = 1; i < collection.Count; i++)
            {
                int j = i; T index = collection[i];

                while ((j > 0) && (comparer.Compare(collection[j - 1], index) == 1))
                {
                    collection[j] = collection[j - 1]; j--;
                }

                collection[j] = index;
            }
        }

        public static void Sort<T>(this ObservableCollection<T> collection, IEnumerable<T> sortedItems)
        {
            var sortedItemsList = sortedItems.ToList();

            foreach(var item in sortedItemsList)
            {
                collection.Move(collection.IndexOf(item), sortedItemsList.IndexOf(item));
            }
        }

    }

    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        public ObservableCollectionEx() : base()
        {
        }
        public ObservableCollectionEx(IEnumerable<T> collection) : base(collection)
        {
        }
        public ObservableCollectionEx(List<T> list) : base(list)
        {
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) return;

            foreach(var item in collection)
            {
                this.Items.Add(item);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
            );
        }

        public void Reset(IEnumerable<T> collection)
        {
            this.Items.Clear();

            this.AddRange(collection);
        }
    }

    #region Sorting Functions

    public class PrioritizableObservableCollection<T> : ObservableCollection<T>
    {
        public virtual void Prioritize<TKey>(SortKey<T, TKey> firstSortKey, SortKey<T, TKey> secondSortKey)
        {
            if (firstSortKey == null || secondSortKey == null) return;

            Func<T, TKey> firstKeySelector = firstSortKey.SortFuncKey;
            bool firstSortOrder = firstSortKey.SortOrder == SortOrder.Ascending;
            Func<T, TKey> secondKeySelector = secondSortKey.SortFuncKey;
            bool secondSortOrder = secondSortKey.SortOrder == SortOrder.Ascending;

            this.Prioritize(firstKeySelector, secondKeySelector, firstSortOrder, secondSortOrder);

        }

        /// <summary>
        /// Sort the collection in ascending or descending order according to field1 and field2.
        /// </summary>
        /// <typeparam name="TKey">The type of the key returned by</typeparam>
        /// <param name="firstKeySelector">A function to extract a key from field1 item.</param>
        /// <param name="secondKeySelector">A function to extract a key from field2 item.</param>
        /// <param name="ascendingfield1">Specifying ascending or descending order for field1</param>
        /// <param name="ascendingfield2">Specifying ascending or descending order for field2</param>
        public virtual void Prioritize<TKey>(Func<T, TKey> firstKeySelector, Func<T, TKey> secondKeySelector, bool ascendingfield1 = true, bool ascendingfield2 = true)
        {
            if (ascendingfield1 && ascendingfield2)
            {
                this.SortCollection(Items.OrderBy(firstKeySelector).ThenBy(secondKeySelector));
            }
            else if (ascendingfield1 && ascendingfield2 == false)
            {
                this.SortCollection(Items.OrderBy(firstKeySelector).ThenByDescending(secondKeySelector));
            }
            else if (ascendingfield1 == false && ascendingfield2)
            {
                this.SortCollection(Items.OrderByDescending(firstKeySelector).ThenBy(secondKeySelector));
            }
            else // descending both fields
            {
                this.SortCollection(Items.OrderByDescending(firstKeySelector).ThenByDescending(secondKeySelector));
            }
        }

        /// <summary>
        /// Moves the items of the collection so that their orders are the same as those of the items provided.
        /// </summary>
        /// <param name="sortedItems">An <see cref="IEnumerable{T}"/> to provide item orders.</param>
        protected void SortCollection(IEnumerable<T> sortedItems)
        {
            var sortedItemsList = sortedItems.ToList();

            foreach(var item in sortedItemsList)
            {
                this.Move(IndexOf(item), sortedItemsList.IndexOf(item));
            }
        }

    }

    public class SortKey<T, TKey>
    {
        public SortKey(Func<T, TKey> sortKey, SortOrder sortOrder = SortOrder.Ascending)
        {
            this.SortFuncKey = sortKey;
            this.SortOrder = sortOrder;
        }

        public Func<T, TKey> SortFuncKey { get; set; }
        public SortOrder SortOrder { get; set; }
    }

    #endregion

}
