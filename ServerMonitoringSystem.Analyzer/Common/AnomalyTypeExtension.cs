using System.Text;

namespace ServerMonitoringSystem.Analyzer.Common;

public static class AnomalyTypeExtension
{
    public static string ToFriendlyString(this AnomalyType anomalyType)
    {
        if (anomalyType == AnomalyType.None)
        {
            return "None";
        }

        var sb = new StringBuilder();
        var first = true;

        foreach (AnomalyType type in Enum.GetValues(typeof(AnomalyType)))
        {
            if (type == AnomalyType.None || !anomalyType.HasFlag(type)) continue;
            if (!first)
            {
                sb.Append(", ");
            }

            sb.Append(type);
            first = false;
        }

        return sb.ToString();
    }
}