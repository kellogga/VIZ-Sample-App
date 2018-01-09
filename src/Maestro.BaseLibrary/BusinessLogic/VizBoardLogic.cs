using System;
using System.Collections.Generic;
using System.Globalization;
using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Config;
using Maestro.BaseLibrary.Entities;
using Maestro.BaseLibrary.Interfaces;
using Maestro.Entities;
using System.Threading;
using VizWrapper;
using System.Threading.Tasks;
using System.Linq;

namespace Maestro.BaseLibrary.BusinessLogic
{
    public class VizBoardLogic
    {
        public event OnCandidateListUpdatedEventHandler OnCandidateListUpdated;
        public delegate void OnCandidateListUpdatedEventHandler(List<Candidate> candidates);

        public event OnVizConnectionHandler VizConnection;
        public delegate void OnVizConnectionHandler(bool connected, RemoteServerInfo vizEngine);

        public event OnDataReadyHandler OnDataReady;
        public delegate void OnDataReadyHandler(object sender, DataReadyEventArgs e);

        public event OnDataReceivedHandler OnDataReceived;
        public delegate void OnDataReceivedHandler(object sender, byte[] data);

        public event OnDisconnectedHandler OnDisconnected;
        public delegate void OnDisconnectedHandler(object sender, RemoteServerInfo vizEngine);

        private string _previousMiniData = "";

        //public string ElectionMode { get { return _vizConfig.Type; } }

        private struct MiniBar
        {
            //VIZ bar object name: P1_ Thru P7_
            public string BarName;
            //Party code associated with this bar
            public string PartyCode;
            //the PartyID associated with this bar
            public int PartyID;
            //Which rank this bar will occupy on the current mini iteration
            public int CurrentRank;
            //The value to be displayed on this bar on the current mini iteration
            public double CurrentValue;
            //Which rank this bar occupied on the last mini iteration
            public int PreviousRank;
            //The value that was displayed on this bar on the last mini iteration
            public double PreviousValue;
        }

        private readonly Viz _viz = new Viz();
        //private DataManager _dataManager;
        private readonly RemoteServerInfo _serverInfo;
        private readonly Riding _riding = new Riding();
        private readonly Region _region = new Region();
        private readonly RegionRiding _regionRiding = new RegionRiding();
        private readonly Summary _summary = new Summary();
        private IBaseBoard _currentBoard;
        private string _miniType;
        private string _miniTypeCurrent;

        //public int MainParties { get { return _vizConfig.NumberOfMainParties; } }
        /// <summary>
        /// Public constructor
        /// </summary>
        public VizBoardLogic(RemoteServerInfo serverInfo)
        {
            _viz.OnDataReady += HandleOnDataReady;
            _viz.OnDataReceived += HandleOnDataReceived;
            _viz.OnDisconnected += HandleOnDisconnected;
            _serverInfo = serverInfo;
        }

        private void HandleOnDisconnected(object sender, EventArgs e)
        {
            OnDisconnected?.Invoke(this, _serverInfo);
        }

        private void HandleOnDataReady(object sender, DataReadyEventArgs e)
        {
            OnDataReady?.Invoke(this, e);
        }

        private void HandleOnDataReceived(object sender, byte[] e)
        {
            OnDataReceived?.Invoke(this, e);
        }

        public async Task<bool> ConnectToViz()
        {
            try
            {
                var connected = await _viz.Connect(_serverInfo);
                VizConnection?.Invoke(connected, _serverInfo);
                return true;
            }
            catch (Exception)
            {
                VizConnection?.Invoke(false, _serverInfo);
                return false;
            }
        }

        public void SetOnAir()
        {
            _viz.SetOnAir();
        }

        public void LoadScene(string scene)
        {
            _viz.LoadScene(scene);
        }

        /// <summary>
        /// Gathers all required data into our _riding object in preparation for generating
        /// one riding board
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="ridingId"></param>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        public string ProcessRidingBoard(Scene scene, int ridingId, int candidateId = 0)
        {
            var retval = "N/A";
            if (scene == null) return retval;
            //Update our riding object with static data 
            //_riding.Update(_dataManager.GetRidingById(ridingId), scene, _vizConfig.Type);
            //Get the dynamic data for this board from the database
            //_riding = _dataManager.GetRidingData(_riding);
            //Start on first page
            _riding.SetPageNumber(1);
            if (candidateId > 0)
            {
                //Extra logic for single hero board.  We need to tell the board logic which candidate
                //is to be displayed on-air, after the dynamic data has been retrieved and the candidates
                //sorted.
                _riding.SetCandidateIndex(candidateId);
            }
            //Store the title of the board in case the user wants to do a refresh
            //Keep track of the current board in a module level variable
            _currentBoard = _riding;
            //Generate board
            GenerateRidingBoard(_riding);
            //Build an informative string which may be used to update the UI
            retval = _riding.CurrentPageNumber + "/" + _riding.NumberOfPages;
            return retval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="boardText"></param>
        /// <param name="party"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public string ProcessNoQueryBoard(Scene scene, string boardText, BaseParty party, int regionId = 0)
        {
            var n = new NoQuery();
            //n.Update(scene, party, _vizConfig.Type, boardText);
            if (regionId > 0)
            {
                //_region.Update(_dataManager.GetRegionById(regionId), scene, _vizConfig.NumberOfMainParties, _vizConfig.Type);
                //n.SetPartyOrder(_dataManager.GetPartyStandingsData(_region));
            }
            //Generate board
            GenerateNoQueryBoard(n);
            //Store the title of the board in case the user wants to do a refresh
            //Update the UI label which tells the user how many pages we have for this board
            return n.CurrentPageNumber + "/" + n.NumberOfPages;
        }

        /// <summary>
        /// Gathers all required data into our _region object in preparation for generating
        /// one region board
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public string ProcessRegionBoard(Scene scene, int regionId)
        {
            var retval = "N/A";
            if ((scene == null)) return retval;
            //Update our region object with static data
            //_region.Update(_dataManager.GetRegionById(regionId), scene, _vizConfig.NumberOfMainParties, _vizConfig.Type);
            //Get the dynamic data for this board from the database
            //_region = _dataManager.GetPartyStandingsData(_region);
            if (_region.TotalItems <= 0) return retval;
            if (scene.Name.ToLower() == "total votes")
            {
                //Extra processing for Total Votes board - run an extra query to retrieve the current
                //number of reporting polls and the total number of polls 
                //_region = _dataManager.GetPollsData(_region);
            }
            //Start on first page
            _region.SetPageNumber(1);
            //Store the title of the board in case the user wants to do a refresh
            //Keep track of the current board in a module level variable
            _currentBoard = _region;
            //Generate board
            GenerateRegionBoard(_region);
            //Update the UI label which tells the user how many pages we have for this board
            retval = _region.CurrentPageNumber + "/" + _region.NumberOfPages;
            return retval;
        }

        public bool IsConnected()
        {
            return _viz.IsConnected();
        }

        public void VizBackground(bool showBackground)
        {
            _viz.VizBackground(showBackground);
        }

        public void VizYearOnMap(bool showYear)
        {
            _viz.SendVizCommand(showYear ? "YEAR=ON;" : "YEAR=OFF;");
        }

        public void VizAnimatedBackground(bool showAnimation)
        {
            _viz.SendVizCommand(showAnimation ? "BG=CONTINUE;" : "BG=STOP;");
        }

        /// <summary>
        /// Gathers all required data into our _region object in preparation for generating
        /// a region/party board
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="regionId"></param>
        /// <param name="party"></param>
        /// <returns></returns>
        public string ProcessRegionPartyBoard(Scene scene, int regionId, BaseParty party)
        {
            var retval = "N/A";
            if ((scene == null)) return retval;
            //Update our region object with static data
            //_region.Update(_dataManager.GetRegionById(regionID), scene, _vizConfig.NumberOfMainParties, _vizConfig.Type, "", party);
            //Get the dynamic data for this board from the database
            //_region = _dataManager.GetPartyStandingsData(_region);
            if (_region.TotalItems <= 0) return retval;
            //Start on first page
            _region.SetPageNumber(1);
            //We've found some data to display so lets continue...
            //Store the title of the board in case the user wants to do a refresh
            //Keep track of the current board in a module level variable
            _currentBoard = _region;
            //Call routine which controls rendering of board to air
            GenerateRegionBoard(_region);
            //Build an informative string which may be used to update the UI
            retval = _region.CurrentPageNumber + "/" + _region.NumberOfPages;
            return retval;
        }

        public string ProcessRegionRidingsBoard(Scene scene, int regionId, BaseParty party, int votesMargin)
        {
            var retval = "N/A";
            if ((scene == null)) return retval;
            //Update our regionriding object with static data
            //_regionRiding.Update(_dataManager.GetRegionById(regionId), scene, _vizConfig.NumberOfMainParties, _vizConfig.Type, party);
            //Get the dynamic data for this board from the database
            switch (scene.Name.ToLower())
            {
                case "3 way race":
                    //_regionRiding = _dataManager.GetRegionRidingsData(_regionRiding, votesMargin, 3);
                    break;
                case "too close to call":
                    //_regionRiding = _dataManager.GetRegionRidingsData(_regionRiding, votesMargin);
                    break;
                case "top 5":
                    //_regionRiding = _dataManager.GetRegionRidingsData(_regionRiding);
                    break;
                case "ub region elect flash":
                    //_regionRiding = _dataManager.GetRegionElectsData(_regionRiding);
                    break;
                case "ub party elect flash":
                    //_regionRiding = _dataManager.GetRegionElectsByPartyData(_regionRiding, party.PartyID);
                    break;
                default:
                    break;
            }
            if (_regionRiding.TotalItems <= 0) return retval;
            //Start on first page
            _regionRiding.SetPageNumber(1);
            //Store the title of the board in case the user wants to do a refresh
            //Keep track of the current board in a module level variable
            _currentBoard = _regionRiding;
            //Call routine which controls rendering of board to air
            GenerateRegionRidingsBoard(_regionRiding);
            //Update the UI label which tells the user how many pages we have for this board
            retval = _regionRiding.CurrentPageNumber.ToString() + "/" + _regionRiding.NumberOfPages.ToString();
            return retval;
        }

        public string ProcessSummaryBoard(Scene scene, int summaryId, BaseParty party)
        {
            var retval = "N/A";
            if (scene == null) return retval;
            if (summaryId <= 0) return retval;
            //Update our regionriding object with static data
            //_summary.Update(_dataManager.GetSummaryById(summaryId), scene, _vizConfig.NumberOfMainParties, _vizConfig.Type, party);
            //Get the dynamic data for this board from the database
            //_summary = _dataManager.GetSummaryData(_summary);
            if (_summary.TotalItems <= 0) return retval;
            //Start on first page
            _summary.SetPageNumber(1);
            //Store the title of the board in case the user wants to do a refresh
            //Keep track of the current board in a module level variable
            _currentBoard = _summary;
            //Call routine which controls rendering of board to air
            GenerateSummaryBoard(_summary);
            //Update the UI label which tells the user how many pages we have for this board
            retval = _summary.CurrentPageNumber + "/" + _summary.NumberOfPages;
            return retval;
        }

        private void GenerateNoQueryBoard(NoQuery n)
        {
            switch (n.BoardKey.ToLower())
            {
                case "projection":
                case "opposition":
                    VizProjection(n);
                    break;
                case "government":
                    VizGovernment(n);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Calls appropriate VIZ generation routine to send one Region board on-air
        /// </summary>
        /// <param name="r"></param>
        private void GenerateRegionBoard(Region r)
        {
            switch (r.BoardKey.ToLower())
            {
                case "mini - seats":
                case "mini - share":
                    //VizMini(r);
                    break;
                case "vote share":
                    VizVoteShare(r);
                    break;
                case "seat change":
                    VizSeatChange(r);
                    break;
                case "vote change":
                    VizVoteChange(r);
                    break;
                case "woman's vote":
                    VizWomansVote(r);
                    break;
                case "party standings":
                    VizPartyStandings(r);
                    break;
                case "total votes":
                    VizTotalVotes(r);
                    break;
                case "party status":
                    VizPartyStatus(r);
                    break;
                case "election night":
                    VizElectionNight(r);
                    break;
                case "possible majority":
                    VizPossibleMajority(r);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Calls appropriate VIZ generation routine to send one Region Riding board on-air
        /// </summary>
        private void GenerateRegionRidingsBoard(RegionRiding r)
        {
            switch (r.BoardKey.ToLower())
            {
                case "too close to call":
                    VizTooCloseToCall(r);
                    break;
                case "3 way race":
                    Viz3WayRace(r);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Call appropriate VIZ routine to generate one Riding board
        /// </summary>
        /// <param name="r"></param>
        private void GenerateRidingBoard(Riding r)
        {
            switch (r.Scene.Name.ToLower())
            {
                case "quad hero":
                case "triple hero":
                case "maphero":
                    // send the list of candidates for this riding to any listeners
                    OnCandidateListUpdated?.Invoke(r.Candidates);
                    VizHero(r);
                    break;
                case "single hero":
                    //The desiredCandiateID has already been set so just call the VizHero board routine
                    VizHero(r);
                    break;
                case "single hero - load":
                    // send the list of candidates for this riding to any listeners
                    OnCandidateListUpdated?.Invoke(r.Candidates);
                    break;
                case "ub riding report":
                    VizUbRidingReport(r);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Call appropriate VIZ routine to generate one Summary board
        /// </summary>
        private void GenerateSummaryBoard(Summary s)
        {
            if (s.Scene.Name.ToLower() == "summary")
                VizSummary(s);
        }

        /// <summary>
        /// Generate a 3 way race board using datapool
        /// </summary>
        /// <param name="r"></param>
        private void Viz3WayRace(RegionRiding r)
        {
            int i;

            _viz.PrepareForNewScene(r);

            //Build the board title
            string boardTitle;
            if (r.MainRegion)
            {
                boardTitle = "3 Way Race";
            }
            else
            {
                //Put the region name in the title if this is not the main region
                boardTitle = "3 Way Race: " + r.DisplayName1;
            }

            var dataPoolData = "THREEWAY[0-300]={" + "\n";
            dataPoolData += "{Title=" + boardTitle + ";}," + "\n";

            //Data for each riding
            var itemsDisplayed = 0;
            for (i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                //Get the Riding object and the first 3 candidate objects
                var riding = r.Ridings[i];
                var c1 = riding.Candidates[0];
                var c2 = riding.Candidates[1];
                var c3 = riding.Candidates[2];
                //Calculate the total difference between 1st and 3rd place, and the percentage of total votes that 
                //number represents
                var marginValue = c1.Votes - c3.Votes;
                var percentageValue = ((float)marginValue / riding.VotesCounted()) * 100;
                dataPoolData += "{RidingName=" + riding.DisplayName1 + ";P1Name=" + c1.Party.PartyCode + ";P2Name=" + c2.Party.PartyCode + ";P3Name=" + c3.Party.PartyCode + ";MarginValue=" + marginValue.ToString() + ";VotePercent=" + percentageValue.ToString("0.#") + ";}," + "\n";
                itemsDisplayed += 1;
            }
            if (itemsDisplayed < r.MaxItemsPerPage)
            {
                for (i = itemsDisplayed + 1; i <= r.MaxItemsPerPage; i++)
                {
                    //Send 0's to objects that are not needed.  They will be hidden by logic embedded in VIZ board.
                    dataPoolData += "{RidingName=HideObject;P1Name=NONE;P2Name=NONE;P3Name=NONE;MarginValue=0;VotePercent=0;}," + "\n";
                }
            }
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate an Election Night board using datapool
        /// </summary>
        /// <param name="r"></param>
        public void VizElectionNight(Region r)
        {
            //Find the Party object we want to use.  If we can't find the party or if there are no votes
            //for the desired party then don't display the board
            var p = r.GetPartyByID(r.DesiredParty.PartyID);
            if (p == null)
                return;
            if (p.CurrentVotes == 0)
                return;

            _viz.PrepareForNewScene(r);

            //Construct headshot name
            var headShot = p.PartyCode + "_Leader";

            //Calculate values needed by board
            var currentSeats = p.CurrentElectedSeats + p.CurrentLeadingSeats;
            var seatChange = (p.CurrentElectedSeats + p.CurrentLeadingSeats) - p.PrevReportingSeats;
            var currentVoteShare = ((float)p.CurrentVotes / r.CurrentTotalVotes) * 100;
            var previousVoteShare = ((float)p.PreviousVotes / r.PreviousTotalVotes) * 100;
            var partyChange = currentVoteShare - previousVoteShare;

            //Build the datapool string
            var dataPoolData = "ElectionNight[0-8]={" + "\n";
            dataPoolData += "{HeadShot=" + headShot + ";MAP=" + r.MapCode + ";}," + "\n";
            dataPoolData += "{Title=Election Night;}," + "\n";
            dataPoolData += "{Party=" + p.PartyCode + ";SeatsValue=" + currentSeats + ";ChangeValue=" + FormatSign(seatChange) + Math.Abs(seatChange) + ";VoteShareValue=" + currentVoteShare.ToString("0.#") + "%;VSChangeValue=" + partyChange.ToString("0.#") + ";}," + "\n";
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate a Government board using datapool
        /// </summary>
        /// <param name="n"></param>
        private void VizGovernment(NoQuery n)
        {
            //Deal with any previous scene loaded in this layer (bring it off air) and load this scene
            _viz.PrepareForNewScene(n);
            //Build the datapool string
            var dataPoolData = "GOVERNMENT[0-50]={" + "\n";
            dataPoolData += "{Projection=" + n.BoardText + ";}," + "\n";
            dataPoolData += "{Title=CBC News Projects;}," + "\n";
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(n.DatapoolName, dataPoolData);
        }

        /// <summary>
        /// Generate a Hero board using datapool
        /// </summary>
        /// <param name="r"></param>
        private void VizHero(Riding r)
        {
            _viz.PrepareForNewScene(r);

            var mapName = r.ProvinceCode;
            var heroNote = "";
            var dataPoolData = "QuadHero[0-8]={" + "\n";
            if (r.Scene.Name.ToLower() == "maphero")
            {
                //Special case for maphero (initiated from external touchscreen operator)
                //The only difference between the MapHero board and the regular Hero board is the board
                //prefix passed to the VIZ engine, MapHero[0-50] instead of QuadHero[0-8].
                dataPoolData = "MapHero[0-50]={" + "\n";
            }
            dataPoolData += "{MAP=" + mapName + ";}," + "\n";
            dataPoolData += "{RidingName=" + r.DisplayName2 + ";}," + "\n";
            dataPoolData += "{NumberOfPolls=" + r.ReportingPolls + " of " + r.TotalPolls + ";}," + "\n";
            for (var i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                var c = r.Candidates[i];
                var headShot = r.RidingNumber.ToString("D3") + "_" + c.Party.PartyCode + "_" + c.LastName.ToUpper();
                //Special processing for the first candidate.
                if (i == r.FirstItem)
                {
                    if (((r.ReportStatus & 8) != 0) & ((c.Status & 8) != 0))
                    {
                        //If there is an elect in this riding then we want to display the winning candidate's
                        //hero note on the first display of this riding only
                        if (!r.HeroNoteShown)
                        {
                            heroNote = c.HeroNote.Replace("\r\n"," ");
                            //Turn off the HeroNote flag in this riding
                            r.SetHeroNoteShownFlag(true);
                        }
                    }
                    var candidateStatus = c.CandidateStatus;
                    if (candidateStatus.ToUpper() == "LEAD")
                    {
                        candidateStatus = "";
                    }
                    dataPoolData += "{Name=" + c.DisplayName2 + ";Votes=" + c.Votes + ";Party=" + c.Party.PartyCode + ";LeadingValue=" + c.Lead + ";Gain=" + candidateStatus + ";HeadShot=" + headShot + ";Note=" + heroNote + ";}," + "\n";
                }
                else
                {
                    dataPoolData += "{Name=" + c.DisplayName1 + ";Votes=" + c.Votes + ";Party=" + c.Party.PartyCode + ";HeadShot=" + headShot + ";}," + "\n";
                }
            }
            if (r.NumberOfItemsToDisplay < 4)
            {
                for (var i = r.NumberOfItemsToDisplay + 1; i <= 4; i++)
                {
                    dataPoolData += "{Name=HideObject;Votes=0;Party=NONE;HeadShot=0;}," + "\n";
                }
            }
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.Scene.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate a Party Standings board using datapool
        /// </summary>
        /// <param name="r"></param>
        public void VizPartyStandings(Region r)
        {
            //Bail out if we have no data
            if (r.TotalItems == 0)
                return;

            _viz.PrepareForNewScene(r);

            //Build the datapool string
            var dataPoolData = "PartyStandings[0-10]={" + "\n";

            var itemsDisplayed = 0;
            for (var i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                //Get the party object
                var p = r.Parties[i];
                if (p.PartyCode.ToUpper().StartsWith("*TP")) continue;
                if ((p.CurrentElectedSeats + p.CurrentLeadingSeats) <= 0) continue;
                dataPoolData += "{ElectedValue=" + p.CurrentElectedSeats + ";LeadingValue=" + p.CurrentLeadingSeats + ";Party=" + p.PartyCode + ";}," + "\n";
                itemsDisplayed += 1;
            }
            if (itemsDisplayed < r.MaxItemsPerPage)
            {
                for (var i = itemsDisplayed + 1; i <= r.MaxItemsPerPage; i++)
                {
                    //Send 0's to objects that are not needed.  They will be hidden by logic embedded in VIZ board.
                    dataPoolData += "{ElectedValue=0;LeadingValue=0;Party=NONE;}," + "\n";
                }
            }

            //Figure out what the board title, map and region name should be
            var mapCode = "";
            string regionName;
            var boardTitle = "";
            if (r.MapCode.Length > 0)
            {
                mapCode = r.MapCode;
                regionName = r.DisplayName1;
                //There is a map so we don't have to display the region name in the title
                boardTitle = "Party Standings";
            }
            else
            {
                //Assign the Main Region's mapcode to this board because there is not a more specific
                //map associated with this region.  We also have to display the region in the board title
                //so the users can determine which region the data refers to
                //mapCode = _dataManager.GetRegionById(_dataManager.MainRegionID).MapCode;
                regionName = "";
                //We don't have a map so put the region name in the title
                boardTitle += "Party Standings: " + r.DisplayName1;
            }
            dataPoolData += "{MAP=" + mapCode + ";}," + "\n";
            dataPoolData += "{RegionName=" + regionName + ";}," + "\n";
            dataPoolData += "{Title=" + boardTitle + ";}," + "\n";

            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate a Party Status board using datapool
        /// </summary>
        /// <param name="r"></param>
        public void VizPartyStatus(Region r)
        {
            //Find the Party object we want to use.  If we can't find the party or if there are no votes
            //for the desired party then don't display the board
            Party p = r.GetPartyByID(r.DesiredParty.PartyID);
            if (p == null || p.CurrentVotes == 0)
                return;

            _viz.PrepareForNewScene(r);

            //Calculate values needed by board
            var currentSeats = p.CurrentElectedSeats + p.CurrentLeadingSeats;
            var seatChange = (p.CurrentElectedSeats + p.CurrentLeadingSeats) - p.PrevReportingSeats;
            var currentVoteShare = ((float)p.CurrentVotes / r.CurrentTotalVotes) * 100;
            var previousVoteShare = ((float)p.PreviousVotes / r.PreviousTotalVotes) * 100;
            var partyChange = currentVoteShare - previousVoteShare;

            //Build the datapool string
            var dataPoolData = "PARTYSTATUS[0-8]={" + "\n";
            dataPoolData += "{MAP=" + r.MapCode + ";}," + "\n";
            if (r.MainRegion)
            {
                dataPoolData += "{Title=Party Status;}," + "\n";
            }
            else
            {
                dataPoolData += "{Title=Party Status:  " + r.DisplayName1 + ";}," + "\n";
            }
            dataPoolData += "{MapTitle=" + p.DisplayName1 + " Party Vote;}," + "\n";
            dataPoolData += "{Party=" + p.PartyCode + ";SeatsValue=" + currentSeats + ";ChangeValue=" + FormatSign(seatChange) + Math.Abs(seatChange) + ";VoteShareValue=" + currentVoteShare.ToString("0.#") + "%;VSChangeValue=" + partyChange.ToString("0.#") + ";}," + "\n";
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);

        }

        /// <summary>
        /// Generate a Possible Majority board using datapool
        /// </summary>
        /// <param name="r"></param>
        public void VizPossibleMajority(Region r)
        {
            var p = r.GetPartyByID(r.DesiredParty.PartyID);
            if (p == null)
                return;
            if (p.CurrentLeadingSeats == 0 & p.CurrentElectedSeats == 0)
                return;

            //Calculate the values needed by this board...
            var majorityValue = ((r.TotalRidings / 2) + 1);
            var leadingElectedValue = p.CurrentLeadingSeats + p.CurrentElectedSeats;
            var holdValue = p.PrevNonReportingSeats;

            var finalValue = majorityValue - (leadingElectedValue + holdValue);
            var gapValue = 0;
            var overValue = 0;
            if (finalValue < 0)
            {
                //This party is in majority territory
                overValue = Math.Abs(finalValue);
            }
            else
            {
                //The party is in minority territory
                gapValue = finalValue;
            }

            //Deal with any previous scene loaded in this layer (bring it off air) and load this scene
            _viz.PrepareForNewScene(r);
            //Build the datapool string
            var mapCode = r.MapCode.Length > 0 ? r.MapCode : "CA";
            var dataPoolData = "POSSIBLEMAJORITY[0-50]={" + "\n";
            dataPoolData += "{MAP=" + mapCode + ";Building=" + mapCode + ";}," + "\n";
            dataPoolData += "{Title=Possible Majority?;}," + "\n";
            dataPoolData += "{Party=" + p.PartyCode + ";}," + "\n";
            dataPoolData += "{MajorityValue=" + majorityValue + ";}," + "\n";
            dataPoolData += "{LeadingElected=" + leadingElectedValue + ";Hold=" + holdValue + ";Gap=" + gapValue + ";Over=" + overValue + ";}," + "\n";
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate a Projection board using datapool
        /// </summary>
        /// <param name="n"></param>
        private void VizProjection(NoQuery n)
        {
            //Deal with any previous scene loaded in this layer (bring it off air) and load this scene
            _viz.PrepareForNewScene(n);
            var dataPoolData = "PROJECTION[0-50]={{" + "\n";
            dataPoolData += "Title=CBC News Projects;" + "\n";
            var headShot = n.Party.PartyCode + "_Leader";
            dataPoolData += "Party=" + n.Party.PartyCode + ";" + "\n";
            dataPoolData += "Projection=" + n.BoardText + ";" + "\n";
            dataPoolData += "HeadShot=" + headShot + ";" + "\n";
            if (n.Parties.Count > 0)
            {
                var pNumber = 1;
                foreach (var p in n.Parties)
                {
                    dataPoolData += "P" + pNumber + "Party=" + p.PartyCode + ";" + "\n";
                    if (pNumber > 5)
                    {
                        break; 
                    }
                    pNumber += 1;
                }
                if (n.Parties.Count < 5)
                {
                    for (pNumber = n.Parties.Count + 1; pNumber <= 5; pNumber++)
                    {
                        dataPoolData += "P" + pNumber + "Party=OTH;" + "\n";
                    }
                }
            }
            else
            {
                dataPoolData += "P1Party=CON;" + "\n";
                dataPoolData += "P2Party=NDP;" + "\n";
                dataPoolData += "P3Party=LIB;" + "\n";
                dataPoolData += "P4Party=BQ;" + "\n";
                dataPoolData += "P5Party=GRN;" + "\n";
            }
            dataPoolData += "}};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(n.DatapoolName, dataPoolData);
        }

        /// <summary>
        /// Generate a Seat Change board using datapool
        /// </summary>
        /// <param name="r"></param>
        public void VizSeatChange(Region r)
        {
            //Bail out if we have no data
            if (r.TotalItems == 0)
                return;

            _viz.PrepareForNewScene(r);

            //Build the datapool string
            var dataPoolData = "SeatChange[0-50]={" + "\n";

            var itemsDisplayed = 0;
            for (var i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                //Get the party object
                var p = r.Parties[i];
                if (p.PartyCode.ToUpper().StartsWith("*TP")) continue;
                var curSeats = p.CurrentElectedSeats + p.CurrentLeadingSeats;
                var prevSeats = p.PrevReportingSeats;
                var seatChangeValue = curSeats - prevSeats;
                if (seatChangeValue == 0) continue;
                dataPoolData += "{ElectedValue=" + curSeats + ";SeatChangeValue=" + FormatSign(seatChangeValue) + Math.Abs(seatChangeValue) + ";Party=" + p.PartyCode + ";}," + "\n";
                itemsDisplayed += 1;
            }
            if (itemsDisplayed < r.MaxItemsPerPage)
            {
                for (var i = itemsDisplayed + 1; i <= r.MaxItemsPerPage; i++)
                {
                    //Send 0's to objects that are not needed.  They will be hidden by logic embedded in VIZ board.
                    dataPoolData += "{ElectedValue=0;SeatChangeValue=0;Party=HideObject;}," + "\n";
                }
            }

            //Figure out what the board title, map and region name should be
            //string mapCode = _dataManager.GetRegionById(_dataManager.MainRegionID).MapCode;
            string regionName;
            string boardTitle;
            if (r.MainRegion)
            {
                regionName = "";
                boardTitle = "Seat Change";
            }
            else
            {
                //This region may have a defined map that is more specific than the regional map so set it...
                if (r.MapCode.Length > 0)
                {
                   // mapCode = r.MapCode;
                }
                regionName = r.DisplayName1;
                boardTitle = "Seat Change: " + r.DisplayName1;
            }
            //dataPoolData += "{MAP=" + mapCode + ";}," + "\n";
            dataPoolData += "{RegionName=" + regionName + ";}," + "\n";
            dataPoolData += "{Title=" + boardTitle + ";}," + "\n";
            dataPoolData += "};";

            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate a Summary board using datapool
        /// </summary>
        /// <param name="s"></param>
        private void VizSummary(Summary s)
        {
            int i;

            _viz.PrepareForNewScene(s);

            //The first region in the SummaryRegions is the reference region.  Get a pointer to it...
            var referenceRegion = s.SummaryRegions[0];

            //Build the board title
            var boardTitle = "";
            if (referenceRegion.MapCode.Length > 0)
            {
                //There is a map so we don't have to display the region name in the title
                boardTitle = "Summary";
            }
            else
            {
                //We don't have a map so put the region name in the title
                boardTitle += "Summary: " + s.DisplayName1;
            }

            //Build the datapool string
            var dataPoolData = "SUMMARY[0-25]={" + "\n";
            dataPoolData += "{MAP=" + referenceRegion.MapCode + ";}," + "\n";
            dataPoolData += "{Title=" + boardTitle + ";}," + "\n";
            dataPoolData += "{RegionName=" + referenceRegion.DisplayName1 + ";}," + "\n";
            dataPoolData += "{Party=" + s.Party.PartyCode + ";BreakdownValue=" + referenceRegion.CurrentVoteShare.ToString("0.#") + ";}," + "\n";
            for (i = s.FirstItem; i <= s.FirstItem + s.NumberOfItemsToDisplay - 1; i++)
            {
                //Get the Region object
                var r = s.SummaryRegions[i];
                if (r == null) continue;
                //Figure out the difference in vote share
                var changeValue = ((Convert.ToSingle(r.CurrentVotes) / r.CurrentTotalVotes) * 100) - ((Convert.ToSingle(r.PreviousVotes) / r.PreviousTotalVotes) * 100);
                var changeVal = changeValue.ToString("0.#");
                changeVal = Convert.ToString((changeValue >= 0 ? "+" + changeVal : "" + changeVal));
                dataPoolData += "{RTitle=" + r.DisplayName1 + ";RValue=" + r.CurrentVoteShare.ToString("0.#") + ";RChange=" + changeVal + ";}," + "\n";
            }
            if (s.NumberOfItemsToDisplay < s.Scene.NumberOfItems)
            {
                //Send 0's to region objects that are not needed.  They will be hidden by logic embedded in VIZ board.
                for (i = s.NumberOfItemsToDisplay; i <= s.Scene.NumberOfItems; i++)
                {
                    dataPoolData += "{RTitle=HideObject;RValue=0;RChange=0;}," + "\n";
                }
            }
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(s.Scene.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate a Too Close to Call board using datapool
        /// </summary>
        /// <param name="r"></param>
        private void VizTooCloseToCall(RegionRiding r)
        {
            int i;

            _viz.PrepareForNewScene(r);

            //Build the board title
            string boardTitle;
            if (r.MainRegion)
            {
                boardTitle = "Too Close to Call";
            }
            else
            {
                //Put the region name in the title if this is not the main region
                boardTitle = "Too Close to Call: " + r.DisplayName1;
            }

            var dataPoolData = "TOOCLOSE[0-300]={" + "\n";
            dataPoolData += "{Title=" + boardTitle + ";}," + "\n";

            //Data for each riding
            var itemsDisplayed = 0;
            for (i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                //Get the Riding object and the first 2 candidate objects
                var riding = r.Ridings[i];
                var c1 = riding.Candidates[0];
                var c2 = riding.Candidates[1];
                //Calculate the total difference between 1st and 2nd place, and the percentage of total votes that 
                //number represents
                var marginValue = c1.Votes - c2.Votes;
                var percentageValue = ((float)marginValue / riding.VotesCounted()) * 100;
                dataPoolData += "{RidingName=" + riding.DisplayName1 + ";P1Name=" + c1.Party.PartyCode + ";P2Name=" + c2.Party.PartyCode + ";MarginValue=" + marginValue.ToString() + ";PercentageValue=" + percentageValue.ToString("0.#") + ";}," + "\n";
                itemsDisplayed += 1;
            }
            if (itemsDisplayed < r.MaxItemsPerPage)
            {
                for (i = itemsDisplayed + 1; i <= r.MaxItemsPerPage; i++)
                {
                    //Send 0's to objects that are not needed.  They will be hidden by logic embedded in VIZ board.
                    dataPoolData += "{RidingName=HideObject;P1Name=NONE;P2Name=NONE;MarginValue=0;PercentageValue=0;}," + "\n";
                }
            }
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate a Total Votes board using datapool
        /// </summary>
        /// <param name="r"></param>
        public void VizTotalVotes(Region r)
        {
            //Bail out if we have no data
            if (r.TotalItems == 0)
                return;

            _viz.PrepareForNewScene(r);

            //Build the datapool string
            var dataPoolData = "TOTALVOTES[0-100]={" + "\n";

            //Figure out what the board title, map and region name should be
            var mapCode = "";
            var boardTitle = "";
            if (r.MapCode.Length > 0)
            {
                mapCode = r.MapCode;
                //There is a map so we don't have to display the region name in the title
                boardTitle = "Total Votes";
            }
            else
            {
                //Assign the Main Region's mapcode to this board because there is not a more specific
                //map associated with this region.  We also have to display the region in the board title
                //so the users can determine which region the data refers to
                //mapCode = _dataManager.GetRegionById(_dataManager.MainRegionID).MapCode;
                //We don't have a map so put the region name in the title
                boardTitle += "Total Votes: " + r.DisplayName1;
            }
            //dataPoolData &= "{RegionName=" & regionName & ";}," & vbLf
            dataPoolData += "{Title=" + boardTitle + ";}," + "\n";
            dataPoolData += "{MAP=" + mapCode + ";}," + "\n";

            var totalVotes = 0;
            var itemsDisplayed = 0;
            for (var i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                //Get the party object
                IParty p = r.Parties[i];
                if (p.PartyCode.ToUpper().StartsWith("*TP")) continue;
                //Calculate the vote share for this party
                dataPoolData += "{Party=" + p.PartyCode + ";Votes=" + p.CurrentVotes + ";}," + "\n";
                totalVotes += p.CurrentVotes;
                itemsDisplayed += 1;
            }
            if (itemsDisplayed < r.MaxItemsPerPage)
            {
                for (var i = itemsDisplayed + 1; i <= r.MaxItemsPerPage; i++)
                {
                    //Send 0's to objects that are not needed.  They will be hidden by logic embedded in VIZ board.
                    dataPoolData += "{Party=HideObject;Votes=0;}," + "\n";
                }
            }
            dataPoolData += "{TotalVotes=" + totalVotes + ";}," + "\n";
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        public void VizubGenericBoard(Scene scene)
        {
            //_viz.PrepareForNewUBScene((IBaseBoard)scene);

        }

        private void VizUbRidingReport(Riding r)
        {
            //_viz.PrepareForNewUBScene(r);

            var dataPoolData = "UBRIDING[0-50]={" + "\n";
            dataPoolData += "{RidingName=" + r.DisplayName1 + ";}," + "\n";
            for (var i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                var c = r.Candidates[i];
                //Special processing for the first candidate.
                if (i == r.FirstItem)
                {
                    var headShot = r.RidingNumber.ToString("D3") + "_" + c.Party.PartyCode + "_" + c.LastName.ToUpper();
                    dataPoolData += "{HeadShot=" + headShot + ";Name=" + c.DisplayName2 + ";Party=" + c.Party.PartyCode + ";Votes=" + c.Votes + ";Gain=" + c.CandidateStatus + ";}," + "\n";
                }
                else
                {
                    dataPoolData += "{Name=" + c.DisplayName2 + ";Party=" + c.Party.PartyCode + ";Votes=" + c.Votes + ";}," + "\n";
                }
            }
            if (r.NumberOfItemsToDisplay < r.Scene.NumberOfItems)
            {
                for (var i = r.NumberOfItemsToDisplay + 1; i <= r.Scene.NumberOfItems; i++)
                {
                    dataPoolData += "{Name=HideObject;Party=HideObject;Votes=0;}," + "\n";
                }
            }
            dataPoolData += "{NumberOfPolls=" + r.ReportingPolls + "/" + r.TotalPolls + ";}," + "\n";
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.Scene.DatapoolName, dataPoolData);

        }

        /// <summary>
        /// Generate a Vote Change board using datapool
        /// </summary>
        /// <param name="r"></param>
        public void VizVoteChange(Region r)
        {
            //Bail out if we have no data
            if (r.TotalItems == 0)
                return;

            _viz.PrepareForNewScene(r);

            //Build the datapool string
            var itemsDisplayed = 0;
            var dataPoolData = "VoteChange[0-50]={" + "\n";
            for (var i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                //Get the party object
                var p = r.Parties[i];
                if (p.PartyCode.ToUpper().StartsWith("*TP")) continue;
                //Figure out the difference in vote share
                var currentVoteShare = ((float)p.CurrentVotes / r.CurrentTotalVotes) * 100;
                var previousVoteShare = ((float)p.PreviousVotes / r.PreviousTotalVotes) * 100;
                var partyChange = currentVoteShare - previousVoteShare;
                dataPoolData += "{ElectedValue=" + Convert.ToSingle(p.CurrentVotes) + ";SeatChangeValue=" + partyChange.ToString("0.#") + ";Party=" + p.PartyCode + ";VoteShareValue=" + currentVoteShare.ToString("0.#") + ";}," + "\n";
                itemsDisplayed += 1;
            }
            if (itemsDisplayed < r.MaxItemsPerPage)
            {
                for (var i = itemsDisplayed + 1; i <= r.MaxItemsPerPage; i++)
                {
                    //Send 0's to objects that are not needed.  They will be hidden by logic embedded in VIZ board.
                    dataPoolData += "{ElectedValue=0;SeatChangeValue=0;Party=HideObject;VoteShareValue=0;}," + "\n";
                }
            }

            //Figure out what the board title, map and region name should be
            //string mapCode = _dataManager.GetRegionById(_dataManager.MainRegionID).MapCode;
            string regionName;
            string boardTitle;
            if (r.MainRegion)
            {
                regionName = "";
                boardTitle = "Vote Change";
            }
            else
            {
                //This region may have a defined map that is more specific than the regional map so set it...
                if (r.MapCode.Length > 0)
                {
                    //mapCode = r.MapCode;
                }
                regionName = r.DisplayName1;
                boardTitle = "Vote Change: " + r.DisplayName1;
            }
            //dataPoolData += "{MAP=" + mapCode + ";}," + "\n";
            dataPoolData += "{RegionName=" + regionName + ";}," + "\n";
            dataPoolData += "{Title=" + boardTitle + ";}," + "\n";
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate a Vote Share board using datapool
        /// </summary>
        /// <param name="r"></param>
        private void VizVoteShare(Region r)
        {
            //Bail out if we have no data
            if (r.TotalItems == 0)
                return;

            _viz.PrepareForNewScene(r);

            //Build the datapool string
            var dataPoolData = "SHAREOFVOTE={" + "\n";

            //Figure out what the board title, map and region name should be
            string mapCode;
            var boardTitle = "";
            if (r.MainRegion)
            {
                mapCode = r.MapCode;
                boardTitle = "Share of Vote";
            }
            else
            {
                mapCode = r.MapCode;
                boardTitle += "Share of Vote: " + r.DisplayName1;
            }
            dataPoolData += "MAP=" + mapCode + ";" + "\n";
            dataPoolData += "Title=" + boardTitle + ";" + "\n";

            var itemsDisplayed = 0;
            for (var i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                //Get the party object
                IParty p = r.Parties[i];
                if (p.PartyCode.ToUpper().StartsWith("*TP")) continue;
                //Build the appropriate field names for datapool
                var partyCode = "P" + (itemsDisplayed + 1) + "Name";
                var partyPercent = "P" + (itemsDisplayed + 1) + "Party";
                //Calculate the vote share for this party
                var partyVoteSharePct = ((float)p.CurrentVotes / r.CurrentTotalVotes) * 100;
                //partyVoteSharePct = GetRandom(2, 45)
                dataPoolData += partyCode + "=" + p.PartyCode + ";" + "\n";
                dataPoolData += partyPercent + "=" + partyVoteSharePct.ToString("0.#") + ";" + "\n";
                itemsDisplayed += 1;
            }
            if (itemsDisplayed < r.MaxItemsPerPage)
            {
                for (var i = itemsDisplayed + 1; i <= r.MaxItemsPerPage; i++)
                {
                    //Send 0's to objects that are not needed.  They will be hidden by logic embedded in VIZ board.
                    //Build the appropriate field names for datapool
                    var partyCode = "P" + (i) + "Name";
                    var partyPercent = "P" + (i) + "Party";
                    dataPoolData += partyCode + "=NONE;" + "\n";
                    dataPoolData += partyPercent + "=0;" + "\n";
                }
            }

            dataPoolData += "TotalPercent=60;" + "\n";
            dataPoolData += "TotalAngle=180;" + "\n";
            dataPoolData += "MainRegion=" + (r.MainRegion ? "True" : "False") + ";" + "\n";
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData, _viz.DoingRefresh);
        }

        /// <summary>
        /// Generate a Woman's Vote board using datapool
        /// </summary>
        /// <param name="r"></param>
        public void VizWomansVote(Region r)
        {
            //Bail out if we have no data
            if (r.TotalItems == 0)
                return;

            _viz.PrepareForNewScene(r);

            var totalElected = 0;
            var totalLeading = 0;
            var totalChange = 0;
            //Build the datapool string
            var dataPoolData = "WOMEN[0-150]={" + "\n";

            //Figure out what the board title should be
            var boardTitle = "";
            if (r.MainRegion)
            {
                boardTitle = "Women Candidates";
            }
            else
            {
                boardTitle += "Women Candidates: " + r.DisplayName1;
            }
            dataPoolData += "{Title=" + boardTitle + ";}," + "\n";
            dataPoolData += "{MAP=" + r.MapCode + ";}," + "\n";

            var itemsDisplayed = 0;
            for (var i = r.FirstItem; i <= r.FirstItem + r.NumberOfItemsToDisplay - 1; i++)
            {
                //Get the party object
                var p = r.Parties[i];
                if (p.PartyCode.ToUpper().StartsWith("*TP")) continue;
                var changeValue = p.CurrentElectedSeats + p.CurrentLeadingSeats - p.PrevReportingSeats;
                dataPoolData += "{Party=" + p.PartyCode + ";ElectedValue=" + p.CurrentElectedSeats + ";LeadingValue=" + p.CurrentLeadingSeats + ";ChangeValue=" + FormatSign(changeValue) + Math.Abs(changeValue) + ";}," + "\n";
                //Accumulate totals
                totalElected += p.CurrentElectedSeats;
                totalLeading += p.CurrentLeadingSeats;
                totalChange += changeValue;
                itemsDisplayed += 1;
            }
            if (itemsDisplayed < r.MaxItemsPerPage)
            {
                for (var i = itemsDisplayed + 1; i <= r.MaxItemsPerPage; i++)
                {
                    //Send 0's to objects that are not needed.  They will be hidden by logic embedded in VIZ board.
                    dataPoolData += "{Party=HideObject;ElectedValue=0;LeadingValue=0;ChangeValue=0;}\n";
                }
            }
            //bhata-Elected Total Value and Leading Total Value;
            dataPoolData += "{ElectedTotalValue=" + totalElected + ";LeadingTotalValue=" + totalLeading + ";ChangeValue=" + FormatSign(totalChange) + Math.Abs(totalChange) + ";}," + "\n";
            dataPoolData += "{TotalParties=" + itemsDisplayed + ";}," + "\n";
            dataPoolData += "};";

            //Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene(r.DatapoolName, dataPoolData);
        }

        public void UsCall(string candidate, string partyCode)
        {
            var dataPoolData = "PROJECTION_DATA={\n";
            dataPoolData += "Title=CBC News Projects;\n";
            dataPoolData += "Party=" + partyCode + ";\n";
            dataPoolData += "Projection=PRESIDENT-ELECT;\n";
            dataPoolData += "};";
            _viz.PrepareForNewScene(500, "FF_HERO_PROJECTION");
            _viz.DisplayScene("FF_HERO_PROJECTION", dataPoolData);
        }

        public void UsCallFr(string candidate, string partyCode)
        {
            var dataPoolDataFr = "CALL={\n";
            dataPoolDataFr += "Party=" + PartyCodeFr(partyCode) + ";\n";
            dataPoolDataFr += "};";
            _viz.PrepareForNewScene(900, "FF_CALL");
            _viz.DisplaySceneFf("FF_CALL", dataPoolDataFr);
        }

        public void UsHouse(string demValue, string gopValue, string othValue)
        {
            var dataPoolData = "HOUSE_DATA={\n";
            dataPoolData += "P1Party=DEM;" + "\n";
            dataPoolData += "P1Value=" + demValue + ";\n";
            dataPoolData += "P2Party=GOP;\n";
            dataPoolData += "P2Value=" + gopValue + ";\n";
            dataPoolData += "P3Party=OTH;\n";
            dataPoolData += "P3Value=" + othValue + ";\n";
            dataPoolData += "};";
            _viz.PrepareForNewScene(500, "FF_HOUSE");
            // Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene("FF_HOUSE", dataPoolData);
        }

        public void UsHouseFr(string demValue, string gopValue, string othValue)
        {
            var dataPoolDataFr = "HOUSE={\n";
            dataPoolDataFr += "P1=" + (short.Parse(demValue) >= short.Parse(gopValue) ? "D" : "R") + ";\n";
            dataPoolDataFr += "S1Number=" + (short.Parse(demValue) >= short.Parse(gopValue) ? demValue : gopValue) + ";\n";
            dataPoolDataFr += "P2=" + (short.Parse(demValue) >= short.Parse(gopValue) ? "R" : "D") + ";\n";
            dataPoolDataFr += "S2Number=" + (short.Parse(demValue) >= short.Parse(gopValue) ? gopValue : demValue) + ";\n";
            dataPoolDataFr += "};";
            _viz.PrepareForNewScene(900, "FF_HOUSE");
            // Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene("FF_HOUSE", dataPoolDataFr);
        }

        public void UsSenate(string demValue, string gopValue, string othValue)
        {
            var dataPoolData = "SENATE_DATA={\n";
            dataPoolData += "P1Party=DEM;\n";
            dataPoolData += "P1Value=" + demValue + ";\n";
            dataPoolData += "P2Party=GOP;\n";
            dataPoolData += "P2Value=" + gopValue + ";\n";
            dataPoolData += "P3Party=OTH;\n";
            dataPoolData += "P3Value=" + othValue + ";\n";
            dataPoolData += "};";
            _viz.PrepareForNewScene(500, "FF_SENATE");
            // Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene("FF_SENATE", dataPoolData);
        }

        public void UsSenateFr(string demValue, string gopValue, string othValue)
        {
            var dataPoolDataFr = "SENATE={\n";
            dataPoolDataFr += "P1=" + (short.Parse(demValue) >= short.Parse(gopValue) ? "D" : "R") + ";\n";
            dataPoolDataFr += "S1Number=" + (short.Parse(demValue) >= short.Parse(gopValue) ? demValue : gopValue) + ";\n";
            dataPoolDataFr += "P2=" + (short.Parse(demValue) >= short.Parse(gopValue) ? "R" : "D") + ";\n";
            dataPoolDataFr += "S2Number=" + (short.Parse(demValue) >= short.Parse(gopValue) ? gopValue : demValue) + ";\n";
            dataPoolDataFr += "};";
            _viz.PrepareForNewScene(900, "FF_SENATE");
            // Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene("FF_SENATE", dataPoolDataFr);
        }

        public void UsStandingFr(string demEcValue, string demPercValue, string gopEcValue, string gopPercValue)
        {
            var dataPoolDataFr = "STANDING={\n";
            dataPoolDataFr += "P1=" + (short.Parse(demEcValue) >= short.Parse(gopEcValue) ? "D" : "R") + ";\n";
            dataPoolDataFr += "P1Ec=" + (short.Parse(demEcValue) >= short.Parse(gopEcValue) ? demEcValue : gopEcValue) + ";\n";
            dataPoolDataFr += "P1Perc=" + (short.Parse(demEcValue) >= short.Parse(gopEcValue) ? demPercValue : gopPercValue) + ";\n";
            dataPoolDataFr += "P2=" + (short.Parse(demEcValue) >= short.Parse(gopEcValue) ? "R" : "D") + ";\n";
            dataPoolDataFr += "P2Ec=" + (short.Parse(demEcValue) >= short.Parse(gopEcValue) ? gopEcValue : demEcValue ) + ";\n";
            dataPoolDataFr += "P2Perc=" + (short.Parse(demEcValue) >= short.Parse(gopEcValue) ? gopPercValue : demPercValue) + ";\n";
            dataPoolDataFr += "};";
            _viz.PrepareForNewScene(900, "FF_STANDING");
            // Send data to VIZ datapool and bring scene on-air
            _viz.DisplaySceneFf("FF_STANDING", dataPoolDataFr);
        }

        public void UsMap(string dataPoolString, bool doingRefresh)
        {
            var dataPoolData = "FF_MAP_RESULTS_DATA={\n";
            dataPoolData += "Title=2016 RESULTS;\n";
            dataPoolData += dataPoolString;
            dataPoolData += "};";
            if (doingRefresh)
            {
                _viz.DisplayScene("", dataPoolData);
            }
            else
            {
                _viz.PrepareForNewScene(500, "FF_MAP_RESULTS");
                _viz.DisplayScene("FF_MAP_RESULTS", dataPoolData);
            }
        }

        public void UsMapFr(string dataPoolString, bool doingRefresh)
        {

            var dataPoolDataFr = "MAP={\n";
            dataPoolDataFr += dataPoolString;
            dataPoolDataFr += "};";
            _viz.DisplayScene(doingRefresh ? "" : "FF_MAP", dataPoolDataFr);
            _viz.SendVizCommand("Year=2016;");
        }

        public void UsMapOff()
        {
            _viz.SendVizCommand("FF_MAP=OFF;");
        }

        public void UsMapPrevious()
        {
            var dataPoolData = "FF_MAP_RESULTS_2012_DATA={\n";
            dataPoolData += "Title=2012 RESULTS;";
            dataPoolData += "};";
            _viz.PrepareForNewScene(500, "FF_MAP_2012_RESULTS");
            _viz.DisplayScene("FF_MAP_2012_RESULTS", dataPoolData);
        }

        public void UsMapPreviousFr()
        {
            var dataPoolDataFr = "MAP={\n";
            dataPoolDataFr += "AL=R;";
            dataPoolDataFr += "AK=R;";
            dataPoolDataFr += "AZ=R;";
            dataPoolDataFr += "AR=R;";
            dataPoolDataFr += "CA=D;";
            dataPoolDataFr += "NC=R;";
            dataPoolDataFr += "SC=R;";
            dataPoolDataFr += "CO=D;";
            dataPoolDataFr += "CT=D;";
            dataPoolDataFr += "ND=R;";
            dataPoolDataFr += "SD=R;";
            dataPoolDataFr += "DE=D;";
            dataPoolDataFr += "DC=D;";
            dataPoolDataFr += "FL=D;";
            dataPoolDataFr += "GA=R;";
            dataPoolDataFr += "HI=D;";
            dataPoolDataFr += "ID=R;";
            dataPoolDataFr += "IL=D;";
            dataPoolDataFr += "IN=R;";
            dataPoolDataFr += "IA=D;";
            dataPoolDataFr += "KS=R;";
            dataPoolDataFr += "KY=R;";
            dataPoolDataFr += "LA=R;";
            dataPoolDataFr += "ME=D;";
            dataPoolDataFr += "MD=D;";
            dataPoolDataFr += "MA=D;";
            dataPoolDataFr += "MI=D;";
            dataPoolDataFr += "MN=D;";
            dataPoolDataFr += "MS=R;";
            dataPoolDataFr += "MO=R;";
            dataPoolDataFr += "MT=R;";
            dataPoolDataFr += "NE=R;";
            dataPoolDataFr += "NV=D;";
            dataPoolDataFr += "NH=D;";
            dataPoolDataFr += "NJ=D;";
            dataPoolDataFr += "NY=D;";
            dataPoolDataFr += "NM=D;";
            dataPoolDataFr += "OH=D;";
            dataPoolDataFr += "OK=R;";
            dataPoolDataFr += "OR=D;";
            dataPoolDataFr += "PA=D;";
            dataPoolDataFr += "RI=D;";
            dataPoolDataFr += "TN=R;";
            dataPoolDataFr += "TX=R;";
            dataPoolDataFr += "UT=R;";
            dataPoolDataFr += "VT=D;";
            dataPoolDataFr += "VA=D;";
            dataPoolDataFr += "WV=R;";
            dataPoolDataFr += "WA=D;";
            dataPoolDataFr += "WI=D;";
            dataPoolDataFr += "WY=R;";
            dataPoolDataFr += "};";
            _viz.DisplayScene("", dataPoolDataFr);
            _viz.SendVizCommand("Year=2012;");
        }

        public void UsMini(string miniType, string demVotes, string gopVotes, string othVotes, string message)
        {
            _miniType = miniType.ToLower() == "ecvotes" ? "MINI" : "MINI_SHARE";
            if (_miniType != _miniTypeCurrent)
            {
                _viz.SendVizCommand(_miniTypeCurrent + "=OFF;");
                _previousMiniData = "";
                _miniTypeCurrent = _miniType;
            }
            var parties = new MiniBar[3];
            parties[0].PartyCode = "DEM";
            parties[0].CurrentValue = Convert.ToDouble(demVotes);
            parties[0].BarName = "CLINTON";
            parties[1].PartyCode = "GOP";
            parties[1].CurrentValue = Convert.ToDouble(gopVotes);
            parties[1].BarName = "TRUMP";
            parties[2].PartyCode = "OTH";
            parties[2].CurrentValue = Convert.ToDouble(othVotes);
            parties[2].BarName = "OTH";

            var sortedParties = parties.OrderByDescending(si => si.CurrentValue).ToList();

            var dataPoolData = "";
            switch (miniType.ToLower())
            {
                case "ecvotes":
                    dataPoolData = "MINI_DATA={\n";
                    dataPoolData += "MiniNote=" + (message.Length > 0 ? message : "270 ELECTORAL COLLEGE VOTES NEEDED TO WIN") + ";\n";
                    break;
                case "voteshare":
                    dataPoolData = "MINI_SHARE_DATA={\n";
                    dataPoolData += "MiniNote=SHARE OF VOTE;\n";
                    break;
                default:
                    break;
            }
            var totalParties = 0;
            for (var i = 0; i < sortedParties.Count; i++)
            {
                if (!(sortedParties[i].CurrentValue > 0)) continue;
                totalParties += 1;
                dataPoolData += "P" + (i + 1) + "Party=" + sortedParties[i].PartyCode + ";\n";
                dataPoolData += "P" + (i + 1) + "Total=" + sortedParties[i].CurrentValue.ToString(CultureInfo.InvariantCulture) + ";\n";
                dataPoolData += "P" + (i + 1) + "Name=" + sortedParties[i].BarName + ";\n";
            }
            dataPoolData += "TotalParties=" + totalParties + ";\n";
            dataPoolData += "};";
            if (dataPoolData == _previousMiniData) return;
            _viz.DisplayScene(_miniType, dataPoolData);
            _previousMiniData = dataPoolData;
        }

        public void UsMiniFr(string miniType, string demVotes, string gopVotes, string othVotes)
        {
            var selectType = miniType.ToLower() == "ecvotes" ? "EC" : "VoteShare";
            _miniTypeCurrent = "MINIS";

            switch (_previousMiniData.ToUpper())
            {
                case "":
                    _viz.SendVizCommand(_miniTypeCurrent + "=ON;");
                    break;
                case "MINI_HERO":
                    _viz.SendVizCommand("MINI_HERO=OFF;");
                    _viz.SendVizCommand("PARTIES=ON;");
                    break;
                default:
                    break;
            }

            var parties = new MiniBar[2];
            parties[0].PartyCode = "D";
            parties[0].CurrentValue = Convert.ToDouble(demVotes);
            parties[1].PartyCode = "R";
            parties[1].CurrentValue = Convert.ToDouble(gopVotes);

            var sortedParties = parties.OrderByDescending(si => si.CurrentValue).ToList();

            var dataPoolDataFr = "MINI_BAR={\n";

            for (var i = 0; i < sortedParties.Count; i++)
            {
                dataPoolDataFr += "P" + (i + 1) + "=" + sortedParties[i].PartyCode + ";\n";
            }
            dataPoolDataFr += "};";

            _viz.DisplayScene(_miniTypeCurrent, dataPoolDataFr, true);

            dataPoolDataFr = "MINI_NUMBER={\n";

            for (var i = 0; i < sortedParties.Count; i++)
            {
                dataPoolDataFr += "NumberSelect" + (i + 1) + "=" + selectType + ";\n";
                dataPoolDataFr += "P" + (i + 1) + "Number=" + sortedParties[i].CurrentValue + ";\n";
            }
            dataPoolDataFr += "};";

            _viz.DisplayScene(_miniTypeCurrent, dataPoolDataFr, true);

            _previousMiniData = _miniTypeCurrent;
        }

        public void UsMiniOff()
        {
            _viz.SendVizCommand(_miniTypeCurrent + "=OFF;");
            _previousMiniData = "";
        }

        public void VizBrand(bool bringBoardOn)
        {
            var cmd = bringBoardOn ? "ON;" : "OFF;";
            _viz.SendVizCommand("BRAND=" + cmd);
        }

        public void VizLive(bool bringBoardOn)
        {
            var cmd = bringBoardOn ? "ON;" : "OFF;";
            _viz.SendVizCommand("LIVE=" + cmd);
        }

        public void VizGem(bool bringBoardOn)
        {
            var cmd = bringBoardOn ? "ON;" : "OFF;";
            _viz.SendVizCommand("GEM=" + cmd);
        }

        public void VizMiniResults(bool bringBoardOn)
        {
            var cmd = bringBoardOn ? "ON;" : "OFF;";
            _viz.SendVizCommand("MINI_RESULTS=" + cmd);
        }

        public void VizMiniProjection(bool bringBoardOn, string dem, string gop)
        {
            if (bringBoardOn)
            {
                var demValue = Convert.ToInt32(dem);
                var gopValue = Convert.ToInt32(gop);
                var partyCode = demValue > gopValue ? "DEM" : "GOP";

                var dataPoolData = "MINI_PROJECTION_DATA={\n";
                dataPoolData += "Party=" + partyCode + ";\n";
                dataPoolData += "};";

                _viz.DisplayScene("MINI_PROJECTION", dataPoolData);
            }
            else
            {
                _viz.SendVizCommand("MINI_PROJECTION=OFF;");
            }
        }

        public void VizZeroMini(bool bringBoardOn)
        {
            var cmd = bringBoardOn ? "ON;" : "OFF;";
            _viz.SendVizCommand("MINI_ZERO=" + cmd);
        }

        public void VizMiniBackground(bool bringBoardOn)
        {
            _viz.SendVizCommand("MINI_RESULTS_BG=ON;");
            Thread.Sleep(1000);
            _viz.SendVizCommand("MINI_RESULTS_BG=OFF;");
        }

        /// <summary>
        /// Generate the hero board for one state - full frame graphic
        /// </summary>
        /// <param name="heroState"></param>
        public void UsStateBoard(USHeroState heroState)
        {
            // State hero board - sort by electoral college votes or votePercentage based on elect status
            heroState.Parties = heroState.Elect ? heroState.Parties.OrderByDescending(p => p.ElectoralCollegeVotes).ThenByDescending(p => p.VotePercentage).ToList() : heroState.Parties.OrderByDescending(p => p.VotePercentage).ToList();

            var dataPoolData = "HERO_DATA={\n";
            dataPoolData += "P1Party=" + heroState.Parties[0].PartyCode + ";\n";
            dataPoolData += "P2Party=" + heroState.Parties[1].PartyCode + ";\n";
            dataPoolData += "MAP=" + heroState.StateCode + ";\n";
            dataPoolData += "TotalValue=" + heroState.ElectoralCollegeVotes + ";\n";
            dataPoolData += heroState.Elect ? "Gain=Elect;\n" : "Gain=;\n";
            dataPoolData += heroState.Elect ? "P1Value=HideObject;\n" : "P1Value=" + heroState.Parties[0].VotePercentage + ";\n";
            dataPoolData += heroState.Elect ? "P2Value=HideObject;\n" : "P2Value=" + heroState.Parties[1].VotePercentage + ";\n";
            dataPoolData += "};";
            _viz.PrepareForNewScene(1000, "FF_HERO");
            // Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene("FF_HERO", dataPoolData);
        }

        /// <summary>
        /// Generate the hero board for one state - full frame graphic
        /// </summary>
        /// <param name="heroState"></param>
        public void UsStateBoardFr(USHeroState heroState)
        {
            // State hero board - sort by electoral college votes or votePercentage based on elect status
            heroState.Parties = heroState.Elect ? heroState.Parties.OrderByDescending(p => p.ElectoralCollegeVotes).ThenByDescending(p => p.VotePercentage).ToList() : heroState.Parties.OrderByDescending(p => p.VotePercentage).ToList();

            var dataPoolDataFr = "HERO={\n";
            dataPoolDataFr += "State=" + heroState.StateCode + ";\n";
            dataPoolDataFr += heroState.Elect ? "Elected=YES;\n" : "Elected=NO;\n";
            dataPoolDataFr += "P1=" + PartyCodeFr(heroState.Parties[0].PartyCode) + ";\n";
            dataPoolDataFr += "P1Perc=" + heroState.Parties[0].VotePercentage + ";\n";
            dataPoolDataFr += "P2=" + PartyCodeFr(heroState.Parties[1].PartyCode) + ";\n";
            dataPoolDataFr += "P2Perc=" + heroState.Parties[1].VotePercentage + ";\n";
            dataPoolDataFr += "EcNumber=" + heroState.ElectoralCollegeVotes + ";\n";
            dataPoolDataFr += "};";
            _viz.PrepareForNewScene(900, "FF_HERO");
            // Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene("FF_HERO", dataPoolDataFr);
        }

        public void UsStateHeroFr(USHeroState heroState)
        {
            // State hero board - sort by electoral college votes or votePercentage based on elect status
            heroState.Parties = heroState.Elect ? heroState.Parties.OrderByDescending(p => p.ElectoralCollegeVotes).ThenByDescending(p => p.VotePercentage).ToList() : heroState.Parties.OrderByDescending(p => p.VotePercentage).ToList();

            if (heroState.Parties[0].ElectoralCollegeVotes == 0) return;                      // No results, do nothing

            var dataPoolDataFr = "MINIHERO={\n";
            dataPoolDataFr += "State=" + heroState.StateCode + ";\n";
            dataPoolDataFr += "Party=" + PartyCodeFr(heroState.Parties[0].PartyCode) + ";\n";
            dataPoolDataFr += "EcNumber=" + heroState.Parties[0].ElectoralCollegeVotes + ";\n";
            dataPoolDataFr += "};";

            if (_previousMiniData.ToUpper() != "MINIS")
            {
                if (_previousMiniData.ToUpper() == "MINI_HERO")
                {
                    //_viz.PrepareForNewScene(500, "MINI_HERO");
                    _viz.SendVizCommand("MINI_HERO=OFF;");
                }
            }
            else
            {
                _viz.SendVizCommand("PARTIES=OFF;");
            }
            Thread.Sleep(750);

            // Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene("MINI_HERO", dataPoolDataFr);

            _previousMiniData = "MINI_HERO";
        }

        /// <summary>
        /// Generate the hero board for one state - lower third graphic
        /// </summary>
        /// <param name="heroState"></param>
        public void UsStateBoardUb(USHeroState heroState)
        {
            // State hero board - sort by electoral college votes or votePercentage based on elect status
            heroState.Parties = heroState.Elect ? heroState.Parties.OrderByDescending(p => p.ElectoralCollegeVotes).ThenByDescending(p => p.VotePercentage).ToList() : heroState.Parties.OrderByDescending(p => p.VotePercentage).ToList();

            string dataPoolData = "MINI_RESULTS_DATA={\n";
            dataPoolData += "P1Party=" + heroState.Parties[0].PartyCode + ";\n";
            dataPoolData += "P2Party=" + heroState.Parties[1].PartyCode + ";\n";
            dataPoolData += "MAP=" + heroState.StateCode + ";\n";
            dataPoolData += "TotalValue=" + heroState.ElectoralCollegeVotes + ";\n";
            dataPoolData += heroState.Elect ? "Gain=Elect;\n" : "Gain=;\n";
            dataPoolData += "P1Value=HideObject;\n";
            dataPoolData += "P2Value=HideObject;\n";
            dataPoolData += "};";
            _viz.PrepareForNewScene(1000, "MINI_RESULTS");
            Thread.Sleep(200);
            // Send data to VIZ datapool and bring scene on-air
            _viz.DisplayScene("MINI_RESULTS", dataPoolData);
        }

        public void ContinueScene()
        {
            _viz.ContinueScene();
        }

        public void VizReset()
        {
            _viz.SendVizCommand("RESET_ALL=ON;");
        }

        public void VizResetAllDir()
        {
            _viz.SendVizCommand("RESET_ALL_DIR=ON;");
        }

        public void VizResetMinis()
        {
            _viz.SendVizCommand("RESET_MINIS=ON;");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public string MoveToPage(MoveDirection direction)
        {
            var retval = "";

            //If we're not currently on the last or first page...
            if ((direction == MoveDirection.PrevPage & _currentBoard.CurrentPageNumber == 1) |
                (direction == MoveDirection.NextPage & _currentBoard.CurrentPageNumber == _currentBoard.NumberOfPages))
                return retval;
            //Update the page # we want to display
            _currentBoard.SetPageNumber(_currentBoard.CurrentPageNumber + (int)direction);
            switch (_currentBoard.Scene.TypeOfData)
            {
                case DataType.Riding:
                    //Regenerate the board with a new starting page
                    GenerateRidingBoard(_riding);
                    break;
                case DataType.Region:
                    if (string.Compare(_region.BoardKey, "Senate Votes", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
                    {
                        //Special processing for Senate Votes board which can have multiple pages.  We don't want to completely
                        //animate the board off then on, rather, just take the current ridings off and bring the next ones on
                        _viz.SendVizCommand("SENATORS_BRD=PAGE" + _currentBoard.CurrentPageNumber.ToString() + ";");
                        _viz.DoingRefresh = true;
                        Thread.Sleep(500);
                        GenerateRegionBoard(_region);
                        _viz.DoingRefresh = false;
                    }
                    else
                    {
                        //Regenerate the board with a new starting page
                        GenerateRegionBoard(_region);
                    }
                    break;
                case DataType.RegionRiding:
                    if (string.Compare(_regionRiding.BoardKey, "Too Close to Call", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
                    {
                        //Special processing for Too Close to Call board which can have multiple pages.  We don't want to completely
                        //animate the board off then on, rather, just take the current ridings off and bring the next ones on
                        _viz.SendVizCommand("TOOCLOSE_BRD=PAGE" + _currentBoard.CurrentPageNumber.ToString() + ";");
                        _viz.DoingRefresh = true;
                        Thread.Sleep(500);
                        GenerateRegionRidingsBoard(_regionRiding);
                        _viz.DoingRefresh = false;
                    }
                    else
                    {
                        //Regenerate the board with a new starting page
                        GenerateRegionRidingsBoard(_regionRiding);
                    }
                    break;
            }
            //Recalculate the page n of nn display string
            retval = _currentBoard.CurrentPageNumber.ToString() + "/" + _currentBoard.NumberOfPages.ToString();

            //Update the UI label which tells the user how many pages we have for this board
            return retval;
        }

        /// <summary>
        /// Returns the sign corresponding to the number passed in
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string FormatSign(float value)
        {
            if (value > 0)
            {
                return "+";
            }
            return value < 0 ? "-" : "";
        }

        private static string PartyCodeFr(string partyCode)
        {
            switch (partyCode)
            {
                case "DEM":
                    return "D";
                case "GOP":
                    return "R";
                case "OTH":
                    return "O";
                case "IND":
                    return "I";
                default:
                    return "";
            }
        }
        public void UsTest()
        {

        }
    }
}
