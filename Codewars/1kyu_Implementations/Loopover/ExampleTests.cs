using NUnit.Framework;

namespace _1kyu_Implementations.Loopover
{
    [TestFixture]
    public class LooperTest
    {
        
        [TestCase("WCMDJ\nORFBA\nKNGLY\nPHVSE\nTXQUI", "ABCDE\nFGHIJ\nKLMNO\nPQRST\nUVWXY")]

        public void Negative_Solvable(string start, string solved)
        {
            var test_start = BoardHelpers.FormatStringToBoard(start);
            var test_check = BoardHelpers.FormatStringToBoard(start);
            var test_solved = BoardHelpers.FormatStringToBoard(solved);

            var moves = Check(test_start, test_solved);

            Assert.AreEqual(false, Check_Moves(moves, test_check, test_solved));
            Assert.AreEqual(false, moves != null);
        }

        [TestCase("KCBH\nFLJE\nGAID", "ABCD\nEFGH\nIJKL")]
        [TestCase("DGVO\nPHUX\nNWRB\nJQMS\nLTCA\nIEFK", "ABCD\nEFGH\nIJKL\nMNOP\nQRST\nUVWX")]
        [TestCase("WCMDJ0\nORFBA1\nKNGLY2\nPHVSE3\nTXQUI4\nZ56789", "ABCDEF\nGHIJKL\nMNOPQR\nSTUVWX\nYZ0123\n456789")]
        [TestCase("UFBCDEPK\nJSLXRATH\nOGNQVMWI", "ABCDEFGH\nIJKLMNOP\nQRSTUVWX")]
        [TestCase("BF\nCE\nAD\nHG", "AB\nCD\nEF\nGH")]
        [TestCase("ACDBE\nFGHIJ\nKLMNO\nPQRST", "ABCDE\nFGHIJ\nKLMNO\nPQRST")]
        [TestCase("ACDBE\nFGHIJ\nKLMNO\nPQRST", "ABCDE\nFGHIJ\nKLMNO\nPQRST")]
        [TestCase("8nFX4pU\nHlLQ2Is\nMTCKoYq\n5AmNk9e\nGBZOdgW\nVSDj7cE\ni6Pf0Rb\n3Ja1thr", "ABCDEFG\nHIJKLMN\nOPQRSTU\nVWXYZ01\n2345678\n9abcdef\nghijklm\nnopqrst")]
        public void Positive_Solvable(string start, string solved)
        {
            var test_start = BoardHelpers.FormatStringToBoard(start);
            var test_check = BoardHelpers.FormatStringToBoard(start);
            var test_solved = BoardHelpers.FormatStringToBoard(solved);

            var moves = Check(test_start, test_solved);

            Assert.AreEqual(true, Check_Moves(moves, test_check, test_solved));
            Assert.AreEqual(true, moves != null);
        }

        private static List<string> Check(char[][] startingboard, char[][] solvedboard)
        {
            LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(startingboard));
            return Loopover.Solve(startingboard, solvedboard);
        }


        private static bool Check_Moves(List<string> moves, char[][] startingboard, char[][] solvedboard)
        {
            if (moves == null) return false;
            startingboard.MoveBoard(moves);
            return startingboard.IsBoardEquals(solvedboard);
        }

    }
}
