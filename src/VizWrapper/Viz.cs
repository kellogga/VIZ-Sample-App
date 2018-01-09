using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VizWrapper
{
    public enum DataType
    {
        None = 0,
        Riding = 1,
        Region = 2,
        Summary = 3,
        RegionRiding = 4,
        RegionParty = 5
    }
    
    /// <summary>
    /// Class to encapsulate communication with an external VIZ engine. All commands
    /// sent to VIZ must be in a specific format (leading command number, null terminated etc.)
    /// This class ensures that these rules are followed.
    /// </summary>
    public class Viz : IViz
    {
        private readonly AsyncTcpClient _client;
        private int _commandNumber;
        private string _currentScene = "";
        private string _currentUbScene = "";
        private string _sceneAction = "";

        public event EventHandler<DataReadyEventArgs> OnDataReady;
        public event EventHandler<byte[]> OnDataReceived;
        public event EventHandler OnDisconnected;

        public bool DoingRefresh { get; set; }

        public Viz()
        {
            _client = new AsyncTcpClient();
            _commandNumber = 1;
            _client.OnDataReceived += HandleOnDataReceived;
            _client.OnDisconnected += HandleOnDisconnected;
        }

        private void HandleOnDisconnected(object sender, EventArgs e)
        {
            OnDisconnected?.Invoke(this, e);
        }

        protected virtual void HandleOnDataReady(DataReadyEventArgs e)
        {
            OnDataReady?.Invoke(this, e);
        }

        protected virtual void HandleOnDataReceived(object sender, byte[] data)
        {
            OnDataReceived?.Invoke(this, data);
        }

        public async Task<bool> Connect(RemoteServerInfo server)
        {
            try
            {
                await _client.ConnectAsync(server);
                return _client.IsConnected;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsConnected()
        {
            return _client.IsConnected;
        }

        public async void ContinueScene()
        {
            if (_currentScene.Length <= 0) return;
            switch (_currentScene.ToLower())
            {
                case "women_brd":
                case "party_strength_brd":
                case "possible_majority_brd":
                case "election_night_bq_brd":
                case "projection_brd":
                    //This is a 2 page board...
                    if (string.IsNullOrEmpty(_sceneAction) | _sceneAction == "=OFF;")
                    {
                        //Move to the second page
                        _sceneAction = "=CONTINUE;";
                    }
                    else if (_sceneAction != "=OFF2;")
                    {
                        //trigger the board off animation
                        _sceneAction = "=OFF;";
                    }
                    break;
                default:
                    //trigger the board off animation
                    _sceneAction = "=OFF;";
                    break;
            }

            await _client.SendAsync(PrepareVizCommand(SetDataPool(_currentScene + _sceneAction)));
            if (_sceneAction == "=OFF;" | _sceneAction == "=OFF2;")
            {
                //We've just taken a board off-air so blank out the current scene's name
                _currentScene = "";
            }
        }

        public async void ContinueUbScene()
        {
            if (_currentUbScene.Length <= 0) return;
            await _client.SendAsync(PrepareVizCommand(SetDataPool(_currentUbScene + "=OFF;")));
            //We've just taken a board off-air so blank out the current scene's name
            _currentScene = "";
        }

        public async void DisplayScene(string datapoolName, string dataPoolData, bool doingRefresh = false)
        {
            if (doingRefresh)
            {
                await _client.SendAsync(PrepareVizCommand(SetDataPool(dataPoolData)));
            }
            else
            {
                if (datapoolName.Length > 0)
                {
                    await _client.SendAsync(PrepareVizCommand(SetDataPool(datapoolName + "=ON;")));
                }
                if (dataPoolData.Length > 0)
                {
                    await _client.SendAsync(PrepareVizCommand(SetDataPool(dataPoolData)));
                }
            }
        }

        public async void DisplaySceneFf(string datapoolName, string dataPoolData, bool doingRefresh = false)
        {
            if (doingRefresh)
            {
                await _client.SendAsync(PrepareVizCommand(SetDataPool(dataPoolData)));
            }
            else
            {
                if (dataPoolData.Length > 0)
                {
                    await _client.SendAsync(PrepareVizCommand(SetDataPool(dataPoolData)));
                }
                if (datapoolName.Length > 0)
                {
                    await _client.SendAsync(PrepareVizCommand(SetDataPool(datapoolName + "=ON;")));
                }
            }
        }

        /// <summary>
        /// Called at the start of each VIZ routine. Handles taking the previously loaded scene
        /// off-air.
        /// </summary>
        public string PrepareForNewScene(int pauseInterval, string boardName)
        {
            if (!DoingRefresh)
            {
                ContinueScene();
                if (_currentScene.Length > 0)
                {
                    //Pause this background thread to give the current on-air scene enough
                    //time to animate off
                    Thread.Sleep(pauseInterval);
                }
                _sceneAction = "";
            }
            _currentScene = boardName.ToUpper();
            return _currentScene;
        }

        public string PrepareForNewScene(IBaseBoard boardData)
        {
            if (!DoingRefresh)
            {
                if (_currentScene.ToLower() == "women_brd" || _currentScene.ToLower() == "party_strength_brd" ||
                    _currentScene.ToLower() == "possible_majority_brd" ||
                    _currentScene.ToLower() == "election_night_bq_brd")
                    if (_sceneAction.ToLower() != "=continue;")
                    {
                        _sceneAction = "=OFF2;";
                    }
                ContinueScene();
                if (_currentScene.Length > 0)
                {
                    //Pause this background thread to give the current on-air scene enough
                    //time to animate off
                    Thread.Sleep(boardData.PauseInterval);
                }
                _sceneAction = "";
            }
            _currentScene = boardData.DatapoolName.ToUpper();
            return _currentScene;
        }

        public string PrepareForNewUbScene(IScene scene)
        {
            ContinueUbScene();
            if (_currentUbScene.Length > 0)
            {
                //Pause this background thread to give the current on-air scene enough
                //time to animate off
                Thread.Sleep(scene.PauseInterval);
            }
            _currentUbScene = scene.DatapoolName.ToUpper();
            return _currentUbScene;
        }

        /// <summary>
        /// Formats a VIZ command. VIZ expects a byte array that starts with an integer
        /// (command number) followed by the command and terminated by a null character
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private byte[] PrepareVizCommand(string cmd)
        {
            HandleOnDataReady(new DataReadyEventArgs {CommandNumber = _commandNumber, Command = cmd });
            var command = Encoding.UTF8.GetBytes(_commandNumber + " " + cmd + "\0");
            _commandNumber += 1;
            if (_commandNumber > 999)
            {
                //reset when command the command number reaches 1000
                _commandNumber = 1;
            }
            return command;
        }

        public async void LoadScene(string scene)
        {
            await _client.SendAsync(PrepareVizCommand("RENDERER*MAIN_LAYER SET_OBJECT SCENE*" + scene));
            await _client.ReceiveAsync();
        }

        public async void VizBackground(bool bringBoardOn)
        {
            if (bringBoardOn)
            {
                await _client.SendAsync(PrepareVizCommand(SetDataPool("BG=ON;"))); 
            }
            else
            {
                await _client.SendAsync(PrepareVizCommand(SetDataPool("BG=OFF;")));
            }
            await _client.ReceiveAsync();
        }

        public async void SetOnAir()
        {
            if (_client.IsConnected)
            {
                await _client.SendAsync(PrepareVizCommand("MAIN SWITCH_EXTERNAL"));
            }
        }

        /// <summary>
        /// Sends one command to VIZ asynchronously
        /// </summary>
        /// <param name="cmd"></param>
        public async void SendVizCommand(string cmd)
        {
            await _client.SendAsync(PrepareVizCommand(SetDataPool(cmd)));
        }

        /// <summary>
        /// Prepends the datapool string required by VIZ to set a value in the VIZ datapool.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static string SetDataPool(string cmd)
        {
            return "MAIN_SCENE*FUNCTION*DataPool*Data SET " + cmd;
        }
    }
}
