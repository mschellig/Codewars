namespace _1kyu_Implementations.Loopover
{
    public static class MoveHelpers
    {
        public static IEnumerable<Move> MoveBoard(this char[][] board, IEnumerable<Move> movelist)
        {
            return movelist.Select(board.MoveBoard);
        }

        public static Move MoveBoard(this char[][] board, Move move) =>
            MoveBoard(board, move.Direction, move.Index);

        public static IEnumerable<Move> MoveBoard(this char[][] board, IEnumerable<string> movelist)
        {
            var ret = new List<Move>();
            foreach (var move in movelist)
            {
                var moveArray = move.ToCharArray();
                ret.Add(board.MoveBoard(moveArray[0], int.Parse(moveArray[1].ToString())));
            }

            return ret;
        }

        public static IEnumerable<Move> MoveBoard(this char[][] board, IEnumerable<Tuple<char, int>> movelist)
        {
            return movelist.Select(move => board.MoveBoard(move.Item1, move.Item2));
        }

        public static IEnumerable<Move> MoveBoard(this char[][] board, IEnumerable<Tuple<string, int>> movelist)
        {
            return movelist.Select(move => board.MoveBoard(move.Item1, move.Item2));
        }

        public static Move MoveBoard(this char[][] board, char direction, int rowOrCol) =>
            MoveBoard(board, direction.ToString(), rowOrCol);

        public static Move MoveBoard(this char[][] board, string direction, int rowOrCol)
        {

            return direction.ToLower() switch
            {
                "r" => board.MoveRight(rowOrCol),
                "l" => board.MoveLeft(rowOrCol),
                "u" => board.MoveUp(rowOrCol),
                "d" => board.MoveDown(rowOrCol),
                _ => null
            };
        }

        private static Move MoveRight(this char[][] board, int index)
        {
            if (board.Length < index)
                throw new ArgumentException("MoveRight: Index out of Bounds");

            var newrow = new char[board[index].Length];
            newrow[0] = board[index][^1];
            for (var i = 1; i < board[index].Length; i++)
            {
                newrow[i] = board[index][i - 1];
            }
            board[index] = newrow;
            return new Move("R", index);
        }

        private static Move MoveLeft(this char[][] board, int index)
        {
            if (board.Length < index)
                throw new ArgumentException("MoveLeft: Index out of Bounds");

            var newrow = new char[board[index].Length];
            newrow[board[index].Length - 1] = board[index][0];
            for (var i = 1; i < board[index].Length; i++)
            {
                newrow[i - 1] = board[index][i];
            }
            board[index] = newrow;
            return new Move("L", index);
        }

        private static Move MoveUp(this char[][] board, int index)
        {
            if (board[0].Length < index)
                throw new ArgumentException("MoveUp: Index out of Bounds");

            var newcell = board[0][index];
            for (var i = 1; i < board.Length; i++)
            {
                board[i - 1][index] = board[i][index];
            }
            board[^1][index] = newcell;
            return new Move("U", index);
        }

        private static Move MoveDown(this char[][] board, int index)
        {
            if (board[0].Length < index)
                throw new ArgumentException("MoveUp: Index out of Bounds");

            var newcell = board[^1][index];
            for (var i = board.Length - 2; i >= 0; i--)
            {
                board[i + 1][index] = board[i][index];
            }
            board[0][index] = newcell;
            return new Move("D", index);
        }

        public static List<string> ConvertToString(List<Move> moves)
        {
            return moves.Select(move => $"{move.Direction}{move.Index}").ToList();
        }
    }
}
