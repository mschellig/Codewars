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
    private static char[][] _workingboard;

    private List<string> _moveList = new();

    private int _dimension_x = 0;
    private int _dimension_y = 0;
    private Tuple<int, int> _dimensionTuple;

    private static int looper = 0;
    #endregion


    #region CONST
    public Loopover(char[][] startingboard, char[][] solvedboard)
    {
        _workingboard = (char[][])startingboard.Clone();
        _solvedboard = solvedboard;
        _dimension_y = startingboard.Length;
        _dimension_x = startingboard[0].Length;
        _dimensionTuple = Tuple.Create(_dimension_x, _dimension_y);

    }
    #endregion

    #region entrypoint

    public static List<string> Solve(char[][] mixedUpBoard, char[][] solvedBoard)
    {
        LoggingHelpers.LogHeadline("Start solving loopover");
        LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(mixedUpBoard));

        var loopover = new Loopover(mixedUpBoard, solvedBoard);
        if (!_workingboard.IsBoardEquals(loopover._solvedboard))
            loopover._moveList.AddRange(loopover.SolvePhase1());

        if (!_workingboard.IsBoardEquals(loopover._solvedboard))
            loopover._moveList.AddRange(loopover.SolvePhase2());

        if (!_workingboard.IsBoardEquals(loopover._solvedboard))
            loopover._moveList.AddRange(loopover.SolvePhase3());

        if (!_workingboard.IsBoardEquals(loopover._solvedboard))
        {
            if ((loopover._dimension_y % 2 == 0 && loopover._dimension_x % 2 == 0) && _workingboard.IsParity(loopover._solvedboard, loopover._dimensionTuple))
            {
                LoggingHelpers.LogHeadline("Start solving parity");
                loopover._moveList.AddRange(loopover.SolveEvenParity());
                LoggingHelpers.LogHeadline(_workingboard.IsBoardEquals(loopover._solvedboard)
                    ? "Loopopver solved"
                    : "Parity solving failed");
                return loopover._moveList;
            }
            else if ((loopover._dimension_y % 2 == 0) && _workingboard.IsParity(loopover._solvedboard, loopover._dimensionTuple))
            {
                //ManuallySolveParity(loopover);
                LoggingHelpers.LogHeadline("Start solving mixed parity");
                loopover._moveList.AddRange(loopover.SolveOddEvenParity(loopover._solvedboard));
                LoggingHelpers.LogHeadline(_workingboard.IsBoardEquals(loopover._solvedboard)
                    ? "Loopopver solved"
                    : "Parity solving failed");

                return loopover._moveList;
            }
            else if (loopover._dimension_x % 2 == 0 &&_workingboard.IsParity(loopover._solvedboard, loopover._dimensionTuple))
            {
                //ManuallySolveParity(loopover);
                LoggingHelpers.LogHeadline("Start solving mixed parity");
                loopover._moveList.AddRange(loopover.SolveEvenOddParity(loopover._solvedboard));
                LoggingHelpers.LogHeadline(_workingboard.IsBoardEquals(loopover._solvedboard)
                    ? "Loopopver solved"
                    : "Parity solving failed");

                return loopover._moveList;
            }
            else
            {
                LoggingHelpers.LogHeadline("Odd Parity - not solvable");
                return null;
            }



            //if (looper == 0)
            //    {
            //        looper++;
            //        var firsttry = Solve(_workingboard, loopover._solvedboard);
            //        loopover._moveList.AddRange(firsttry);
            //        if (_workingboard.IsBoardEquals(loopover._solvedboard))
            //        {
            //            Console.WriteLine("Done");

            //            return loopover._moveList;
            //        }
            //        else
            //        {
            //            Console.WriteLine("Not solvable");
            //            return null;
            //        }
            //    }

            //}

        }
        else
        {
            LoggingHelpers.LogHeadline("Loopopver solved");
            LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));
            return loopover._moveList;
        }
        LoggingHelpers.LogHeadline("Something went wrong");
        return loopover._moveList;
    }


    #endregion

    #region PHASES
    //Phase 1 solves the inital quad (x=length-1, y=length-1)
    private List<string> SolvePhase1()
    {
        LoggingHelpers.LogHeadline("Starting phase 1 - initial quad");

        var dimensionTuple = new Tuple<int, int>(_dimension_x, _dimension_y);
        var returnal = new List<string>();

        for (var i = 0; i < _dimension_y - 1; i++)
        {
            for (var j = 0; j < _dimension_x - 1; j++)
            {
                var currentPosition = _workingboard.FindPositionInBoard(_solvedboard[i][j]);
                LoggingHelpers.Log($"Next Character: {_solvedboard[i][j]} at current position X:{currentPosition.Item1}/Y:{currentPosition.Item2}");

                var movelist = PathHelpers.FindPathPhase1(currentPosition, new Tuple<int, int>(_dimension_x - 1, i), dimensionTuple);
                movelist.Add(new Tuple<char, int>('L', i));

                foreach (var m in movelist)
                {
                    _workingboard.MoveBoard(m.Item1, m.Item2);
                }
                returnal.AddRange(movelist.Select(x => $"{x.Item1}{x.Item2}"));
            }
        }

        LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));
        return returnal;
    }

    //Phase 2 solves the last column except the last field
    private List<string> SolvePhase2()
    {
        LoggingHelpers.LogHeadline("Starting phase 2 - last column (except last tile)");
        var returnal = new List<string>();
        var sparePosition = Tuple.Create(_dimension_x - 2, _dimension_y - 1);

        //Move top right tile in place (special case)
        var toprightPosition = _workingboard.FindPositionInBoard(_solvedboard[0][_dimension_x - 1]);
        LoggingHelpers.Log($"Next Character: {_solvedboard[0][_dimension_x - 1]} at current position X:{toprightPosition.Item1}/Y:{toprightPosition.Item2}");
        var toprightMoves = PathHelpers.FindPathPhase1(toprightPosition, Tuple.Create(_dimension_x - 1, 0), _dimensionTuple);
        _workingboard.MoveBoard(toprightMoves);
        returnal.AddRange(toprightMoves.Select(x => $"{x.Item1}{x.Item2}"));


        for (var i = 1; i < _dimension_y - 1; i++)
        {
            var currentPosition = _workingboard.FindPositionInBoard(_solvedboard[i][_dimension_x - 1]);
            LoggingHelpers.Log($"Next Character: {_solvedboard[i][_dimension_x - 1]} at current position X:{currentPosition.Item1}/Y:{currentPosition.Item2}");

            //move needed tile in range
            var tileInRangeMoves = PathHelpers.FindPathPhase2(currentPosition, sparePosition, _dimensionTuple);
            _workingboard.MoveBoard(tileInRangeMoves);
            returnal.AddRange(tileInRangeMoves.Select(x => $"{x.Item1}{x.Item2}"));

            //move corrected column in range
            var currentColumnBeginningPosition = _workingboard.FindPositionInBoard(_solvedboard[0][_dimension_x - 1]);
            var correctedColumnMoves = PathHelpers.FindPathPhase2(currentColumnBeginningPosition,
                Tuple.Create(_dimension_x - 1, _dimension_y - 1 - i), _dimensionTuple);
            _workingboard.MoveBoard(correctedColumnMoves);
            returnal.AddRange(correctedColumnMoves.Select(x => $"{x.Item1}{x.Item2}"));

            //move needed tile in place
            var movelist = PathHelpers.FindPathPhase2(sparePosition, new Tuple<int, int>(_dimension_x - 1, _dimension_y - 1), _dimensionTuple);

            //move corrected column to top
            currentColumnBeginningPosition = _workingboard.FindPositionInBoard(_solvedboard[0][_dimension_x - 1]);
            movelist.AddRange(PathHelpers.FindPathPhase2(currentColumnBeginningPosition, Tuple.Create(_dimension_x - 1, 0), _dimensionTuple));
            _workingboard.MoveBoard(movelist);
            returnal.AddRange(movelist.Select(x => $"{x.Item1}{x.Item2}"));

        }
        LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));

        return returnal;
    }

    //Phase 3 solves last row
    private List<string> SolvePhase3()
    {
        LoggingHelpers.LogHeadline("Starting phase 3 - last row ");
        var returnal = new List<string>();
        var sparePosition = Tuple.Create(_dimension_x - 1, _dimension_y - 1);
        var spareAttachPosition = Tuple.Create(_dimension_x - 2, _dimension_y - 1);
        var stuckPosition = Tuple.Create(_dimension_x - 1, _dimension_y - 2);

        for (var i = 1; i < _dimension_x - 1; i++)
        {
            var currentPosition = _workingboard.FindPositionInBoard(_solvedboard[_dimension_y - 1][i]);
            LoggingHelpers.Log($"Next Character: {_solvedboard[_dimension_y - 1][i]} at current position X:{currentPosition.Item1}/Y:{currentPosition.Item2}");

            //unstuck tile if stuck
            if (currentPosition.Equals(stuckPosition))
            {
                //move first tile to start
                var firstTilePosition = _workingboard.FindPositionInBoard(_solvedboard[_dimension_y - 1][0]);
                var firstTileMove = PathHelpers.FindPathPhase2(firstTilePosition, Tuple.Create(0, _dimension_y - 1),
                    _dimensionTuple);
                _workingboard.MoveBoard(firstTileMove);
                returnal.AddRange(firstTileMove.Select(x => $"{x.Item1}{x.Item2}"));

                //move stuck in spare
                _workingboard.MoveBoard('D', _dimension_x - 1);
                returnal.Add("D" + (_dimension_x - 1));
                //move stuck out of the way
                _workingboard.MoveBoard('R', _dimension_y - 1);
                returnal.Add("R" + (_dimension_y - 1));
                //return last column to start
                _workingboard.MoveBoard('U', _dimension_x - 1);
                returnal.Add("U" + (_dimension_x - 1));
                //move stuck in spare
                _workingboard.MoveBoard('L', _dimension_y - 1);
                returnal.Add("L" + (_dimension_y - 1));
            }

            //move needed tile in range
            //find position again, if unstuck
            currentPosition = _workingboard.FindPositionInBoard(_solvedboard[_dimension_y - 1][i]);
            var firstTileRangeMove = PathHelpers.FindPathPhase2(currentPosition, sparePosition, _dimensionTuple);
            _workingboard.MoveBoard(firstTileRangeMove);
            returnal.AddRange(firstTileRangeMove.Select(x => $"{x.Item1}{x.Item2}"));

            //move needed tile in storage
            _workingboard.MoveBoard('D', _dimension_x - 1);
            returnal.Add("D" + (_dimension_x - 1));

            //move previous tile in range
            var previousPosition = _workingboard.FindPositionInBoard(_solvedboard[_dimension_y - 1][i - 1]);
            var previousTileRangeMove =
                PathHelpers.FindPathPhase2(previousPosition, spareAttachPosition, _dimensionTuple);
            _workingboard.MoveBoard(previousTileRangeMove);
            returnal.AddRange(previousTileRangeMove.Select(x => $"{x.Item1}{x.Item2}"));

            //move needed tile back from storage
            _workingboard.MoveBoard('U', _dimension_x - 1);
            returnal.Add("U" + (_dimension_x - 1));

        }
        _workingboard.MoveBoard('L', _dimension_y - 1);
        returnal.Add("L" + (_dimension_y - 1));

        LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));

        return returnal;
    }


    //Parity Solver
    private List<string> SolveEvenParity()
    {
        var returnal = new List<string>();

        for (var i = 0; i < _dimension_x / 2; i++)
        {
            _workingboard.MoveBoard('L', _dimension_y - 1);
            returnal.Add("L" + (_dimension_y - 1));
            _workingboard.MoveBoard('D', _dimension_x - 1);
            returnal.Add("D" + (_dimension_x - 1));
            _workingboard.MoveBoard('L', _dimension_y - 1);
            returnal.Add("L" + (_dimension_y - 1));
            _workingboard.MoveBoard('U', _dimension_x - 1);
            returnal.Add("U" + (_dimension_x - 1));
        }
        _workingboard.MoveBoard('L', _dimension_y - 1);
        returnal.Add("L" + (_dimension_y - 1));
        return returnal;
    }

    private List<string> SolveEvenOddParity(char[][]solvedboard)
    {
        var returnal = new List<string>();
        var loopcounter = 0;
        while (loopcounter < 4 && !_workingboard.IsBoardEquals(solvedboard))
        {
            _workingboard.MoveBoard('L', _dimension_y - 1);
            returnal.Add("L" + (_dimension_y - 1));
            _workingboard.MoveBoard('D', _dimension_x - 1);
            returnal.Add("D" + (_dimension_x - 1));
            _workingboard.MoveBoard('L', _dimension_y - 1);
            returnal.Add("L" + (_dimension_y - 1));
            _workingboard.MoveBoard('U', _dimension_x - 1);
            returnal.Add("U" + (_dimension_x - 1));
            _workingboard.MoveBoard('L', _dimension_y - 1);
            returnal.Add("L" + (_dimension_y - 1));

            if (!_workingboard.IsBoardEquals(solvedboard))
            {
                _workingboard.MoveBoard('R', _dimension_y - 1);
                returnal.Add("R" + (_dimension_y - 1));
                loopcounter++;
            }
        }

        return returnal;
    }

    private List<string> SolveOddEvenParity(char[][] solvedboard)
    {
        var returnal = new List<string>();
        var loopcounter = 0;

        _workingboard.MoveBoard('R', _dimension_y - 1);
        returnal.Add("R" + (_dimension_y - 1));
        _workingboard.MoveBoard('D', _dimension_x - 1);
        returnal.Add("D" + (_dimension_x - 1));
        _workingboard.MoveBoard('L', _dimension_y - 1);
        returnal.Add("L" + (_dimension_y - 1));
        _workingboard.MoveBoard('U', _dimension_x - 1);
        returnal.Add("U" + (_dimension_x - 1));

        _workingboard.MoveBoard('D', _dimension_x - 1);
        returnal.Add("D" + (_dimension_x - 1));
        while (loopcounter < _dimension_y && !_workingboard.IsBoardEquals(solvedboard))
        {
            _workingboard.MoveBoard('R', _dimension_y - 1);
            returnal.Add("R" + (_dimension_y - 1));
            _workingboard.MoveBoard('D', _dimension_x - 1);
            returnal.Add("D" + (_dimension_x - 1));
            _workingboard.MoveBoard('L', _dimension_y - 1);
            returnal.Add("L" + (_dimension_y - 1));
            _workingboard.MoveBoard('D', _dimension_x - 1);
            returnal.Add("D" + (_dimension_x - 1));
        }

        return returnal;
    }
    #endregion

    private static void ManuallySolve(Loopover loopover)
    {
        //try again
        while (!_workingboard.IsBoardEquals(loopover._solvedboard))
        {
            var input = Console.ReadLine();
            var move = input.ToCharArray()[0].ToString();
            var index = int.Parse(input.ToCharArray()[1].ToString());
            _workingboard.MoveBoard(move, index);

            Console.WriteLine("---------- Move done: " + move + index + " ----------");
            Console.WriteLine(BoardHelpers.GetReadableBoard(_workingboard));


        }
    }

    private static void ManuallySolveParity(Loopover loopover)
    {
        //try again
        while (!_workingboard.IsBoardEquals(loopover._solvedboard))
        {
            var input = Console.ReadLine();
            var move = input;
            var index = 0;

            if (move.Equals("D") || move.Equals("U"))
            {
                index = loopover._dimension_x - 1;
            }
            else
            {
                index = loopover._dimension_y - 1;
            }

            _workingboard.MoveBoard(move, index);

            Console.WriteLine("---------- Move done: " + move + index + " ----------");
            Console.WriteLine(BoardHelpers.GetReadableBoard(_workingboard));


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

    public static Tuple<int, int> FindPositionInBoard(this char[][] board1, char search)
    {
        var positionY = Array.FindIndex(board1, 0, board1.Length, x => x.Contains(search));
        var positionX = Array.FindIndex(board1[positionY], 0, board1[positionY].Length, x => x.Equals(search));
        return new Tuple<int, int>(positionX, positionY);
    }

    public static bool IsParity(this char[][] board1, char[][] board2, Tuple<int, int> dimensions)
    {
        var firstTile = board2[dimensions.Item2 - 2][dimensions.Item1 - 1];
        var firstTileShould = board1[dimensions.Item2 - 2][dimensions.Item1 - 1];
        var secondTile = board2[dimensions.Item2 - 1][dimensions.Item1 - 1];
        var secondTileShould = board1[dimensions.Item2 - 1][dimensions.Item1 - 1];

        return firstTile.Equals(secondTileShould) &&
               secondTile.Equals(firstTileShould);
    }
}

public static class MoveHelpers
{
    public static char[][] MoveBoard(this char[][] board, List<string> movelist)
    {
        foreach (var move in movelist)
        {
            var charmoves = move.ToCharArray();
            board.MoveBoard(charmoves[0], int.Parse(charmoves[1].ToString()));
        }

        return board;
    }

    public static char[][] MoveBoard(this char[][] board, List<Tuple<char, int>> movelist)
    {
        foreach (var move in movelist)
        {
            board.MoveBoard(move.Item1, move.Item2);
        }
        return board;
    }

    public static char[][] MoveBoard(this char[][] board, List<Tuple<string, int>> movelist)
    {

        foreach (var move in movelist)
        {
            board.MoveBoard(move.Item1, move.Item2);
        }
        return board;
    }



    public static char[][] MoveBoard(this char[][] board, char direction, int rowOrCol) =>
        MoveBoard(board, direction.ToString(), rowOrCol);

    public static char[][] MoveBoard(this char[][] board, string direction, int rowOrCol)
    {
        //LoggingHelpers.Log($"New Movement initialized: {direction}{rowOrCol}");
        board = direction.ToLower() switch
        {
            "r" => MoveRight(board, rowOrCol),
            "l" => MoveLeft(board, rowOrCol),
            "u" => MoveUp(board, rowOrCol),
            "d" => MoveDown(board, rowOrCol),
            _ => board
        };
        //LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(board));

        return board;
    }

    private static char[][] MoveRight(this char[][] board, int index)
    {
        if (board.Length < index)
            throw new ArgumentException("MoveRight: Index out of Bounds");

        var newrow = new char[board[index].Length];
        newrow[0] = board[index][board[index].Length - 1];
        for (var i = 1; i < board[index].Length; i++)
        {
            newrow[i] = board[index][i - 1];
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
            newrow[i - 1] = board[index][i];
        }
        board[index] = newrow;
        return board;
    }

    private static char[][] MoveUp(this char[][] board, int index)
    {
        if (board[0].Length < index)
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
        if (board[0].Length < index)
            throw new ArgumentException("MoveUp: Index out of Bounds");

        var newcell = board[board.Length - 1][index];
        for (var i = board.Length - 2; i >= 0; i--)
        {
            board[i + 1][index] = board[i][index];
        }
        board[0][index] = newcell;
        return board;
    }
}

public static class PathHelpers
{
    public static List<Tuple<char, int>> FindPathPhase1(Tuple<int, int> currentposition, Tuple<int, int> targetposition, Tuple<int, int> dimensions)
    {
        var movelist = new List<Tuple<char, int>>();
        if (currentposition.Equals(targetposition)) return movelist;


        // for easier use, make dimensions match array logic
        dimensions = new Tuple<int, int>(dimensions.Item1 - 1, dimensions.Item2 - 1);
        var isNotLastRow = currentposition.Item2 != dimensions.Item2 && currentposition.Item1 != targetposition.Item1;

        //If item is not in last row and not same column, move one field down
        //avoids unwanted movements of correct tiles behind
        //REMEMBER: move column up at the end again
        if (isNotLastRow)
        {
            movelist.Add(new Tuple<char, int>('D', currentposition.Item1));
        }

        //Check which horizontal way is faster

        //var distanceLeft = CalculateDistance(currentposition.Item1, targetposition.Item1, dimensions.Item1);
        //var distanceRight = CalculateDistance(currentposition.Item1, targetposition.Item1+1, dimensions.Item1);

        var distanceHorizontal = CalculateHorizontalDistance(currentposition.Item1, targetposition.Item1, dimensions.Item1 + 1); // 'BAC' has a count of 3
                                                                                                                                 //Add horizontal moves to list
        for (var i = 0; i < distanceHorizontal.Item2; i++)
        {
            movelist.Add(new Tuple<char, int>(distanceHorizontal.Item1, isNotLastRow ? currentposition.Item2 + 1 : currentposition.Item2));
        }

        //check which vertical way is faster
        var distanceVertical = CalculateVerticalDistance(currentposition.Item2, targetposition.Item2, dimensions.Item2 + 1); // 'BAC' has a count of 3
                                                                                                                             //Add vertical moves to list
        for (var i = 0; i < distanceVertical.Item2; i++)
        {
            movelist.Add(new Tuple<char, int>(distanceVertical.Item1, targetposition.Item1));
        }

        //Add Upwards move according first task
        if (isNotLastRow)
        {
            movelist.Add(new Tuple<char, int>('U', targetposition.Item1));
            movelist.Add(new Tuple<char, int>('U', currentposition.Item1));
        }
        return movelist;
    }

    public static List<Tuple<char, int>> FindPathPhase2(Tuple<int, int> currentposition, Tuple<int, int> targetposition, Tuple<int, int> dimensions)
    {
        var movelist = new List<Tuple<char, int>>();
        if (currentposition.Equals(targetposition)) return movelist;

        // for easier use, make dimensions match array logic
        dimensions = new Tuple<int, int>(dimensions.Item1 - 1, dimensions.Item2 - 1);

        //check which vertical way is faster
        var distanceVertical = CalculateVerticalDistance(currentposition.Item2, targetposition.Item2, dimensions.Item2 + 1); // 'BAC' has a count of 3
                                                                                                                             //Add vertical moves to list
        for (var i = 0; i < distanceVertical.Item2; i++)
        {
            movelist.Add(new Tuple<char, int>(distanceVertical.Item1, currentposition.Item1));
        }

        //Check which horizontal way is faster
        var distanceHorizontal = CalculateHorizontalDistance(currentposition.Item1, targetposition.Item1, dimensions.Item1 + 1); // 'BAC' has a count of 3
                                                                                                                                 //Add horizontal moves to list
        for (var i = 0; i < distanceHorizontal.Item2; i++)
        {
            movelist.Add(new Tuple<char, int>(distanceHorizontal.Item1, targetposition.Item2));
        }


        return movelist;
    }


    private static Tuple<char, int> CalculateHorizontalDistance(int startPosition, int targetPosition, int dimension)
    {
        var distanceLeft = (startPosition - targetPosition + dimension) % dimension;
        var distanceRight = (targetPosition - startPosition + dimension) % dimension;
        return distanceLeft < distanceRight
            ? new Tuple<char, int>('L', distanceLeft)
            : new Tuple<char, int>('R', distanceRight);
    }

    private static Tuple<char, int> CalculateVerticalDistance(int startPosition, int targetPosition, int dimension)
    {
        var distanceUp = (startPosition - targetPosition + dimension) % dimension;
        var distanceDown = (targetPosition - startPosition + dimension) % dimension;
        return distanceUp < distanceDown
            ? new Tuple<char, int>('U', distanceUp)
            : new Tuple<char, int>('D', distanceDown);
    }
}
