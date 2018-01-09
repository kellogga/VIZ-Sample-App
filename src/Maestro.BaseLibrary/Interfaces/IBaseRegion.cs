namespace Maestro.BaseLibrary.Interfaces
{
    public interface IBaseRegion
    {
        int RegionID { get; }
        string RegionName { get; }
        string DisplayName1 { get; }
        string DisplayName2 { get; }
        bool MainRegion { get; }
        string MapCode { get; }
        int TotalRidings { get; }
    }
}
