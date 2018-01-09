namespace Maestro.BaseLibrary.Interfaces
{
    public interface ISummaryRegion : IBaseRegion
    {
        int CurrentLeading { get; }
        int CurrentElected { get; }
        int CurrentVotes { get; }
        int CurrentTotalVotes { get; }
        float CurrentVoteShare { get; }
        int PrevReportingSeats { get; }
        int PreviousVotes { get; }
        int PreviousTotalVotes { get; }
        float PreviousVoteShare { get; }
    }
}
