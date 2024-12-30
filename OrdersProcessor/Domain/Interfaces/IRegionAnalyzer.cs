namespace OrdersProcessor.Domain.Interfaces;

public interface IRegionAnalyzer
{
    string GetMostCommonRegion(List<string> regions);
}
