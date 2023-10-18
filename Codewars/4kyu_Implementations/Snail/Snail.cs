using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4kyu_Implementations.Snail
{
    public class SnailSolution
    {
        public static int[] Snail(int[][] array)
        {
            var returnal = new List<int>();

            var iterations = 0;

            var position_x = 0;
            var position_y = 0;

            var inverse_x = true;
            var inverse_y = false;

            //do first row
            var targetPosition = array[0].Length - 1 - iterations;
            returnal.AddRange(HorizontalSolve(array, position_x, array[0].Length - 1 - iterations, 0));
            position_x = targetPosition;
            position_y++;


            //TODO change while to returnal count < array.Lengt8h * array[0].Length
            //TODO add moveCounter - ++ after each row/col
            //TODO  iteration ++ after 3 moveCount

            while (iterations < (array.Length + array[0].Length-2) /2)
            {
                if (!inverse_y)
                {
                    targetPosition = array.Length - 1 - iterations;
                    returnal.AddRange(VerticalSolve(array, position_y, targetPosition, position_x));
                    position_y = targetPosition;
                    position_x--;
                }
                else
                {
                    targetPosition = 0 + iterations;
                    returnal.AddRange(VerticalSolve(array, position_y, targetPosition, position_x));
                    position_y = targetPosition;
                    position_x++;
                }

                inverse_y = !inverse_y;

                if (!inverse_x)
                {
                    targetPosition = array[0].Length - 1 - iterations;
                    returnal.AddRange(HorizontalSolve(array, position_x, array[0].Length - 1 - iterations, position_y));
                    position_x = targetPosition;
                    position_y++;
                }
                else
                {
                    targetPosition = 0 + iterations;
                    returnal.AddRange(HorizontalSolve(array, position_x, targetPosition, position_y));
                    position_x = targetPosition;
                    position_y--;
                }
                inverse_x = !inverse_x;


                iterations++;
            }

            return returnal.ToArray();
        }

        public static IEnumerable<int> HorizontalSolve(int[][] array, int from, int to, int row)
        {
            if(from>to)
            {
                for (var i = from; i >= to; i--)
                {
                    yield return array[row][i];
                }
            }else
            {
                for (var i = from; i <= to; i++)
                {
                    yield return array[row][i];
                }
            }
        }

        public static IEnumerable<int> VerticalSolve(int[][] array, int from, int to, int column)
        {
            if (from > to)
            {
                for (var i = from; i >= to; i--)
                {
                    yield return array[i][column];
                }
            }
            else
            {
                for (var i = from; i <= to; i++)
                {
                    yield return array[i][column];
                }
            }
        }
    }
}
