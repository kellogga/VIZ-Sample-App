using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Entities;
using System.Collections.Generic;

namespace Maestro.BaseLibrary.Interfaces
{
    public interface IRegionRiding : IBaseRegion
    {
        BaseParty DesiredParty { get; }
        List<Riding> Ridings { get; }
    }
}
