using System.Collections.Generic;

namespace net5app
{
    internal interface IMetric
    {
        string GetName();
        byte GetMetricValue(Dictionary<string, byte> subjectValues);
        byte GetMinValue();
        byte GetMaxValue();
    }
}