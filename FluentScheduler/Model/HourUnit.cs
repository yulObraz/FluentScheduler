using System;
using FluentScheduler.Extensions;

namespace FluentScheduler.Model
{
	public class HourUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }

		public HourUnit(Schedule schedule, int duration)
		{
			Schedule = schedule;
			Duration = duration;
			if (Duration < 1)
				Duration = 1;
			Schedule.CalculateNextRun = x => {
				var nextRun = x.AddHours(Duration);
				return (x > nextRun) ? nextRun.AddHours(Duration) : nextRun;
			};
		}

		/// <summary>
		/// Schedules the specified task to run at the minute specified.  If the minute has passed, the task will execute the next hour.
		/// </summary>
		/// <param name="minutes">0-59: Represents the minute of the hour</param>
		/// <returns></returns>
		public HourUnit At(int minutes)
		{
			Schedule.CalculateNextRun = x => {
				var nearestCall = x.ClearMinutesAndSeconds().AddMinutes(minutes);
				if(x.Minute - minutes > 30) {
					nearestCall = nearestCall.AddHours(1);
				} else if(minutes - x.Minute> 30) {
					nearestCall = nearestCall.AddHours(-1);
				}
				//var nextRun = x.AddHours(Duration-1).ClearMinutesAndSeconds().AddMinutes(minutes);
				return (x < nearestCall) ? nearestCall : nearestCall.AddHours(Duration);
			};
			return this;
		}
		public void SkipHours(TimeSpan start, TimeSpan end) {
			var old = Schedule.CalculateNextRun;
			Schedule.CalculateNextRun = x => {
				var nextRun = old(x);
				while(start < end ? (nextRun.TimeOfDay < start || nextRun.TimeOfDay > end.Add(new TimeSpan(0,0,0,0,2))) : (nextRun.TimeOfDay < start && nextRun.TimeOfDay > end.Add(new TimeSpan(0,0,0,0,2)))) {
					if(Duration % 24 == 0) {
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
