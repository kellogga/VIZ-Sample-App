using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Maestro.BaseLibrary.Interfaces;
using System.Data;
using Maestro.BaseLibrary.BaseObjects;
using Maestro.BaseLibrary.Config;
using System.Globalization;
using VizWrapper;

namespace Maestro.BaseLibrary.Entities
{
    public class Riding : IBaseBoard, IRiding, IEnumerable
	{
		private List<Candidate> _candidates = new List<Candidate>();
		private List<RidingResult> _results;

		public Riding()
		{
			_results = new List<RidingResult>();
		}

		//IBaseBoard property variables
		private string _electionType;
		private int _numberOfPages;
		private int _currentPageNumber;
		private int _firstItem;
		private int _numberOfItemsToDisplay;
		private Scene _scene;

		private int _ridingID;
		private int _ridingNumber;
		private string _provinceCode;
		private string _ridingName;
		private string _displayName1;
		private string _displayName2;
		private int _incumbentPartyID;
		private int _totalPolls;
		private int _totalVoters;
		private int _prevTotalVoters;
		private int _reportingPolls;
		private bool _heroNoteShown;
		private string _frComments;
		private int _reportStatus;
		private bool _hasResults;

		public int CurrentPageNumber { get { return _currentPageNumber; } }
		public string DisplayName1 { get { return _displayName1; } }
		public string DisplayName2 { get { return _displayName2; } }
		public string ElectionType { get { return _electionType; } }
		public int FirstItem { get { return _firstItem; } }
		public string FrComments { get { return _frComments; } }
		public bool HasResults { get { return _hasResults; } }
		public bool HeroNoteShown { get { return _heroNoteShown; } set {_heroNoteShown = value; } }
		public int IncumbentPartyID { get { return _incumbentPartyID; } }
		public int NumberOfItemsToDisplay { get { return _numberOfItemsToDisplay; } }
		public int NumberOfPages { get
			{
				_numberOfPages = _candidates.Count / _scene.NumberOfItems;
				if (_candidates.Count % _scene.NumberOfItems > 0)
				{
					_numberOfPages += 1;
				}
				return _numberOfPages;
			}
		}
		public int PrevTotalVoters { get { return _prevTotalVoters; } }
		public string ProvinceCode { get { return _provinceCode; } }
		public int ReportingPolls { get { return _reportingPolls; } }
		public int ReportStatus { get { return _reportStatus; } }
		public int RidingID { get { return _ridingID; } }
		public string RidingName { get { return _ridingName; } }
		public int RidingNumber { get { return _ridingNumber; } }
		public int TotalItems { get { return _candidates.Count(); } }
		public int TotalPolls { get { return _totalPolls; } }
		public int TotalVoters { get { return _totalVoters; } }
		public Scene Scene { get { return _scene; } }
		public List<Candidate> Candidates { get { return _candidates; } }

		public int PauseInterval
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string DatapoolName
		{
			get
			{
				throw new NotImplementedException();
			}
		}

        IScene IBaseBoard.Scene
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerator GetEnumerator()
		{
			return _results.GetEnumerator();
		}

		/// <summary>
		/// Return the number of results we have for this riding
		/// </summary>
		/// <returns></returns>
		public int ResultsCount()
		{
			return _results.Count();
		}

		/// <summary>
		/// Clear the riding results
		/// </summary>
		public void ResultsClear()
		{
			_results.Clear();
		}

		/// <summary>
		/// Find the requested candidate based on candidate Id
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public Candidate GetCandidateByID(int id)
		{
			foreach (Candidate c in _candidates)
			{
				if (c.CandidateID == id)
				{
					return c;
				}
			}
			return null;
		}

		/// <summary>
		/// Set the Hero Note flag
		/// </summary>
		/// <param name="flag"></param>
		public void SetHeroNoteShownFlag(bool flag)
		{
			_heroNoteShown = flag;
		}

		/// <summary>
		/// Set the page number, number of Items to display and first item values for this riding
		/// based on the page number specified
		/// </summary>
		/// <param name="pageNumber"></param>
		public void SetPageNumber(int pageNumber)
		{
			_currentPageNumber = pageNumber;
			if (_currentPageNumber * _scene.NumberOfItems <= TotalItems)
			{
				_numberOfItemsToDisplay = _scene.NumberOfItems;
			}
			else
			{
				_numberOfItemsToDisplay = TotalItems % _scene.NumberOfItems;
			}
			_firstItem = (_currentPageNumber - 1) * _scene.NumberOfItems;
		}

		/// <summary>
		/// Add one result record to this riding from the database
		/// </summary>
		/// <param name="row"></param>
		public void AddResult(DataRow row)
		{
			_results.Add(RidingResult.Create(row));
		}

		/// <summary>
		/// Update the dynamic values for this riding
		/// </summary>
		/// <param name="row"></param>
		public void DynamicUpdate(DataRow row)
		{
			_reportingPolls = Int32.Parse(row["DDIPPollsReported"].ToString());
			_reportStatus = Int32.Parse(row["DDIPStatus"].ToString());
			_totalPolls = Int32.Parse(row["TotalPolls"].ToString());
		}
		
		/// <summary>
		/// Factory method to create one Riding object from a BaseRiding and it's associated
		/// BaseCandidates objects
		/// </summary>
		/// <param name="br"></param>
		/// <returns></returns>
		public static Riding Create(BaseRiding br)
		{
			Riding r = new Riding();
			foreach (BaseCandidate c in br.BaseCandidates)
			{
				r._candidates.Add(Candidate.Create(c));
			}
			r._displayName1 = br.DisplayName1;
			r._displayName2 = br.DisplayName2;
			r._heroNoteShown = br.HeroNoteShown;
			r._incumbentPartyID = br.IncumbentPartyID;
			r._provinceCode = br.ProvinceCode;
			r._ridingID = br.RidingID;
			r._ridingName = br.RidingName;
			r._ridingNumber = br.RidingNumber;
			r._totalPolls = br.TotalPolls;
			r._totalVoters = br.TotalVoters;
			r._prevTotalVoters = br.PrevTotalVoters;
			r._frComments = br.FrComments;
			return r;
		}

		public void Update(BaseRiding baseRiding, Scene scene, string electionType)
		{
			//Assign properties from baseRiding object
			_candidates.Clear();
			foreach (BaseCandidate c in baseRiding.BaseCandidates)
			{
				_candidates.Add(Candidate.Create(c));
			}
			_displayName1 = baseRiding.DisplayName1;
			_displayName2 = baseRiding.DisplayName2;
			_heroNoteShown = baseRiding.HeroNoteShown;
			_incumbentPartyID = baseRiding.IncumbentPartyID;
			_provinceCode = baseRiding.ProvinceCode;
			_ridingID = baseRiding.RidingID;
			_ridingName = baseRiding.RidingName;
			_ridingNumber = baseRiding.RidingNumber;
			_totalPolls = baseRiding.TotalPolls;
			_totalVoters = baseRiding.TotalVoters;
			_frComments = baseRiding.FrComments;
			//Assign other values
			_currentPageNumber = 0;
			_electionType = electionType;
			_scene = scene;
		}

		/// <summary>
		/// Updates the VIZ related values for this riding based on a scene object
		/// </summary>
		/// <param name="scene"></param>
		public void Update(Scene scene)
		{
			//Assign properties from scene object
			if ((scene != null))
			{
				_scene = scene;
			}
		}

		/// <summary>
		/// Set the HasResults flag
		/// </summary>
		/// <param name="totalVotes"></param>
		public void SetHasResults(int totalVotes)
		{
			_hasResults = totalVotes > 0;
		}

		/// <summary>
		/// Set the firstItem property base on the position of the desired candidate
		/// in the candidates collection (used for single hero board).
		/// </summary>
		/// <param name="id"></param>
		public void SetCandidateIndex(int id)
		{
			int i = 0;
			foreach (Candidate c in _candidates)
			{
				if (c.CandidateID == id)
				{
					_firstItem = i;
					break; 
				}
				i += 1;
			}
		}

		/// <summary>
		/// Update dynamic data on the candidate records
		/// </summary>
		public void SetCandidateValues()
		{
			int rank = 0;
			foreach (Candidate c in _candidates)
			{
				rank += 1;
				c.SetRank(rank);
				c.SetCandidateStatus(GetCandidateStatus(c));
			}
		}

		/// <summary>
		/// Returns the current status of a candidate as a string
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		private string GetCandidateStatus(Candidate c)
		{
			string retval = null;
			if ((_reportStatus & 0) != 0)
			{
				//No results in this riding yet
				retval = "To Come";
			}
			else if ((c.Status & 8) != 0)
			{
				//This candidate has been elected
				if (c.Rank == 1)
				{
					if ((c.Status & 16) != 0)
					{
						//Party flip
						retval = "Gain";
					}
					else
					{
						//Party has retained this riding
						retval = "Elect";
					}
				}
				else
				{
					retval = "Defeated";
				}
			}
			else
			{
				//This candidate has not yet been elected
				if (c.Votes == 0 & c.Lead == 0)
				{
					retval = "";
				}
				else if (c.Lead == 0)
				{
					retval = "Tie";
				}
				else if (c.Lead > 0)
				{
					retval = "Lead";
				}
				else
				{
					retval = "Trail";
				}
			}
			return retval;
		}

		/// <summary>
		/// Sort the candidates for this riding
		/// </summary>
		public void SortCandidates()
		{
			if (_hasResults)
			{
				_candidates.Sort(new CandidateComparerResults());
			}
			else
			{
				_candidates.Sort(new CandidateComparerNoResults());
			}
		}

		/// <summary>
		/// Total up the votes for this entire riding
		/// </summary>
		/// <returns></returns>
		public int VotesCounted()
		{
			int votes = 0;
			foreach (Candidate c in _candidates)
			{
				votes += c.Votes;
			}
			return votes;
		}
	}

	/// <summary>
	/// Helper class used to sort list of Riding objects by votes margin between the 1st candidate and the
	/// nth candidate, identified by the depth parameter
	/// </summary>
	internal class RidingComparerByMargin : IComparer<Riding>
	{
		private int _depth;
		/// <summary>
		/// Constructor - this class requires a depth variable which is the number of candidates to use for the
		/// margin comparison.
		/// </summary>
		/// <param name="depth"></param>
		/// <remarks></remarks>
		public RidingComparerByMargin(int depth)
		{
			_depth = depth;
		}

		/// <summary>
		/// Delegate to a private method to do the sort
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		public int Compare(Riding r1, Riding r2)
		{
			return CompareMargin((Riding)r1, (Riding)r2, _depth);
		}

		/// <summary>
		/// Sorts Riding object based on the votes received for each candidate.  Compares the first candidate's votes to
		/// the specified depth candidate's votes (for example, if depth = 3 then this routine will sort the ridings based on 
		/// the difference in votes between 1st place and 3rd place).  Used for Too Close To Call board (1st and 2nd place) and for 
		/// 3 Way Race board (1st and 3rd place).
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <param name="depth"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		private int CompareMargin(Riding r1, Riding r2, int depth)
		{
			int canIndex = depth - 1;
			//We can only do this if we have at least depth number of candidates in each riding
			if (r1.TotalItems > canIndex & r2.TotalItems > canIndex)
			{
				//Get the votes margin between 1st and nth place for both ridings...
				int r1Margin = r1.Candidates[0].Votes - r1.Candidates[canIndex].Votes;
				int r2Margin = r2.Candidates[0].Votes - r2.Candidates[canIndex].Votes;
				//Sort the ridings by the vote margin between the 1st and nth place candidates
				if ((r1Margin > r2Margin))
				{
					return 1;
				}
				if ((r1Margin < r2Margin))
				{
					return -1;
				}
				else
				{
					//We've got a tie so use the Riding Name as our secondary sort criteria
					return string.Compare(r1.RidingName, r2.RidingName, new CultureInfo("en-CA"), CompareOptions.IgnoreCase);
				}
			}
			else
			{
				return 0;
			}
		}

	}
}
