using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentScheduler.Model {
	public class ChainUnit {
	   	internal Schedule Schedule { get; private set; }

		public ChainUnit(Schedule schedule)
		{
			Schedule = schedule;
		}
		public TimeUnit AndEvery(int interval) {
			var parent = Schedule.Parent ?? Schedule;

			var child = new Schedule(Schedule.Tasks) {
				Parent = parent,
				Reentrant = parent.Reentrant,
				Name = parent.Name
			};

			parent.AdditionalSchedules.Add(child);

			return child.ToRunEvery(interval);
		}
	}
}
