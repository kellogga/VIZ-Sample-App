using Maestro.BaseLibrary.Entities;

namespace Maestro.BaseLibrary.Interfaces
{
    public interface IRiding : IBaseRiding
    {
        int ReportingPolls { get; }
        bool HasResults { get; }
        int ReportStatus { get; }
    }
}
