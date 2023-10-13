namespace _8kyu_Implementations.GetPanetNameById
{
    internal class Kata
    {
        public static string GetPlanetName(int id)
        {
            var name = id switch
            {
                1 => "Mercury",
                2 => "Venus",
                3 => "Earth",
                4 => "Mars",
                5 => "Jupiter",
                6 => "Saturn",
                7 => "Uranus",
                8 => "Neptune",
                _ => string.Empty
            };
            return name;
        }
    }
}
