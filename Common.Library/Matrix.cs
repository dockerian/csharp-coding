using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Extensions;

namespace Common.Library
{
    public class Matrix<T>
    {
        #region Fields

        private int _columns = 0;
        private int _rows = 0;

        #endregion

        #region Constructors

        public Matrix(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;

            Elements = new T[rows, columns];
        }

        public Matrix(T[,] matrix, bool cloneElements = false)
        {
            if (matrix == null) return;

            _rows = matrix.GetLength(0);
            _columns = matrix.GetLength(1);

            if (cloneElements)
            {
                Elements = matrix.Clone<T>();
            }
            else // taking the reference
            {
                Elements = matrix;
            }
        }

        #endregion

        #region Properties

        private T[,] _elements = new T[0, 0];
        public T[,] Elements
        {
            get { return _elements; }
            private set
            {
                _elements = value;
            }
        }

        public T this[int row, int column]
        {
            get
            {
                return this.Elements[row, column];
            }
            set
            {
                this.Elements[row, column] = value;
            }
        }

        public int Columns { get { return _columns; } }
        public int Rows { get { return _rows; } }

        #endregion

        #region Functions

        /*//NOTE: Use Clone in CommonExtensions instead
        private T[,] Clone(T[,] matrix)
        {
            if (matrix == null || matrix.Length < 1) return new T[0, ];

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            T[,] clone = new T[rows, columns];

            for(var x = 0; x < rows; x++)
            {
                for(var y = 0; y < columns; y++)
                {
                    clone[x, y] = matrix[x, y];
                }
            }

            return clone;
        }
        //*/

        #endregion

        #region Methods

        public Matrix<T> Clone()
        {
            Matrix<T> clone = new Matrix<T>(_rows, _columns);

            for(var x = 0; x < _rows; x++)
            {
                for(var y = 0; y < _columns; y++)
                {
                    clone.Elements[x, y] = this.Elements[x, y];
                }
            }

            return clone;
        }

        public bool Compare(Matrix<T> matrix)
        {
            bool isNotNull = matrix != null && Elements != null;

            return isNotNull && Elements.Compare(matrix.Elements);
        }

        public T[,] FlipHorizontally()
        {
            T[,] rotation = new T[_rows, _columns];

            for(var x = 0; x < _rows; x++)
            {
                for(var y = 0; y < _columns; y++)
                {
                    rotation[x, y] = this.Elements[x, _columns - y - 1];
                }
            }

            return rotation;
        }

        public T[,] FlipVertically()
        {
            T[,] rotation = new T[_rows, _columns];

            for(var x = 0; x < _rows; x++)
            {
                for(var y = 0; y < _columns; y++)
                {
                    rotation[x, y] = this.Elements[_rows - x - 1, y];
                }
            }

            return rotation;
        }

        public T[,] RotateClockwise()
        {
            T[,] rotation = new T[_columns, _rows];

            for(var y = 0; y < _columns; y++)
            {
                for(var x = 0; x < _rows; x++)
                {
                    rotation[y, x] = this.Elements[_rows - x - 1, y];
                }
            }

            return rotation;
        }

        public T[,] RotateClockwise180()
        {
            T[,] rotation = new T[_rows, _columns];

            for(var x = 0; x < _rows; x++)
            {
                for(var y = 0; y < _columns; y++)
                {
                    rotation[x, y] = this.Elements[_rows - x - 1, _columns - y - 1];
                }
            }

            return rotation;
        }

        public T[,] RotateCounterClockwise()
        {
            T[,] rotation = new T[_columns, _rows];

            for(var y = 0; y < _columns; y++)
            {
                for(var x = 0; x < _rows; x++)
                {
                    rotation[y, x] = this.Elements[x, _columns - y - 1];
                }
            }

            return rotation;
        }

        #endregion

    }
}
