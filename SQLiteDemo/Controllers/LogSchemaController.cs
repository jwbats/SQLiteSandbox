using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo.Controllers
{
	public class LogSchemaController : ISQLiteSchemaController
	{

		#region ================================================== Singleton Stuff ==================================================

		private LogSchemaController()
		{
		}

		private static LogSchemaController instance;
		public  static LogSchemaController Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new LogSchemaController();
				}

				return instance;
			}
		}

		#endregion ================================================== Singleton Stuff ==================================================




		#region ================================================== Private Methods ==================================================

		private void EnsureSchemaComplete0(SQLiteController sqliteController)
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine("CREATE TABLE IF NOT EXISTS Logs(");
			builder.AppendLine("ID		INTEGER PRIMARY KEY ASC AUTOINCREMENT,");
			builder.AppendLine("Date	DATE NOT NULL,");
			builder.AppendLine("Type	TEXT NOT NULL,");
			builder.AppendLine("Entry	TEXT NOT NULL");
			builder.AppendLine(");");

			sqliteController.ExecuteNonQuery(builder.ToString());
		}

		#endregion ================================================== Private Methods ==================================================




		#region ================================================== Public Methods ==================================================

		public void EnsureSchemaComplete(SQLiteController sqliteController)
		{
			EnsureSchemaComplete0(sqliteController);
		}

		#endregion ================================================== Public Methods ==================================================

	}
}
