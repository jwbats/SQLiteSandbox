using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SQLiteDemo.Extensions
{
	public static class ExtDateTime
	{

		#region ================================================== Public DateTime ==================================================

		public static string ToYMDHMS(this DateTime dateTime)
		{
			return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
		}

		#endregion ================================================== Public DateTime ==================================================

	}
}