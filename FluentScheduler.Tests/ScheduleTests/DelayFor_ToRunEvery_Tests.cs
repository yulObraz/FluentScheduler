using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class DelayFor_ToRunEvery_Tests
	{
		[Test]
		public void Should_Delay_ToRunEvery_For_2_Seconds()
		{
			TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Seconds").ToRunEvery(10).Seconds().DelayFor(2).Seconds());
			DateTime expectedTime = DateTime.Now.AddSeconds(12);

			DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Seconds").NextRunTime;

			Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
		}
		[Test]
		public void Should_Delay_ToRunEvery_For_2_Minutes()
		{
			TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Minutes").ToRunEvery(10).Seconds().DelayFor(2).Minutes());
			DateTime expectedTime = DateTime.Now.AddSeconds(10).AddMinutes(2);

			DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Minutes").NextRunTime;

			Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
		}
		[Test]
		public void Should_Delay_ToRunEvery_For_2_Hours()
		{
			TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Hours").ToRunEvery(10).Seconds().DelayFor(2).Hours());
			DateTime expectedTime = DateTime.Now.AddSeconds(10).AddHours(2);

			DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Hours").NextRunTime;

			Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
		}
		[Test]
		public void Should_Delay_ToRunEvery_For_2_Days()
		{
			TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Days").ToRunEvery(10).Seconds().DelayFor(2).Days());
			DateTime expectedTime = DateTime.Now.AddSeconds(10).AddDays(2);

			DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Days").NextRunTime;

			Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
		}
		[Test]
		public void Should_Delay_ToRunEvery_For_2_Weeks()
		{
			TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Weeks").ToRunEvery(10).Seconds().DelayFor(2).Weeks());
			DateTime expectedTime = DateTime.Now.AddSeconds(10).AddDays(14);

			DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Weeks").NextRunTime;

			Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
		}
		[Test]
		public void Should_Delay_ToRunEvery_For_2_Months()
		{
			TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Months").ToRunEvery(10).Seconds().DelayFor(2).Months());
			DateTime expectedTime = DateTime.Now.AddSeconds(10).AddMonths(2);

			DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Months").NextRunTime;

			Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
		}
		[Test]
		public void Should_Delay_ToRunEvery_For_2_Years()
		{
			TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Years").ToRunEvery(10).Seconds().DelayFor(2).Years());
			DateTime expectedTime = DateTime.Now.AddSeconds(10).AddYears(2);

			DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Years").NextRunTime;

			Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
		}
		[Test]
		public void Should_RunAfter_Future() {
			string name = "RunAfter";
			TaskManager.AddTask(() => { }, x => {
				x.WithName(name).ToRunEvery(5).Hours().At(30).SkipHours(new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0));
				x.RunAfter(DateTime.Now.AddDays(2).Add(new TimeSpan(18, 15, 0)).Subtract(DateTime.Now.TimeOfDay));
			});
			//DateTime expectedTime = DateTime.Now.AddYears(2);

			DateTime actualTime = TaskManager.GetSchedule(name).NextRunTime;
			Assert.AreEqual(DateTime.Now.AddDays(2).Date, actualTime.Date);
			Assert.AreEqual(actualTime.Hour, 18);
			TaskManager.RemoveTask(name);
		}
		[Test]
		public void Should_RunAfter_Past() {
			string name = "RunAfter";
			TaskManager.AddTask(() => { }, x => {
				x.WithName(name).ToRunEvery(12).Hours().At(30).SkipHours(new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0));
				x.RunAfter(DateTime.Now.Subtract(DateTime.Now.TimeOfDay).Add(new TimeSpan(1,1,0)));
			});

			DateTime actualTime = TaskManager.GetSchedule(name).NextRunTime;
			if(DateTime.Now.TimeOfDay< new TimeSpan(13,31,0))
				Assert.AreEqual(DateTime.Now.Date, actualTime.Date);
			else
				Assert.AreEqual(DateTime.Now.AddDays(1).Date, actualTime.Date);
			Assert.AreEqual(actualTime.Hour, 13);
			TaskManager.RemoveTask(name);
		}

	}
}
