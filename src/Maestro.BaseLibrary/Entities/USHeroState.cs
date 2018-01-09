using System.Collections.Generic;

namespace Maestro.BaseLibrary.Entities
{
    public class USHeroState
    {
        public string StateCode { get; set; }
        public bool Elect { get; set; }
        public int ElectoralCollegeVotes { get; set; }
        public string ElectPartyCode { get; set; }
        public List<USParty> Parties { get; set; }
    }
}
