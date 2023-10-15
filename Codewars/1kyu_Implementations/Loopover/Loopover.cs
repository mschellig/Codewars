using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace _1kyu_Implementations.Loopover
{
    public class Loopover
    {
        #region VAR

        private char[][] _startingboard;
        private char[][] _solvedboard;
        private char[][] _workingboard;

        private List<string> _moveList =new();

        private int _dimension_x = 0;
        private int _dimension_y = 0;
        private Tuple<int, int> _dimensionTuple;

        private static int looper = 0;
        #endregion

        #region PROP
        #endregion

        #region CONST
        public Loopover(char[][] startingboard, char[][] solvedboard)
        {
            _startingboard = startingboard;
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
            if (!loopover._workingboard.IsBoardEquals(loopover._solvedboard))
                loopover._moveList.AddRange(loopover.SolvePhase1());
            
            if (!loopover._workingboard.IsBoardEquals(loopover._solvedboard))
                loopover._moveList.AddRange(loopover.SolvePhase2());
            
            if (!loopover._workingboard.IsBoardEquals(loopover._solvedboard))
                loopover._moveList.AddRange(loopover.SolvePhase3());

            if (!loopover._workingboard.IsBoardEquals(loopover._solvedboard))
            {
                if (loopover._dimension_x % 2 == 0 && loopover._workingboard.IsParity(loopover._solvedboard, loopover._dimensionTuple))
                {
                    LoggingHelpers.LogHeadline("Start solving parity");
                    loopover._moveList.AddRange(loopover.SolveEvenParity());
                    LoggingHelpers.LogHeadline(loopover._workingboard.IsBoardEquals(loopover._solvedboard)
                        ? "Loopopver solved"
                        : "Parity solving failed");
                    return loopover._moveList;
                }
                else if (loopover._workingboard.IsParity(loopover._solvedboard, loopover._dimensionTuple))
                {
                    LoggingHelpers.LogHeadline("Odd Parity - not solvable");
                    if (looper  == 0)
                    {
                        looper++;
                        return Solve(loopover._workingboard, loopover._solvedboard);
                    }

                    return null;
                }

            }
            else
            {
                LoggingHelpers.LogHeadline("Loopopver solved");
                LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(loopover._workingboard));
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

            var dimensionTuple = new Tuple<int,int>(_dimension_x, _dimension_y);
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
                        //LoggingHelpers.Log($"New Movement initialized: {m.Item1}{m.Item2}");
                        _workingboard.MoveBoard(m.Item1, m.Item2);
                        //LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));
                    }
                    returnal.AddRange(movelist.Select(x=>$"{x.Item1}{x.Item2}"));
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


            for (var i = 1; i < _dimension_y - 1; i++)
            {
                var currentPosition = _workingboard.FindPositionInBoard(_solvedboard[i][_dimension_x - 1]);
                LoggingHelpers.Log($"Next Character: {_solvedboard[i][_dimension_x - 1]} at current position X:{currentPosition.Item1}/Y:{currentPosition.Item2}");

                //move needed tile in range
                _workingboard.MoveBoard(PathHelpers.FindPathPhase2(currentPosition, sparePosition, _dimensionTuple));
                
                //move corrected column in range
                var currentColumnBeginningPosition = _workingboard.FindPositionInBoard(_solvedboard[0][_dimension_x - 1]);
                _workingboard.MoveBoard(PathHelpers.FindPathPhase2(currentColumnBeginningPosition, Tuple.Create(_dimension_x - 1, _dimension_y - 1-i), _dimensionTuple));

                //move needed tile in place
                var movelist = PathHelpers.FindPathPhase2(sparePosition, new Tuple<int, int>(_dimension_x - 1, _dimension_y - 1), _dimensionTuple);
                
                //move corrected column to top
                currentColumnBeginningPosition = _workingboard.FindPositionInBoard(_solvedboard[0][_dimension_x - 1]);
                movelist.AddRange(PathHelpers.FindPathPhase2(currentColumnBeginningPosition, Tuple.Create(_dimension_x - 1, 0), _dimensionTuple));
                _workingboard.MoveBoard(movelist);
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
                var currentPosition = _workingboard.FindPositionInBoard(_solvedboard[_dimension_y-1][i]);
                LoggingHelpers.Log($"Next Character: {_solvedboard[_dimension_y - 1][i]} at current position X:{currentPosition.Item1}/Y:{currentPosition.Item2}");

                //unstuck tile if stuck
                if (currentPosition.Equals(stuckPosition))
                {
                    //move first tile to start
                    var firstTilePosition = _workingboard.FindPositionInBoard(_solvedboard[_dimension_y-1][0]);
                    _workingboard.MoveBoard(PathHelpers.FindPathPhase2(firstTilePosition, Tuple.Create(0, _dimension_y-1), _dimensionTuple));
                    //move stuck in spare
                    _workingboard.MoveBoard('D', _dimension_x - 1);
                    //move stuck out of the way
                    _workingboard.MoveBoard('R', _dimension_y - 1);
                    //return last column to start
                    _workingboard.MoveBoard('U', _dimension_x - 1);
                    //move stuck in spare
                    _workingboard.MoveBoard('L', _dimension_y - 1);
                }

                //move needed tile in range
                //find position again, if unstuck
                currentPosition = _workingboard.FindPositionInBoard(_solvedboard[_dimension_y - 1][i]);
                _workingboard.MoveBoard(PathHelpers.FindPathPhase2(currentPosition, sparePosition, _dimensionTuple));

                //move needed tile in storage
                _workingboard.MoveBoard('D', _dimension_x - 1);

                //move previous tile in range
                var previousPosition = _workingboard.FindPositionInBoard(_solvedboard[_dimension_y - 1][i-1]);
                _workingboard.MoveBoard(PathHelpers.FindPathPhase2(previousPosition, spareAttachPosition, _dimensionTuple));

                //move needed tile back from storage
                _workingboard.MoveBoard('U', _dimension_x - 1);

            }
            _workingboard.MoveBoard('L', _dimension_y - 1);

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
        #endregion

        private static void ManuallySolve(Loopover loopover)
        {
            //try again
            while (!loopover._workingboard.IsBoardEquals(loopover._solvedboard))
            {
                var input = Console.ReadLine();
                var move = input.ToCharArray()[0].ToString();
                var index = int.Parse(input.ToCharArray()[1].ToString());
                loopover._workingboard.MoveBoard(move, index);

                Console.WriteLine("---------- Move done: " + move + index + " ----------");
                Console.WriteLine(BoardHelpers.GetReadableBoard(loopover._workingboard));


            }
        }

    }
}
