using System;

namespace Editor
{
    public static class ArrayUtils
    {
        public static bool CanTrimArray(int[,] original)
        {
            // get array dimensions
            int rows = original.GetLength(0);
            int cols = original.GetLength(1);
        
            bool hasZeroOnEdge = false;
            // check top and bottom edges (rows)
            for (int j = 0; j < cols; j++)
            {
                if (original[0, j] == 0 || original[rows - 1, j] == 0)
                {
                    hasZeroOnEdge = true;
                    break;
                }
            }
            // check left and right edges (columns)
            for (int i = 0; i < rows && !hasZeroOnEdge; i++)
            {
                if (original[i, 0] == 0 || original[i, cols - 1] == 0)
                {
                    hasZeroOnEdge = true;
                }
            }
            return hasZeroOnEdge;
        }
        
        public static int[,] ResizeArray(int[,] original, int newRow, int newCol)
        {
            int originalRows = original.GetLength(0);
            int originalCols = original.GetLength(1);

            int[,] result = new int[newRow, newCol];

            int rowStart = Math.Max((newRow - originalRows) / 2, 0);
            int colStart = Math.Max((newCol - originalCols) / 2, 0);

            int rowEnd = Math.Min(rowStart + originalRows, newRow);
            int colEnd = Math.Min(colStart + originalCols, newCol);

            int originalRowStart = rowStart < (newRow - originalRows) / 2 ? (originalRows - newRow) / 2 : 0;
            int originalColStart = colStart < (newCol - originalCols) / 2 ? (originalCols - newCol) / 2 : 0;

            for (int i = rowStart; i < rowEnd; i++)
            {
                for (int j = colStart; j < colEnd; j++)
                {
                    result[i, j] = original[i - rowStart + originalRowStart, j - colStart + originalColStart];
                }
            }

            return result;
        }
        
        public static int[,] TrimArray(int[,] original)
        {
            // Get original array dimensions
            int originalRows = original.GetLength(0);
            int originalCols = original.GetLength(1);

            // Initialize bounds
            int minRow = originalRows, maxRow = -1, minCol = originalCols, maxCol = -1;

            // Find the bounds for trimming
            for (int i = 0; i < originalRows; i++)
            {
                for (int j = 0; j < originalCols; j++)
                {
                    if (original[i, j] != 0)  // Or whichever value you consider to be 'empty'
                    {
                        minRow = Math.Min(minRow, i);
                        maxRow = Math.Max(maxRow, i);
                        minCol = Math.Min(minCol, j);
                        maxCol = Math.Max(maxCol, j);
                    }
                }
            }

            // Compute new array dimensions
            int rows = maxRow - minRow + 1;
            int cols = maxCol - minCol + 1;

            // Create new array
            int[,] result = new int[rows, cols];

            // Copy values from old to new array
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = original[minRow + i, minCol + j];
                }
            }

            return result;
        }
    }
}