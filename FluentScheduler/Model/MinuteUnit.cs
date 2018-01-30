using System;
using FluentScheduler.Extensions;

namespace FluentScheduler.Model
{
	public class MinuteUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }

		public MinuteUnit(Schedule schedule, int duration)
		{
			Schedule = schedule;
			Duration = duration;

			Schedule.CalculateNextRun = x => x.AddMinutes(Duration);
		}
		public void SkipHours(TimeSpan start, TimeSpan end) {
			if(start.TotalSeconds < 0) {
				start = start.Add(new TimeSpan(1,0,0,0));
			}
			if(end.TotalSeconds < 0) {
				end = end.Add(new TimeSpan(1,0,0,0));
			}
			var old = Schedule.CalculateNextRun;
			Schedule.CalculateNextRun = x => {
				var nextRun = old(x);
				while(start < end ? (nextRun.TimeOfDay < start || nextRun.TimeOfDay > end.Add(new TimeSpan(0, 0, 0, 0, 2))) : (nextRun.TimeOfDay < start && nextRun.TimeOfDay > end.Add(new TimeSpan(0, 0, 0, 0, 2)))) {
					if(Duration % (24 * 60) == 0) {
						nextRun = nextRun.AddHours(1);
					} else {
						nextRun = old(nextRun.AddMilliseconds(1));
					}
				}
				return nextRun;
			};
		}
	}
}
