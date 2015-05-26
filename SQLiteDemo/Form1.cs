using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SQLiteDemo.Controllers;
using SQLiteDemo.Extensions;

namespace SQLiteDemo
{
	public partial class Form1 : Form
	{

		#region ================================================== Constructor ==================================================

		public Form1()
		{
			InitializeComponent();
		}

		#endregion ================================================== Constructor ==================================================




		#region ================================================== Private Statement Creation Methods ==================================================

		private string GenerateInsertStatement(int count)
		{
			List<string> valueClauses = new List<string>();

			for (int i = 0; i < count; i++)
			{
				valueClauses.Add(
					"('thisisalongerthanaveragedomainname.com', '2014-10-15 15:00:00', '2015-10-15 15:00:00', 'False', '2014-10-15 16:00:00', '10', '10', '10000', '1000', '1000', '1000', 'blah')"
				);
			}

			return String.Format(
				"INSERT INTO Domains (Domain, ScrapeDate, ExpiryDate, Expired, MajDate, MajTrustFlow, MajCitationFlow, MajExtBacklinks, MajRefDomains, MajRefIPs, MajRefSubnets, MajAnchorPercentages) VALUES {0}",
				String.Join(", ", valueClauses)
			);
		}




		private List<string> GenerateInsertStatements(int count)
		{
			List<string> insertStatements = new List<string>();

			for (int i = 0; i < count; i++)
			{
				insertStatements.Add(
					"INSERT INTO Domains (Domain, ScrapeDate, ExpiryDate, Expired, MajDate, MajTrustFlow, MajCitationFlow, MajExtBacklinks, MajRefDomains, MajRefIPs, MajRefSubnets, MajAnchorPercentages) VALUES " + "('thisisalongerthanaveragedomainname.com', '2014-10-15 15:00:00', '2015-10-15 15:00:00', 'False', '2014-10-15 16:00:00', '10', '10', '10000', '1000', '1000', '1000', 'blah')"
				);
			}

			return insertStatements;
		}




		private string GenerateDeleteStatement(List<long> ids)
		{
			List<string> sIDs = ids.Select(x => "" + Convert.ToString(x) + "").ToList();

			return "DELETE FROM Domains WHERE ID IN (" + String.Join(", ", sIDs) + ")";
		}



		
		private List<string> GenerateDeleteStatements(List<long> ids)
		{
			List<string> deleteStatements = new List<string>();

			foreach (int id in ids)
			{
				deleteStatements.Add("DELETE FROM Domains WHERE ID = '" + Convert.ToString(id) + "'");
			}

			return deleteStatements;
		}

		#endregion ================================================== Private Statement Creation Methods ==================================================


	
		
		#region ================================================== Private SQLiteController Test Methods ==================================================
		
		private void DoSingleStatementInsertions(string insertStatement)
		{
			using (SQLiteController sqliteController = new SQLiteController("MyDatabase.hawk", DomainHawkSchemaController.Instance))
			{
				sqliteController.ExecuteNonQuery(insertStatement);
			}
		}



		
		private void DoTransactionInsertions(List<string> insertStatements)
		{
			using (SQLiteController sqliteController = new SQLiteController("MyDatabase.hawk", DomainHawkSchemaController.Instance))
			{
				sqliteController.ExecuteNonQueries(insertStatements);
			}
		}
		

	
		
		private void DoSingleStatementDeletion(string deleteStatement)
		{
			using (SQLiteController sqliteController = new SQLiteController("MyDatabase.hawk", DomainHawkSchemaController.Instance))
			{
				sqliteController.ExecuteNonQuery(deleteStatement);
			}
		}



		
		private void DoTransactionDeletions(List<string> deleteStatements)
		{
			using (SQLiteController sqliteController = new SQLiteController("MyDatabase.hawk", DomainHawkSchemaController.Instance))
			{
				sqliteController.ExecuteNonQueries(deleteStatements);
			}
		}




		private List<long> RetrieveIDs()
		{
			DataTable dataTable = new DataTable();

			using (SQLiteController sqliteController = new SQLiteController("MyDatabase.hawk", DomainHawkSchemaController.Instance))
			{
				dataTable = sqliteController.ExecuteReader("SELECT ID FROM Domains", "Domains");
			}

			return dataTable.Rows.GetValuesFromColumn<long>("ID");
		}




		private void PerformSQLiteControllerTests()
		{
			int baseRowCount = 500;
			int iterations = 100;

			List<string> insertStatementsMultiple = new string[iterations].Select(x => GenerateInsertStatement(baseRowCount)).ToList();
			List<string> insertStatementsSingle   = GenerateInsertStatements(baseRowCount * iterations);
			string       deleteStatement          = null;
			List<string> deleteStatements         = null;
			List<long>   ids = null;




			// TEST 1 - FASTEST
			long ms1 = StopwatchController.Instance.MeasureMilliseconds(() => {
				DoTransactionInsertions(insertStatementsMultiple);
			});

			long ms2 = StopwatchController.Instance.MeasureMilliseconds(() => {
				ids = RetrieveIDs();
			});

			deleteStatement = GenerateDeleteStatement(ids);

			long ms3 = StopwatchController.Instance.MeasureMilliseconds(() => {
				DoSingleStatementDeletion(deleteStatement);
			});

			
			
			
			// TEST 1 - SLOWEST
			long ms4 = StopwatchController.Instance.MeasureMilliseconds(() => {
				DoTransactionInsertions(insertStatementsSingle);
			});

			long ms5 = StopwatchController.Instance.MeasureMilliseconds(() => {
				ids = RetrieveIDs();
			});

			deleteStatements = GenerateDeleteStatements(ids);

			long ms6 = StopwatchController.Instance.MeasureMilliseconds(() => {
				DoTransactionDeletions(deleteStatements);
			});


			
			
			// output
			tbSQLiteControllerTest.Clear();
			tbSQLiteControllerTest.AppendText("DoTransactionInsertions (multi value) : " + ms1 + "\r\n");
			tbSQLiteControllerTest.AppendText("RetrieveIDs : " + ms2 + "\r\n");
			tbSQLiteControllerTest.AppendText("DoSingleStatementDeletion : " + ms3 + "\r\n");
			tbSQLiteControllerTest.AppendText("\r\n");
			tbSQLiteControllerTest.AppendText("DoTransactionInsertions (single value) : " + ms4 + "\r\n");
			tbSQLiteControllerTest.AppendText("RetrieveIDs : " + ms5 + "\r\n");
			tbSQLiteControllerTest.AppendText("DoTransactionDeletions : " + ms6 + "\r\n");
		}

		#endregion ================================================== Private SQLiteController Test Methods ==================================================



		
		#region ================================================== Private DataTable Extensions Test Methods ==================================================

		private void InsertValuesIntoRecord(DataRow drDomain)
		{
			// (Domain, ScrapeDate, ExpiryDate, Expired, MajDate, MajTrustFlow, MajCitationFlow, MajExtBacklinks, MajRefDomains, MajRefIPs, MajRefSubnets, MajAnchorPercentages)
			
			// 'thisisalongerthanaveragedomainname.com', '2014-10-15 15:00:00', '2015-10-15 15:00:00', 'False', '2014-10-15 16:00:00', '10', '10', '10000', '1000', '1000', '1000', 'blah'

			// drDomain["ID"]         = null;
			drDomain["Domain"]               = "thisisalongerthanaveragedomainname.com";
			drDomain["ScrapeDate"]           = DateTime.Parse("2014-10-15 15:00:00").ToYMDHMS();
			drDomain["ExpiryDate"]           = DateTime.Parse("2015-10-15 15:00:00").ToYMDHMS();
			drDomain["Expired"]              = false;
			drDomain["MajDate"]              = DateTime.Parse("2014-10-15 16:00:00").ToYMDHMS();
			drDomain["MajTrustFlow"]         = 10;
			drDomain["MajCitationFlow"]      = 10;
			drDomain["MajExtBacklinks"]      = 10000;
			drDomain["MajRefDomains"]        = 1000;
			drDomain["MajRefIPs"]            = 1000;
			drDomain["MajRefSubnets"]        = 1000;
			drDomain["MajAnchorPercentages"] = "blah";
		}




		private void InsertRecordsIntoTable(DataTable dtDomains, int nrRecords)
		{
			for (int i = 0; i < nrRecords; i++)
			{
				DataRow drDomain = dtDomains.NewRow();
				InsertValuesIntoRecord(drDomain);
				dtDomains.Rows.Add(drDomain);
			}
		}




		private void UpdateRecordsInTable(DataTable dtDomains)
		{
			foreach (DataRow drDomain in dtDomains.Rows)
			{
				drDomain["Domain"] = Guid.NewGuid().ToString();
			}
		}




		private void FilterRowsWithEvenIDs(DataTable dtDomains)
		{
			foreach (DataRow drDomain in dtDomains.Rows)
			{
				if ((long)drDomain["ID"] % 2 == 0)
				{
					drDomain.Delete();
				}
			}
		}




		private void PerformDataTableExtensionsTests()
		{
			tbDataTableExtensionsTest.Clear();

			using (SQLiteController sqliteController = new SQLiteController("MyDatabase.hawk", DomainHawkSchemaController.Instance))
			{
				// delete all
				sqliteController.DeleteAllRecordsFromTable("Domains");

				// get schema
				DataTable dtDomains = sqliteController.GetTableSchema("Domains");

				// output column types
				foreach (DataColumn dataColumn in dtDomains.Columns.ToList())
				{
					tbDataTableExtensionsTest.AppendText(dataColumn.ColumnName + " - " + dataColumn.DataType.Name + "\r\n");
				}

				// insert records into table
				InsertRecordsIntoTable(dtDomains, 50);

				// insert records in table to database
				dtDomains.InsertIntoDatabase("ID", sqliteController);

				// read records back into table from database
				dtDomains = sqliteController.ExecuteReader("SELECT * FROM Domains", "Domains");

				// update records
				UpdateRecordsInTable(dtDomains);

				// update records in table to database
				dtDomains.UpdateToDatabase("ID", new List<string>() { "Domain" }, sqliteController);

				// read records back into table from database
				dtDomains = sqliteController.ExecuteReader("SELECT * FROM Domains", "Domains");

				// filter rows with even ids
				FilterRowsWithEvenIDs(dtDomains);

				// delete records in table from database
				dtDomains.DeleteFromDatabase("ID", sqliteController);
			}
		}

		#endregion ================================================== Private DataTable Extensions Test Methods ==================================================




		#region ================================================== Control Events ==================================================

		private void btnSQLiteControllerTest_Click(object sender, EventArgs e)
		{
			try
			{
				Logger.Instance.LogInfo("Begin SQLiteController Tests.");
				PerformSQLiteControllerTests();
				Logger.Instance.LogInfo("End SQLiteController Tests.");
			}
			catch (Exception exception)
			{
				Logger.Instance.LogException(exception);
				MessageBox.Show(exception.Message);
			}
		}




		private void btnDataTableExtensionsTest_Click(object sender, EventArgs e)
		{
			try
			{
				Logger.Instance.LogInfo("Begin DataTable Extensions Tests.");
				PerformDataTableExtensionsTests();
				Logger.Instance.LogInfo("End DataTable Extensions Tests.");
			}
			catch (Exception exception)
			{
				Logger.Instance.LogException(exception);
				MessageBox.Show(exception.Message);
			}
		}

		#endregion ================================================== Control Events ==================================================
	
	}
}
