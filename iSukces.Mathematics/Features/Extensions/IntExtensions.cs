namespace iSukces.Mathematics;

public static class IntExtensions
{
    extension(int x)
    {
        public int DivUp(int b)
        {
            return (x + b - 1) / b;
        }

        public int RoundUp(int round)
        {
            x =  x + round - 1;
            x /= round;
            x *= round;
            return x;
        }
    }
}
