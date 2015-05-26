using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo.Controllers
{
	public class StopwatchController
	{

		#region ================================================== Singleton Stuff ==================================================

		private StopwatchController()
		{
		}

		private static StopwatchController instance;
		public  static StopwatchController Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new StopwatchController();
				}

				return instance;
			}
		}

		#endregion ================================================== Singleton Stuff ==================================================



		
		#region ================================================== Public Methods ==================================================

		public long MeasureMilliseconds(Action action)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			action();

			stopwatch.Stop();
			
			return stopwatch.ElapsedMilliseconds;
		}

		#endregion ================================================== Public Methods ==================================================

	}
}
