namespace _1kyu_Implementations.Loopover
{
    public class Position
    {
        public Position(int x, int y)
        {
            X =x; 
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                var p = (Position)obj;
                return (X == p.X) && (Y == p.Y);
            }
        }
    }
    
}
