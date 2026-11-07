namespace Soso.Serialization.Tests.Models;

public struct BlittableTest
{
    public int A;
    public byte B;

    public BlittableTest(int a, byte b)
    {
        A = a;
        B = b;
    }
}