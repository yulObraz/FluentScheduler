using System;
using FluentScheduler.Model;

namespace FluentScheduler
{
	/// <summary>
	/// Extensions for RunAfter() functionality
	/// </summary>
	public static class RunAfterExtensions
	{
		public static RunAfterUnit RunAfter(this Schedule schedule, DateTime date)
		{
			return new RunAfterUnit(schedule, date);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time date.
		/// </summary>
		public static RunAfterUnit RunAfter(this SpecificRunTime runTime, DateTime date)
		{
			return RunAfter(runTime.Schedule, date);
		}

		/// <summary>
		/// Delay first execution of the task for the specified time date.
		/// </summary>
		public static RunAfterUnit RunAfter(this SecondUnit timeUnit, DateTime date)
		{
			return RunAfter(timeUnit.Schedule, date);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time date.
		/// </summary>
		public static RunAfterUnit RunAfter(this MinuteUnit timeUnit, DateTime date)
		{
			return RunAfter(timeUnit.Schedule, date);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time date.
		/// </summary>
		public static RunAfterUnit RunAfter(this HourUnit timeUnit, DateTime date)
		{
			return RunAfter(timeUnit.Schedule, date);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time date.
		/// </summary>
		public static RunAfterUnit RunAfter(this DayUnit timeUnit, DateTime date)
		{
			return RunAfter(timeUnit.Schedule, date);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time date.
		/// </summary>
		public static RunAfterUnit RunAfter(this WeekUnit timeUnit, DateTime date)
		{
			return RunAfter(timeUnit.Schedule, date);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time date.
		/// </summary>
		public static RunAfterUnit RunAfter(this MonthUnit timeUnit, DateTime date)
		{
			return RunAfter(timeUnit.Schedule, date);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time date.
		/// </summary>
		public static RunAfterUnit RunAfter(this YearUnit timeUnit, DateTime date)
		{
			return RunAfter(timeUnit.Schedule, date);
		}
		public static RunAfterUnit RunAfter(this ChainUnit timeUnit, DateTime date) {
			return RunAfter(timeUnit.Schedule, date);
		}
	}
}
