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
            var targetPosition = 0;
            var position_x = 0;
            var position_y = 0;
            var inverse_x = false;
            var inverse_y = false;

            while (returnal.Count < array.Length * array[0].Length)
            {
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

                if (returnal.Count >= array.Length * array[0].Length) break;
                if (!inverse_y)
                {
                    targetPosition = array.Length - 1 - iterations;
                    returnal.AddRange(VerticalSolve(array, position_y, targetPosition, position_x));
                    position_y = targetPosition;
                    position_x--;
                }
                else
                {
                    iterations++;
                    targetPosition = 0 + iterations;
                    returnal.AddRange(VerticalSolve(array, position_y, targetPosition, position_x));
                    position_y = targetPosition;
                    position_x++;
                }

                inverse_y = !inverse_y;
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
