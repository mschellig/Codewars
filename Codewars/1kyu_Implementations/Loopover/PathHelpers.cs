namespace _1kyu_Implementations.Loopover
{
    public static class PathHelpers
    {
        public static List<Tuple<char, int>> FindPathPhase1(Tuple<int,int> currentposition, Tuple<int,int> targetposition, Tuple<int, int> dimensions)
        {
            var movelist = new List<Tuple<char, int>>();
            if (currentposition.Equals(targetposition)) return movelist;


            // for easier use, make dimensions match array logic
            dimensions = new Tuple<int, int>(dimensions.Item1-1, dimensions.Item2-1);
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

            var distanceHorizontal = CalculateHorizontalDistance(currentposition.Item1, targetposition.Item1, dimensions.Item1+1); // 'BAC' has a count of 3
            //Add horizontal moves to list
            for (var i = 0; i < distanceHorizontal.Item2; i++)
            {
                movelist.Add(new Tuple<char, int>(distanceHorizontal.Item1, isNotLastRow?currentposition.Item2+1: currentposition.Item2));
            }

            //check which vertical way is faster
            var distanceVertical = CalculateVerticalDistance(currentposition.Item2, targetposition.Item2, dimensions.Item1+1); // 'BAC' has a count of 3
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
            var distanceVertical = CalculateVerticalDistance(currentposition.Item2, targetposition.Item2, dimensions.Item1 + 1); // 'BAC' has a count of 3
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


        private static Tuple<char,int> CalculateHorizontalDistance(int startPosition, int targetPosition, int dimension)
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
}
