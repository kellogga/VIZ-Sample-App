using System;
using System.Collections.Generic;

namespace Maestro
{
    public class USTopLevel
    {
        public List<President> president { get; set; }
        public House house { get; set; }
        public Senate senate { get; set; }
    }

    public class President
    {
        public int id { get; set; }
        public string electTotal { get; set; }
        public string lastUpdated { get; set; }
        public string precinctsReportingPct { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string party { get; set; }
        public int voteCount { get; set; }
        public int electWon { get; set; }
        public string winner { get; set; }
        public string winner_cbc { get; set; }
        public string winner_set { get; set; }
        public string nextRequest { get; set; }
        public string popularVotePct { get; set; }
        public string voteCountDisp { get; set; }
    }

    public class Party
    {
        public string party { get; set; }
        public object membersElected { get; set; }
        public object voteCount { get; set; }
        public string voteCountDisp { get; set; }
    }

    public class House
    {
        public int seatsDecided { get; set; }
        public List<Party> parties { get; set; }
    }

        public class Senate
    {
        public int seatsDecided { get; set; }
        public List<Party> parties { get; set; }
    }
}
