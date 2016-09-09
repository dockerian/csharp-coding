/*
 ************************************************************
 * Source: Sorting.cs
 * System: Microsoft Windows with .NET Framework
 * Author: Jason Zhu <jason_zhuyx@hotmail.com>
 * Update: 2013-08-11 Initial version
 *         
 ************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


namespace Common.Library.Sorting
{
    public static class BubbleSort
    {
        public static void BubbleSortEx<T>(this IList<T> list) where T : IComparable
        {
            if (list == null || list.Count < 2) return;
            int itemCount = list.Count;
            bool isChanged;
            do
            {
                isChanged = false;
                itemCount--;
                for (int i = 0; i < itemCount; i++)
                {
                    if (list[i].CompareTo(list[i + 1]) > 0)
                    {
                        T temp = list[i + 1];
                        list[i + 1] = list[i];
                        list[i] = temp;
                        isChanged = true;
                    }
                }
            } while (isChanged);
        }

        public static void BubbleSortEx<T>(this T[] array) where T : IComparable
        {
            if (array == null || array.Length < 2) return;
            int itemCount = array.Length;
            bool isChanged;
            do
            {
                isChanged = false;
                itemCount--;
                for (int i = 0; i < itemCount; i++)
                {
                    if (array[i].CompareTo(array[i + 1]) > 0)
                    {
                        T temp = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = temp;
                        isChanged = true;
                    }
                }
            } while (isChanged);
        }

        public static void Sort<T>(IList<T> list) where T : IComparable
        {
            list.BubbleSortEx();
        }

        public static void Sort<T>(T[] array) where T : IComparable
        {
            array.BubbleSortEx();
        }
    }

    public static class HeapSort
    {
        public static void HeapSortEx<T>(this T[] array) where T : IComparable
        {
            Heapsort_<T>(array, 0, array.Length, Comparer<T>.Default);
        }
        private static void Heapsort_<T>(T[] array, int offset, int length, IComparer<T> comparer)
        {
            HeapSort_<T>(array, offset, length, comparer.Compare);
        }
        private static void HeapSort_<T>(T[] array, int offset, int length, Comparison<T> comparison)
        {
            // build binary heap from all items
            for(int i = 0; i < length; i++)
            {
                int index = i;
                T item = array[offset + i]; // use next item

                // and move it on top, if greater than parent
                while (index > 0 && comparison(array[offset + (index - 1) / 2], item) < 0)
                {
                    int top = (index - 1) / 2;
                    array[offset + index] = array[offset + top];
                    index = top;
                }
                array[offset + index] = item;
            }

            for(int i = length - 1; i > 0; i--)
            {
                // delete max and place it as last
                T last = array[offset + i];
                array[offset + i] = array[offset];

                int index = 0;
                // the last one positioned in the heap
                while (index * 2 + 1 < i)
                {
                    int left = index * 2 + 1, right = left + 1;

                    if (right < i && comparison(array[offset + left], array[offset + right]) < 0)
                    {
                        if (comparison(last, array[offset + right]) > 0) break;

                        array[offset + index] = array[offset + right];
                        index = right;
                    }
                    else
                    {
                        if (comparison(last, array[offset + left]) > 0) break;

                        array[offset + index] = array[offset + left];
                        index = left;
                    }
                }
                array[offset + index] = last;
            }
        }

        public static void Sort<T>(T[] array) where T : IComparable
        {
            array.HeapSortEx();
        }

    }

    public static class InsertionSort
    {
        #region Sorting :: InsertionSort
        /// <summary>
        /// InsertionSort; O(n) ~ O(n*n)
        /// </summary>
        /// <param name="array"></param>
        public static void InsertionSortEx(this int[] array)
        {
            if (array == null || array.Length < 2) return;

            int size = array.Length;

            for(int i = 0; i < size; i++)
            {
                int insertValue = array[i];
                int shiftPos = i;

                while(shiftPos > 0 && insertValue < array[shiftPos - 1])
                {
                    array[shiftPos] = array[shiftPos - 1];
                    shiftPos = shiftPos - 1;
                }
                array[shiftPos] = insertValue;
            }
        }

        public static void Sort(int[] array)
        {
            array.InsertionSortEx();
        }

        #endregion
    }

    public static class MergeSort
    {
        #region Sorting :: MergeSort
        /// <summary>
        /// MergeSort; O(n log n) ~ O(n lg n + n + O(lg n))
        /// MergeSort is based on the divide-and-conquer paradigm. Its worst-case running time has a lower order of growth than insertion sort.
        /// </summary>
        /// <param name="a"></param>
        public static void MergeSortEx<T>(this T[] a) where T : IComparable
        {
            if (a == null || a.Length < 2) return;

            MergeSort_(a, 0, a.Length - 1);

        }
        private static void MergeSort_<T>(T[] array, int i = 0, int j = 0) where T : IComparable
        {
            //*//
            if (array == null || array.Length < 2) return;
            if (i < 0 || i >= array.Length) return;
            if (j < 0 || j >= array.Length) return;
            //*/
            if (i >= j) return;

            int mid = (i + j) / 2;

            MergeSort_(array, i, mid);
            MergeSort_(array, mid + 1, j);
            MergeArray_(array, i, j);

        }
        /*//
        private static void Merge_<T>(T[] array, int i, int j) where T : IComparable
        {
            if (array == null || array.Length < 2) return;
            if (i < 0 || i >= array.Length) return;
            if (j < 0 || j >= array.Length) return;
            if (i >= j) return;

            T[] result = new T[j - i + 1];

            int posMid = (i + j) / 2, posL = i, posR = posMid + 1, k = 0;

            // Simultaniously go thru both and store the smallest element
            while (posL <= posMid && posR <= j)
            {
                if (array[posL].CompareTo(array[posR]) < 0)
                {
                    result[k++] = array[posL++];
                }
                else
                {
                    result[k++] = array[posR++];
                }
            }
            // Add any element remained in first part
            while (posL <= posMid)
            {
                result[k++] = array[posL++];
            }
            // Add any element remained in second part
            while (posR <= j)
            {
                result[k++] = array[posR++];
            }
            // Now the merged sorted elements in result

            // Copy back to original array
            for(int x = 0, p = i; k < result.Length; x++)
            {
                array[p++] = result[x];
            }
        }
        //*/
        private static void MergeArray_<T>(T[] array, int i = 0, int j = 0) where T : IComparable
        {
            T[] result = new T[array.Length];

            int start = i;
            int posMid = (i + j) / 2;
            int posL = i;
            int posR = posMid + 1;

            while (i <= posMid && posR <= j)
            {
                if (array[i].CompareTo(array[posR]) <= 0)
                {
                    result[posL++] = array[i++];
                }
                else
                {
                    result[posL++] = array[posR++];
                }
            }
            if (i > posMid)
            {
                for (; posR <= j; )
                {
                    result[posL++] = array[posR++];
                }
            }
            else // posR > j
            {
                for (; i <= posMid; )
                {
                    result[posL++] = array[i++];
                }
            }

            // copy result back to the array
            for (posL = start; posL <= j; posL++)
            {
                array[posL] = result[posL];
            }
        }

        public static void Sort<T>(T[] array) where T : IComparable
        {
            array.MergeSortEx();
        }

        #endregion

        #region Sorting :: MergeSort (List<T>)

        public static void MergeSortEx<T>(this List<T> a) where T : IComparable
        {
            if (a == null || a.Count < 2) return;

            MergeSort_(a, 0, a.Count - 1);

        }
        private static void MergeSort_<T>(List<T> array, int i = 0, int j = 0) where T : IComparable
        {
            //*//
            if (array == null || array.Count < 2) return;
            if (i < 0 || i >= array.Count) return;
            if (j < 0 || j >= array.Count) return;
            //*/
            if (i >= j) return;

            int mid = (i + j) / 2;

            MergeSort_(array, i, mid);
            MergeSort_(array, mid + 1, j);
            MergeArray(array, i, j);

        }
        private static void MergeArray<T>(List<T> array, int i = 0, int j = 0) where T : IComparable
        {
            T[] result = new T[array.Count];

            int start = i;
            int posMid = (i + j) / 2;
            int posL = i;
            int posR = posMid + 1;

            while (i <= posMid && posR <= j)
            {
                if (array[i].CompareTo(array[posR]) <= 0)
                {
                    result[posL++] = array[i++];
                }
                else
                {
                    result[posL++] = array[posR++];
                }
            }
            if (i > posMid)
            {
                for (; posR <= j; )
                {
                    result[posL++] = array[posR++];
                }
            }
            else // posR > j
            {
                for (; i <= posMid; )
                {
                    result[posL++] = array[i++];
                }
            }

            // copy result back to the array
            for (posL = start; posL <= j; posL++)
            {
                array[posL] = result[posL];
            }
        }
 
        public static void Sort<T>(List<T> list) where T : IComparable
        {
            list.MergeSortEx();
        }

        #endregion

    }

    public static class QuickSort
    {
        #region Sorting :: QuickSort
        /// <summary>
        /// QuickSort; O(n log n) ~ O(n*n)
        /// </summary>
        /// <param name="a">original array</param>
        public static void QuickSortEx<T>(this T[] a) where T : IComparable
        {
            if (a == null || a.Length < 2) return;

            QuickSort_(a, 0, a.Length - 1);

        }
        private static void QuickSort_<T>(T[] array, int i = 0, int j = 0) where T : IComparable
        {
            //*//
            if (array == null || array.Length < 2) return;
            if (i < 0 || i >= array.Length) return;
            if (j < 0 || j >= array.Length) return;
            //*/
            if (i >= j) return;

            Random rand = new Random();

            //*/ choose any element of the array to be the pivot.//*/
            int pivotIndex = rand.Next(i, j);
            T pivot = array[pivotIndex];
            int posL = i - 1; // starts from the left
            int posR = j; // starts from the right

            // swap pivot and the last element
            array[pivotIndex] = array[j];
            array[j] = pivot;

            do
            {
                //*/ All elements less than the pivot must be in the first partition.//*/
                do { posL++; } while (array[posL].CompareTo( pivot) < 0);
                //*/ All elements greater than the pivot must be in the second partition.//*/
                do { posR--; } while (array[posR].CompareTo( pivot) > 0 && posR > i);

                if (posL < posR)
                {
                    // swap elements between current left and right positions
                    T swap = array[posL];
                    array[posL] = array[posR];
                    array[posR] = swap;
                }
            } while (posL < posR); // untill posL and posR cross each other

            // For posL >= posR, array[posL] must be greater than pivot, now swap with the pivot in array[j]
            array[j] = array[posL];
            // Put the pivot in the middle
            array[posL] = pivot;

            QuickSort_(array, i, posL-1);
            QuickSort_(array, posL+1, j);
        }

        public static void Sort<T>(T[] array) where T : IComparable
        {
            array.QuickSortEx();
        }

        #endregion

    }

    public static class SelectionSort
    {
        #region Sorting :: SelectionSort
        /// <summary>
        /// SelectionSort; O(n) ~ O(n*n)
        /// </summary>
        /// <param name="array"></param>
        public static void SelectionSortEx<T>(this T[] array) where T : IComparable
        {
            if (array == null || array.Length < 2) return;

            int size = array.Length;

            for(int i = 0; i < size - 1; i++)
            {
                /*// Find the min element in the unsorted a[], assuming the first element is the minimum //*/
                int indexMinimum = i;
                /*// Go thru elements after i to find the smallest //*/
                for(int j = i + 1; j < size; j++)
                {
                    /*// If this element is less, then it is the new minimum //*/
                    if (array[j].CompareTo(array[indexMinimum]) < 0)
                    {
                        /*// Found new minimum; remember the index //*/
                        indexMinimum = j;
                    }
                }

                /*// For the index of the minimum element, swap it with the current position //*/
                if (indexMinimum != i)
                {
                    T s = array[i];
                    array[i] = array[indexMinimum];
                    array[indexMinimum] = s;
                }
            }
        }

        public static void Sort<T>(T[] array) where T : IComparable
        {
            array.SelectionSortEx();
        }

        #endregion

        #region Sorting :: SelectionSort (IList<IComparable>)
        public static void SelectionSortEx<T>(this IList<T> array) where T : IComparable
        {
            if (array == null || array.Count < 2) return;

            int size = array.Count;

            for (int i = 0; i < size - 1; i++)
            {
                /*// Find the min element in the unsorted a[], assuming the first element is the minimum //*/
                int indexMinimum = i;
                /*// Go thru elements after i to find the smallest //*/
                for (int j = i + 1; j < size; j++)
                {
                    /*// If this element is less, then it is the new minimum //*/
                    if (array[j].CompareTo(array[indexMinimum]) < 0)
                    {
                        /*// Found new minimum; remember the index //*/
                        indexMinimum = j;
                    }
                }

                /*// For the index of the minimum element, swap it with the current position //*/
                if (indexMinimum != i)
                {
                    T s = array[i];
                    array[i] = array[indexMinimum];
                    array[indexMinimum] = s;
                }
            }
        }

        public static void Sort<T>(IList<T> array) where T : IComparable
        {
            array.SelectionSortEx();
        }

        #endregion

    }// class SelectionSort

    public static class StoogeSort
    {
        public static void StoogeSortEx<T>(this List<T> list) where T : IComparable
        {
            if (list.Count > 1)
            {
                StoogeSort_(list, 0, list.Count - 1);
            }
        }
        private static void StoogeSort_<T>(List<T> L, int i, int j) where T : IComparable
        {
            if (L[j].CompareTo(L[i]) < 0)
            {
                T tmp = L[i];
                L[i] = L[j];
                L[j] = tmp;
            }
            if (j - i > 1)
            {
                int t = (j - i + 1) / 3;
                StoogeSort_(L, i, j - t);
                StoogeSort_(L, i + t, j);
                StoogeSort_(L, i, j - t);
            }
        }

        public static void Sort<T>(List<T> list) where T : IComparable
        {
            list.StoogeSortEx();
        }
    }

}// namespace Common.Library.Sorting
