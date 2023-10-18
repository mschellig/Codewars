using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;

public class Loopover
{
    #region VAR

    private char[][] _solvedboard;
    private char[][] _workingboard;

    private List<Move> _moveList = new();

    private Position _arrayDimensionPosition;
    private Position _realDimensionTuple;

    #endregion


    #region CONST

    public Loopover(char[][] startingboard, char[][] solvedboard)
    {
        _workingboard = (char[][])startingboard.Clone();
        _solvedboard = solvedboard;
        _realDimensionTuple = new Position(startingboard[0].Length, startingboard.Length);
        _arrayDimensionPosition = new Position(startingboard[0].Length - 1, startingboard.Length - 1);
    }

    #endregion

    #region entrypoint

    public static List<string> Solve(char[][] mixedUpBoard, char[][] solvedBoard)
    {
        LoggingHelpers.LogHeadline("Start solving loopover");
        LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(mixedUpBoard));
        var loopover = new Loopover(mixedUpBoard, solvedBoard);

        if (!loopover._workingboard.IsBoardEquals(loopover._solvedboard))
            loopover._moveList.AddRange(loopover.SolvePhase1());

        if (!loopover._workingboard.IsBoardEquals(loopover._solvedboard))
            loopover._moveList.AddRange(loopover.SolvePhase2());

        if (!loopover._workingboard.IsBoardEquals(loopover._solvedboard))
            loopover._moveList.AddRange(loopover.SolvePhase3());

        if (!loopover._workingboard.IsBoardEquals(loopover._solvedboard))
        {
            if ((loopover._realDimensionTuple.X % 2 == 0 && loopover._realDimensionTuple.Y % 2 == 0) &&
                loopover._workingboard.IsParity(loopover._solvedboard, loopover._arrayDimensionPosition))
            {
                LoggingHelpers.LogHeadline("Start solving parity");
                loopover._moveList.AddRange(loopover.SolveEvenParity());
                LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(loopover._workingboard));
                return MoveHelpers.ConvertToString(loopover._moveList);
            }

            if ((loopover._realDimensionTuple.Y % 2 == 0) &&
                loopover._workingboard.IsParity(loopover._solvedboard, loopover._arrayDimensionPosition))
            {
                LoggingHelpers.LogHeadline("Start solving mixed parity");
                loopover._moveList.AddRange(loopover.SolveOddEvenParity(loopover._solvedboard));
                LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(loopover._workingboard));
                return MoveHelpers.ConvertToString(loopover._moveList);
            }

            if (loopover._realDimensionTuple.X % 2 == 0 &&
                loopover._workingboard.IsParity(loopover._solvedboard, loopover._arrayDimensionPosition))
            {
                LoggingHelpers.LogHeadline("Start solving mixed parity");
                loopover._moveList.AddRange(loopover.SolveEvenOddParity(loopover._solvedboard));
                LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(loopover._workingboard));
                return MoveHelpers.ConvertToString(loopover._moveList);
            }
        }
        else
        {
            LoggingHelpers.LogHeadline("Loopopver solved");
            LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(loopover._workingboard));
            return MoveHelpers.ConvertToString(loopover._moveList);
        }

        return null;

    }


    #endregion

    #region PHASES

    //Phase 1 solves the inital quad (x=length-1, y=length-1)
    private IEnumerable<Move> SolvePhase1()
    {
        LoggingHelpers.LogHeadline("Starting phase 1 - initial quad");
        var returnal = new List<Move>();

        for (var i = 0; i < _arrayDimensionPosition.Y; i++)
        {
            for (var j = 0; j < _arrayDimensionPosition.X; j++)
            {
                var currentPosition = _workingboard.FindPositionInBoard(_solvedboard[i][j]);
                var movelist = PathHelpers.FindPathPhase1(currentPosition, new Position(_arrayDimensionPosition.X, i), _arrayDimensionPosition).ToList();
                movelist.Add(new Move("L", i));

                returnal.AddRange(_workingboard.MoveBoard(movelist));
            }
        }
        LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));
        return returnal;
    }

    //Phase 2 solves the last column except the last field
    private IEnumerable<Move> SolvePhase2()
    {
        LoggingHelpers.LogHeadline("Starting phase 2 - last column (except last tile)");
        var sparePosition = new Position(_arrayDimensionPosition.X - 1, _arrayDimensionPosition.Y);

        //Move top right tile in place (special case)
        var toprightPosition = _workingboard.FindPositionInBoard(_solvedboard[0][_arrayDimensionPosition.X]);
        var toprightMoves = PathHelpers.FindPathPhase1(toprightPosition, new Position(_arrayDimensionPosition.X, 0), _arrayDimensionPosition);
        var returnal = _workingboard.MoveBoard(toprightMoves).ToList();


        for (var i = 1; i < _arrayDimensionPosition.Y; i++)
        {
            var currentPosition = _workingboard.FindPositionInBoard(_solvedboard[i][_arrayDimensionPosition.X]);

            //move needed tile in range
            var tileInRangeMoves = PathHelpers.FindPathPhase2(currentPosition, sparePosition, _arrayDimensionPosition);
            returnal.AddRange(_workingboard.MoveBoard(tileInRangeMoves));

            //move corrected column in range
            var currentColumnBeginningPosition = _workingboard.FindPositionInBoard(_solvedboard[0][_arrayDimensionPosition.X]);
            var correctedColumnMoves = PathHelpers.FindPathPhase2(currentColumnBeginningPosition, new Position(_arrayDimensionPosition.X, _arrayDimensionPosition.Y - i), _arrayDimensionPosition);
            returnal.AddRange(_workingboard.MoveBoard(correctedColumnMoves));

            //move needed tile in place
            var movelist = PathHelpers.FindPathPhase2(sparePosition, new Position(_arrayDimensionPosition.X, _arrayDimensionPosition.Y), _arrayDimensionPosition).ToList();

            //move corrected column to top
            currentColumnBeginningPosition = _workingboard.FindPositionInBoard(_solvedboard[0][_arrayDimensionPosition.X]);
            movelist.AddRange(PathHelpers.FindPathPhase2(currentColumnBeginningPosition, new Position(_arrayDimensionPosition.X, 0), _arrayDimensionPosition));
            returnal.AddRange(_workingboard.MoveBoard(movelist));
        }

        LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));

        return returnal;
    }

    //Phase 3 solves last row
    private IEnumerable<Move> SolvePhase3()
    {
        LoggingHelpers.LogHeadline("Starting phase 3 - last row ");
        var returnal = new List<Move>();

        var sparePosition = new Position(_arrayDimensionPosition.X, _arrayDimensionPosition.Y);
        var spareAttachPosition = new Position(_arrayDimensionPosition.X - 1, _arrayDimensionPosition.Y);
        var stuckPosition = new Position(_arrayDimensionPosition.X, _arrayDimensionPosition.Y - 1);

        for (var i = 1; i < _arrayDimensionPosition.X; i++)
        {
            var currentPosition = _workingboard.FindPositionInBoard(_solvedboard[_arrayDimensionPosition.Y][i]);

            //unstuck tile if stuck
            if (currentPosition.Equals(stuckPosition))
            {
                //move first tile to start
                var firstTilePosition = _workingboard.FindPositionInBoard(_solvedboard[_arrayDimensionPosition.Y][0]);
                var firstTileMove = PathHelpers.FindPathPhase2(firstTilePosition, new Position(0, _arrayDimensionPosition.Y), _arrayDimensionPosition);
                returnal.AddRange(_workingboard.MoveBoard(firstTileMove));

                //move stuck in spare
                returnal.Add(_workingboard.MoveBoard('D', _arrayDimensionPosition.X));
                //move stuck out of the way
                returnal.Add(_workingboard.MoveBoard('R', _arrayDimensionPosition.Y));
                //return last column to start
                returnal.Add(_workingboard.MoveBoard('U', _arrayDimensionPosition.X));
                //move stuck in spare
                returnal.Add(_workingboard.MoveBoard('L', _arrayDimensionPosition.Y));
            }

            //move needed tile in range
            //find position again, if unstuck
            currentPosition = _workingboard.FindPositionInBoard(_solvedboard[_arrayDimensionPosition.Y][i]);
            var firstTileRangeMove = PathHelpers.FindPathPhase2(currentPosition, sparePosition, _arrayDimensionPosition);
            returnal.AddRange(_workingboard.MoveBoard(firstTileRangeMove));

            //move needed tile in storage
            returnal.Add(_workingboard.MoveBoard('D', _arrayDimensionPosition.X));

            //move previous tile in range
            var previousPosition = _workingboard.FindPositionInBoard(_solvedboard[_arrayDimensionPosition.Y][i - 1]);
            var previousTileRangeMove =
                PathHelpers.FindPathPhase2(previousPosition, spareAttachPosition, _arrayDimensionPosition);
            returnal.AddRange(_workingboard.MoveBoard(previousTileRangeMove));

            //move needed tile back from storage
            returnal.Add(_workingboard.MoveBoard("U", _arrayDimensionPosition.X));

        }

        returnal.Add(_workingboard.MoveBoard('L', _arrayDimensionPosition.Y));
        LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));

        return returnal;
    }


    //Parity Solver
    private IEnumerable<Move> SolveEvenParity()
    {
        var returnal = new List<Move>();

        for (var i = 0; i < _realDimensionTuple.X / 2; i++)
        {
            returnal.Add(_workingboard.MoveBoard('L', _arrayDimensionPosition.Y));
            returnal.Add(_workingboard.MoveBoard('D', _arrayDimensionPosition.X));
            returnal.Add(_workingboard.MoveBoard('L', _arrayDimensionPosition.Y));
            returnal.Add(_workingboard.MoveBoard('U', _arrayDimensionPosition.X));
        }
        returnal.Add(_workingboard.MoveBoard('L', _arrayDimensionPosition.Y));
        return returnal;
    }

    private IEnumerable<Move> SolveEvenOddParity(char[][] solvedboard)
    {
        var loopcounter = 0;
        while (loopcounter < 4 && !_workingboard.IsBoardEquals(solvedboard))
        {
            yield return _workingboard.MoveBoard('L', _arrayDimensionPosition.Y);
            yield return _workingboard.MoveBoard('D', _arrayDimensionPosition.X);
            yield return _workingboard.MoveBoard('L', _arrayDimensionPosition.Y);
            yield return _workingboard.MoveBoard('U', _arrayDimensionPosition.X);
            yield return _workingboard.MoveBoard('L', _arrayDimensionPosition.Y);

            if (!_workingboard.IsBoardEquals(solvedboard))
            {
                yield return _workingboard.MoveBoard('R', _arrayDimensionPosition.Y);
                loopcounter++;
            }
        }
    }

    private IEnumerable<Move> SolveOddEvenParity(char[][] solvedboard)
    {
        var loopcounter = 0;

        yield return _workingboard.MoveBoard('R', _arrayDimensionPosition.Y);
        yield return _workingboard.MoveBoard('D', _arrayDimensionPosition.X);
        yield return _workingboard.MoveBoard('L', _arrayDimensionPosition.Y);
        yield return _workingboard.MoveBoard('U', _arrayDimensionPosition.X);

        yield return _workingboard.MoveBoard('D', _arrayDimensionPosition.X);
        while (loopcounter < _arrayDimensionPosition.Y && !_workingboard.IsBoardEquals(solvedboard))
        {
            yield return _workingboard.MoveBoard('R', _arrayDimensionPosition.Y);
            yield return _workingboard.MoveBoard('D', _arrayDimensionPosition.X);
            yield return _workingboard.MoveBoard('L', _arrayDimensionPosition.Y);
            yield return _workingboard.MoveBoard('D', _arrayDimensionPosition.X);
            loopcounter++;
        }
    }

    #endregion
}

public static class BoardHelpers
{
    public static string GetReadableBoard(char[][] board)
    {
        var returnal = string.Empty;
        return board.Aggregate(returnal,
            (currentRow, row) => row.Aggregate(currentRow, (currentCol, c) => currentCol + c + " ") + "\n");
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
        var board1Readable = board1.Aggregate(seed,
            (currentRow, row) => row.Aggregate(currentRow, (currentCol, c) => currentCol + c) + "##");
        var board2Readable = board2.Aggregate(seed,
            (currentRow, row) => row.Aggregate(currentRow, (currentCol, c) => currentCol + c) + "##");
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

public static class PathHelpers
{
    public static IEnumerable<Move> FindPathPhase1(Position currentposition, Position targetposition, Position dimensions)
    {
        if (currentposition.Equals(targetposition)) yield break;

        var isNotLastRow = currentposition.Y != dimensions.Y && currentposition.X != targetposition.X;

        //If item is not in last row and not same column, move one field down
        //avoids unwanted movements of correct tiles behind
        //REMEMBER: move column up at the end again
        if (isNotLastRow)
        {
            yield return new Move("D", currentposition.X);
        }

        var distanceHorizontal = CalculateHorizontalDistance(currentposition.X, targetposition.X, dimensions.X + 1);
        for (var i = 0; i < distanceHorizontal.Item2; i++)
        {
            yield return new Move(distanceHorizontal.Item1.ToString(), isNotLastRow ? currentposition.Y + 1 : currentposition.Y);
        }

        //check which vertical way is faster
        var distanceVertical = CalculateVerticalDistance(currentposition.Y, targetposition.Y, dimensions.Y + 1);
        for (var i = 0; i < distanceVertical.Item2; i++)
        {
            yield return new Move(distanceVertical.Item1.ToString(), targetposition.X);
        }

        //Add Upwards move according first task
        if (isNotLastRow)
        {
            yield return new Move("U", targetposition.X);
            yield return new Move("U", currentposition.X);
        }
    }

    public static IEnumerable<Move> FindPathPhase2(Position currentposition, Position targetposition, Position dimensions)
    {
        if (currentposition.Equals(targetposition)) yield break;

        //check which vertical way is faster
        var distanceVertical = CalculateVerticalDistance(currentposition.Y, targetposition.Y, dimensions.Y + 1);
        for (var i = 0; i < distanceVertical.Item2; i++)
        {
            yield return new Move(distanceVertical.Item1, currentposition.X);
        }

        //Check which horizontal way is faster
        var distanceHorizontal = CalculateHorizontalDistance(currentposition.X, targetposition.X, dimensions.X + 1);
        for (var i = 0; i < distanceHorizontal.Item2; i++)
        {
            yield return new Move(distanceHorizontal.Item1, targetposition.Y);
        }
    }


    private static Tuple<string, int> CalculateHorizontalDistance(int startPosition, int targetPosition, int dimension)
    {
        var distanceLeft = (startPosition - targetPosition + dimension) % dimension;
        var distanceRight = (targetPosition - startPosition + dimension) % dimension;
        return distanceLeft < distanceRight
            ? new Tuple<string, int>("L", distanceLeft)
            : new Tuple<string, int>("R", distanceRight);
    }

    private static Tuple<string, int> CalculateVerticalDistance(int startPosition, int targetPosition, int dimension)
    {
        var distanceUp = (startPosition - targetPosition + dimension) % dimension;
        var distanceDown = (targetPosition - startPosition + dimension) % dimension;
        return distanceUp < distanceDown
            ? new Tuple<string, int>("U", distanceUp)
            : new Tuple<string, int>("D", distanceDown);
    }
}

public class Move
{
    public Move(string direction, int index)
    {
        Direction = direction;
        Index = index;
    }

    public string Direction { get; set; }
    public int Index { get; set; }

}

public class Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }

    public override bool Equals(Object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            var p = (Position)obj;
            return (X == p.X) && (Y == p.Y);
        }
    }
}

public static class LoggingHelpers
{
    public static void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now:dd.MM.yyyy - HH:mm:ss}: {message}");
    }

    public static void LogHeadline(string message)
    {
        Console.WriteLine("#######################################");
        Console.WriteLine(message);
        Console.WriteLine("#######################################");
    }

    public static void LogBlank(string message)
    {
        Console.WriteLine(message);
    }
}