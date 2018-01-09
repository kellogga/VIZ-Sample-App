namespace Maestro.BaseLibrary.Interfaces
{
    public interface IBaseParty
    {
        int PartyID { get; }
        int PartyPriority { get; }
        string PartyCode { get; }
        string PartyName { get; }
        string DisplayName1 { get; }
        string DisplayName2 { get; }
    }
}
