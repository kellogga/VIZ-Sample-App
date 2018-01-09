using System.Collections.Generic;
using System.Globalization;
using System.Collections.Specialized;
using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.DataObjects;

namespace Maestro.BaseLibrary
{
    public class DataManager
    {
        internal DBLogic.DDIQAccessController _DDIQController;
        private NameValueCollection _sqlStatements = new NameValueCollection();
        private NameValueCollection _sqlParams = new NameValueCollection();
        int _mediaId;
        int _mainRegionID;
        int _referendumRegionID;
        private List<Map> _maps;
        private List<BaseParty> _baseParties;
        private List<BaseSummary> _baseSummaries;
        private List<BaseRegion> _baseRegions;
        private List<BaseRiding> _baseRidings;
        private int _thirdPartyID;
        private int _otherPartyID;
        private int _eligibleVoters;
        private int _prevTotalVoters;
        private int _geographicContrastId;

        public int MainRegionID { get {return _mainRegionID; }  }
        public int ReferendumRegionID { get { return _referendumRegionID; } }
        public DataManager(int mediaId)
        {

            _mediaId = mediaId;
            //Instantiate a new DDIQAccessController to give access to the DB and XML SQL document
            _DDIQController = new DBLogic.DDIQAccessController(false);

            //_sqlStatements.Add("GetPlaylistData", _DDIQController.GetSQL("GetPlaylistData"));
            //_sqlStatements.Add("GetBaseParties", _DDIQController.GetSQL("GetBaseParties"));
            //_sqlStatements.Add("GetSummaryList", _DDIQController.GetSQL("GetSummaryList"));
            //_sqlStatements.Add("GetRegionList", _DDIQController.GetSQL("GetRegionList"));
            //_sqlStatements.Add("GetDisplayNames", _DDIQController.GetSQL("GetDisplayNames"));
            //_sqlStatements.Add("GetCandidateDisplayNames", _DDIQController.GetSQL("GetCandidateDisplayNames"));
            //_sqlStatements.Add("GetRegionElectsByPartyData", _DDIQController.GetSQL("GetRegionElectsByPartyData"));
            //_sqlStatements.Add("GetRegionElectsData", _DDIQController.GetSQL("GetRegionElectsData"));
            //_sqlStatements.Add("GetRegionRidingsData", _DDIQController.GetSQL("GetRegionRidingsData"));
            //_sqlStatements.Add("GetRidingReportResult", _DDIQController.GetSQL("GetRidingReportResult"));
            //_sqlStatements.Add("GetRegionRidingIDs", _DDIQController.GetSQL("GetRegionRidingIDs"));
            //_sqlStatements.Add("GetInProgressRidingByMargin", _DDIQController.GetSQL("GetInProgressRidingByMargin"));
            //_sqlStatements.Add("GetRidingWithResults", _DDIQController.GetSQL("GetRidingWithResults"));
            //_sqlStatements.Add("GetPartyStandingsResult", _DDIQController.GetSQL("GetPartyStandingsResult"));
            //_sqlStatements.Add("GetPartyStandingsWomenResult", _DDIQController.GetSQL("GetPartyStandingsWomenResult"));
            //_sqlStatements.Add("GetSummaryResults", _DDIQController.GetSQL("GetSummaryResults"));
            //_sqlStatements.Add("GetRegionRidings", _DDIQController.GetSQL("GetRegionRidings"));
            //_sqlStatements.Add("GetTouchScreenControls", _DDIQController.GetSQL("GetTouchScreenControls"));
            //_sqlStatements.Add("GetRidingReportHistoryResult", _DDIQController.GetSQL("GetRidingReportHistoryResult"));
            //_sqlStatements.Add("GetSummaryRegionResults", _DDIQController.GetSQL("GetSummaryRegionResults"));
            //_sqlStatements.Add("GetRegionRidingsStatusData", "SELECT CANDIDATE.CAN_ID, CANDIDATE.RID_ID, CANDIDATE.CAN_DDIP_VOTES, CANDIDATE.CAN_DDIP_VOTES_POSITION, CANDIDATE.CAN_DDIP_STATUS, RIDING.RID_DDIP_CAN_VOTES_LEAD FROM CANDIDATE INNER JOIN RIDING ON RIDING.RID_ID = CANDIDATE.RID_ID INNER JOIN REGION_RIDING ON RIDING.RID_ID = REGION_RIDING.RID_ID INNER JOIN REGION ON REGION.REG_ID = REGION_RIDING.REG_ID WHERE(REGION.REG_ID = % RegionID %) ORDER BY REGION_RIDING.CREATION_DATE ASC, CANDIDATE.RID_ID, RIDING.RID_DDIP_CAN_VOTES_LEAD, CANDIDATE.CAN_DDIP_VOTES DESC");
            //_sqlStatements.Add("GetRidingsCandidates", _DDIQController.GetSQL("GetRidingsCandidates"));
            //_sqlStatements.Add("GetRegionRidingsCloseData", _DDIQController.GetSQL("GetRegionRidingsCloseData"));

            //_maps = LoadMaps();
            //_baseParties = LoadParties();
            //_baseSummaries = LoadSummaries();
            //_baseRegions = LoadRegions();
            //_baseRidings = LoadRidings(_mainRegionID);
            _eligibleVoters = GetEligibleVoters();
            _prevTotalVoters = GetPrevTotalVoters();
        }

        public List<Map> Maps { get { return _maps; } }

        public List<BaseParty> BaseParties { get { return _baseParties; } }

        public List<BaseSummary> BaseSummaries { get { return _baseSummaries; } }

        public List<BaseRegion> BaseRegions { get { return _baseRegions; } }

        public List<BaseRiding> BaseRidings { get { return _baseRidings; } }
        
        public int OtherPartyID { get { return _otherPartyID; } }

        public int ThirdPartyID { get { return _thirdPartyID; }  }

        public int EligibleVoters { get { return _eligibleVoters; }  }

        public int PrevTotalVoters { get { return _prevTotalVoters; }  }
        
        /// <summary>
        /// Special processing for some regions. There are a handful of regions that may be related to
        /// specific maps embedded in some VIZ scenes. If a region has a 'MaestroMapCode' value,
        /// then that region corresponds to a map object in some scenes. Keep track of these 'maps'
        /// in our _maps collection.
        /// </summary>
        /// <returns></returns>
        //private List<Map> LoadMaps()
        //{
        //    List<Map> newMaps = new List<Map>();
        //    DataTable dt = new DataTable();

        //    //Add the MediaID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("MediaID", _mediaId.ToString());
        //    //Get the required SQL statement from our private collection
        //    string sqlStatement = _sqlStatements.Get("GetRegionList");

        //    try
        //    {
        //        //Execute the SQL statement
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "Maps", true, _sqlParams);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                if (row["MaestroMapCode"].ToString().Length > 0)
        //                {
        //                    //Add each map to the main maps collection
        //                    newMaps.Add(Map.Create(row));
        //                }
        //            }
        //        }
        //        return newMaps;
        //    } catch (Exception ex) {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Populates our BaseParties list at application startup
        /// </summary>
        /// <returns></returns>
        //private List<BaseParty> LoadParties()
        //{
        //    DataTable dt = new DataTable();
        //    List<BaseParty> parties = new List<BaseParty>();

        //    //Add the MediaID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("MediaID", _mediaId.ToString());
        //    //Get the required SQL statement from our private collection
        //    string sqlStatement = _sqlStatements.Get("GetBaseParties");

        //    try
        //    {
        //        //Execute the SQL statement
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "BaseParties",true, _sqlParams);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                if (row["sPriority"].ToString().Length > 0)
        //                {
        //                    //Add each party to the main parties collection
        //                    parties.Add(BaseParty.Create(row, _mediaId));
        //                    BaseParty bp = parties[parties.Count - 1];
        //                    if (string.Compare(bp.PartyCode, "*TP", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
        //                    {
        //                        //We need to keep track of the Third Party ID so we can filter out these
        //                        //dummy candidates for riding boards - Quad, Riding reports etc.
        //                        _thirdPartyID = bp.PartyID;
        //                    }
        //                    if (string.Compare(bp.PartyCode, "OTH", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
        //                    {
        //                        //We need to keep track of the Other party ID so we can consolidate party values
        //                        //into one 'virtual' party for several boards - Vote Share, Vote change etc.
        //                        _otherPartyID = bp.PartyID;
        //                    }
        //                }
        //            }
        //        }

        //        //Specify the parameters necessary to retrieve displayname info for this election's parties
        //        _sqlParams.Clear();
        //        _sqlParams.Add("MediaID", _mediaId.ToString());
        //        _sqlParams.Add("DisplayName1", "Parties1");
        //        _sqlParams.Add("DisplayName2", "Parties2");
        //        //Get the required SQL statement from our private collection
        //        sqlStatement = _sqlStatements.Get("GetDisplayNames");
        //        try
        //        {
        //            //Execute the SQL statement
        //            _DDIQController.GetData(false, ref sqlStatement, dt, "DisplayNames", true, _sqlParams);

        //            if (dt.Rows.Count > 0)
        //            {
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    //Get the correct party object
        //                    foreach (BaseParty p in parties)
        //                    {
        //                        if (p.PartyID == Int32.Parse(row["EntityId"].ToString()))
        //                        {
        //                            //Assign this display name to the party object
        //                            p.UpdateDisplayName(row);
        //                            break;                                 }
        //                    }
        //                }
        //            }
        //            return parties;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //internal Region GetPartyStandingsData(Region r)
        //{
        //    r.ReportingRidings = 0;
        //    r.CurrentTotalVotes = 0;
        //    r.PreviousTotalVotes = 0;
        //    r.Parties.Clear();

        //    //Add the regionID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("prmRegId", r.RegionID.ToString());
        //    string sqlStatement = "";

        //    //Select one of 2 different queries to get Party Standings data.  The one for Woman's vote
        //    //is almost identical to GetPartyStandingsResult except is has one additional WHERE statement
        //    //to return only results for ridings where a woman is leading or elected.
        //    if (r.BoardKey.ToLower() == "woman's vote")
        //    {
        //        //Get the required SQL statement from our private collection
        //        sqlStatement = _sqlStatements.Get("GetPartyStandingsWomenResult");
        //    }
        //    else
        //    {
        //        //Get the required SQL statement from our private collection
        //        sqlStatement = _sqlStatements.Get("GetPartyStandingsResult");
        //    }

        //    try
        //    {
        //        //Execute the SQL statement
        //        DataTable dt = new DataTable();
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "RegionData", true, _sqlParams);

        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                r.Parties.Add(Party.Create(GetPartyByID(Int32.Parse(row["PartyID"].ToString()))));
        //                Party p = r.Parties[r.Parties.Count - 1];
        //                p.Update(row);
        //                r.CurrentTotalVotes += p.CurrentVotes;
        //                r.PreviousTotalVotes += p.PreviousVotes;
        //                if (p.PartyID != _thirdPartyID)
        //                {
        //                    r.ReportingRidings += p.CurrentElectedSeats + p.CurrentLeadingSeats;
        //                }
        //            }
        //        }

        //        //Special override for the woman's vote board.  This board focuses on ridings that
        //        //are held by female candidates and compares the current election to the previous
        //        //one.  Update each party's values in this region with values from the previous election.
        //        //These values are read from the application's config file.
        //        if (r.BoardKey.ToLower() == "woman's vote")
        //        {
        //            r.UpdateWomanSeats(MaestroConfig.GetConfig().WomanSeats, _baseParties);
        //        }

        //        //Sort the party data
        //        if (r.CalculateVoteShare)
        //        {
        //            r.Parties.Sort(new PartyComparerVotes());
        //        }
        //        else
        //        {
        //            r.Parties.Sort(new PartyComparerSeats());
        //        }

        //        if (!r.CalculateVoteShare)
        //        {
        //            //If we're not concerned with vote share then remove all parties from the parties
        //            //collection that have no elected or leading value right now and that had no seats
        //            //in this region in the previous election either
        //            for (int idx = r.Parties.Count - 1; idx >= 0; idx += -1)
        //            {
        //                Party p = r.Parties[idx];
        //                if ((p != null))
        //                {
        //                    if (p.CurrentElectedSeats == 0 & p.CurrentLeadingSeats == 0 & p.PrevReportingSeats == 0)
        //                    {
        //                        r.Parties.RemoveAt(idx);
        //                    }
        //                }
        //            }
        //        }

        //        if (r.ShowOthers)
        //        {
        //            int numMainParties = r.ItemsPerPage - 1;
        //            if (r.Parties.Count > r.ItemsPerPage)
        //            {
        //                //Create a virtual 'Other' party object and initialize it
        //                Party otherParty = (Party.Create(GetPartyByID(_otherPartyID)));
        //                //Accumulate the values for all parties greater than the number of main parties into one
        //                //virtual party record which we will display on air as the 'OTHER' party.
        //                for (int idx = r.Parties.Count - 1; idx >= numMainParties; idx += -1)
        //                {
        //                    otherParty.AddValues(r.Parties[idx]);
        //                    //Now that we have accumulated the values for this 'Other' party, remove it from
        //                    //the ArrayList.
        //                    r.Parties.RemoveAt(idx);
        //                }
        //                //Now, add the Other party to the array
        //                r.Parties.Add(otherParty);
        //            }
        //        }
        //        //Return our populated RegionData object
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Populates our BaseSummaries list at application startup
        /// </summary>
        /// <returns></returns>
        //private List<BaseSummary> LoadSummaries()
        //{
        //    DataTable dt = new DataTable();
        //    List<BaseSummary> summaries = new List<BaseSummary>();

        //    //Add the MediaID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("medID", _mediaId.ToString());
        //    //Get the required SQL statement from our private collection
        //    string sqlStatement = _sqlStatements.Get("GetSummaryList");

        //    try
        //    {
        //        //Execute the SQL statement
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "BaseSummaries", true, _sqlParams);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                //Add each summary to the main summaries collection
        //                summaries.Add(BaseSummary.Create(row, _mediaId));
        //            }
        //        }

        //        //Specify the parameters necessary to retrieve displayname info for this election's summaries
        //        _sqlParams.Clear();
        //        _sqlParams.Add("MediaID", _mediaId.ToString());
        //        _sqlParams.Add("DisplayName1", "Summaries1");
        //        _sqlParams.Add("DisplayName2", "Summaries2");
        //        //Get the required SQL statement from our private collection
        //        sqlStatement = _sqlStatements.Get("GetDisplayNames");
        //        try
        //        {
        //            //Execute the SQL statement
        //            _DDIQController.GetData(false, ref sqlStatement, dt, "DisplayNames", true, _sqlParams);

        //            if (dt.Rows.Count > 0)
        //            {
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    //Get the correct party object
        //                    foreach (BaseSummary s in summaries)
        //                    {
        //                        if (s.SummaryID == Int32.Parse(row["EntityId"].ToString()))
        //                        {
        //                            //Assign this display name to the party object
        //                            s.UpdateDisplayName(row);
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            return summaries;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Populates our BaseRegions list at application startup
        /// </summary>
        /// <returns></returns>
        //private List<BaseRegion> LoadRegions()
        //{
        //    DataTable dt = new DataTable();
        //    List<BaseRegion> regions = new List<BaseRegion>();

        //    //Add the MediaID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("MediaID", _mediaId.ToString());
        //    //Get the required SQL statement from our private collection
        //    string sqlStatement = _sqlStatements.Get("GetRegionList");

        //    try
        //    {
        //        //Execute the SQL statement
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "BaseRegions", true, _sqlParams);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                //Add each region to the main regions collection
        //                regions.Add(BaseRegion.Create(row, _mediaId, _maps));
        //            }
        //        }

        //        //Find and assign the main region and referendum region ids 
        //        {
        //            string referendumRegionName = "";
        //            foreach (BaseRegion r in regions)
        //            {
        //                if (r.MainRegion)
        //                {
        //                    _mainRegionID = r.RegionID;
        //                    referendumRegionName = "z-" + r.RegionName;
        //                    break;
        //                }
        //            }

        //            foreach (BaseRegion r in regions)
        //            {
        //                if (string.Compare(r.RegionName, referendumRegionName, new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
        //                {
        //                    _referendumRegionID = r.RegionID;
        //                    break;
        //                }
        //            }

        //        }
        //        //Specify the parameters necessary to retrieve displayname info for this election's regions
        //        _sqlParams.Clear();
        //        _sqlParams.Add("MediaID", _mediaId.ToString());
        //        _sqlParams.Add("DisplayName1", "Regions1");
        //        _sqlParams.Add("DisplayName2", "Regions2");
        //        //Get the required SQL statement from our private collection
        //        sqlStatement = _sqlStatements.Get("GetDisplayNames");
        //        try
        //        {
        //            //Execute the SQL statement
        //            _DDIQController.GetData(false, ref sqlStatement, dt, "DisplayNames", true, _sqlParams);

        //            if (dt.Rows.Count > 0)
        //            {
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    //Get the correct party object
        //                    foreach (BaseRegion r in regions)
        //                    {
        //                        if (r.RegionID == Int32.Parse(row["EntityId"].ToString()))
        //                        {
        //                            //Assign this display name to the party object
        //                            r.UpdateDisplayName(row);
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            return regions;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public RegionRiding GetRegionRidingsData(RegionRiding r, int votesMargin)
        //{
        //    //Make sure we don't have any ridings to begin with
        //    r.Ridings.Clear();

        //    //Add the regionID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("RegionID", r.RegionID.ToString());
        //    _sqlParams.Add("VotesMargin", votesMargin.ToString());
        //    //Get the required SQL statement from our private collection
        //    string sqlStatement = _sqlStatements.Get("GetRegionRidingsData");

        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        //Execute the SQL statement
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "RegionRidingsData", true, _sqlParams);

        //        if (dt.Rows.Count > 0)
        //        {
        //            //We've got some data to report so lets continue.
        //            int previousRidingID = -1;
        //            Riding riding = new Riding();
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                int ridingId = Int32.Parse(row["RidingID"].ToString());
        //                if (ridingId != previousRidingID)
        //                {
        //                    previousRidingID = ridingId;
        //                    r.Ridings.Add(Riding.Create(GetRidingById(ridingId)));
        //                    riding = r.Ridings[r.Ridings.Count - 1];
        //                }
        //                Candidate c = riding.GetCandidateByID(Int32.Parse(row["CandidateID"].ToString()));
        //                if ((c != null))
        //                {
        //                    c.SetVotes(Int32.Parse(row["DDIPVotes"].ToString()));
        //                }
        //                //Make sure the HasResults flag is set for this riding
        //                riding.SetHasResults(1);
        //            }
        //            //Sort the candidates
        //            r.SortCandidates();
        //            //Sort the ridings
        //            r.Ridings.Sort(new RidingComparerByMargin(2));
        //        }
        //        //Return our populated Region object
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public RegionRiding GetRegionRidingsData(RegionRiding r, int votesMargin, int depth)
        //{
        //    //Make sure we don't have any ridings to begin with
        //    r.Ridings.Clear();

        //    //Add the regionID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("RegionID", r.RegionID.ToString());
        //    //Get the required SQL statement from our private collection
        //    string sqlStatement = _sqlStatements.Get("GetRegionRidingsCloseData");

        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        //Execute the SQL statement to get a list of all ridings and candidate details that are still in play...
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "RegionRidingsCloseData", true, _sqlParams);

        //        if (dt.Rows.Count > 0)
        //        {
        //            //We've got some data to report so lets continue.  Load the resultset into a 
        //            //temporary work structure...
        //            RegionRiding rWork = new RegionRiding();
        //            int previousRidingID = -1;
        //            Riding riding = new Riding();
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                int ridingId = Int32.Parse(row["RidingID"].ToString());
        //                if (ridingId != previousRidingID)
        //                {
        //                    previousRidingID = ridingId;
        //                    rWork.Ridings.Add(Riding.Create(GetRidingById(ridingId)));
        //                    riding = rWork.Ridings[rWork.Ridings.Count - 1];
        //                }
        //                Candidate c = riding.GetCandidateByID(Int32.Parse(row["CandidateID"].ToString()));
        //                if ((c != null))
        //                {
        //                    c.SetVotes(Int32.Parse(row["DDIPVotes"].ToString()));
        //                }
        //                //Make sure the HasResults flag is set for this riding
        //                riding.SetHasResults(1);
        //            }
        //            //Sort the candidates
        //            rWork.SortCandidates();

        //            //Now that we have results for all ridings that are not elected yet, find ridings where
        //            //1st and nth position are within votesMargin value and add them to our official RegionRiding object...
        //            foreach (Riding ridWork in rWork.Ridings)
        //            {
        //                if (ridWork.Candidates.Count > depth - 1)
        //                {
        //                    //We've got at least the required number of candidates to perform
        //                    //the comparison...
        //                    Candidate c1 = ridWork.Candidates[0];
        //                    Candidate c2 = ridWork.Candidates[depth - 1];
        //                    int marginValue = c1.Votes - c2.Votes;
        //                    if (marginValue <= votesMargin)
        //                    {
        //                        //Add to our real RegionRiding object. the one that is used by the board logic
        //                        r.Ridings.Add(ridWork);
        //                    }
        //                }
        //            }
        //            //Sort the ridings
        //            r.Ridings.Sort(new RidingComparerByMargin(depth));
        //        }

        //        //Return our populated RegionRiding object
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Get a BaseRiding object based on the riding Id
        /// </summary>
        /// <param name="ridingId"></param>
        /// <returns></returns>
        public BaseRiding GetRidingById(int ridingId)
        {
            foreach (BaseRiding r in _baseRidings)
            {
                if (r.RidingID == ridingId)
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// Get a BaseRiding object based on the riding name
        /// </summary>
        /// <param name="ridingName"></param>
        /// <returns></returns>
        public BaseRiding GetRidingByName(string ridingName)
        {
            foreach (BaseRiding r in _baseRidings)
            {
                if (string.Compare(r.RidingName, ridingName, new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// Get a BaseRegion object based on the region Id
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public BaseRegion GetRegionById(int regionId)
        {
            foreach (BaseRegion r in _baseRegions)
            {
                if (r.RegionID == regionId)
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// Get a BaseRegion object based on the region name
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public BaseRegion GetRegionByName(string regionName)
        {
            foreach (BaseRegion r in _baseRegions)
            {
                if (string.Compare(r.RegionName, regionName, new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
                {
                    return r;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Get a BaseRiding object based on the riding number
        /// </summary>
        /// <param name="ridingNumber"></param>
        /// <returns></returns>
        public BaseRiding GetRidingByNumber(int ridingNumber)
        {
            foreach (BaseRiding r in _baseRidings)
            {
                if (r.RidingNumber == ridingNumber)
                {
                    return r;
                }
            }
            return null;
        }

        //public Riding GetRidingData(Riding r)
        //{
        //    int leadVotes = 0;
        //    int totalVotes = 0;

        //    //Add the riding ID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("prmRidId", r.RidingID.ToString());
        //    _sqlParams.Add("medID", _mediaId.ToString());
        //    //Get the required SQL statement from our private collection
        //    string sqlStatement = _sqlStatements.Get("GetRidingReportResult");

        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        //Execute the SQL statement
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "RidingResults", true, _sqlParams, "", "", "CAN.DDIPVotes DESC");

        //        if (dt.Rows.Count > 0)
        //        {
        //            //Add the dynamic data for this riding to the ridingData object
        //            r.DynamicUpdate(dt.Rows[0]);
        //            bool firstRecord = true;
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                //Make sure we're updating the correct candidate record
        //                Candidate c = r.GetCandidateByID(Int32.Parse(row["CandidateID"].ToString()));
        //                if ((c != null))
        //                {
        //                    leadVotes = c.UpdateValues(row, firstRecord, leadVotes);
        //                    firstRecord = false;
        //                    totalVotes += c.Votes;
        //                }
        //            }

        //            //Set a flag to indicate if this riding has results yet
        //            r.SetHasResults(totalVotes);
        //            //Make sure the candidates are sorted in the correct order
        //            r.SortCandidates();
        //            r.SetCandidateValues();
        //        }
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}

        /// <summary>
        /// Get a BaseSumamry object based on the summary Id
        /// </summary>
        /// <param name="summaryId"></param>
        /// <returns></returns>
        public BaseSummary GetSummaryById(int summaryId)
        {
            foreach (BaseSummary s in _baseSummaries)
            {
                if (s.SummaryID == summaryId)
                {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// Get a BaseSummary object based on the summary name
        /// </summary>
        /// <param name="summaryName"></param>
        /// <returns></returns>
        public BaseSummary GetSummaryByName(string summaryName)
        {
            foreach (BaseSummary s in _baseSummaries)
            {
                if (string.Compare(s.SummaryName, summaryName, new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
                {
                    return s;
                }
            }
            return null;
        }

        //public Summary GetSummaryData(Summary s)
        //{
        //    //Add the summaryID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("sumID", s.SummaryID.ToString());
        //    string sqlStatement = "";
        //    //Get the required SQL statement from our private collection
        //    sqlStatement = _sqlStatements.Get("GetSummaryResults");

        //    try
        //    {
        //        //Execute the SQL statement (including the ORDER BY clause)
        //        DataTable dt = new DataTable();
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "SummaryData", true, _sqlParams, "", "", "RegionOrder");
        //        int prevRegionID = -1;
        //        if (dt.Rows.Count > 0)
        //        {
        //            SummaryRegion r = null;
        //            int regionTotalVotes = 0;
        //            int regionTotalVotesPrevElection = 0;
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                int regionID = Int32.Parse(row["RegionID"].ToString());
        //                if (regionID != prevRegionID)
        //                {
        //                    if (r != null)
        //                    {
        //                        //Update the vote share values for the previous region
        //                        r.UpdateVoteShareValues(regionTotalVotes, regionTotalVotesPrevElection);
        //                    }
        //                    //Instantiate a new SummaryRegionData object from our baseRegions collection
        //                    s.SummaryRegions.Add(SummaryRegion.Create(GetRegionById(regionID)));
        //                    r = s.SummaryRegions[s.SummaryRegions.Count - 1];
        //                    //Reset our breakpoint values
        //                    regionTotalVotes = 0;
        //                    regionTotalVotesPrevElection = 0;
        //                    prevRegionID = regionID;
        //                }
        //                if (r != null)
        //                {
        //                    //Accumulate the total number of votes cast in this region
        //                    regionTotalVotes += Int32.Parse(row["PartyTotVotes"].ToString());
        //                    regionTotalVotesPrevElection += Int32.Parse(row["PartyTotVotesPrevElect"].ToString());
        //                    int partyID = Int32.Parse(row["PartyID"].ToString());
        //                    if (partyID == s.Party.PartyID)
        //                    {
        //                        //We only care about one party at a time
        //                        r.UpdateValues(row);
        //                    }
        //                }
        //            }
        //            if (r != null)
        //            {
        //                //Update the vote share values for the last region
        //                r.UpdateVoteShareValues(regionTotalVotes, regionTotalVotesPrevElection);
        //            }
        //        }
        //        //Return the Summary object 
        //        return s;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Populates our BaseRidings list at application startup
        /// </summary>
        /// <param name="mainRegionID"></param>
        /// <returns></returns>
        //private List<BaseRiding> LoadRidings(int mainRegionID)
        //{
        //    DataTable dt = new DataTable();
        //    List<BaseRiding> ridings = new List<BaseRiding>();
        //    BaseRiding r = null;
        //    BaseCandidate c = null;

        //    //Add the MediaID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("MediaID", _mediaId.ToString());
        //    _sqlParams.Add("RegionID", mainRegionID.ToString());
        //    //Get the required SQL statement from our private collection
        //    string sqlStatement = _sqlStatements.Get("GetRidingsCandidates");

        //    try
        //    {
        //        //Execute the SQL statement
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "BaseRidingsandCandidates", true, _sqlParams);
        //        if (dt.Rows.Count > 0)
        //        {
        //            int prevRidingNum = -1;
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                if (Int32.Parse(row["RidingNumber"].ToString()) != prevRidingNum)
        //                {
        //                    //We've got a new riding so add one riding object to our Ridings collection
        //                    ridings.Add(BaseRiding.Create(row, _maps, _mediaId));
        //                    r = ridings[ridings.Count - 1];
        //                    prevRidingNum = r.RidingNumber;
        //                }
        //                int partyId = Int32.Parse(row["PartyID"].ToString());
        //                //Weed out the third party candidates
        //                //CAN_LOGICAL added to weed out dummy candidates that are there solely to properly
        //                //reflect party votes from previous election.  ACK Feb 2012

        //                {
        //                    if (dt.Columns.Contains("IsDummy"))
        //                    {
        //                        if (row["IsDummy"].ToString() != "1")
        //                        {
        //                            //Add one candidate to the riding
        //                            r.BaseCandidates.Add(BaseCandidate.Create(row, GetPartyByID(partyId)));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (partyId != _thirdPartyID)
        //                        {
        //                            //Add one candidate to the riding
        //                            r.BaseCandidates.Add(BaseCandidate.Create(row, GetPartyByID(partyId)));
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //Specify the parameters necessary to retrieve displayname info for this election's candidates
        //        _sqlParams.Clear();
        //        _sqlParams.Add("MediaID", _mediaId.ToString());
        //        _sqlParams.Add("DisplayName1", "Candidates1");
        //        _sqlParams.Add("DisplayName2", "Candidates2");
        //        //Get the required SQL statement from our private collection
        //        sqlStatement = _sqlStatements.Get("GetCandidateDisplayNames");
        //        //Execute the SQL statement
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "DisplayNames", true, _sqlParams);

        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                //Get the correct party object
        //                foreach (BaseRiding br in ridings)
        //                {
        //                    if (br.RidingID == Int32.Parse(row["RidingID"].ToString()))
        //                    {
        //                        c = br.Candidate(Int32.Parse(row["EntityID"].ToString()));
        //                        if ((c != null))
        //                        {
        //                            //Assign this display name to the riding object
        //                            c.UpdateDisplayName(row);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        //Specify the parameters necessary to retrieve displayname info for this election's ridings
        //        _sqlParams.Clear();
        //        _sqlParams.Add("MediaID", _mediaId.ToString());
        //        _sqlParams.Add("DisplayName1", "Ridings1");
        //        _sqlParams.Add("DisplayName2", "Ridings2");
        //        //Get the required SQL statement from our private collection
        //        sqlStatement = _sqlStatements.Get("GetDisplayNames");
        //        //Execute the SQL statement
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "DisplayNames", true, _sqlParams);

        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                //Get the correct party object
        //                foreach (BaseRiding br in ridings)
        //                {
        //                    if (br.RidingID == Int32.Parse(row["EntityId"].ToString()))
        //                    {
        //                        //Assign this display name to the party object
        //                        br.UpdateDisplayName(row);
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        //Return our populated Ridings object
        //        return ridings;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void SetGeographicContrastID(int Id)
        {
            _geographicContrastId = Id;
        }

        /// <summary>
        /// Sums the eligible voters from the BaseRiding list to get the 
        /// total number of eligible voters in the current election
        /// </summary>
        /// <returns></returns>
        private int GetEligibleVoters()
        {
            int eligibleVoters = 0;
            foreach (BaseRiding r in _baseRidings)
            {
                eligibleVoters += r.TotalVoters;
            }
            return eligibleVoters;
        }

        /// <summary>
        /// Sums the eligible voters from the BaseRiding list to get
        /// the total number of eligible voters in the previous election
        /// </summary>
        /// <returns></returns>
        private int GetPrevTotalVoters()
        {
            int prevTotalVoters = 0;
            foreach (BaseRiding r in _baseRidings)
            {
                prevTotalVoters += r.PrevTotalVoters;
            }
            return prevTotalVoters;
        }

        //public List<Playlist> GetPlaylists(int electionId, int mediaId)
        //{
        //    List<Playlist> playlists = new List<Playlist>();
        //    Playlist p = null;
        //    string previousName = "";

        //    try
        //    {
        //        //Execute the SQL statement
        //        //DataTable dt = DataExchanger.GetPlaylistDetails(electionId, UserInformation.MediaID);
        //        DataTable dt = DataExchanger.GetPlaylists(electionId, mediaId);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                if (row["PL_ID"].ToString() + " - " + row["ENGNAME"].ToString() != previousName)
        //                {
        //                    //Add this new playlist to the Playlists collection
        //                    playlists.Add(Playlist.Create(row));
        //                    p = playlists[playlists.Count - 1];
        //                    previousName = p.DisplayName;
        //                }
        //                if ((p != null))
        //                {
        //                    //Add the playlist item's board details to the playlist object
        //                    p.Add(PlaylistItem.Create(row));
        //                }
        //            }
        //        }

        //        //Return populated Playlist structure
        //        return playlists;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        /// <summary>
        /// Get the current poll counts
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        //public Region GetPollsData(Region r)
        //{
        //    //Initially set the polls data to 0...
        //    r.SetPollsData(0, 0);
        //    //Add the summaryID parameter to our parameters collection
        //    _sqlParams.Clear();
        //    _sqlParams.Add("sumID", _geographicContrastId.ToString());
        //    string sqlStatement = "";
        //    //Get the required SQL statement from our private collection
        //    sqlStatement = _sqlStatements.Get("GetSummaryRegionResults");

        //    try
        //    {
        //        //Execute the SQL statement
        //        DataTable dt = new DataTable();
        //        //Run a query to retrieve the polling data for all regions
        //        _DDIQController.GetData(false, ref sqlStatement, dt, "SummaryData", true, _sqlParams);

        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                int regionID = Int32.Parse(row["RegionID"].ToString());
        //                if (regionID == r.RegionID)
        //                {
        //                    //Found the right region so update the regionData object with poll values
        //                    r.SetPollsData(Int32.Parse(row["SumOfPollsReported"].ToString()), Int32.Parse(row["SumOfTotalPolls"].ToString()));
        //                    break; 
        //                }
        //            }
        //        }
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Finds one party object based on the partyID passed in
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private BaseParty GetPartyByID(int value)
        {
            foreach (BaseParty p in _baseParties)
            {
                if (p.PartyID == value)
                {
                    return p;
                }
            }
            return null;
        }
    }
}
