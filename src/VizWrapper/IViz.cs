using System;
using System.Threading.Tasks;

namespace VizWrapper
{
    public interface IViz
    {
        bool DoingRefresh { get; set; }

        event EventHandler<DataReadyEventArgs> OnDataReady;
        event EventHandler<byte[]> OnDataReceived;
        event EventHandler OnDisconnected;

        Task<bool> Connect(RemoteServerInfo server);
        void ContinueScene();
        void ContinueUbScene();
        bool IsConnected();
        void LoadScene(string scene);
        void SetOnAir();
        void VizBackground(bool bringBoardOn);
        string PrepareForNewScene(IBaseBoard baseBoard);
        string PrepareForNewScene(int pauseInterval, string boardName);
        string PrepareForNewUbScene(IScene scene);
        void DisplayScene(string datapoolName, string dataPoolData, bool doingRefresh = false);
        void SendVizCommand(string cmd);
    }
}
