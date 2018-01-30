using System;
using System.Linq;

namespace FluentScheduler.Model
{
	public class RunAfterUnit
	{
		internal Schedule Schedule { get; private set; }
		internal DateTime Date { get; private set; }
		internal Func<DateTime, DateTime> Old { get; private set; }
		public RunAfterUnit(Schedule schedule, DateTime date)
		{
			Schedule = schedule;
			Date = date;
			Old = schedule.CalculateNextRun;
			Schedule.CalculateNextRun = x => {
				if(Old == null) {
					return x > Date ? DateTime.MaxValue : Date;
				}
				var current = Date.AddMilliseconds(-1);//defense from cycling
				var next = Old(current);
				
				while(next < x && next > current) {
					current = next.AddMilliseconds(1);
					next = Old(current);
				}
				schedule.CalculateNextRun = Old;//only first time call
				return next;
			};
			Schedule.AdditionalSchedules.ToList().ForEach(it => new RunAfterUnit(it, date));
		}
	}
}
