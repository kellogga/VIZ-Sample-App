namespace Maestro.BaseLibrary.Interfaces
{
    public interface ICandidate : IBaseCandidate
    {
        int Status { get; }
        string CandidateStatus { get; }
        int Votes { get; }
        int Lead { get; }
        int Rank { get; }
    }
}
