namespace _1kyu_Implementations.Loopover
{
    public static class MoveHelpers
    {
        public static char[][] MoveBoard(this char[][] board, char direction, int rowOrCol) =>
            MoveBoard(board, direction.ToString(), rowOrCol);

        public static char[][] MoveBoard(this char[][] board, string direction, int rowOrCol)
        {
            return direction.ToLower() switch
            {
                "r" => MoveRight(board, rowOrCol),
                "l" => MoveLeft(board, rowOrCol),
                "u" => MoveUp(board, rowOrCol),
                "d" => MoveDown(board, rowOrCol),
                _ => throw new ArgumentException("Invalid movement parameter received")
            };
        }

        private static char[][] MoveRight(this char[][] board, int index)
        {
            if (board.Length < index)
                throw new ArgumentException("MoveRight: Index out of Bounds");

            var newrow = new char[board[index].Length];
            newrow[0] = board[index][board[index].Length-1];
            for (var i = 1; i < board[index].Length; i++)
            {
                newrow[i] = board[index][i-1];
            }
            board[index] = newrow;
            return board;
        }

        private static char[][] MoveLeft(this char[][] board, int index)
        {
            if (board.Length < index)
                throw new ArgumentException("MoveLeft: Index out of Bounds");

            var newrow = new char[board[index].Length];
            newrow[board[index].Length - 1] = board[index][0];
            for (var i = 1; i < board[index].Length; i++)
            {
                newrow[i-1] = board[index][i];
            }
            board[index] = newrow;
            return board;
        }

        private static char[][] MoveUp(this char[][] board, int index)
        {
            if (board.Length < index)
                throw new ArgumentException("MoveUp: Index out of Bounds");

            var newcell = board[0][index];
            for (var i = 1; i < board.Length; i++)
            {
                board[i - 1][index] = board[i][index];
            }
            board[board.Length - 1][index] = newcell;
            return board;
        }

        private static char[][] MoveDown(this char[][] board, int index)
        {
            if (board.Length < index)
                throw new ArgumentException("MoveUp: Index out of Bounds");
            //var tempboard = (char[][])

            var newcell = board[board.Length-1][index];
            for (var i = board.Length-2; i >= 0; i--)
            {
                board[i+1][index] = board[i][index];
            }
            board[0][index] = newcell;
            return board;
        }
    }
}
