using System;
using System.Data;

namespace Maestro.BaseLibrary.DataObjects
{
    public class Map
    {
        private string _name;
        string _provCode;
        int _startRiding;
        int _endRiding;

        public string Name { get { return _name; } }
        public string ProvCode { get { return _provCode; } }
        public int StartRiding { get { return _startRiding; } }
        public int EndRiding { get { return _endRiding; } }

        public static Map Create(DataRow row)
        {
            Map m = new Map();
            try
            {
                //set the properties passed into the method
                m._name = row["EnglishName"].ToString();
                m._provCode = row["MaestroMapCode"].ToString();
                m._startRiding = Int32.Parse(row["MaestroMapStartRidingId"].ToString());
                m._endRiding = Int32.Parse(row["MaestroMapEndRidingId"].ToString());
                return m;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
