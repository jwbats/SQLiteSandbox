using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLiteDemo.Controllers;
using SQLiteDemo.Extensions;

namespace SQLiteDemo.Controllers
{
	public class Logger
	{

		#region ================================================== Singleton Stuff ==================================================

		private Logger()
		{
		}

		private static Logger instance;
		public  static Logger Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Logger();
				}

				return instance;
			}
		}

		#endregion ================================================== Singleton Stuff ==================================================




		#region ================================================== Private Methods ==================================================

		private string CreateLogInsertStatement(string type, string entry)
		{
			return String.Format(
				"INSERT INTO Logs (Date, Type, Entry) VALUES ('{0}', '{1}', '{2}')",
				DateTime.Now.ToYMDHMS(),
				type,
				entry
			);
		}




		private void InsertLogEntry(string insertStatement)
		{
			using (SQLiteController sqliteController = new SQLiteController("Logs.hawk", LogSchemaController.Instance))
			{
				sqliteController.ExecuteNonQuery(insertStatement);
			}
		}

		#endregion ================================================== Private Methods ==================================================




		#region ================================================== Public Methods ==================================================

		public void LogInfo(string entry)
		{
			InsertLogEntry(
				CreateLogInsertStatement("Info", entry)
			);
		}




		public void LogWarning(string entry)
		{
			InsertLogEntry(
				CreateLogInsertStatement("Warning", entry)
			);
		}




		public void LogError(string entry)
		{
			InsertLogEntry(
				CreateLogInsertStatement("Error", entry)
			);
		}




		public void LogException(Exception exception)
		{
			LogError(exception.ToString());
		}

		#endregion ================================================== Public Methods ==================================================

	}
}
