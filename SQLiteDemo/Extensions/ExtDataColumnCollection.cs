using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SQLiteDemo.Extensions
{
	public static class ExtDataColumnCollection
	{

		#region ================================================== Public DataColumnCollection ==================================================

		public static List<DataColumn> ToList(this DataColumnCollection dataColumnCollection)
		{
			return dataColumnCollection.Cast<DataColumn>().ToList();
		}

		#endregion ================================================== Public DataColumnCollection ==================================================

	}
}