using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using SQLiteDemo.Controllers;

namespace SQLiteDemo.Extensions
{
	public static class ExtDataTable
	{

		#region ================================================== Private Common Methods ==================================================

		/// <summary>
		/// Custom converters like these need to be moved into a seperate class/namespace.
		/// </summary>
		private static string DataRowObjectToString(DataRow dataRow, string column)
		{
			Type dataType = dataRow.Table.Columns[column].DataType;

			if (dataRow.RowState == DataRowState.Deleted)
			{
				if (dataType == typeof(DateTime)) return ((DateTime)dataRow[column, DataRowVersion.Original]).ToYMDHMS();

				return Convert.ToString(dataRow[column, DataRowVersion.Original]).Replace("'", "''");
			}
			else
			{
				if (dataType == typeof(DateTime)) return ((DateTime)dataRow[column]).ToYMDHMS();

				return Convert.ToString(dataRow[column]).Replace("'", "''");
			}
		}

		#endregion ================================================== Private Common Methods ==================================================




		#region ================================================== Private Insert Methods ==================================================
		
		private static string CreateColumnsClause(List<string> columnsNoPK, string primaryKey)
		{
			return "(" + String.Join(", ", columnsNoPK) + ")";
		}




		private static string CreateValueSetClause(DataRow dataRow, List<string> columnsNoPK)
		{
			List<string> values = columnsNoPK.Select(x => "'" + DataRowObjectToString(dataRow, x) + "'").ToList();

			return "(" + String.Join(", ", values) + ")";
		}




		private static string CreateValueSetsClause(List<DataRow> dataRows, List<string> columnsNoPK)
		{
			return String.Join(
				", ",
				dataRows.Select(x => CreateValueSetClause(x, columnsNoPK))
			);
		}

		
		
		
		private static string CreateInsertStatement(string tableName, List<DataRow> dataRows, string primaryKey, List<string> columnsNoPK)
		{
			string columnsClause   = CreateColumnsClause(columnsNoPK, primaryKey);
			string valueSetsClause = CreateValueSetsClause(dataRows, columnsNoPK);

			return String.Format("INSERT INTO {0} {1} VALUES {2}", tableName, columnsClause, valueSetsClause);
		}




		private static List<string> CreateInsertStatements(DataTable dataTable, string primaryKey)
		{
			List<string> columnsNoPK = dataTable.Columns.ToList().Where(x => x.ColumnName != primaryKey).Select(x => x.ColumnName).ToList();
			
			List<List<DataRow>> dataRowSets = dataTable.Rows.ToList().Chunk<DataRow>(500).ToList();

			return dataRowSets.Select(x => CreateInsertStatement(dataTable.TableName, x, primaryKey, columnsNoPK)).ToList();
		}

		#endregion ================================================== Private Insert Methods ==================================================




		#region ================================================== Private Update Methods ==================================================
		
		private static string CreateSetClause(DataRow dataRow, string column)
		{
			return String.Format("{0} = '{1}'", column, DataRowObjectToString(dataRow, column));
		}




		private static string CreateSetClauses(DataRow dataRow, List<string> columns)
		{
			return String.Join(
				", ",
				columns.Select(x => CreateSetClause(dataRow, x))
			);
		}




		private static string CreateWhereClause(DataRow dataRow, string primaryKey)
		{
			return String.Format("{0} = '{1}'", primaryKey, DataRowObjectToString(dataRow, primaryKey));
		}




		private static string CreateUpdateStatement(DataRow dataRow, string primaryKey, List<string> columns)
		{
			string tableName   = dataRow.Table.TableName;
			string setClause   = CreateSetClauses(dataRow, columns);
			string whereClause = CreateWhereClause(dataRow, primaryKey);

			return String.Format("UPDATE {0} SET {1} WHERE {2}", tableName, setClause, whereClause);
		}

		#endregion ================================================== Private Update Methods ==================================================




		#region ================================================== Private Delete Methods ==================================================

		private static string CreateKeyClause(DataTable dataTable, string primaryKey)
		{
			List<string> values = dataTable.Rows.ToList()
				.Where(x => x.RowState == DataRowState.Deleted)
				.Select(x => DataRowObjectToString(x, primaryKey)).ToList();

			return "(" + String.Join(", ", values.Select(x => "'" + x + "'")) + ")";
		}



		
		private static string CreateDeleteStatement(DataTable dataTable, string primaryKey)
		{
			string keyClause = CreateKeyClause(dataTable, primaryKey);

			return String.Format("DELETE FROM {0} WHERE {1} IN {2}", dataTable.TableName, primaryKey, keyClause);
		}

		#endregion ================================================== Private Delete Methods ==================================================




		#region ================================================== Public DataTable ==================================================

		/// <summary>
		/// Inserts the datatable to the database.
		/// Takes a primary key name.
		/// Build multiple insert statements, each with multiple value clauses. This gives fast performance.
		/// Uses the table name to figure out which table to insert to.
		/// </summary>
		public static int InsertIntoDatabase(this DataTable dataTable, string primaryKey, SQLiteController sqliteController)
		{
			List<string> insertStatements = CreateInsertStatements(dataTable, primaryKey);

			return sqliteController.ExecuteNonQueries(insertStatements);
		}




		/// <summary>
		/// Updates the datatable to the database.
		/// Takes a primary key name.
		/// Takes a bunch of columns to update. If left empty, simply updates all columns.
		/// Uses the table name to figure out which table to update.
		/// </summary>
		public static int UpdateToDatabase(this DataTable dataTable, string primaryKey, List<string> columns, SQLiteController sqliteController)
		{
			if (columns == null || columns.Count == 0)
			{
				columns = dataTable.Columns.ToList().Select(x => x.ColumnName).ToList();
			}

			List<string> updateStatements = dataTable.Rows.ToList().Select(x => CreateUpdateStatement(x, primaryKey, columns)).ToList();

			return sqliteController.ExecuteNonQueries(updateStatements);
		}




		/// <summary>
		/// Deletes the datatable from the database.
		/// Takes a primary key name.
		/// Deletes multiple records with a single delete statement. This gives fast performance.
		/// Uses the table name to figure out which table to delete from.
		/// </summary>
		public static int DeleteFromDatabase(this DataTable dataTable, string primaryKey, SQLiteController sqliteController)
		{
			string deleteStatement = CreateDeleteStatement(dataTable, primaryKey);

			return sqliteController.ExecuteNonQuery(deleteStatement);
		}

		#endregion ================================================== Public DataTable ==================================================

	}
}