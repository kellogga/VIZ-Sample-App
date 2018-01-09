using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Entities;
using System.Collections.Generic;

namespace Maestro.BaseLibrary.Interfaces
{
    public interface IRegion : IBaseRegion
    {

        int CurrentTotalVotes { get; set; }
        string MiniMessage { get; }
        int PreviousTotalVotes { get; set; }
        int ReportingRidings { get; set; }
        BaseParty DesiredParty { get; }
        List<Party> Parties { get; }
    }
}
