using OrdersProcessor.Domain.Interfaces;

namespace OrdersProcessor.Application.Helpers;

public class RegionAnalyzer : IRegionAnalyzer
{
    public string GetMostCommonRegion(IEnumerable<string> regions)
    {
        if (regions == null || regions.Count() == 0)
            throw new ArgumentException("Regions collection cannot be null or empty");

        return regions.GroupBy(r => r)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .First();
    }
}