namespace _6kyu_Implementations.DetectPangram
{

    public static class Kata
    {
        public static bool IsPangram(string str)
        {
            var allChars = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            var strArray = str.ToLower().ToCharArray().Distinct();

            return !allChars.Except(strArray).Any();
        }
    }
}
