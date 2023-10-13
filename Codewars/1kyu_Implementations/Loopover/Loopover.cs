using System;
using System.Collections.Generic;

namespace _1kyu_Implementations.Loopover
{
    public class Loopover
    {
        #region VAR

        private char[][] _startingboard;
        private char[][] _workingboard;
        private char[][] _tempboard;

        private List<string> _moveList =new List<string>();

        private int dimension_x = 0;
        private int dimension_y = 0;
        #endregion

        #region CONST
        public Loopover(char[][] startingboard)
        {
            _startingboard = startingboard;
            dimension_y = startingboard.Length;
            dimension_x = startingboard[0].Length;
        }
        #endregion

        #region entrypoint

        public static List<string> Solve(char[][] mixedUpBoard, char[][] solvedBoard)
        {
            var loopover = new Loopover(mixedUpBoard);
            return null;
        }


        #endregion



    }
}
