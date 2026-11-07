namespace Soso.Serialization.Tests.Models
{
    public class Demo : IDemo
    {
        public string Test { get; set; }
        public float TestF { get; set; }

        public override string ToString()
        {
            return $"Demo - {nameof(Test)}:{Test}, {nameof(TestF)}:{TestF}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Demo d && d.Test == Test && Math.Abs(d.TestF - TestF) < 0.001f;
        }
    }
}