using Maestro.BaseLibrary.DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Maestro.BaseLibrary.BaseObjects
{
	public class BaseRegion
	{
		private int _regionID;
		private string _regionName;
		private string _displayName1;
		private string _displayName2;
		private bool _mainRegion;
		private string _mapCode;
		private int _totalRidings;
		public int RegionID { get { return _regionID; } }
		public string RegionName { get { return _regionName; } }
		public string DisplayName1 { get { return _displayName1; } }
		public string DisplayName2 { get { return _displayName2; } }
		public bool MainRegion { get { return _mainRegion; } }
		public string MapCode { get { return _mapCode; } }
		public int TotalRidings { get { return _totalRidings; } }

		/// <summary>
		/// Factory method to instantiate one BaseRegion object from DataRow
		/// </summary>
		/// <param name="row"></param>
		/// <param name="mediaId"></param>
		/// <param name="maps"></param>
		/// <returns></returns>
		public static BaseRegion Create(DataRow row, int mediaId, List<Map> maps)
		{
			BaseRegion r = new BaseRegion();
			try
			{
				//set the properties passed into the method
				r._regionID = Int32.Parse(row["Id"].ToString());
				r._totalRidings = int.Parse(row["TotalRidings"].ToString());
				if ((bool)row["IsGlobalRegion"] == true)
				{
					r._mainRegion = true;
				}
				else
				{
					r._mainRegion = false;
				}
				if (mediaId == 1)
				{
					r._regionName = row["EnglishName"].ToString();
				}
				else
				{
					if (row.Table.Columns.Contains("FrenchName"))
					{
						r._regionName = row["FrenchName"].ToString();
					}
					else
					{
						r._regionName = row["EnglishName"].ToString();
					}
				}
				r._displayName1 = BaseUtilities.ProperCase(r._regionName);
				r._displayName2 = r._displayName1;
				r._mapCode = "";
				foreach (Map m in maps)
				{
					if (string.Compare(m.Name, r._regionName, new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
					{
						r._mapCode = m.ProvCode;
					}
				}
				return r;
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Figure out which type of display name we have and assign to the appropriate property
		/// </summary>
		/// <param name="row"></param>
		public void UpdateDisplayName(DataRow row)
		{
			if (string.Compare(row["TypeName"].ToString(), "Regions1", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
			{
				_displayName1 = row["DisplayName"].ToString();
			}
			if (string.Compare(row["TypeName"].ToString(), "Regions2", new CultureInfo("en-CA"), CompareOptions.IgnoreCase) == 0)
			{
				_displayName2 = row["DisplayName"].ToString();
			}
		}

		/// <summary>
		/// Override the ToString() method of this object to return the region name, not the object type.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _regionName.ToUpper();
		}
	}
}
