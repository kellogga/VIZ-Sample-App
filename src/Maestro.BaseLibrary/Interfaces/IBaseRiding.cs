namespace Maestro.BaseLibrary.Entities
{
    public interface IBaseRiding
    {
        int RidingID { get; }
        int RidingNumber { get; }
        string ProvinceCode { get; }
        string RidingName { get; }
        string DisplayName1 { get; }
        string DisplayName2 { get; }
        int IncumbentPartyID { get; }
        int TotalPolls { get; }
        int TotalVoters { get; }
        int PrevTotalVoters { get; }
        bool HeroNoteShown { get; set; }
        string FrComments { get; }
    }
}
