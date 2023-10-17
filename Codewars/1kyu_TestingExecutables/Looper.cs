//using NUnit.Framework;

//namespace _1kyu_Implementations.Loopover
//{
//    [TestFixture]
//    class Looper2
//    {
//        [TestCase("UFBCDEPK\nJSLXRATH\nOGNQVMWI", "ABCDEFGH\nIJKLMNOP\nQRSTUVWX")]
//        [TestCase(7)]
//        public void Positive_Solvable(string start, string solved)
//        {
//            var test_start = BoardHelpers.FormatStringToBoard(start);
//            var test_check = BoardHelpers.FormatStringToBoard(start);
//            var test_solved = BoardHelpers.FormatStringToBoard(solved);

//            var moves = Check(test_start, test_solved);
            
//            Assert.AreEqual(true, Check_Moves(moves, test_check, test_solved));
//            Assert.AreEqual(true, moves != null);
//        }

//        static void Main(string[] args)
//        {
//            //done
//            var test1_starting= BoardHelpers.FormatStringToBoard("UFBCDEPK\nJSLXRATH\nOGNQVMWI");
//            var test1_check= BoardHelpers.FormatStringToBoard("UFBCDEPK\nJSLXRATH\nOGNQVMWI");
//            var test1_solved = BoardHelpers.FormatStringToBoard("ABCDEFGH\nIJKLMNOP\nQRSTUVWX");
//            //var moves = Check(test1_starting, test1_solved);
//            //Check2(moves, test1_check, test1_solved);

//            var test2_starting = BoardHelpers.FormatStringToBoard("BF\nCE\nAD\nHG");
//            var test2_check = BoardHelpers.FormatStringToBoard("BF\nCE\nAD\nHG");
//            var test2_solved = BoardHelpers.FormatStringToBoard("AB\nCD\nEF\nGH");
//            //var moves2 = Check(test2_starting, test2_solved);
//            //Check2(moves2, test2_check, test2_solved);


//            var test3_starting = BoardHelpers.FormatStringToBoard("ACDBE\nFGHIJ\nKLMNO\nPQRST");
//            var test3_check = BoardHelpers.FormatStringToBoard("ACDBE\nFGHIJ\nKLMNO\nPQRST");
//            var test3_solved = BoardHelpers.FormatStringToBoard("ABCDE\nFGHIJ\nKLMNO\nPQRST");
//            //var moves3 = Check(test3_starting, test3_solved);
//            //Check2(moves3, test3_check, test3_solved);


//            var test4_starting = BoardHelpers.FormatStringToBoard("RBDAFO\nPMELCJ\nHKQIGN");
//            var test4_check = BoardHelpers.FormatStringToBoard("RBDAFO\nPMELCJ\nHKQIGN");
//            var test4_solved = BoardHelpers.FormatStringToBoard("ABCDEF\nGHIJKL\nMNOPQR");
//            //var moves4 = Check(test4_starting, test4_solved);
//            //Check2(moves4, test4_check, test4_solved);

//            var test5_starting = BoardHelpers.FormatStringToBoard("ACDBE\nFGHIJ\nKLMNO\nPQRST");
//            var test5_check = BoardHelpers.FormatStringToBoard("ACDBE\nFGHIJ\nKLMNO\nPQRST");
//            var test5_solved = BoardHelpers.FormatStringToBoard("ABCDE\nFGHIJ\nKLMNO\nPQRST");
//            //var moves5 = Check(test5_starting, test5_solved);
//            //Check2(moves5, test5_check, test5_solved);


//            var test6_starting = BoardHelpers.FormatStringToBoard("8nFX4pU\nHlLQ2Is\nMTCKoYq\n5AmNk9e\nGBZOdgW\nVSDj7cE\ni6Pf0Rb\n3Ja1thr");
//            var test6_check = BoardHelpers.FormatStringToBoard("8nFX4pU\nHlLQ2Is\nMTCKoYq\n5AmNk9e\nGBZOdgW\nVSDj7cE\ni6Pf0Rb\n3Ja1thr");
//            var test6_solved = BoardHelpers.FormatStringToBoard("ABCDEFG\nHIJKLMN\nOPQRSTU\nVWXYZ01\n2345678\n9abcdef\nghijklm\nnopqrst");
//            //var moves5 = Check(test6_starting, test6_solved);
//            //Check2(moves5, test6_check, test6_solved);
//         }

//        static void Main2(string[] args)
//        {
//            //done
//            var test1_starting = BoardHelpers.FormatStringToBoard("AB\nCD");
//            var test1_solved = BoardHelpers.FormatStringToBoard("AB\nCD");
//            //Check(test1_starting, test1_solved);

//            //done
//            var test2_starting = BoardHelpers.FormatStringToBoard("DB\nCA");
//            var test2_solved = BoardHelpers.FormatStringToBoard("AB\nCD");
//            //Check(test2_starting, test2_solved);

//            //unsolved parity - rerunning solves
//            var test3_starting = BoardHelpers.FormatStringToBoard("ACDBE\nFGHIJ\nKLMNO\nPQRST");
//            var test3_solved = BoardHelpers.FormatStringToBoard("ABCDE\nFGHIJ\nKLMNO\nPQRST");
//            //Check(test3_starting, test3_solved);

//            //odd parity
//            var test4_starting = BoardHelpers.FormatStringToBoard("WCMDJ\nORFBA\nKNGLY\nPHVSE\nTXQUI");
//            var test4_solved = BoardHelpers.FormatStringToBoard("ABCDE\nFGHIJ\nKLMNO\nPQRST\nUVWXY");
//            //Check(test4_starting, test4_solved);


//            var test5_starting = BoardHelpers.FormatStringToBoard("WCMDJ0\nORFBA1\nKNGLY2\nPHVSE3\nTXQUI4\nZ56789");
//            var test5_check = BoardHelpers.FormatStringToBoard("WCMDJ0\nORFBA1\nKNGLY2\nPHVSE3\nTXQUI4\nZ56789");
//            var test5_solved = BoardHelpers.FormatStringToBoard("ABCDEF\nGHIJKL\nMNOPQR\nSTUVWX\nYZ0123\n456789");
//            //var moves = Check(test5_starting, test5_solved);
//            //Check2(moves, test5_check, test5_solved);


//            //done
//            var test6_starting = BoardHelpers.FormatStringToBoard("GDF\nCBH\nAEI");
//            var test6_solved = BoardHelpers.FormatStringToBoard("ABC\nDEF\nGHI");
//            //Check(test6_starting, test6_solved);

//            //var startingboard1 = BoardHelpers.FormatStringToBoard("CBA\nFED\nIHG");
//            //var solvedboard1 = BoardHelpers.FormatStringToBoard("ABC\nDEF\nGHI");
//            //Check(startingboard1, solvedboard1);

//            //var startingboard2 = BoardHelpers.FormatStringToBoard("AYCDQ\nJKEIP\nHNLGM\nBOFXR\nUSTVW");
//            //var solvedboard2 = BoardHelpers.FormatStringToBoard("ABCDE\nFGHIJ\nKLMNO\nPQRST\nUVWXY");
//            //Check(startingboard2, solvedboard2);


//            //Console.WriteLine("---------- START GAME ----------");
//            //Console.WriteLine(BoardHelpers.GetReadableBoard(startingboard));


//            //while (!tempboard.IsBoardEquals(solvedboard))
//            //{
//            //    var input = Console.ReadLine();
//            //    var move = input.ToCharArray()[0].ToString();
//            //    var index = int.Parse(input.ToCharArray()[1].ToString());
//            //    tempboard.MoveBoard(move, index);

//            //    Console.WriteLine("---------- Move done: "+move+index+ " ----------");
//            //    Console.WriteLine(BoardHelpers.GetReadableBoard(tempboard));


//            //}


//        }


//        private static List<string>Check(char[][] startingboard,char[][] solvedboard)
//        {
//            LoggingHelpers.LogBlank(BoardHelpers.GetReadableBoard(startingboard));
//            return Loopover.Solve(startingboard, solvedboard);
//        }


//        private static bool Check_Moves(List<string> moves, char[][] startingboard, char[][] solvedboard)
//        {
//            startingboard.MoveBoard(moves);
//            return startingboard.IsBoardEquals(solvedboard);
//        }

//    }
//}
