namespace _7kyu_Implementations.DescendingOrder
{
    public static class Kata
    {
        public static int DescendingOrder(int num)
        {
            var numArr = num.ToString().Select(x => int.Parse(x.ToString()));
            var sortedArr = numArr.OrderByDescending(x => x);
            
            return int.Parse(string.Join("", sortedArr));
        }
    }
}
