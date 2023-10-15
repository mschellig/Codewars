namespace _1kyu_Implementations.Loopover
{
    class Looper
    {
        static void Main(string[] args)
        {
            //done
            var test1_starting= BoardHelpers.FormatStringToBoard("AB\nCD");
            var test1_solved = BoardHelpers.FormatStringToBoard("AB\nCD");
            //Check(test1_starting, test1_solved);

            //done
            var test2_starting = BoardHelpers.FormatStringToBoard("DB\nCA");
            var test2_solved = BoardHelpers.FormatStringToBoard("AB\nCD");
            //Check(test2_starting, test2_solved);

            //unsolved parity - rerunning solves
            var test3_starting = BoardHelpers.FormatStringToBoard("ACDBE\nFGHIJ\nKLMNO\nPQRST");
            var test3_solved = BoardHelpers.FormatStringToBoard("ABCDE\nFGHIJ\nKLMNO\nPQRST");
            //Check(test3_starting, test3_solved);

            //odd parity
            var test4_starting = BoardHelpers.FormatStringToBoard("WCMDJ\nORFBA\nKNGLY\nPHVSE\nTXQUI");
            var test4_solved = BoardHelpers.FormatStringToBoard("ABCDE\nFGHIJ\nKLMNO\nPQRST\nUVWXY");
            //Check(test4_starting, test4_solved);

            
            var test5_starting = BoardHelpers.FormatStringToBoard("WCMDJ0\nORFBA1\nKNGLY2\nPHVSE3\nTXQUI4\nZ56789");
            var test5_solved = BoardHelpers.FormatStringToBoard("ABCDEF\nGHIJKL\nMNOPQR\nSTUVWX\nYZ0123\n456789");
            Check(test5_starting, test5_solved);

            //done
            var test6_starting = BoardHelpers.FormatStringToBoard("GDF\nCBH\nAEI");
            var test6_solved = BoardHelpers.FormatStringToBoard("ABC\nDEF\nGHI");
            //Check(test6_starting, test6_solved);

            //var startingboard1 = BoardHelpers.FormatStringToBoard("CBA\nFED\nIHG");
            //var solvedboard1 = BoardHelpers.FormatStringToBoard("ABC\nDEF\nGHI");
            //Check(startingboard1, solvedboard1);

            //var startingboard2 = BoardHelpers.FormatStringToBoard("AYCDQ\nJKEIP\nHNLGM\nBOFXR\nUSTVW");
            //var solvedboard2 = BoardHelpers.FormatStringToBoard("ABCDE\nFGHIJ\nKLMNO\nPQRST\nUVWXY");
            //Check(startingboard2, solvedboard2);


            //Console.WriteLine("---------- START GAME ----------");
            //Console.WriteLine(BoardHelpers.GetReadableBoard(startingboard));


            //while (!tempboard.IsBoardEquals(solvedboard))
            //{
            //    var input = Console.ReadLine();
            //    var move = input.ToCharArray()[0].ToString();
            //    var index = int.Parse(input.ToCharArray()[1].ToString());
            //    tempboard.MoveBoard(move, index);

            //    Console.WriteLine("---------- Move done: "+move+index+ " ----------");
            //    Console.WriteLine(BoardHelpers.GetReadableBoard(tempboard));


            //}


        }

        private static void Check(char[][] startingboard,char[][] solvedboard)
        {
            char[][] solved;
            var movelist = Loopover.Solve(startingboard, solvedboard);
        }


        
    }
}
