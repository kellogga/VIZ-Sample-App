namespace VizWrapper
{
    public interface IBaseBoard
    {
        int NumberOfPages { get; }
        int CurrentPageNumber { get; }
        string ElectionType { get; }
        int FirstItem { get; }
        int NumberOfItemsToDisplay { get; }
        void SetPageNumber(int pageNumber);
        int PauseInterval { get; }
        string DatapoolName { get; }
        IScene Scene { get; }
    }
}
