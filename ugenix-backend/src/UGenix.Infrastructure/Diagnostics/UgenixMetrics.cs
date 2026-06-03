using System.Diagnostics.Metrics;

namespace UGenix.Infrastructure.Diagnostics;

public static class UgenixMetrics
{
    public const string MeterName = "UGenix.Metrics";
    private static readonly Meter Meter = new(MeterName);

    public static readonly Counter<long> VouchersPurchased = Meter.CreateCounter<long>(
        "ugenix.vouchers.purchased",
        description: "Number of vouchers successfully purchased");

    public static readonly Histogram<double> SpatialSearchDuration = Meter.CreateHistogram<double>(
        "ugenix.spatial.search.duration",
        unit: "ms",
        description: "Duration of spatial discovery searches");
}
