// ---------------------------------------------------------------------------
// <copyright file="Questions.cs" company="Boathill">
//   Copyright (c) Jason Zhu.  All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------
/*
 *****************************************************************************
 * Source: Questions.cs
 * System: Microsoft Windows with .NET Framework
 * Author: Jason Zhu <jason_zhuyx@hotmail.com>
 * Update: 2013-05-12 Initial version
 *         
 *****************************************************************************
 */

namespace Coding.App
{
    using System;
    using System.Collections.Generic;
    using Common.Extensions;
    using Common.Library;

    /// <summary>
    /// The Questions class contains a set of interview questions.
    /// </summary>
    public class Questions
    {
        #region Algorithm

        /// <summary>
        /// Get angle between the Hour and the Minute hands
        /// </summary>
        /// <param name="dateTime">specified date time (only Hour, Minute, and Second count)</param>
        /// <param name="ignoringSecond">ignoring angle by the Second hand</param>
        /// <param name="clockDialBy24Hours">using 24 hours clock dial</param>
        /// <returns>angle between the Hour and the Minute hands</returns>
        public static double GetAngle(DateTime dateTime, bool ignoringSecond = false, bool clockDialBy24Hours = false)
        {
            double hrsFraction = clockDialBy24Hours ? 24 : 12;
            double angleSecond = 360 * dateTime.Second / 60.0;
            double angleMinute = 360 * dateTime.Minute / 60.0 + (ignoringSecond ? 0 : angleSecond / 60.0);
            double angleHour = ((360 *(dateTime.Hour % hrsFraction)) + angleMinute) / hrsFraction;
            double angle = angleMinute - angleHour;

            return Math.Abs(angle);
        }

        #endregion

        #region Array

        /// <summary>
        /// Given an array of n integers which can contain integers from 1 to n only. 
        /// Some elements can be repeated multiple times and some other elements can be absent from the array. 
        /// Write a running code on paper which takes O(1) space apart from the input array 
        /// and O(n) time to print which elements are not present in the array 
        /// and the count of every element which is there in the array along with the element number. 
        /// NOTE: The array isn't necessarily sorted.
        /// </summary>
        /// <param name="array">given array</param>
        /// <param name="range">range to indicate integers from 1 ~ range</param>
        /// <param name="calculateInline">Use O(1) space</param>
        /// <returns>The result array.</returns>
        public static int[] CheckDuplicate(int[] array, int range, bool calculateInline = false)
        {
            if (array == null || array.Length < 1)
            {
                return array;
            }

            int[] result = new int[range];

            if (calculateInline)
            {
                if (array.Length < range)
                {
                    Array.Resize(ref array, range);
                }

                result = array;
            }
            else // using a separate array to build hash table
            {
                for(int i = 0; i < range && i < array.Length; i++)
                {
                    result[i] = array[i];
                }
            }

            for(int i = 0; i < range; i++)
            {
                if (array[i] <= 0 || array[i] > range)
                {
                    result[i] = 0;
                }
            }

            int pos = 0, targetIndex = 0;

            while(pos < range)
            {
                if (result[pos] < 0)
                {
                    pos++;
                    continue;
                }

                targetIndex = result[pos] - 1;

                if (targetIndex >= 0 && targetIndex < range)
                {
                    if (result[targetIndex] > 0)
                    {
                        if (result[targetIndex] <= range)
                        {
                            result[pos] = result[targetIndex];
                            result[targetIndex] = -1;
                            continue;
                        }
                        else // integer is not in the range
                        {
                            result[targetIndex] = 0;
                        }
                    }
                    else // found another integer (whose value = targetIndex + 1, from result[pos])
                    {
                        result[targetIndex]--;
                    }
                }

                result[pos] = 0;
                pos++;
            }

            return result;
        }

        /// <summary>
        /// For given sentence, find the longest sequential letters.
        /// </summary>
        /// <param name="sentence">The given sentence of letters.</param>
        /// <param name="bydescending">Indicates if searching the sequence by descending or not.</param>
        /// <returns>The longest string with sequential letters.</returns>
        public static string FindLongestSequence(string sentence, bool bydescending = false)
        {
            if (string.IsNullOrEmpty(sentence))
            {
                return string.Empty;
            }

            int sizeKept = 1;
            int startIdx = 0;

            for(int i = 1, j = 1; i < sentence.Length; i++)
            {
                int diff = sentence[i] - sentence[i - 1];
                var keep = bydescending ? (diff == -1) : (diff == 1);

                if (keep)
                {
                    if (++j > sizeKept)
                    {
                        startIdx = i - j + 1;
                        sizeKept = j;
                    }
                }
                else
                {
                    j = 1;
                }
            }

            var result = sentence.Substring(startIdx, sizeKept);

            return result;
        }

        /// <summary>
        /// Given an array of integers, find the longest sequence.
        /// For example: if input = {4,5,6,8,3,1,4,5,6,5,4,3,1}
        /// Result: {4,5,6}
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="bydescending">Indicates to find the sequence by descending.</param>
        /// <param name="oneIntervalOnly">Specifies the interval by 1 only.</param>
        /// <returns>The list of the sequence.</returns>
        public static List<int> FindLongestSequenceArray(List<int> sourceArray, bool bydescending = false, bool oneIntervalOnly = false)
        {
            List<int> a = new List<int>();
            List<int> b = new List<int>();

            if (sourceArray == null || sourceArray.Count < 2)
            {
                return a;
            }

            a.Add(sourceArray[0]);
            b.Add(sourceArray[0]);

            for(int i = 1; i < sourceArray.Count; i++)
            {
                var diffPrevious = sourceArray[i] - sourceArray[i - 1];
                var hasSameIntvl = bydescending ? (oneIntervalOnly ? diffPrevious == -1 : diffPrevious < 0) : (oneIntervalOnly ? diffPrevious == 1 : diffPrevious > 0);
                var hasSameTrend = bydescending ? (diffPrevious <= 0) : (diffPrevious >= 0);

                if (oneIntervalOnly && hasSameIntvl || !oneIntervalOnly && hasSameTrend)
                {
                    a.Add(sourceArray[i]);
                }
                else // prossible new sequence starts
                {
                    if (a.Count > b.Count)
                    {
                        b = a; // keep the longest sequence
                    }

                    a = new List<int>();

                    a.Add(sourceArray[i]);
                }
            }

            return (a.Count > b.Count) ? a : b;
        }

        /// <summary>
        /// Given an array find the subsequence that needs to be removed so that the array becomes sorted. 
        /// There can be many resultant sorted arrays. Need to find longest. Expected time complexity O(n). 
        /// e.g. 9 9 6 1 3 4 4 5 1 7 => 1 3 4 4 5 7 so need to remove 9 9 6 1
        /// </summary>
        /// <param name="sourceArray">source data array</param>
        /// <param name="sortedArray">sequenced items in source array</param>
        /// <param name="bydescending">defines if the sequence lookup by descending</param>
        /// <returns>the array contains out-of-sequence items</returns>
        public static List<int> FindOutOfSequence(List<int> sourceArray, out List<int> sortedArray, bool bydescending = false)
        {
            List<int> a = new List<int>();
            sortedArray = new List<int>();

            if (sourceArray == null || sourceArray.Count < 1)
            {
                return a;
            }

            sortedArray.Add(sourceArray[0]);

            if (sourceArray.Count < 1)
            {
                return a;
            }

            int cb = 0, ce = 0, xb = 0, xe = 0;

            for(int i = 1; i <= sourceArray.Count; i++)
            {
                if (i < sourceArray.Count)
                {
                    a.Add(sourceArray[i]);
                    sortedArray.Add(sourceArray[i]);
                }

                var changedTrend =
                    (i == sourceArray.Count) || // ensure no out-of-range exception
                    (bydescending ? sourceArray[i] > sourceArray[i - 1] : sourceArray[i] < sourceArray[i - 1]);

                if (changedTrend)
                {
                    if ((ce - cb) > (xe - xb))
                    {
                        xb = cb; xe = ce; // keep the longest sequence index
                    }

                    cb = ce = i;
                }
                else // continue on current sequence
                {
                    ce++; // move the end position
                }
            }

            #region Algorithm with O(n)

            if (xe < (sortedArray.Count - 1))
            {
                sortedArray.RemoveRange(xe + 1, sourceArray.Count - 1 - xe);
            }

            if (xb > 0)
            {
                sortedArray.RemoveRange(0, xb);
            }

            #endregion

            #region Algorithem with O(2n)

            /*//
            a.Clear();
            sortedArray.Clear();

            for(int i = 0; i <= sourceArray.Count; i++)
            {
                if (i >= xb && i <= xe)
                {
                    sortedArray.Add(sourceArray[i]);
                }
                else // items not in found sequence
                {
                    a.Add(sourceArray[i]);
                }
            }
            //*/

            #endregion

            a.RemoveRange(xb, xe - xb);

            return a;
        }

        /// <summary>
        /// Get the maximum sum of sequential sub array from a given integer array, requiring O(n)
        /// </summary>
        /// <param name="array">integer array (can include any positive/negative integers)</param>
        /// <param name="maxSubArraySum">maximum sum</param>
        /// <returns>The sub array.</returns>
        public static int[] GetMaxSubArray(int[] array, out int maxSubArraySum)
        {
            if (array == null || array.Length < 2)
            {
                maxSubArraySum = 0;
                return array;
            }

            List<int> subArray = new List<int>(array);

            int[] result;
            int curIdx = 0, curLen = 1, curSum = array[0];
            int maxIdx = 0, maxLen = 1, maxSum = array[0];

            for(int i = 1; i < array.Length; i++)
            {
                int curVal = array[i];

                if (curVal < 0 || curSum < 0 && curSum < curVal)
                {
                    curIdx = i;
                    curLen = 1;
                    curSum = array[i];
                }
                else // sum of sub array keep increasing
                {
                    curSum = curSum + curVal;
                    curLen++;
                }

                if (curSum > maxSum)
                {
                    maxIdx = curIdx;
                    maxLen = curLen;
                    maxSum = curSum;
                }
            }

            result = subArray.GetRange(maxIdx, maxLen).ToArray();
            maxSubArraySum = maxSum;

            return result;
        }

        /// <summary>
        /// Check doors status in following example:
        ///     Imagine 100 doors are closed. 
        ///     In first pass open all of them, 
        ///     in 2nd pass toggle every 2nd door, 
        ///     in 3rd pass toggle every 3rd door, ...
        ///     continue it till 100th pass ... 
        /// Find all the doors that will remain open after 100 passes
        /// </summary>
        /// <param name="numbersOfDoors">numbers of doors</param>
        /// <returns>doors status in a boolean array</returns>
        public static bool[] GetDoorsStatus(int numbersOfDoors = 100)
        {
            if (numbersOfDoors <= 0)
            {
                return null;
            }

            bool[] doorStates = new bool[numbersOfDoors];

            for(int i = 0; i < numbersOfDoors; i++)
            {
                doorStates[i] = false;
            }

            for(int round = 1; round <= numbersOfDoors; round++)
            {
                for(int n = 0; n < numbersOfDoors;)
                {
                    doorStates[n] = !doorStates[n];
                    n += round + 1;
                }
            }

            return doorStates;
        }

        /// <summary>
        /// Matrix rotation. See Matrix class and Array extensions (CommonExtensions.cs) for rotation inline and more
        /// </summary>
        /// <param name="matrix">The string matrix.</param>
        /// <param name="degree">The degree of rotation (90/-90, 180/-180, 270/-270 valid).</param>
        /// <returns>The rotated matrix.</returns>
        public static string[, ] GetMatrixRotation(string[, ] matrix, int degree = 90)
        {
            string[, ] rotation = null;
            int rotatingDegree = degree % 360;

            switch(rotatingDegree)
            {
                case -90:
                case 270:
                    {
                        rotation = matrix.RotateCounterClockwise();
                        break;
                    }

                case -180:
                case 180:
                    {
                        rotation = matrix.RotateClockwise180();
                        break;
                    }

                case -270:
                case 90:
                    {
                        rotation = matrix.RotateClockwise();
                        break;
                    }

            }
            return rotation;
        }

        /// <summary>
        /// For any element with value 0, set zero to its diagonal lines
        /// </summary>
        /// <param name="matrix">matrix of integers</param>
        public static void SetDiagonalZero(int[, ] matrix)
        {
            if (matrix == null || matrix.Length < 1)
            {
                return;
            }

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            bool[,] marker = new bool[rows, columns];

            for(var x = 0; x < rows; x++)
            {
                for(var y = 0; y < columns; y++)
                {
                    if (matrix[x, y] == 0)
                    {
                        for(int m = x - 1, n = y - 1; m >= 0 && n >= 0; m--, n--)
                        {
                            marker[m, n] = true;
                        }

                        for(int m = x + 1, n = y + 1; m < rows && n < columns; m++, n++)
                        {
                            marker[m, n] = true;
                        }

                        for(int m = x - 1, n = y + 1; m >= 0 && n < columns; m--, n++)
                        {
                            marker[m, n] = true;
                        }

                        for(int m = x + 1, n = y - 1; m < rows && n >= 0; m++, n--)
                        {
                            marker[m, n] = true;
                        }
                    }
                }
            }

            for(var x = 0; x < rows; x++)
            {
                for(var y = 0; y < columns; y++)
                {
                    if (marker[x, y])
                    {
                        matrix[x, y] = 0;
                    }
                }
            }
        }

        #endregion

        #region Bits/Binary/Bitwise

        /// <summary>
        /// Count the number of bits set in a number
        /// </summary>
        /// <param name="number">an integer number</param>
        /// <returns>count of bits</returns>
        public static int CountNumberOfBits(int number)
        {
            var count = 0;

            for(; number != 0;)
            {
                count += number & 1;
                number >>= 1;
            }

            return count;
        }

        #endregion

        #region Linked List

        /// <summary>
        /// Using linked list to represent a number, add two numbers.
        /// </summary>
        /// <param name="a">number 1 as linked list</param>
        /// <param name="b">number 2 as linked list</param>
        /// <returns>result of addition</returns>
        public static SingleLinkedList<uint> AddListsAsNumbers(SingleLinkedList<uint> a, SingleLinkedList<uint> b)
        {
            SingleLinkedList<uint> result = new SingleLinkedList<uint>();
            SingleNode<uint> p1 = a.Head;
            SingleNode<uint> p2 = b.Head;

            uint vp = 0;

            while(p1 != null || p2 != null || vp != 0)
            {
                uint v1 = (p1 != null) ? p1.Value : 0;
                uint v2 = (p2 != null) ? p2.Value : 0;
                uint va = (v1 + v2 + vp) % 10;

                result.AddNode(va);

                vp = (v1 + v2 + vp) / 10;
                p1 = (p1 != null) ? p1.Next : null;
                p2 = (p2 != null) ? p2.Next : null;
            }

            return result;
        }

        #endregion

        #region Stack

        #endregion

        #region Strings

        /// <summary>
        /// Delete duplicate chars in a string (without using any additional buffer)
        /// </summary>
        /// <param name="str">a string</param>
        /// <returns>a string without duplicate chars</returns>
        public static string DeleteDuplicateChars(string str)
        {
            return str;
        }

        /// <summary>
        /// Decode URL
        /// </summary>
        /// <param name="url">URL string</param>
        /// <returns>decoded URL</returns>
        public static string DecodeURL(string url)
        {
            return url;
        }

        /// <summary>
        /// Encode URL
        /// </summary>
        /// <param name="url">URL string</param>
        /// <returns>encoded URL</returns>
        public static string EncodeURL(string url)
        {
            return url;
        }

        /// <summary>
        /// Given a sentence and a word as arguments to a functions,
        /// write a efficient algorithm for finding the number of times the 
        /// word exists in the sentence.
        /// </summary>
        /// <param name="sentence">sentence to search</param>
        /// <param name="word">a word to be searched in sentence</param>
        /// <param name="firstIndex">index of first appearance</param>
        /// <returns>numbers of appearance</returns>
        public static int FindInstancesInSentence(string sentence, string word, out int firstIndex)
        {
            firstIndex = -1;

            if (string.IsNullOrEmpty(word) ||
                string.IsNullOrEmpty(sentence) || 
                word.Length > sentence.Length)
            {
                return 0;
            }

            int counter = 0;
            int sizWord = word.Length;
            int sizFind = sentence.Length - sizWord + 1;

            for(int i = 0, j = 0, k = 0; i < sizFind; i++)
            {
                if (sentence[i] == word[k])
                {
                    if (k == 0) j = i;

                    if (++k == sizWord)
                    {
                        if (counter == 0)
                        {
                            firstIndex = j;
                        }

                        counter++;
                        k = 0;
                    }
                }
                else
                {
                    k = 0;
                }
            }

            return counter;
        }

        /// <summary>
        /// Implement T9 dictionary for mobile phone
        /// </summary>
        /// <param name="input">string contains 0-9</param>
        /// <param name="dictionary">T9 dictionary</param>
        /// <returns>The result of T9 string.</returns>
        public static string GetT9String(string input, string[] dictionary)
        {
            return input;
        }

        /// <summary>
        /// Check if two strings are anagrams (identical counts for each unique char)
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="target">The target string to compare.</param>
        /// <returns>Returns true if two strings are anagrams; otherwise, false.</returns>
        public static bool IsAnagram(string source, string target)
        {
            return false;
        }

        /// <summary>
        /// Check if a string has all unique characters. (What if no additional data structure is allowed?)
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>true/false</returns>
        public static bool IsUniqueChars(string str)
        {
            return false;
        }

        #endregion
    }// class Questions
}// namespace Coding.App
