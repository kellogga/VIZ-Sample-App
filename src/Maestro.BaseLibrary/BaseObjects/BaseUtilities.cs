using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using Maestro.Entities;

namespace Maestro.BaseLibrary.BaseObjects
{
    public static class BaseUtilities
    {
        private static CultureInfo _ci = Thread.CurrentThread.CurrentCulture;
        private static TextInfo _ti = _ci.TextInfo;
        public static string ProperCase(string value)
        {
            return _ti.ToTitleCase(value.ToLower());
        }
    }
}
