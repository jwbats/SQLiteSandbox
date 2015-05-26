using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SQLiteDemo.Extensions
{
	public static class ExtDataRowCollection
	{

		#region ================================================== Public DataRowCollection ==================================================

		public static List<DataRow> ToList(this DataRowCollection dataRowCollection)
		{
			return dataRowCollection.Cast<DataRow>().ToList();
		}




		public static Dictionary<T1, DataRow> ToDictionary<T1>(this DataRowCollection dataRows, string columnKey)
		{
			Dictionary<T1, DataRow> dictionary = new Dictionary<T1, DataRow>();

			foreach (DataRow dataRow in dataRows)
			{
				T1 key   = (T1)dataRow[columnKey];

				dictionary[key] = dataRow;
			}

			return dictionary;
		}


		
		
		public static Dictionary<T1, T2> ToDictionary<T1, T2>(this DataRowCollection dataRows, string columnKey, string columnValue)
		{
			Dictionary<T1, T2> dictionary = new Dictionary<T1, T2>();

			foreach (DataRow dataRow in dataRows)
			{
				T1 key   = (T1)dataRow[columnKey];
				T2 value = (T2)dataRow[columnValue];

				dictionary[key] = value;
			}

			return dictionary;
		}




		public static List<T> GetValuesFromColumn<T>(this DataRowCollection dataRows, string column)
		{
			List<T> values = new List<T>();

			foreach (DataRow dataRow in dataRows)
			{
				T value = (T)dataRow[column];

				values.Add(value);
			}

			return values;
		}




		public static List<string> GetValuesFromColumnAsStrings(this DataRowCollection dataRows, string column)
		{
			return dataRows.ToList().Select(x => Convert.ToString(x[column])).ToList();
		}

		#endregion ================================================== Public DataRowCollection ==================================================

	}
}