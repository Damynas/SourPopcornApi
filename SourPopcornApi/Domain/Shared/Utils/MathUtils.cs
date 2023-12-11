namespace Domain.Shared.Utils;

public static class MathUtils
{
    public static double Round(double value, int digits = 2, MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        return Math.Round(value, digits, mode);
    }
}
