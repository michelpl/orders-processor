namespace OrdersProcessor.Domain.Interfaces;

public interface IRegionAnalyzer
{
    string GetMostCommonRegion(IEnumerable<string> regions);
}
