
namespace VizWrapper
{
    public interface IScene
    {
        bool CalculateVoteShare { get; }
        int Channel { get; }
        string ChannelName { get; }
        string DatapoolName { get; }
        string InternalName { get; }
        string Name { get; }
        int NumberOfItems { get; }
        int PauseInterval { get; }
        bool ShowOthers { get; set; }
        DataType TypeOfData { get; }
    }
}