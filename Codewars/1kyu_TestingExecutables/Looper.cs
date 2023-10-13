namespace _1kyu_Implementations.Loopover
{
    class Looper
    {
        static void Main(string[] args)
        {


            var startingboard = BoardHelpers.FormatStringToBoard("CBA\nFED\nIHG");
            var solvedboard = BoardHelpers.FormatStringToBoard("ABC\nDEF\nGHI");
            //Check(startingboard, solvedboard);

            var startingboard2 = BoardHelpers.FormatStringToBoard("AYCDQ\nJKEIP\nHNLGM\nBOFXR\nUSTVW");
            var solvedboard2 = BoardHelpers.FormatStringToBoard("ABCDE\nFGHIJ\nKLMNO\nPQRST\nUVWXY");
            Check(startingboard2, solvedboard2);


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
