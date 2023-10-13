using System.ComponentModel.DataAnnotations;

namespace _1kyu_Implementations.Loopover
{
    public static class BoardHelpers
    {
        public static string GetReadableBoard(char[][] board)
        {
            var returnal = string.Empty;
            return board.Aggregate(returnal, (currentRow, row) => row.Aggregate(currentRow, (currentCol, c) => currentCol + c + " ") + "\n");
        }

        public static char[][] FormatStringToBoard(string board)
        {
            var splittedlines = board.Split('\n');

            var numRows = splittedlines.Length;
            var numCols = splittedlines[0].Length;

            var charArray = new char[numRows][];
            for (var i = 0; i < numRows; i++)
            {
                charArray[i] = new char[numCols];
                for (var j = 0; j < numCols; j++)
                {
                    charArray[i][j] = splittedlines[i][j];
                }
            }

            return charArray;
        }

        public static bool IsBoardEquals(this char[][] board1, char[][] board2)
        {
            var seed = string.Empty;
            var board1Readable = board1.Aggregate(seed, (currentRow, row) => row.Aggregate(currentRow, (currentCol, c) => currentCol + c) + "##");
            var board2Readable = board2.Aggregate(seed, (currentRow, row) => row.Aggregate(currentRow, (currentCol, c) => currentCol + c) + "##");

            return board1Readable.Equals(board2Readable, StringComparison.Ordinal);
        }
    }
}
