using NUnit.Framework.Interfaces;
//using Solution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace _1kyu_Implementations.Loopover
{
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
            _arrayDimensionPosition = new Position(startingboard[0].Length-1, startingboard.Length-1);
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
                movelist.AddRange(PathHelpers.FindPathPhase2(currentColumnBeginningPosition,new Position(_arrayDimensionPosition.X, 0), _arrayDimensionPosition));
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
}
