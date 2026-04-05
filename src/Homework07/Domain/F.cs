namespace Homework07.Domain;

public sealed class F
{
    public int i1;
    public int i2;
    public int i3;
    public int i4;
    public int i5;

    public static F Get()
    {
        return new F
        {
            i1 = 1,
            i2 = 2,
            i3 = 3,
            i4 = 4,
            i5 = 5
        };
    }
}
