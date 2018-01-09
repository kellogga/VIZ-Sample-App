using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Maestro.BaseLibrary.Config;
using Maestro.BaseLibrary.BusinessLogic;
using Maestro.BaseLibrary.Entities;
using NLog;
using NLog.Targets;
using VizWrapper;
using System.Xml.Linq;
using System.Linq;
using System.Threading;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Maestro
{
    public partial class frmMain : Form
    {

        public enum ConnectStatus
        {
            Normal,
            Error
        }

        // This delegate enables asynchronous calls for setting
        // the log messages on a ListBox control.
        delegate void SetMessageCallback(DataReadyEventArgs e);
        //delegate void SetCandidateListCallback(List<Candidate> Candidates);

        private static HttpClient _client = new HttpClient();

        static Logger _logger = LogManager.GetCurrentClassLogger();
        private VizBoardLogic _vizBoardLogicMain;
        private VizBoardLogic _vizBoardLogicMini;
        private Button _currentButton;
        private string _miniType;
        private string _canopyURI;
        private string _fullResultsEndPoint;
        private string _topLevelResultsEndPoint;
        private string _postElectEndPoint;
        private string _jsonExportFile;
        private string _jsonImportFile;
        private bool _allowCanopyUpdates = false;
        private string _vizScene;
        private string _vizSceneFR;
        private System.Windows.Forms.Timer _canopyTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer _mainTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer _stateTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer _mapTimer = new System.Windows.Forms.Timer();
        private bool _processingCanopyData = false;
        private int _playlistRow;
        private int _nextRow = -1;
        private bool _mapActive;

        public frmMain()
        {
            InitializeComponent();

            LoadUSElectionDataFromXML(Directory.GetCurrentDirectory() + "\\USElection.xml");

            //Initialize HttpClient
            _client.BaseAddress = new Uri(_canopyURI);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _logger.Info("-----");
            _logger.Info("US Election 2016 Maestro started.");
            _logger.Info("-----");

            _canopyTimer.Tick += new EventHandler(CanopyTimerElapsedEvent);
            _canopyTimer.Interval = 500;

            _mainTimer.Tick += new EventHandler(MainTimerElapsedEvent);
            _mainTimer.Interval = 1000;

            _stateTimer.Tick += new EventHandler(StateTimerElapsedEvent);
            _stateTimer.Interval = 500;

            _mapTimer.Tick += new EventHandler(MapTimerElapsedEvent);
            _mapTimer.Interval = 2000;

            optFeedOn.Checked = false;

            lblCanopyStatusValue.Text = "Read Only";
            txtSDem.Text = "0";
            txtSRep.Text = "0";
            txtSOth.Text = "0";
        }

        /// <summary>
        /// Main timer elapsed event. Handles updates to the Mini graphic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTimerElapsedEvent(object sender, EventArgs e)
        {
            if (_processingCanopyData == false)
            {
                ProcessMini(_miniType);
            }
        }

        /// <summary>
        /// Canopy timer elapsed event. Handles getting updated data from Canopy (Digital Ops Web API)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CanopyTimerElapsedEvent(object sender, EventArgs e)
        {
            try
            {
                _processingCanopyData = true;
                var topLevel = await GetTopLevelData(_topLevelResultsEndPoint);
                if (topLevel == null)
                {
                    _logger.Error("Failed to load Canopy top level results. Please check Canopy endpoint:" + _client.BaseAddress + _topLevelResultsEndPoint);
                    MessageBox.Show("Failed to load Canopy top level results. Please check Canopy endpoint", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                var fullResults = await GetFullResultsData(_fullResultsEndPoint);
                if (fullResults == null)
                {
                    _logger.Error("Failed to load Canopy top level results. Please check Canopy endpoint:" + _client.BaseAddress + _fullResultsEndPoint);
                    MessageBox.Show("Failed to load Canopy full results. Please check Canopy endpoint", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                UpdateTopLevelValues(topLevel);
                UpdateStateGrid(fullResults);
                _canopyTimer.Interval = (int)CanopyRefreshSeconds.Value * 1000;
                _processingCanopyData = false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Timer to control US State winner boards that will appear in the lower third (squeezed right)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StateTimerElapsedEvent(object sender, EventArgs e)
        {
            if (_nextRow >= DgvStates.Rows.Count)
            {
                _nextRow = -1;
            }
            _playlistRow = GetNextElectedState(_nextRow);
            if (_playlistRow > -1)
            {
                if (_vizBoardLogicMini != null)
                {
                    lblUSStateCodeResults.Text = DgvStates.Rows[_playlistRow].Cells["colStateCode"].Value.ToString();
                    _vizBoardLogicMini.UsStateBoardUb(GetStateBoardData(DgvStates.Rows[_playlistRow]));
                    _nextRow = _playlistRow + 1;
                    _stateTimer.Interval = 7000;
                }
            }
            else
            {
                _nextRow = -1;
                _stateTimer.Interval = 500;
            }
        }

        private void MapTimerElapsedEvent(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                _vizBoardLogicMain.UsMapFr(GetMapData(), true);
            }
        }

        /// <summary>
        /// Cycles through state grid looking for the next elected state
        /// </summary>
        /// <param name="startingRow"></param>
        /// <returns></returns>
        private int GetNextElectedState(int startingRow)
        {
            int row = -1;
            if (startingRow < 0) startingRow = 0;
            for (int i = startingRow; i < DgvStates.Rows.Count; i++)
            {
                if (DgvStates.Rows[i].Cells["colWinningParty"].Value.ToString().Length > 0)
                {
                    row = i;
                    break;
                }
            }
            return row;
        }

        private void DgvStates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  //Clicked on header, do nothing

            DataGridViewRow row = DgvStates.Rows[e.RowIndex];
            int ecStateVote = int.Parse(row.Cells["colECVotes"].Value.ToString());
            bool inProgress = row.Cells["colStateName"].Style.BackColor == Color.LightGray;

            try
            {
                switch (e.ColumnIndex)
                {
                    case 3: // Take button clicked
                        if (_vizBoardLogicMain != null)
                        {
                            if (!chkMaestroFR.Checked)
                            {
                                _vizBoardLogicMain.UsStateBoard(GetStateBoardData(row));
                            }
                            else
                            {
                                _vizBoardLogicMain.UsStateBoardFr(GetStateBoardData(row));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not connected to Main VIZ engine. Please connect then try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case 5: // In Progress button clicked
                        if (inProgress)
                        {
                            ProcessPartyElect("", row.Cells);
                            ExportStateElects();
                        }
                        break;
                    case 6: // Democrats button clicked
                        if (inProgress)
                        {
                            ProcessPartyElect("DEM", row.Cells);
                            ExportStateElects();
                        }
                        break;
                    case 7: // Republicans button clicked
                        if (inProgress)
                        {
                            ProcessPartyElect("GOP", row.Cells);
                            ExportStateElects();
                        }
                        break;
                    case 8: // Others button clicked
                        if (inProgress)
                        {
                            ProcessPartyElect("OTH", row.Cells);
                            ExportStateElects();
                        }
                        break;
                    case 12: // Mini button clicked
                        if (_vizBoardLogicMini != null)
                        {
                            _mainTimer.Enabled = false;
                            _vizBoardLogicMini.UsStateHeroFr(GetStateBoardData(row));
                        }
                        else
                        {
                            MessageBox.Show("Not connected to Mini VIZ engine. Please connect then try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void DgvStates_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;                                                 //Clicked on header, do nothing
            DataGridViewRow row = DgvStates.Rows[e.RowIndex];
            if (row.Cells["colStateName"].Style.BackColor != Color.LightGray) return;   //State is not in progress yet, do nothing
            int ecStateVote = int.Parse(row.Cells["colECVotes"].Value.ToString());

            int demValue = 0;
            int gopValue = 0;
            switch (e.ColumnIndex)
            {
                case 6: // Democrats
                    if (row.Cells["colDemocrats"] is DataGridViewTextBoxCell)
                    {
                        if (row.Cells["colDemocrats"].EditedFormattedValue.ToString().Length == 0) return;
                        if (!IsNumber(row.Cells["colDemocrats"].EditedFormattedValue.ToString()))
                        {
                            MessageBox.Show("Value must be numeric", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            int ecVote = int.Parse(row.Cells["colDemocrats"].EditedFormattedValue.ToString());
                            if (ecVote > ecStateVote)
                            {
                                MessageBox.Show("Invalid value.  Must be less than or equal to " + ecStateVote, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                row.Cells["colDemocrats"].Value = ecVote;
                                row.Cells["colDemocrats"].Style.BackColor = Color.Blue;
                                row.Cells["colDemocrats"].Style.ForeColor = Color.White;
                                if (ecVote < ecStateVote)
                                {
                                    row.Cells["colRepublicans"].Value = (ecStateVote - ecVote);
                                    row.Cells["colRepublicans"].Style.ForeColor = Color.White;
                                    row.Cells["colRepublicans"].Style.BackColor = Color.Red;
                                }
                                else
                                {
                                    row.Cells["colRepublicans"].Value = "0";
                                }
                            }
                        }
                    }
                    try
                    {
                        demValue = int.Parse(row.Cells["colDemocrats"].Value.ToString());
                        gopValue = int.Parse(row.Cells["colRepublicans"].Value.ToString());
                        if (demValue > 0 && demValue > gopValue)
                        {
                            row.Cells["colWinningParty"].Value = "DEM";
                            if (_allowCanopyUpdates)
                            {
                                var response = await ElectStateAsync(row.Cells["colStateCode"].Value.ToString(), "DEM", "X");
                                if (response.StatusCode != HttpStatusCode.OK)
                                {
                                    _logger.Error(response);
                                }
                                else
                                {
                                    _logger.Info("Elect in state " + row.Cells["colStateName"].Value.ToString() + " for the 'DEM' party sent to Canopy.");
                                }
                            }
                        }
                        if (gopValue > 0 && gopValue > demValue)
                        {
                            row.Cells["colWinningParty"].Value = "GOP";
                            if (_allowCanopyUpdates)
                            {
                                var response = await ElectStateAsync(row.Cells["colStateCode"].Value.ToString(), "GOP", "X");
                                if (response.StatusCode != HttpStatusCode.OK)
                                {
                                    _logger.Error(response);
                                }
                                else
                                {
                                    _logger.Info("Elect in state " + row.Cells["colStateName"].Value.ToString() + " for the 'GOP' party sent to Canopy.");
                                }
                            }
                        }
                        UpdateECValues();

                    }
                    catch (Exception)
                    {
                    }
                    break;

                case 7: // Republicans
                    if (row.Cells["colRepublicans"] is DataGridViewTextBoxCell)
                    {
                        if (row.Cells["colRepublicans"].EditedFormattedValue.ToString().Length == 0) return;
                        if (!IsNumber(row.Cells["colRepublicans"].EditedFormattedValue.ToString()))
                        {
                            MessageBox.Show("Value must be numeric", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            int ecVote = int.Parse(row.Cells["colRepublicans"].EditedFormattedValue.ToString());
                            if (ecVote > ecStateVote)
                            {
                                MessageBox.Show("Invalid value.  Must be less than or equal to " + ecStateVote, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                row.Cells["colRepublicans"].Value = ecVote;
                                row.Cells["colRepublicans"].Style.BackColor = Color.Red;
                                row.Cells["colRepublicans"].Style.ForeColor = Color.White;
                                if (ecVote < ecStateVote)
                                {
                                    row.Cells["colDemocrats"].Value = (ecStateVote - ecVote);
                                    row.Cells["colDemocrats"].Style.ForeColor = Color.White;
                                    row.Cells["colDemocrats"].Style.BackColor = Color.Blue;
                                }
                                else
                                {
                                    row.Cells["colDemocrats"].Value = "0";
                                }
                            }
                        }
                    }
                    try
                    {
                        demValue = int.Parse(row.Cells["colDemocrats"].Value.ToString());
                        gopValue = int.Parse(row.Cells["colRepublicans"].Value.ToString());
                        if (demValue > 0 && demValue > gopValue)
                        {
                            row.Cells["colWinningParty"].Value = "DEM";
                            if (_allowCanopyUpdates)
                            {
                                var response = await ElectStateAsync(row.Cells["colStateCode"].Value.ToString(), "DEM", "X");
                                if (response.StatusCode != HttpStatusCode.OK)
                                {
                                    _logger.Error(response);
                                }
                                else
                                {
                                    _logger.Info("Elect in state " + row.Cells["colStateName"].Value.ToString() + " for the 'DEM' party sent to Canopy.");
                                }
                            }
                        }
                        if (gopValue > 0 && gopValue > demValue)
                        {
                            row.Cells["colWinningParty"].Value = "GOP";
                            if (_allowCanopyUpdates)
                            {
                                var response = await ElectStateAsync(row.Cells["colStateCode"].Value.ToString(), "GOP", "X");
                                if (response.StatusCode != HttpStatusCode.OK)
                                {
                                    _logger.Error(response);
                                }
                                else
                                {
                                    _logger.Info("Elect in state " + row.Cells["colStateName"].Value.ToString() + " for the 'GOP' party sent to Canopy.");
                                }
                            }
                        }
                        UpdateECValues();

                    }
                    catch (Exception)
                    {
                    }
                    break;
            }
        }

        /// <summary>
        /// Populates a USHeroState object with data for the currently selected state.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="ecStateVote"></param>
        /// <returns></returns>
        private USHeroState GetStateBoardData(DataGridViewRow row)
        {
            int ecStateVote = int.Parse(row.Cells["colECVotes"].Value.ToString());
            USHeroState heroState = new USHeroState
            {
                StateCode = row.Cells["colStateCode"].Value.ToString(),
                ElectoralCollegeVotes = ecStateVote,
                ElectPartyCode = row.Cells["colWinningParty"].Value.ToString()
            };
            heroState.Elect = heroState.ElectPartyCode.Length > 0;
            List<USParty> parties = new List<USParty>
            {
                new USParty
                {
                    PartyCode = "DEM",
                    ElectoralCollegeVotes = heroState.ElectPartyCode == "DEM" ? ecStateVote : 0,
                    VotePercentage = Convert.ToDouble(row.Cells["colDemocrats"].ToolTipText)
                },
                new USParty
                {
                    PartyCode = "GOP",
                    ElectoralCollegeVotes = heroState.ElectPartyCode == "GOP" ? ecStateVote : 0,
                    VotePercentage = Convert.ToDouble(row.Cells["colRepublicans"].ToolTipText)
                },
                new USParty
                {
                    PartyCode = heroState.StateCode == "UT" ? "IND" : "OTH",
                    ElectoralCollegeVotes = heroState.ElectPartyCode == "OTH" ? ecStateVote : 0,
                    VotePercentage = Convert.ToDouble(row.Cells["colOther"].ToolTipText)
                }
            };
            heroState.Parties = parties;
            return heroState;
        }

        private void HandleOnDataReady(object sender, DataReadyEventArgs e)
        {
            SetMessage(e);
        }

        private void SetMessage(DataReadyEventArgs e)
        {
            _logger.Info("VIZ Command:  " + e.CommandNumber + " " + e.Command);
        }

        private static void HandleOnDataReceived(object sender, byte[] data)
        {
            _logger.Info("VIZ Response: " + Encoding.Default.GetString(data));
        }

        private static void HandleOnDisconnected(object sender, RemoteServerInfo serverInfo)
        {
            MessageBox.Show(@"Lost connection to VIZ Engine " + serverInfo, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Only allow numbers in the Close Race text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCloseRace_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SetButton(Button sender)
        {
            SetButtonBackground(false, _currentButton);
            _currentButton = sender;
            SetButtonBackground(true, _currentButton);
            _mapActive = false;
        }

        private void SetButtonBackground(bool active, Button button)
        {
            if (button != null)
            {
                if (active)
                {
                    button.BackColor = Color.LightGreen;
                }
                else
                {
                    button.BackColor = Color.Transparent;
                }
                this.Refresh();
            }
        }

        private void BtnMainBoardsContinue_Click(object sender, EventArgs e)
        {
            SetButtonBackground(false, _currentButton);
            _currentButton = null;
            _vizBoardLogicMain.ContinueScene();
        }

        private async void CmdVIZEngineMainConnect_Click(object sender, EventArgs e)
        {
            var vizConfig = (VIZConfig)cboVIZEngineMain.SelectedItem;
            _vizBoardLogicMain = new VizBoardLogic(new RemoteServerInfo
            {
                Host = vizConfig.Host,
                Port = vizConfig.Port,
                Ssl = false
            });
            _vizBoardLogicMain.VizConnection += HandleVizConnectionMain;
            _vizBoardLogicMain.OnDataReady += HandleOnDataReady;
            _vizBoardLogicMain.OnDataReceived += HandleOnDataReceived;
            _vizBoardLogicMain.OnDisconnected += HandleOnDisconnected;

            if (await _vizBoardLogicMain.ConnectToViz())
            {
                _vizBoardLogicMain.SetOnAir();
                Thread.Sleep(1000);
                if (!chkMaestroFR.Checked)
                {
                    _vizBoardLogicMain.LoadScene(_vizScene);
                }
                else
                {
                    _vizBoardLogicMain.LoadScene(_vizSceneFR);
                    _vizBoardLogicMain.VizResetAllDir();
                }
            }
        }

        private async void CmdVIZEngineMiniConnect_Click(object sender, EventArgs e)
        {
            var vizConfig = (VIZConfig)cboVIZEngineMini.SelectedItem;
            _vizBoardLogicMini = new VizBoardLogic(new RemoteServerInfo
            {
                Host = vizConfig.Host,
                Port = vizConfig.Port,
                Ssl = false
            });
            _vizBoardLogicMini.VizConnection += HandleVizConnectionMini;
            _vizBoardLogicMini.OnDataReady += HandleOnDataReady;
            _vizBoardLogicMini.OnDataReceived += HandleOnDataReceived;
            _vizBoardLogicMini.OnDisconnected += HandleOnDisconnected;

            if (await _vizBoardLogicMini.ConnectToViz())
            {
                _vizBoardLogicMini.SetOnAir();
                Thread.Sleep(1000);
                if (!chkMaestroFR.Checked)
                {
                    _vizBoardLogicMini.LoadScene(_vizScene);
                }
                else
                {
                    _vizBoardLogicMini.LoadScene(_vizSceneFR);
                    _vizBoardLogicMini.VizResetMinis();
                }
            }
        }

        private void HandleVizConnectionMain(bool connected, RemoteServerInfo vizEngine)
        {
            SetConnectionStatus(cboVIZEngineMain, vizEngine, connected ? ConnectStatus.Normal : ConnectStatus.Error);
        }

        private void HandleVizConnectionMini(bool connected, RemoteServerInfo vizEngine)
        {
            SetConnectionStatus(cboVIZEngineMini, vizEngine, connected ? ConnectStatus.Normal : ConnectStatus.Error);
        }

        public void OptBackgroundOn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMain != null)
                {
                    _vizBoardLogicMain.VizBackground(true);
                }
            }
        }

        private void OptBackgroundOff_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMain != null)
                {
                    if (chkMaestroFR.Checked)
                    {
                        _vizBoardLogicMain.ContinueScene();
                    }
                    _vizBoardLogicMain.VizBackground(false);
                }
            }
        }


        /// <summary>
        /// Set the sort mode of each column to be the same in a DataGridView control
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="sortMode"></param>
        private void SetGridViewSortState(DataGridView dgv, DataGridViewColumnSortMode sortMode)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = sortMode;
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ViewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string logFile = LogFile;
            if (logFile.Length == 0)
            {
                MessageBox.Show("No log file has been specified.  Cannot view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!File.Exists(logFile))
                {
                    MessageBox.Show("Log file " + logFile + " does not exist.  Cannot view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string logFileViewer = "C:\\Program Files\\PSPad editor\\pspad.exe";
                    if (!File.Exists(logFileViewer))
                    {
                        logFileViewer = "C:\\Program Files (x86)\\PSPad editor\\pspad.exe";
                        if (!File.Exists(logFileViewer))
                        {
                            logFileViewer = "notepad.exe";
                        }
                    }
                    try
                    {
                        //Will work if PSPad is on the path
                        Process.Start(logFileViewer, logFile);
                    }
                    catch (ApplicationException ex)
                    {
                        //We tried our best...
                        MessageBox.Show("An unexpected error occured while trying to view log file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ResetResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_allowCanopyUpdates)
            {
                MessageBox.Show("You cannot use this menu item. Your instance of Maestro is currently not configured to allow Canopy updates", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Warning. This will un-elect any elects currently recorded in the Canopy database. Are you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in DgvStates.Rows)
                {
                    if ((row.Cells["colWinningParty"].Value.ToString().Length > 0))
                    {
                        ProcessPartyElect("", row.Cells);
                    }
                }
                UpdateECValues();
                MessageBox.Show("Success! All states are now set to un-elected mode.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AllowCanopyUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _allowCanopyUpdates = false;
            if (MessageBox.Show("Warning. Enabling this feature will set the Canopy database into read/write mode. Any changes you make to state elects will be reflected in the Canopy database. Are you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                _allowCanopyUpdates = true;
                lblCanopyStatusValue.Text = "Read/Write";
                MessageBox.Show("Success. Canopy updates enabled.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DisallowCanopyUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Warning. Disabling this feature will mean that any elects you set locally will not be sent to Canopy. Also, if you are currently connected to Canopy to receive updates, any elects you set will be overwritten by Canopy. Are you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                _allowCanopyUpdates = false;
                lblCanopyStatusValue.Text = "Read Only";
                MessageBox.Show("Success. Canopy updates disabled.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportStateElectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(_jsonImportFile))
            {
                var wins = JsonConvert.DeserializeObject<List<USStateWins>>(File.ReadAllText(_jsonImportFile));
                foreach (USStateWins win in wins)
                {
                    foreach (DataGridViewRow row in DgvStates.Rows)
                    {
                        if (row.Cells["colStateCode"].Value.ToString() == win.StateCode)
                        {
                            ProcessPartyElect(win.PartyCode, row.Cells);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Cannot find the import file. Please check config entry <JSONImportFile> in file USElection.xml. Attempted to find file " + _jsonImportFile + " but it does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets the name of the NLog logfile on disk
        /// </summary>
        private string LogFile
        {
            get
            {
                string retval = string.Empty;
                if (((LogManager.Configuration) != null) && (LogManager.Configuration.ConfiguredNamedTargets.Count != 0))
                {
                    Target t = LogManager.Configuration.FindTargetByName("f");
                    if ((t) != null)
                    {
                        NLog.Layouts.Layout layout = ((FileTarget)t).FileName;
                        retval = layout.Render(LogEventInfo.CreateNullEvent());
                    }
                }
                return retval;
            }
        }

        private async void ProcessPartyElect(string partyCode, DataGridViewCellCollection rowCells)
        {
            string clickedParty = "";
            Color clickedPartyColor = Color.Black;
            string ClickedPartyValue = rowCells["colECVotes"].Value.ToString();
            string electFlag = "X";
            string electPartyCode = partyCode;
            string unclickedParty1 = "";
            string unclickedParty2 = "";
            string unclickedParty3 = "";

            switch (partyCode)
            {
                case "":
                    clickedParty = "colInProgress";
                    clickedPartyColor = Color.Green;
                    ClickedPartyValue = rowCells["colInProgress"].Value.ToString();
                    electFlag = "";
                    electPartyCode = rowCells["colWinningParty"].Value.ToString();
                    unclickedParty1 = "colDemocrats";
                    unclickedParty2 = "colRepublicans";
                    unclickedParty3 = "colOther";
                    break;
                case "DEM":
                    clickedParty = "colDemocrats";
                    clickedPartyColor = Color.Blue;
                    unclickedParty1 = "colRepublicans";
                    unclickedParty2 = "colOther";
                    break;
                case "GOP":
                    clickedParty = "colRepublicans";
                    clickedPartyColor = Color.Red;
                    unclickedParty1 = "colDemocrats";
                    unclickedParty2 = "colOther";
                    break;
                case "OTH":
                    clickedParty = "colOther";
                    clickedPartyColor = Color.Gray;
                    unclickedParty1 = "colRepublicans";
                    unclickedParty2 = "colDemocrats";
                    break;
            }

            if (!(rowCells[clickedParty] is DataGridViewTextBoxCell))
            {
                rowCells[clickedParty].Style.BackColor = clickedPartyColor;
                rowCells[clickedParty].Value = ClickedPartyValue;
                rowCells[unclickedParty1].Style.BackColor = rowCells[unclickedParty1].OwningColumn.DefaultCellStyle.BackColor;
                rowCells[unclickedParty1].Value = "";
                rowCells[unclickedParty2].Style.BackColor = rowCells[unclickedParty2].OwningColumn.DefaultCellStyle.BackColor;
                rowCells[unclickedParty2].Value = "";
                if (unclickedParty3.Length > 0)
                {
                    rowCells[unclickedParty3].Style.BackColor = rowCells[unclickedParty3].OwningColumn.DefaultCellStyle.BackColor;
                    rowCells[unclickedParty3].Value = "";
                }

                //Call routine to update Canopy with the CBC call
                if (_allowCanopyUpdates)
                {
                    var response = await ElectStateAsync(rowCells["colStateCode"].Value.ToString(), electPartyCode, electFlag);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _logger.Error(response);
                    }
                    else
                    {
                        if (electFlag.Length > 0)
                        {
                            _logger.Info("Elect in state " + rowCells["colStateName"].Value.ToString() + " for the '" + electPartyCode + "' party sent to Canopy.");
                        }
                        else
                        {
                            _logger.Info("Kill in state " + rowCells["colStateName"].Value.ToString() + " for the '" + electPartyCode + "' party sent to Canopy.");

                        }
                    }
                }
                else
                rowCells["colWinningParty"].Value = partyCode;
                UpdateECValues();
            }

            if (_mapActive)
            {
                if (_vizBoardLogicMain != null)
                {
                    _vizBoardLogicMain.UsMap(GetMapData(), true);
                }
            }

        }

        private void DgvStates_SelectionChanged(object sender, EventArgs e)
        {
                DgvStates.ClearSelection();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                _vizBoardLogicMain.ContinueScene();
            }
            SetButton(btnContinue);
        }


        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                _vizBoardLogicMain.VizReset();
            }
            if (_vizBoardLogicMini != null)
            {
                _vizBoardLogicMini.VizReset();
            }
        }

        /// <summary>
        /// Limit the popular vote text box to numeric values only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtDemPopularVote_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        /// <summary>
        /// Format the popular vote with thousands separators
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtDemPopularVote_Leave(object sender, EventArgs e)
        {
            if (txtDemPopularVote.Text.Length > 0 && IsNumber(txtDemPopularVote.Text))
            {
                txtDemPopularVote.Text = string.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:#,##0}", double.Parse(txtDemPopularVote.Text));
            }
        }

        /// <summary>
        /// Limit the popular vote text box to numeric values only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtGopPopularVote_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        /// <summary>
        /// Format the popular vote with thousands separators
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtGopPopularVote_Leave(object sender, EventArgs e)
        {
            if (txtGopPopularVote.Text.Length > 0 && IsNumber(txtGopPopularVote.Text))
            {
                txtGopPopularVote.Text = string.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:#,##0}", double.Parse(txtGopPopularVote.Text));
            }
        }

        private void TxtOthPopularVote_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void TxtOthPopularVote_Leave(object sender, EventArgs e)
        {
            if (txtOthPopularVote.Text.Length > 0 && IsNumber(txtOthPopularVote.Text))
            {
                txtOthPopularVote.Text = string.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:#,##0}", double.Parse(txtOthPopularVote.Text));
            }
        }

        private void BtnMiniECVotes_Click(object sender, EventArgs e)
        {
            ProcessMini("ECVotes");
        }

        private void BtnMiniVoteShare_Click(object sender, EventArgs e)
        {
            ProcessMini("VoteShare");
        }


        private void BtnMiniClear_Click(object sender, EventArgs e)
        {
            _mainTimer.Enabled = false;
            if (_vizBoardLogicMini != null)
            {
                _vizBoardLogicMini.UsMiniOff();
            }
        }


        private void OptBrandOn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizBrand(true);
                }
            }
        }

        private void OptBrandOff_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizBrand(false);
                }
            }
        }


        private void OptLiveOn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizLive(true);
                }
            }
        }

        private void OptLiveOff_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizLive(false);
                }
            }
        }

        private void OptGemOn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizGem(true);
                }
            }
        }

        private void OptGemOff_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizGem(false);
                }
            }
        }

        private void OptMiniResultsOn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizMiniResults(true);

                }
                _stateTimer.Interval = 500;
                _stateTimer.Enabled = true;
            }
        }

        private void OptMiniResultsOff_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizMiniResults(false);
                }
                _stateTimer.Enabled = false;
                lblUSStateCodeResults.Text = "";
            }
        }

        private void OptZeroMiniOn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizZeroMini(true);
                }
            }
        }

        private void OptZeroMiniOff_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizZeroMini(false);
                }
            }
        }

        private void OptMiniProjectionOn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizMiniProjection(true, lblDemECVotes.Text, lblRepECVotes.Text);
                }
            }
        }

        private void OptMiniProjectionOff_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMini != null)
                {
                    _vizBoardLogicMini.VizMiniProjection(false, lblDemECVotes.Text, lblRepECVotes.Text);
                }
            }
        }

        private void CmdMiniBackgroundOff_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMini != null)
            {
                _vizBoardLogicMini.VizMiniBackground(false);
            }
        }

        private void CmdTimeZone1_Click(object sender, EventArgs e)
        {
            SetTimeZone("1", cmdTimeZone1);
        }

        private void CmdTimeZone2_Click(object sender, EventArgs e)
        {
            SetTimeZone("2", cmdTimeZone2);
        }

        private void CmdTimeZone3_Click(object sender, EventArgs e)
        {
            SetTimeZone("3", cmdTimeZone3);
        }

        private void CmdTimeZone4_Click(object sender, EventArgs e)
        {
            SetTimeZone("4", cmdTimeZone4);
        }

        private void CmdTimeZone5_Click(object sender, EventArgs e)
        {
            SetTimeZone("5", cmdTimeZone5);
        }

        private void CmdTimeZone6_Click(object sender, EventArgs e)
        {
            SetTimeZone("6", cmdTimeZone6);
        }

        private void CmdTimeZone7_Click(object sender, EventArgs e)
        {
            SetTimeZone("7", cmdTimeZone7);
        }

        private void CmdTimeZone8_Click(object sender, EventArgs e)
        {
            SetTimeZone("8", cmdTimeZone8);
        }

        private void CmdTimeZone9_Click(object sender, EventArgs e)
        {
            SetTimeZone("9", cmdTimeZone9);
        }

        /// <summary>
        /// Updates Canopy with state elect for given party
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="winningParty"></param>
        /// <param name="electValue"></param>
        private async Task<HttpResponseMessage> ElectStateAsync(string stateCode, string winningParty, string electValue)
        {
            HttpResponseMessage responseMessage = null;
            if (winningParty == "DEM") winningParty = "Dem";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("slug", stateCode + "-" + winningParty),
                new KeyValuePair<string, string>("value", electValue)
            });
            try
            {
                responseMessage = await _client.PostAsync(_postElectEndPoint, content);
            }
            catch (Exception ex)
            {
                if (responseMessage == null)
                {
                    responseMessage = new HttpResponseMessage();
                }
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                responseMessage.ReasonPhrase = string.Format("ElectStateAsync failed: {0}", ex);
            }
            return responseMessage;
        }

        /// <summary>
        /// Process request to generate the mini
        /// </summary>
        /// <param name="miniType"></param>
        /// <param name="demValue"></param>
        /// <param name="gopValue"></param>
        private void ProcessMini(string miniType)
        {
            string demValue = lblDemECVotes.Text;
            string gopValue = lblRepECVotes.Text;
            string othValue = lblOthECVotes.Text;

            _miniType = miniType;
            if (_miniType == "VoteShare")
            {
                demValue = txtDemPopularVote.Text;
                gopValue = txtGopPopularVote.Text;
                othValue = txtOthPopularVote.Text;
            }
            string miniMessage = (chkMiniMessage.Checked ? cboMiniMessages.Text : "").ToString();
            try
            {
                if (_vizBoardLogicMini != null)
                {
                    if (!chkMaestroFR.Checked)
                    {
                        _vizBoardLogicMini.UsMini(miniType, demValue, gopValue, othValue, miniMessage);
                    }
                    else
                    {
                        _vizBoardLogicMini.UsMiniFr(miniType, demValue, gopValue, othValue);
                        _mainTimer.Interval = 2000;
                    }
                    _mainTimer.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Not connected to Mini VIZ engine. Please connect then try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Determines if passed in string evaluates as a number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean IsNumber(String value)
        {
            return value.All(char.IsDigit);
        }

        /// <summary>
        /// Update the Electoral College votes for the 2 main parties from the current selections on
        /// the state grid
        /// </summary>
        private void UpdateECValues()
        {
            int demECVotes = 0;
            int repECVotes = 0;
            int othECVotes = 0;
            // Iterate through the grid and total up the
            // electoral college votes for both parties
            foreach (DataGridViewRow row in DgvStates.Rows)
            {
                if ((row.Cells["colDemocrats"].Value.ToString() != ""))
                {
                    demECVotes = (demECVotes + int.Parse(row.Cells["colDemocrats"].Value.ToString()));
                }

                if ((row.Cells["colRepublicans"].Value.ToString() != ""))
                {
                    repECVotes = (repECVotes + int.Parse(row.Cells["colRepublicans"].Value.ToString()));
                }
                if ((row.Cells["colOther"].Value.ToString() != ""))
                {
                    othECVotes = (othECVotes + int.Parse(row.Cells["colOther"].Value.ToString()));
                }
            }

            // Assign these values to our mini labels
            lblDemECVotes.Text = demECVotes.ToString();
            lblRepECVotes.Text = repECVotes.ToString();
            lblOthECVotes.Text = othECVotes.ToString();
        }

        private void SetTimeZone(string zone, Button timeZone)
        {
            if (timeZone.BackColor == Color.Green)
            {
                Text = ("US Election 2016");
                timeZone.BackColor = SystemColors.Control;
                timeZone.ForeColor = SystemColors.ControlText;
                foreach (DataGridViewRow row in DgvStates.Rows)
                {
                    if ((row.Cells["colTimeZone"].Value.ToString() == zone))
                    {
                        row.Cells["colStateName"].Style.BackColor = row.Cells["colStateName"].OwningColumn.DefaultCellStyle.BackColor;
                        row.Cells["colInProgress"].Style.BackColor = row.Cells["colInProgress"].OwningColumn.DefaultCellStyle.BackColor;
                        row.Cells["colInProgress"].Style.ForeColor = Color.White;
                        row.Cells["colInProgress"].Value = "";
                    }
                }
            }
            else
            {
                Text = ("US Election 2016 - " + timeZone.Text);
                timeZone.BackColor = Color.Green;
                timeZone.ForeColor = Color.White;
                foreach (DataGridViewRow row in DgvStates.Rows)
                {
                    if ((row.Cells["colTimeZone"].Value.ToString() == zone))
                    {
                        row.Cells["colStateName"].Style.BackColor = Color.LightGray;
                        row.Cells["colInProgress"].Style.BackColor = Color.Green;
                        row.Cells["colInProgress"].Style.ForeColor = Color.White;
                        row.Cells["colInProgress"].Value = timeZone.Text;
                    }
                }
            }
        }

        private void CmdMap_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                if (!chkMaestroFR.Checked)
                {
                    _vizBoardLogicMain.UsMap(GetMapData(), false);
                }
                else
                {
                    //_vizBoardLogicMain.USMapFR(GetMapData(), false);
                    _mapTimer.Enabled = true;
                }
            }
            SetButton(cmdMap);
            _mapActive = true;
        }

        private string GetMapData()
        {
            string dataPoolString = "";
            foreach (DataGridViewRow row in DgvStates.Rows)
            {
                dataPoolString += row.Cells["colStateCode"].Value + "=";
                if (row.Cells["colWinningParty"].Value.ToString() == "DEM")
                {
                    dataPoolString += "D;";    //Called for the Democrats
                }
                else if (row.Cells["colWinningParty"].Value.ToString() == "GOP")
                {
                    dataPoolString += "R;";    //Called for the Republicans
                }
                else if (row.Cells["colWinningParty"].Value.ToString() == "OTH")
                {
                    dataPoolString += "I;";    //Called for an Independant candidate
                }
                else if (row.Cells["colStateName"].Style.BackColor == Color.LightGray)
                {
                    dataPoolString += "U;";    //State is in progress, no call yet
                }
                else
                {
                    dataPoolString += "N;";    //No results yet
                }
                dataPoolString += "\n";
            }
            return dataPoolString;
        }

        private void CmdMapPrevious_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                if (!chkMaestroFR.Checked)
                {
                    _vizBoardLogicMain.UsMapPrevious();
                }
                else
                {
                    _mapTimer.Enabled = false;
                    _vizBoardLogicMain.UsMapPreviousFr();
                }
            }
            SetButton(cmdMapPrevious);
        }

        private void BtnDemocratCall_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                if (!chkMaestroFR.Checked)
                {
                    _vizBoardLogicMain.UsCall("Clinton", "DEM");
                }
                else
                {
                    _vizBoardLogicMain.UsCallFr("Clinton", "DEM");
                }
            }
            if (_allowCanopyUpdates)
            {
                SendNationalCallToCanopy("DEM");
            }
            SetButton(btnDemocratCall);
        }

        private void BtnRepublicanCall_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                if (!chkMaestroFR.Checked)
                {
                    _vizBoardLogicMain.UsCall("Trump", "GOP");
                }
                else
                {
                    _vizBoardLogicMain.UsCallFr("Trump", "GOP");
                }
            }
            if (_allowCanopyUpdates)
            {
                SendNationalCallToCanopy("GOP");
            }
            SetButton(btnRepublicanCall);
        }

        private void CmdSenate_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                if (!chkMaestroFR.Checked)
                {
                    _vizBoardLogicMain.UsSenate(lblSDemTotal.Text, lblSGopTotal.Text, lblSOthTotal.Text);
                }
                else
                {
                    _vizBoardLogicMain.UsSenateFr(lblSDemTotal.Text, lblSGopTotal.Text, lblSOthTotal.Text);
                }
            }
            SetButton(cmdSenate);
        }

        private void CmdHouse_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                if (!chkMaestroFR.Checked)
                {
                    _vizBoardLogicMain.UsHouse(txtHDem.Text, txtHRep.Text, txtHOth.Text);
                }
                else
                {
                    _vizBoardLogicMain.UsHouseFr(txtHDem.Text, txtHRep.Text, txtHOth.Text);
                }
            }
            SetButton(cmdHouse);
        }

        private void OptFeed_CheckedChanged(object sender, EventArgs e)
        {
            if (optFeedOn.Checked)
            {
                _canopyTimer.Enabled = true;
                _logger.Info("Canopy timer started. Checking for changes every " + CanopyRefreshSeconds.Value + " seconds.");
                grpCanopyTimer.BackColor = Color.Green;
            }
            else
            {
                _canopyTimer.Enabled = false;
                _logger.Info("Canopy timer stopped");
                grpCanopyTimer.BackColor = Color.Red;
            }
        }


        /// <summary>
        /// Send CBC call to Canopy for the entire US
        /// </summary>
        /// <param name="partyCode"></param>
        private async void SendNationalCallToCanopy(string partyCode)
        {
            var response = await ElectStateAsync("US", partyCode, "X");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.Error(response);
            }
            else
            {
                _logger.Info("National call for the '" + partyCode + "' party sent to Canopy.");
            }
        }
        private async static Task<USTopLevel> GetTopLevelData(string topLevelEndpoint)
        {
            var USTopLevel = new USTopLevel();
            try
            {
                var responseBody = await _client.GetStringAsync(topLevelEndpoint);
                USTopLevel = JsonConvert.DeserializeObject<USTopLevel>(responseBody);
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e, "failed to get Top Level Data...");
            }
            return USTopLevel;
        }

        private async static Task<USFullResults> GetFullResultsData(string fullResultsEndPoint)
        {
            var USFullResults = new USFullResults();
            try
            {
                var responseBody = await _client.GetStringAsync(fullResultsEndPoint);
                USFullResults = JsonConvert.DeserializeObject<USFullResults>(responseBody);
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e, "failed to get State Level Data...");
            }
            return USFullResults;
        }

        private void UpdateTopLevelValues(USTopLevel topLevel)
        {
            foreach (President president in topLevel.president)
            {
                if (president.party.ToUpper() == "GOP")
                {
                    txtGopPopularVote.Text = president.popularVotePct;
                }
                else if (president.party.ToUpper() == "DEM")
                {
                    txtDemPopularVote.Text = president.popularVotePct;
                }
                else if (president.party.ToUpper() == "OTH")
                {
                    txtOthPopularVote.Text = president.popularVotePct;
                }
            }

            foreach (Party house in topLevel.house.parties)
            {
                if (house.party.ToUpper() == "GOP")
                {
                    txtHRep.Text = house.membersElected.ToString();
                }
                else if (house.party.ToUpper() == "DEM")
                {
                    txtHDem.Text = house.membersElected.ToString();
                }
                else if (house.party.ToUpper() == "OTH")
                {
                    txtHOth.Text = house.membersElected.ToString();
                }
            }

            foreach (Party senate in topLevel.senate.parties)
            {
                if (senate.party.ToUpper() == "GOP")
                {
                    txtSRep.Text = IsNumber(senate.membersElected.ToString()) ? senate.membersElected.ToString() : "0";
                }
                else if (senate.party.ToUpper() == "DEM")
                {
                    txtSDem.Text = IsNumber(senate.membersElected.ToString()) ? senate.membersElected.ToString() : "0";
                }
                else if (senate.party.ToUpper() == "OTH")
                {
                    txtSOth.Text = IsNumber(senate.membersElected.ToString()) ? senate.membersElected.ToString() : "0";
                }
            }
        }

        private void UpdateStateGrid(USFullResults fullResults)
        {
            foreach (DataGridViewRow row in DgvStates.Rows)
            {
                UpdateStateRow(row, fullResults.president);
            }
            UpdateECValues();
        }

        private void UpdateStateRow(DataGridViewRow row, USPresidentState president)
        {
            // Reset all calls (either from AP or from CBC)
            row.Cells["colApCall"].Value = "";
            row.Cells["colApCall"].Style.BackColor = row.Cells["colApCall"].OwningColumn.DefaultCellStyle.BackColor;
            row.Cells["colApCall"].Style.ForeColor = row.Cells["colApCall"].OwningColumn.DefaultCellStyle.ForeColor;
            if (row.Cells["colDemocrats"].GetType() != typeof(DataGridViewTextBoxCell))
            {
                row.Cells["colDemocrats"].Value = "";
                row.Cells["colDemocrats"].Style.BackColor = row.Cells["colDemocrats"].OwningColumn.DefaultCellStyle.BackColor;
                row.Cells["colDemocrats"].Style.ForeColor = row.Cells["colDemocrats"].OwningColumn.DefaultCellStyle.ForeColor;
            }
            if (row.Cells["colDemocrats"].GetType() != typeof(DataGridViewTextBoxCell))
            {
                row.Cells["colRepublicans"].Value = "";
                row.Cells["colRepublicans"].Style.BackColor = row.Cells["colRepublicans"].OwningColumn.DefaultCellStyle.BackColor;
                row.Cells["colRepublicans"].Style.ForeColor = row.Cells["colRepublicans"].OwningColumn.DefaultCellStyle.ForeColor;
            }
            if (row.Cells["colOther"].GetType() != typeof(DataGridViewTextBoxCell))
            {
                row.Cells["colOther"].Value = "";
                row.Cells["colOther"].Style.BackColor = row.Cells["colOther"].OwningColumn.DefaultCellStyle.BackColor;
                row.Cells["colOther"].Style.ForeColor = row.Cells["colOther"].OwningColumn.DefaultCellStyle.ForeColor;
            }

            string usStateCode = row.Cells["colStateCode"].Value.ToString();
            PresidentState usState = GetUSState(president, usStateCode);
            if (usState != null)
            {
                foreach (PresidentCandidate candidate in usState.candidates)
                {
                    if (candidate.party.ToUpper() == "DEM")
                    {
                        UpdateState(row, candidate, "DEM", "colDemocrats", Color.Blue, Color.White);
                    }
                    else if (candidate.party.ToUpper() == "GOP")
                    {
                        UpdateState(row, candidate, "GOP", "colRepublicans", Color.Red, Color.White);
                    }
                    else if (candidate.party.ToUpper() == "OTH")
                    {
                        UpdateState(row, candidate, "OTH", "colOther", Color.DarkGray, Color.White);
                    }
                }
            }
        }

        private void UpdateState(DataGridViewRow row, PresidentCandidate candidate, string partyCode, string partyColumn, Color backColour, Color foreColour)
        {
            row.Cells[partyColumn].ToolTipText = candidate.popularVotePct;
            if (candidate.winner == "X")
            {
                row.Cells["colApCall"].Value = partyCode;
                row.Cells["colApCall"].Style.BackColor = backColour;
                row.Cells["colApCall"].Style.ForeColor = foreColour;
            }
            if ((row.Cells["colDemocrats"].GetType() != typeof(DataGridViewTextBoxCell)) && (candidate.winner_cbc == "X"))
            {
                row.Cells["colWinningParty"].Value = partyCode;
                row.Cells[partyColumn].Value = row.Cells["colECVotes"].Value.ToString();
                row.Cells[partyColumn].Style.BackColor = backColour;
                row.Cells[partyColumn].Style.ForeColor = foreColour;
            }
        }

        private PresidentState GetUSState(USPresidentState president, string usStateCode)
        {
            switch (usStateCode)
            {
                case "AK": return president.AK;
                case "AL": return president.AL;
                case "AR": return president.AR;
                case "AZ": return president.AZ;
                case "CA": return president.CA;
                case "CO": return president.CO;
                case "CT": return president.CT;
                case "DC": return president.DC;
                case "DE": return president.DE;
                case "FL": return president.FL;
                case "GA": return president.GA;
                case "HI": return president.HI;
                case "IA": return president.IA;
                case "ID": return president.ID;
                case "IL": return president.IL;
                case "IN": return president.IN;
                case "KS": return president.KS;
                case "KY": return president.KY;
                case "LA": return president.LA;
                case "MA": return president.MA;
                case "MD": return president.MD;
                case "ME": return president.ME;
                case "MI": return president.MI;
                case "MN": return president.MN;
                case "MO": return president.MO;
                case "MS": return president.MS;
                case "MT": return president.MT;
                case "NC": return president.NC;
                case "ND": return president.ND;
                case "NE": return president.NE;
                case "NH": return president.NH;
                case "NJ": return president.NJ;
                case "NM": return president.NM;
                case "NV": return president.NV;
                case "NY": return president.NY;
                case "OH": return president.OH;
                case "OK": return president.OK;
                case "OR": return president.OR;
                case "PA": return president.PA;
                case "RI": return president.RI;
                case "SC": return president.SC;
                case "SD": return president.SD;
                case "TN": return president.TN;
                case "TX": return president.TX;
                case "UT": return president.UT;
                case "VA": return president.VA;
                case "VT": return president.VT;
                case "WA": return president.WA;
                case "WI": return president.WI;
                case "WV": return president.WV;
                case "WY": return president.WY;
                default: return null;
            }
        }

        /// <summary>
        /// Read US Election data from external XML documment
        /// </summary>
        /// <param name="xmlFile"></param>
        private void LoadUSElectionDataFromXML(string xmlFile)
        {
            try
            {
                if (File.Exists(xmlFile))
                {
                    XDocument xdoc = XDocument.Load(xmlFile);

                    //Populate the main candidate names
                    XElement xCandidates = xdoc.Root.Element("Candidates");
                    foreach (XElement xElem in xCandidates.Elements())
                    {
                        switch (xElem.Name.LocalName)
                        {
                            case "Democrat":
                                lblDemocrat.Text = xElem.Value;
                                btnDemocratCall.Text = xElem.Value + " Call";
                                break;
                            case "Republican":
                                lblRepublican.Text = xElem.Value;
                                btnRepublicanCall.Text = xElem.Value + " Call";
                                break;
                        }
                    }

                    //Populate the VIZ engine config info
                    XElement xIPconfigs = xdoc.Root.Element("IPConfigs");
                    foreach (XElement xIPConfig in xIPconfigs.Elements("IPConfig"))
                    {
                        var vizConfig = new VIZConfig
                        {
                            Name = xIPConfig.Attribute("name").Value,
                            Host = xIPConfig.Attribute("IPAddress").Value,
                            Port = Convert.ToInt32(xIPConfig.Attribute("Port").Value)
                        };
                        cboVIZEngineMain.Items.Add(vizConfig);
                        cboVIZEngineMini.Items.Add(vizConfig);
                    }
                    if (cboVIZEngineMain.Items.Count > 0) cboVIZEngineMain.SelectedIndex = 0;
                    if (cboVIZEngineMini.Items.Count > 0) cboVIZEngineMini.SelectedIndex = 0;

                    _vizScene = xdoc.Root.Element("VIZConfig").Element("Scene").Value;
                    _vizSceneFR = xdoc.Root.Element("VIZConfig").Element("SceneFR").Value;

                    //Populate the Senate Hold numbers
                    XElement xSenate = xdoc.Root.Element("Senate");
                    foreach (XElement xParty in xSenate.Elements("Party"))
                    {
                        switch (xParty.Attribute("partyCode").Value.ToLower())
                        {
                            case "dem":
                                lblSDemHold.Text = xParty.Attribute("Hold").Value;
                                break;
                            case "gop":
                                lblSGopHold.Text = xParty.Attribute("Hold").Value;
                                break;
                            case "oth":
                                lblSOthHold.Text = xParty.Attribute("Hold").Value;
                                break;
                        }
                    }

                    //Populate the Mini Message dropdown
                    XElement xMessages = xdoc.Root.Element("MiniMessage");
                    foreach (XElement xElem in xMessages.Elements("add"))
                    {
                        cboMiniMessages.Items.Add(xElem.Attribute("Message").Value);
                    }

                    //Populate the state grid
                    XElement xStates = xdoc.Root.Element("States");
                    DgvStates.Rows.Clear();
                    int stateNo = 1;
                    foreach (XElement xState in xStates.Elements("State"))
                    {
                        int index = DgvStates.Rows.Add();
                        DataGridViewRow row = DgvStates.Rows[index];
                        row.Tag = "None";

                        row.Cells["colTimeZone"].Value = xState.Attribute("TimeZone").Value;
                        row.Cells["colStateNumber"].Value = stateNo;
                        row.Cells["colStateCode"].Value = xState.Attribute("code").Value;
                        row.Cells["colStateName"].Value = xState.Attribute("name").Value;
                        row.Cells["colECVotes"].Value = xState.Attribute("ECVotes").Value;
                        row.Cells["colInProgress"].Value = "";
                        if (Convert.ToBoolean(xState.Attribute("allowsVoteSplitting").Value) == true)
                        {
                            DataGridViewTextBoxCell txt1 = new DataGridViewTextBoxCell();
                            txt1.Style.BackColor = Color.LightGray;
                            txt1.Style.ForeColor = Color.Black;
                            txt1.Value = "";
                            DataGridViewTextBoxCell txt2 = new DataGridViewTextBoxCell();
                            txt2.Style.BackColor = Color.LightGray;
                            txt2.Style.ForeColor = Color.Black;
                            txt2.Value = "";
                            DataGridViewTextBoxCell txt3 = new DataGridViewTextBoxCell();
                            txt3.Style.BackColor = Color.LightGray;
                            txt3.Style.ForeColor = Color.Black;
                            txt3.Value = "";
                            row.Cells["colDemocrats"] = txt1;
                            row.Cells["colRepublicans"] = txt2;
                            row.Cells["colOther"] = txt3;
                        }
                        else
                        {
                            row.Cells["colDemocrats"].Value = "";
                            row.Cells["colRepublicans"].Value = "";
                            row.Cells["colOther"].Value = "";
                        }
                        row.Cells["colDemocrats"].ToolTipText = "0";
                        row.Cells["colRepublicans"].ToolTipText = "0";
                        row.Cells["colOther"].ToolTipText = "0";
                        row.Cells["colTake"].Value = "Take";
                        row.Cells["colTake"].ToolTipText = xState.Attribute("name").Value;
                        row.Cells["colWinningParty"].Value = "";
                        row.Cells["colMini"].Value = "Mini";
                        row.Cells["colMini"].ToolTipText = xState.Attribute("name").Value;
                        row.Cells["colWinningParty"].Value = "";
                        stateNo += 1;
                    }

                    SetGridViewSortState(DgvStates, DataGridViewColumnSortMode.NotSortable);

                    //Get the Canopy API endpoints
                    _canopyURI = xdoc.Root.Element("Canopy").Element("BaseAddress").Value;
                    _fullResultsEndPoint = xdoc.Root.Element("Canopy").Element("FullResultsEndPoint").Value;
                    _topLevelResultsEndPoint = xdoc.Root.Element("Canopy").Element("TopLevelResultsEndPoint").Value;
                    _postElectEndPoint = xdoc.Root.Element("Canopy").Element("PostElectEndPoint").Value;
                    _jsonExportFile = xdoc.Root.Element("Canopy").Element("JSONExportFile").Value;
                    _jsonImportFile = xdoc.Root.Element("Canopy").Element("JSONImportFile").Value;
                }
                else
                {
                    MessageBox.Show("Cannot find US Election XML document", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetConnectionStatus(ComboBox cbo, RemoteServerInfo vizEngine, ConnectStatus connectStatus)
        {
            if (connectStatus == ConnectStatus.Normal)
            {
                cbo.BackColor = Color.Green;
                cbo.ForeColor = Color.White;
                _logger.Info("Connected to VIZ Engine: " + vizEngine.ToString());
            }
            else
            {
                cbo.BackColor = Color.Red;
                cbo.ForeColor = Color.White;
                _logger.Error("Unable to connect to VIZ Engine: " + vizEngine.ToString());
            }
        }

        private void TxtSDem_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void TxtSDem_TextChanged(object sender, EventArgs e)
        {
            if (txtSDem.Text.Length > 0)
            {
                lblSDemTotal.Text = (Convert.ToInt32(txtSDem.Text) + Convert.ToInt32(lblSDemHold.Text)).ToString();
            }
        }

        private void TxtSRep_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void TxtSRep_TextChanged(object sender, EventArgs e)
        {
            if (txtSRep.Text.Length > 0)
            {
                lblSGopTotal.Text = (Convert.ToInt32(txtSRep.Text) + Convert.ToInt32(lblSGopHold.Text)).ToString();
            }
        }

        private void TxtSOth_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void TxtSOth_TextChanged(object sender, EventArgs e)
        {
            if (txtSOth.Text.Length > 0)
            {
                lblSOthTotal.Text = (Convert.ToInt32(txtSOth.Text) + Convert.ToInt32(lblSOthHold.Text)).ToString();
            }
        }

        /// <summary>
        /// Create an external JSON file with all current elects
        /// </summary>
        private void ExportStateElects()
        {
            if (!Directory.Exists("C:\\USData2016"))
            {
                Directory.CreateDirectory("C:\\USData2016");
            }
            List<USStateWins> wins = new List<USStateWins>();
            foreach (DataGridViewRow row in DgvStates.Rows)
            {
                if (row.Cells["colWinningParty"].Value.ToString().Length > 0)
                {
                    wins.Add(new USStateWins
                    {
                        StateCode = row.Cells["colStateCode"].Value.ToString(),
                        PartyCode = row.Cells["colWinningParty"].Value.ToString()
                    });
                }
            }
            File.WriteAllText(_jsonExportFile, JsonConvert.SerializeObject(wins));
        }

        private void ChkMaestroFR_CheckedChanged(object sender, EventArgs e)
        {
            //btnTest.Visible = chkMaestroFR.Checked;
            grpMaestroFR.Visible = chkMaestroFR.Checked;
            btnReset.Visible = !chkMaestroFR.Checked;
            pnlVIZBackground.Visible = !chkMaestroFR.Checked;

            if (chkMaestroFR.Checked)
            {
                CanopyRefreshSeconds.Value = 25;

                DgvStates.Columns["colStateName"].Width -= 35;
                DgvStates.Columns["colTake"].Width -= 10;
                DgvStates.Columns["colEcVotes"].Width -= 10;

                DgvStates.Columns["colMini"].Visible = true;
            }
            else
            {
                CanopyRefreshSeconds.Value = 10;

                DgvStates.Columns["colStateName"].Width += 35;
                DgvStates.Columns["colTake"].Width += 10;
                DgvStates.Columns["colEcVotes"].Width += 10;

                DgvStates.Columns["colMini"].Visible = false;
            }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                _vizBoardLogicMain.UsTest();
            }
            if (_vizBoardLogicMini != null)
            {
                _vizBoardLogicMini.UsTest();
            }
            SetButton(btnTest);
        }

        private void BtnStanding_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                _vizBoardLogicMain.UsStandingFr(lblDemECVotes.Text,txtDemPopularVote.Text, lblRepECVotes.Text,txtGopPopularVote.Text);
            }
            SetButton(btnStanding);
        }

        private void BtnResetMain_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                _vizBoardLogicMain.VizResetAllDir();

                _vizBoardLogicMain.ContinueScene();

                optMapOn.Checked = false;
                chkMapYear.Checked = false;
                chkAnimatedBG.Checked = true;
            }
        }

        private void BtnResetMini_Click(object sender, EventArgs e)
        {
            if (_vizBoardLogicMini != null)
            {
                _vizBoardLogicMini.VizResetMinis();
            }
        }

        private void OptMapOn_CheckedChanged(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMain != null)
                {
                    _vizBoardLogicMain.VizBackground(true);
                    _vizBoardLogicMain.UsMapFr(GetMapData(), false);
                    _mapTimer.Enabled = true;
                }
            }
        }

        private void OptMapOff_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (_vizBoardLogicMain != null)
                {
                    _mapTimer.Enabled = false;
                    _vizBoardLogicMain.ContinueScene();
                    _vizBoardLogicMain.UsMapOff();
                    _vizBoardLogicMain.VizBackground(false);
                }
            }
        }

        private void ChkMapYear_CheckedChanged(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                _vizBoardLogicMain.VizYearOnMap(chkMapYear.Checked);
            }
        }

        private void ChkAnimatedBG_CheckedChanged(object sender, EventArgs e)
        {
            if (_vizBoardLogicMain != null)
            {
                _vizBoardLogicMain.VizAnimatedBackground(chkAnimatedBG.Checked);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
    
