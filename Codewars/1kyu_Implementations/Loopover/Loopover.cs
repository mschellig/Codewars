using System;
using System.Collections.Generic;

namespace _1kyu_Implementations.Loopover
{
    public class Loopover
    {
        #region VAR

        private char[][] _startingboard;
        private char[][] _solvedboard;
        private char[][] _workingboard;

        private List<string> _moveList =new();

        private int dimension_x = 0;
        private int dimension_y = 0;
        #endregion

        #region PROP
        #endregion

        #region CONST
        public Loopover(char[][] startingboard, char[][] solvedboard)
        {
            _startingboard = startingboard;
            _workingboard = (char[][])startingboard.Clone();
            _solvedboard = solvedboard;
            dimension_y = startingboard.Length;
            dimension_x = startingboard[0].Length;
        }
        #endregion

        #region entrypoint

        public static List<string> Solve(char[][] mixedUpBoard, char[][] solvedBoard)
        {
            LoggingHelpers.LogHeadline("Start solving loopover");
            LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(mixedUpBoard));

            var loopover = new Loopover(mixedUpBoard, solvedBoard);
            loopover._moveList.AddRange(loopover.SolvePhase1());
            return loopover._moveList;
        }


        #endregion

        #region PHASES
        //Phase 1 solves the inital quad (x=length-1, y=length-1)
        private List<string> SolvePhase1()
        {
            LoggingHelpers.LogHeadline("Starting phase 1 - initial quad");

            var dimensionTuple = new Tuple<int,int>(dimension_x, dimension_y);
            var returnal = new List<string>();

            for (var i = 0; i < dimension_y - 1; i++)
            {
                for (var j = 0; j < dimension_x - 1; j++)
                {
                    var currentPosition = _workingboard.FindPositionInBoard(_solvedboard[i][j]);
                    LoggingHelpers.Log($"Next Character: {_solvedboard[i][j]} at current position X:{currentPosition.Item1}/Y:{currentPosition.Item2}");

                    var movelist = PathHelpers.FindPathPhase1(currentPosition, new Tuple<int, int>(dimension_x - 1, i), dimensionTuple);
                    movelist.Add(new Tuple<char, int>('L', i));

                    foreach (var m in movelist)
                    {
                        LoggingHelpers.Log($"New Movement initialized: {m.Item1}{m.Item2}");
                        _workingboard.MoveBoard(m.Item1, m.Item2);
                        LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));
                    }
                    returnal.AddRange(movelist.Select(x=>$"{x.Item1}{x.Item2}"));
                }
            }

            LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(_workingboard));
            return returnal;
        }
        #endregion



    }
}
