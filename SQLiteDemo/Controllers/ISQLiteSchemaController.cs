using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo.Controllers
{
	public interface ISQLiteSchemaController
	{
		void EnsureSchemaComplete(SQLiteController sqliteController);
	}
}
