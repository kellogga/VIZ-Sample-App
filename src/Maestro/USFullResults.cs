using System.Collections.Generic;

namespace Maestro
{
    public class USFullResults
    {
        public USPresidentState president { get; set; }
        public USHouseState house { get; set; }
        public USSenateState senate { get; set; }
        public USNational national { get; set; }
        public string nextRequest { get; set; }
        public int serverTime { get; set; }
    }

    public class USPresidentState
    {
        public PresidentState AK { get; set; }
        public PresidentState AL { get; set; }
        public PresidentState AR { get; set; }
        public PresidentState AZ { get; set; }
        public PresidentState CA { get; set; }
        public PresidentState CO { get; set; }
        public PresidentState CT { get; set; }
        public PresidentState DC { get; set; }
        public PresidentState DE { get; set; }
        public PresidentState FL { get; set; }
        public PresidentState GA { get; set; }
        public PresidentState HI { get; set; }
        public PresidentState IA { get; set; }
        public PresidentState ID { get; set; }
        public PresidentState IL { get; set; }
        public PresidentState IN { get; set; }
        public PresidentState KS { get; set; }
        public PresidentState KY { get; set; }
        public PresidentState LA { get; set; }
        public PresidentState MA { get; set; }
        public PresidentState MD { get; set; }
        public PresidentState ME { get; set; }
        public PresidentState MI { get; set; }
        public PresidentState MN { get; set; }
        public PresidentState MO { get; set; }
        public PresidentState MS { get; set; }
        public PresidentState MT { get; set; }
        public PresidentState NC { get; set; }
        public PresidentState ND { get; set; }
        public PresidentState NE { get; set; }
        public PresidentState NH { get; set; }
        public PresidentState NJ { get; set; }
        public PresidentState NM { get; set; }
        public PresidentState NV { get; set; }
        public PresidentState NY { get; set; }
        public PresidentState OH { get; set; }
        public PresidentState OK { get; set; }
        public PresidentState OR { get; set; }
        public PresidentState PA { get; set; }
        public PresidentState RI { get; set; }
        public PresidentState SC { get; set; }
        public PresidentState SD { get; set; }
        public PresidentState TN { get; set; }
        public PresidentState TX { get; set; }
        public PresidentState UT { get; set; }
        public PresidentState VA { get; set; }
        public PresidentState VT { get; set; }
        public PresidentState WA { get; set; }
        public PresidentState WI { get; set; }
        public PresidentState WV { get; set; }
        public PresidentState WY { get; set; }
    }

    public class PresidentState
    {
        public int electTotal { get; set; }
        public string precinctsReportingPct { get; set; }
        public List<PresidentCandidate> candidates { get; set; }
    }

    public class PresidentCandidate
    {
        public int id { get; set; }
        public string lastUpdated { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string party { get; set; }
        public int voteCount { get; set; }
        public string voteCountDisp { get; set; }
        public int electWon { get; set; }
        public string winner { get; set; }
        public string winner_cbc { get; set; }
        public string winner_set { get; set; }
        public string nextRequest { get; set; }
        public string popularVotePct { get; set; }
    }

    public class USHouseState
    {
        public CongressState AK { get; set; }
        public CongressState AL { get; set; }
        public CongressState AR { get; set; }
        public CongressState AZ { get; set; }
        public CongressState CA { get; set; }
        public CongressState CO { get; set; }
        public CongressState CT { get; set; }
        public CongressState DE { get; set; }
        public CongressState FL { get; set; }
        public CongressState GA { get; set; }
        public CongressState HI { get; set; }
        public CongressState IA { get; set; }
        public CongressState ID { get; set; }
        public CongressState IL { get; set; }
        public CongressState IN { get; set; }
        public CongressState KS { get; set; }
        public CongressState KY { get; set; }
        public CongressState LA { get; set; }
        public CongressState MA { get; set; }
        public CongressState MD { get; set; }
        public CongressState ME { get; set; }
        public CongressState MI { get; set; }
        public CongressState MN { get; set; }
        public CongressState MO { get; set; }
        public CongressState MS { get; set; }
        public CongressState MT { get; set; }
        public CongressState NC { get; set; }
        public CongressState ND { get; set; }
        public CongressState NE { get; set; }
        public CongressState NH { get; set; }
        public CongressState NJ { get; set; }
        public CongressState NM { get; set; }
        public CongressState NV { get; set; }
        public CongressState NY { get; set; }
        public CongressState OH { get; set; }
        public CongressState OK { get; set; }
        public CongressState OR { get; set; }
        public CongressState PA { get; set; }
        public CongressState RI { get; set; }
        public CongressState SC { get; set; }
        public CongressState SD { get; set; }
        public CongressState TN { get; set; }
        public CongressState TX { get; set; }
        public CongressState UT { get; set; }
        public CongressState VA { get; set; }
        public CongressState VT { get; set; }
        public CongressState WA { get; set; }
        public CongressState WI { get; set; }
        public CongressState WV { get; set; }
        public CongressState WY { get; set; }
    }

    public class CongressCandidate
    {
        public int id { get; set; }
        public string lastUpdated { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string party { get; set; }
        public int voteCount { get; set; }
        public string voteCountDisp { get; set; }
        public int electWon { get; set; }
        public string winner { get; set; }
        public string nextRequest { get; set; }
    }

    public class CongressState
    {
        public int electTotal { get; set; }
        public string precinctsReportingPct { get; set; }
        public List<CongressCandidate> candidates { get; set; }
    }

    public class USSenateState
    {
        public CongressState AK { get; set; }
        public CongressState AL { get; set; }
        public CongressState AR { get; set; }
        public CongressState AZ { get; set; }
        public CongressState CA { get; set; }
        public CongressState CO { get; set; }
        public CongressState CT { get; set; }
        public CongressState FL { get; set; }
        public CongressState GA { get; set; }
        public CongressState HI { get; set; }
        public CongressState IA { get; set; }
        public CongressState ID { get; set; }
        public CongressState IL { get; set; }
        public CongressState IN { get; set; }
        public CongressState KS { get; set; }
        public CongressState KY { get; set; }
        public CongressState LA { get; set; }
        public CongressState MD { get; set; }
        public CongressState MO { get; set; }
        public CongressState NC { get; set; }
        public CongressState ND { get; set; }
        public CongressState NH { get; set; }
        public CongressState NV { get; set; }
        public CongressState NY { get; set; }
        public CongressState OH { get; set; }
        public CongressState OK { get; set; }
        public CongressState OR { get; set; }
        public CongressState PA { get; set; }
        public CongressState SC { get; set; }
        public CongressState SD { get; set; }
        public CongressState UT { get; set; }
        public CongressState VT { get; set; }
        public CongressState WA { get; set; }
        public CongressState WI { get; set; }
    }

    public class USNational
    {
        public List<President> president { get; set; }
        public List<House> house { get; set; }
        public List<Senate> senate { get; set; }
    }
}
