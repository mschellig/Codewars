using NUnit.Framework.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

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

        public static Position FindPositionInBoard(this char[][] board1, char search)
        {
            var positionY = Array.FindIndex(board1, 0, board1.Length, x => x.Contains(search));
            var positionX = Array.FindIndex(board1[positionY], 0, board1[positionY].Length, x => x.Equals(search));
            return new Position(positionX, positionY);
        }

        public static bool IsParity(this char[][] board1, char[][] board2, Position dimensions)
        {
            var firstTile = board2[dimensions.Y - 1][dimensions.X];
            var firstTileShould = board1[dimensions.Y - 1][dimensions.X];
            var secondTile = board2[dimensions.Y][dimensions.X];
            var secondTileShould = board1[dimensions.Y][dimensions.X];

            return firstTile.Equals(secondTileShould) &&
                   secondTile.Equals(firstTileShould);
        }
    }
}
