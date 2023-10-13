namespace _1kyu_Implementations.Loopover
{
    class Looper
    {
        static void Main(string[] args)
        {
            var startingboard = BoardHelpers.FormatStringToBoard("CBA\nFED\nIGH");
            var tempboard = (char[][])startingboard.Clone();
            var solvedboard = BoardHelpers.FormatStringToBoard("ABC\nDEF\nGHI");

            Console.WriteLine("---------- START GAME ----------");
            Console.WriteLine(BoardHelpers.GetReadableBoard(startingboard));


            while (!tempboard.IsBoardEquals(solvedboard))
            {
                var input = Console.ReadLine();
                var move = input.ToCharArray()[0].ToString();
                var index = int.Parse(input.ToCharArray()[1].ToString());
                tempboard.MoveBoard(move, index);
                
                Console.WriteLine("---------- Move done: "+move+index+ " ----------");
                Console.WriteLine(BoardHelpers.GetReadableBoard(tempboard));


            }


        }

        private void Check(char[][] startingboard,char[][] solvedboard)
        {
            char[][] solved;
            Loopover.Solve(startingboard, solvedboard);
        }


        
    }
}
