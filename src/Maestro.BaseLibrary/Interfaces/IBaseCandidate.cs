using Maestro.BaseLibrary.BaseObjects;

namespace Maestro.BaseLibrary.Interfaces
{
    public interface IBaseCandidate
    {
        int CandidateID { get; }
        BaseParty Party { get; }
        string FirstName { get; }
        string LastName { get; }
        string DisplayName1 { get; }
        string DisplayName2 { get; }
        int CandidateType { get; }
        string HeroNote { get; }
        int PreviousVotes { get; }
        string Gender { get; }
        string CandidateNote1 { get; }
        string CandidateNote2 { get; }
        bool IsRenegade { get; }
    }
}
