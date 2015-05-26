using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo.Controllers
{
	public class SQLiteController : IDisposable
	{

		#region ================================================== Private Members ==================================================

		private string dbFileName;
		private string connection;

		#endregion ================================================== Private Members ==================================================




		#region ================================================== Constructor / Deconstructor ==================================================

		public SQLiteController(string dbFileName, ISQLiteSchemaController sqliteSchemaController)
		{
			this.dbFileName = dbFileName;
			
			this.connection = String.Format(
				ConfigurationManager.ConnectionStrings["connectionTemplate"].ConnectionString,
				this.dbFileName
			);

			CreateDatabaseIfNotExists();

			sqliteSchemaController.EnsureSchemaComplete(this);
		}




		~SQLiteController()
		{
			Dispose();
		}




		public void Dispose()
		{
			GC.Collect(); // this helps free the database file
		}

		#endregion ================================================== Constructor / Deconstructor ==================================================




		#region ================================================== Private Database Methods ==================================================

		private void CreateDatabaseIfNotExists()
		{
			if (!File.Exists(dbFileName))
			{
				SQLiteConnection.CreateFile(dbFileName);
			}
		}

		#endregion ================================================== Private Database Methods ==================================================




		#region ================================================== Private Helper Methods ==================================================

		private int _ExecuteNonQueries(SQLiteCommand sqliteCommand, List<string> sqlStatements)
		{
			int affectedRows = 0;

			foreach (string sqlStatement in sqlStatements)
			{
				sqliteCommand.CommandText = sqlStatement;
				int _affectedRows = sqliteCommand.ExecuteNonQuery();
				affectedRows += (_affectedRows == -1) ? 0 : _affectedRows;
			}

			return affectedRows;
		}

		#endregion ================================================== Private Helper Methods ==================================================



		
		#region ================================================== Public Non Query Methods ==================================================

		public int ExecuteNonQueries(List<string> sqlStatements)
		{
			int affectedRows = 0;

			using (SQLiteConnection sqliteConnection = new SQLiteConnection(this.connection))
			using (SQLiteCommand    sqliteCommand    = new SQLiteCommand("", sqliteConnection))
			{
				sqliteConnection.Open();
				
				using (SQLiteTransaction sqliteTransaction = sqliteConnection.BeginTransaction())
				{
					try
					{
						int _affectedRows = _ExecuteNonQueries(sqliteCommand, sqlStatements);

						affectedRows = (_affectedRows == -1) ? 0 : _affectedRows;

						sqliteTransaction.Commit();
					}
					catch (Exception exception)
					{
						sqliteTransaction.Rollback();
						Logger.Instance.LogException(exception);
						throw;
					}
				}
			}

			return affectedRows;
		}




		public int ExecuteNonQuery(string sqlStatement)
		{
			return ExecuteNonQueries(new List<string>() { sqlStatement });
		}

		#endregion ================================================== Public Non Query Methods ==================================================




		#region ================================================== Public Reader Methods ==================================================
		
		public DataTable ExecuteReader(string sqlStatement, string tableName)
		{
			DataTable dataTable = new DataTable();

			using (SQLiteConnection  sqliteConnection  = new SQLiteConnection(this.connection))
			using (SQLiteCommand     sqliteCommand     = new SQLiteCommand(sqlStatement, sqliteConnection))
			using (SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(sqliteCommand))
			{
				sqliteConnection.Open();
				sqliteDataAdapter.Fill(dataTable);
				dataTable.TableName = tableName; // SQLiteDataAdapter does not set this itself
			}

			return dataTable;
		}

		#endregion ================================================== Public Reader Methods ==================================================




		#region ================================================== Public Scalar Methods ==================================================

		public T ExecuteScalar<T>(string sqlStatement)
		{
			T scalar = default(T);

			using (SQLiteConnection sqliteConnection = new SQLiteConnection(this.connection))
			using (SQLiteCommand    sqliteCommand    = new SQLiteCommand(sqlStatement, sqliteConnection))
			{
				sqliteConnection.Open();
				scalar = (T)sqliteCommand.ExecuteScalar();
			}

			return scalar;
		}

		#endregion ================================================== Public Scalar Methods ==================================================




		#region ================================================== Public Convenience Methods ==================================================

		public bool TableExists(string tableName)
		{
			string sqlStatement = String.Format("SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{0}';", tableName);
			string scalar = ExecuteScalar<string>(sqlStatement);
			return tableName == scalar;
		}




		public DataTable GetTableInfo(string tableName)
		{
			string sqlStatement = String.Format("PRAGMA table_info({0});", tableName);
			return ExecuteReader(sqlStatement, tableName);
		}




		public int DeleteAllRecordsFromTable(string tableName)
		{
			string sqlStatement = String.Format("DELETE FROM {0}", tableName);
			return ExecuteNonQuery(sqlStatement);
		}




		public DataTable GetTableSchema(string tableName)
		{
			string sqlStatement = String.Format("SELECT * FROM {0} LIMIT 0", tableName); // select nothing
			return ExecuteReader(sqlStatement, tableName); // return empty table
		}

		#endregion ================================================== Public Convenience Methods ==================================================

	}
}
