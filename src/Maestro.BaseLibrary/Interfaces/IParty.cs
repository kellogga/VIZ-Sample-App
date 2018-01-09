namespace Maestro.BaseLibrary.Interfaces
{
    public interface IParty : IBaseParty
    {

        int CurrentElectedSeats { get; }
        int CurrentLeadingSeats { get; }
        int CurrentVotes { get; }
        int PrevReportingSeats { get; }
        int PrevNonReportingSeats { get; }
        int PreviousVotes { get; }
    }
}
