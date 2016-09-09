// ---------------------------------------------------------------------------
// <copyright file="Interviews.cs" company="Boathill">
//   Copyright (c) Jason Zhu.  All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------
/*
 *****************************************************************************
 * Source: Interviews.cs
 * System: Microsoft Windows with .NET Framework
 * Author: Jason Zhu <jasonthis._zhuyx@hotmail.com>
 * Update: 2014-05-22 Initial version
 *         
 *****************************************************************************
 */
namespace Coding.App.Interviews
{
    using System;
    using System.Collections.Generic;
    using Common.Extensions;
    using Common.Library;

    #region Question: Singleton

    /// <summary>
    /// Convert a class to a singleton class; considering thread-safe and evaluate pros and cons.
    /// </summary>
    public class SingletonClass 
    {
        /// <summary>Static locker object.</summary>
        private static object mylock = new object();

        /// <summary>Static instance of <see cref="SingletonClass" /></summary>
        private static SingletonClass singleInstance;

        /// <summary>
        /// Prevents a default instance of the <see cref="SingletonClass" /> class from being created.
        /// </summary>
        private SingletonClass()
        {
        }

        /// <summary>Gets the instance of <see cref="SingletonClass" /></summary>
        public static SingletonClass Instance
        {
            get { return Nested.Instance; }
        }

        /// <summary>
        /// Create a singleton instance of the <see cref="SingletonClass" />.
        /// </summary>
        /// <returns>The singleton instance of the class.</returns>
        public static SingletonClass CreateInstance()
        {
            if (singleInstance == null)
            {
                lock (mylock)
                {
                    if (singleInstance == null)
                    {
                        singleInstance = new SingletonClass();
                    }
                }
            }

            return singleInstance;
        }

        /// <summary>
        /// Some method.
        /// </summary>
        public void Foo()
        {
            //// doing something here ...
        }

        #region Nested helper class

        /// <summary>
        /// A nested class to create singleton instance.
        /// </summary>
        private class Nested
        {
            /// <summary>Readonly static instance of <see cref="SingletonClass" /></summary>
            internal static readonly SingletonClass Instance = new SingletonClass();
        }

        #endregion
    }

    /// <summary>
    /// A class to use singleton.
    /// </summary>
    public class SingletonClassUser
    {
        /// <summary>Instance of <see cref="SingletonClass" /></summary>
        private SingletonClass a;

        /// <summary>
        /// Get a singleton instance of the <see cref="SingletonClass" />.
        /// </summary>
        public void SomeMethod()
        {
            this.a = SingletonClass.CreateInstance();
            this.a.Foo();
        }
    }

    #endregion

    #region Question: Integer matrix ajacent 1s
    /*//
        Given matrix MxN of 0s and 1s, output all groups of adjacent 1s in form of SetID: (row,coll)... 1s are considered adjacent if they are adjacent horizontally or vertically.
        1 1 0 1 0 1
        1 1 1 1 0 1
        0 0 0 0 1 1
        1 0 1 0 1 0

        Output:
        A: (0,0)(1,0)(0,1)(1,1)(1,2)(1,3)(0,3)
        B: (0,5)(1,5)(2,4)(2,5)(3,4)
        C: (3,0)
        D: (3,2)
    //*/

    /// <summary>
    /// A class represents coordinate in an integer matrix.
    /// </summary>
    public class IntMatrixCoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntMatrixCoordinate" /> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x", Justification = "Coordinate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y", Justification = "Coordinate")]
        public IntMatrixCoordinate(uint x, uint y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>Gets or sets coordinate X.</summary>
        public uint X { get; set; }

        /// <summary>Gets or sets coordinate Y.</summary>
        public uint Y { get; set; }
    }

    /// <summary>
    /// The class represents an integer matrix.
    /// </summary>
    public class IntMatrix
    {
        /// <summary>The rows count.</summary>
        private int _rows = 0;

        /// <summary>The columns count.</summary>
        private int _columns = 0;

        /// <summary>The integers matrix.</summary>
        private int[,] _matrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntMatrix" /> class by number of rows and columns.
        /// </summary>
        /// <param name="rows">Number of rows in a matrix.</param>
        /// <param name="columns">Number of columns in a matrix.</param>
        public IntMatrix(int rows, int columns)
        {
            this._rows = rows;
            this._columns = columns;
            this._matrix = new int[this._rows, this._columns];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntMatrix" /> class from an existing matrix.
        /// </summary>
        /// <param name="matrix">An instance of an integer matrix.</param>
        public IntMatrix(int[,] matrix)
        {
            this._rows = matrix.GetLength(0);
            this._columns = matrix.GetLength(1);
            this._matrix = new int[this._rows, this._columns];

            for (var x = 0; x < this._rows; x++)
            {
                for (var y = 0; y < this._columns; y++)
                {
                    this._matrix[x, y] = matrix[x, y];
                }
            }
        }

        /// <summary>
        /// Find adjacent 1's in the matrix.
        /// </summary>
        /// <returns>A collection of coordinates.</returns>
        public IEnumerable<IntMatrixCoordinate> FindAdjacent1s()
        {
            bool[,] marker = new bool[this._rows, this._columns];

            List<IntMatrixCoordinate> result = new List<IntMatrixCoordinate>();

            for (var x = 0; x < this._rows; x++)
            {
                for (var y = 0; y < this._columns; y++)
                {
                    if (!marker[x, y] && this._matrix[x, y] == 1)
                    {
                        if (this.IsInMatrix(x - 1, y) && this._matrix[x - 1, y] == 1)
                        {
                            this.AddResult(x, y, marker, result);
                            this.AddResult(x - 1, y, marker, result);
                        }

                        if (this.IsInMatrix(x, y - 1) && this._matrix[x, y - 1] == 1)
                        {
                            this.AddResult(x, y, marker, result);
                            this.AddResult(x, y - 1, marker, result);
                        }

                        if (this.IsInMatrix(x + 1, y) && this._matrix[x + 1, y] == 1)
                        {
                            this.AddResult(x, y, marker, result);
                            this.AddResult(x + 1, y, marker, result);
                        }

                        if (this.IsInMatrix(x, y + 1) && this._matrix[x, y + 1] == 1)
                        {
                            this.AddResult(x, y, marker, result);
                            this.AddResult(x, y + 1, marker, result);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Add a new coordinate (x,y) to result based on the marker.
        /// </summary>
        /// <param name="x">The coordinate X.</param>
        /// <param name="y">The coordinate Y.</param>
        /// <param name="marker">The marker matrix.</param>
        /// <param name="result">The list of coordinates result.</param>
        private void AddResult(int x, int y, bool[,] marker, List<IntMatrixCoordinate> result)
        {
            if (!marker[x, y])
            {
                result.Add(new IntMatrixCoordinate((uint)x, (uint)y));
                marker[x, y] = true;
            }
        }

        /// <summary>
        /// Check if a coordinate has adjacent 1's in the matrix.
        /// </summary>
        /// <param name="x">The coordinate x.</param>
        /// <param name="y">The coordinate y.</param>
        /// <returns>return True if there is adjacent 1's; otherwise, False.</returns>
        /// <remarks>
        ///         x-1,y
        ///  x,y-1  (x,y)  x,y+1
        ///         x+1,y
        /// </remarks>
        private bool IsAjacent1s(int x, int y)
        {
            return
                this.IsInMatrix(x - 1, y) && this._matrix[x - 1, y] == 1 ||
                this.IsInMatrix(x, y - 1) && this._matrix[x - 1, y] == 1 ||
                this.IsInMatrix(x + 1, y) && this._matrix[x - 1, y] == 1 ||
                this.IsInMatrix(x, y + 1) && this._matrix[x - 1, y] == 1;
        }

        /// <summary>
        /// Check if a coordinate is valid in the matrix.
        /// </summary>
        /// <param name="x">The value of coordinate X.</param>
        /// <param name="y">The value of coordinate Y.</param>
        /// <returns>Returns true if the coordinate in range of the matrix; otherwise, false;</returns>
        private bool IsInMatrix(int x, int y)
        {
            return x >= 0 && x < this._rows && y >= 0 && y < this._columns;
        }
    }

    #endregion
}// namespace Coding.App.Interviews
