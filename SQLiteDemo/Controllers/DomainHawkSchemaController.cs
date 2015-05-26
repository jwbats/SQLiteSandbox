using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo.Controllers
{
	public class DomainHawkSchemaController : ISQLiteSchemaController
	{

		#region ================================================== Singleton Stuff ==================================================

		private DomainHawkSchemaController()
		{
		}

		private static DomainHawkSchemaController instance;
		public  static DomainHawkSchemaController Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DomainHawkSchemaController();
				}

				return instance;
			}
		}

		#endregion ================================================== Singleton Stuff ==================================================




		#region ================================================== Private Methods ==================================================

		private void EnsureSchemaComplete0(SQLiteController sqliteController)
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine("CREATE TABLE IF NOT EXISTS Domains(");
			builder.AppendLine("ID						INTEGER PRIMARY KEY ASC AUTOINCREMENT,");
			builder.AppendLine("Domain					TEXT NOT NULL,");
			builder.AppendLine("ScrapeDate				DATETIME NOT NULL,");
			builder.AppendLine("ExpiryDate				DATETIME NOT NULL,");
			builder.AppendLine("Expired					BOOLEAN NOT NULL,");
			builder.AppendLine("MajDate					DATETIME,");
			builder.AppendLine("MajTrustFlow			TINYINT,");
			builder.AppendLine("MajCitationFlow			TINYINT,");
			builder.AppendLine("MajExtBacklinks			INTEGER,");
			builder.AppendLine("MajRefDomains			INTEGER,");
			builder.AppendLine("MajRefIPs				INTEGER,");
			builder.AppendLine("MajRefSubnets			INTEGER,");
			builder.AppendLine("MajAnchorPercentages	TEXT");
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
