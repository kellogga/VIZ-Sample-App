using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Entities;
using System.Collections.Generic;

namespace Maestro.BaseLibrary.Interfaces
{
    public interface ISummary : IBaseSummary
    {

        BaseParty Party { get; }
        List<SummaryRegion> SummaryRegions { get; }
    }
}
