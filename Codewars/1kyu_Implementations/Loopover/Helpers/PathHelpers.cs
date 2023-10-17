namespace _1kyu_Implementations.Loopover
{
    public static class PathHelpers
    {
        public static IEnumerable<Move> FindPathPhase1(Position currentposition, Position targetposition, Position dimensions)
        {
            if (currentposition.Equals(targetposition)) yield break;

            var isNotLastRow = currentposition.Y != dimensions.Y && currentposition.X != targetposition.X;

            //If item is not in last row and not same column, move one field down
            //avoids unwanted movements of correct tiles behind
            //REMEMBER: move column up at the end again
            if (isNotLastRow)
            {
                yield return new Move("D", currentposition.X);
            }

            var distanceHorizontal = CalculateHorizontalDistance(currentposition.X, targetposition.X, dimensions.X + 1);
            for (var i = 0; i < distanceHorizontal.Item2; i++)
            {
                yield return new Move(distanceHorizontal.Item1.ToString(), isNotLastRow ? currentposition.Y + 1 : currentposition.Y);
            }

            //check which vertical way is faster
            var distanceVertical = CalculateVerticalDistance(currentposition.Y, targetposition.Y, dimensions.Y + 1); 
            for (var i = 0; i < distanceVertical.Item2; i++)
            {
                yield return new Move(distanceVertical.Item1.ToString(), targetposition.X);
            }

            //Add Upwards move according first task
            if (isNotLastRow)
            {
                yield return new Move("U", targetposition.X);
                yield return new Move("U", currentposition.X);
            }
        }

        public static IEnumerable<Move> FindPathPhase2(Position currentposition, Position targetposition, Position dimensions)
        {
            if (currentposition.Equals(targetposition)) yield break;

            //check which vertical way is faster
            var distanceVertical = CalculateVerticalDistance(currentposition.Y, targetposition.Y, dimensions.Y + 1); 
            for (var i = 0; i < distanceVertical.Item2; i++)
            {
               yield return new Move(distanceVertical.Item1, currentposition.X);
            }

            //Check which horizontal way is faster
            var distanceHorizontal = CalculateHorizontalDistance(currentposition.X, targetposition.X, dimensions.X + 1);
            for (var i = 0; i < distanceHorizontal.Item2; i++)
            {
                yield return new Move(distanceHorizontal.Item1, targetposition.Y);
            }
        }


        private static Tuple<string, int> CalculateHorizontalDistance(int startPosition, int targetPosition, int dimension)
        {
            var distanceLeft = (startPosition - targetPosition + dimension) % dimension;
            var distanceRight = (targetPosition - startPosition + dimension) % dimension;
            return distanceLeft < distanceRight
                ? new Tuple<string, int>("L", distanceLeft)
                : new Tuple<string, int>("R", distanceRight);
        }

        private static Tuple<string, int> CalculateVerticalDistance(int startPosition, int targetPosition, int dimension)
        {
            var distanceUp = (startPosition - targetPosition + dimension) % dimension;
            var distanceDown = (targetPosition - startPosition + dimension) % dimension;
            return distanceUp < distanceDown
                ? new Tuple<string, int>("U", distanceUp)
                : new Tuple<string, int>("D", distanceDown);
        }
    }
}
