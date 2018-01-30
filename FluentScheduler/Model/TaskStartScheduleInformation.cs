using System;

namespace FluentScheduler.Model
{
	public class TaskStartScheduleInformation
	{
		public Schedule Schedule { get; set; }
		public string Name { get; set; }
		public DateTime StartTime { get; set; }
	}
}
