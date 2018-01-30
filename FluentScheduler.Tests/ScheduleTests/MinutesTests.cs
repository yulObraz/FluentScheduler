using System;
using System.Linq;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class MinutesTests
	{
		[Test]
		public void Should_Add_Specified_Minutes_To_Next_Run_Date()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(30).Minutes();

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, input.Hour);
			Assert.AreEqual(scheduledTime.Minute, 30);
			Assert.AreEqual(scheduledTime.Second, input.Second);
		}
		[Test]
		public void Should_All_Skip_Hours() {
			var task = new Mock<ITask>();
			var startTime = new TimeSpan(9, 0, 0);
			var endTime = new TimeSpan(21, 0, 0);
			var schedule1 = new Schedule(task.Object);
			schedule1.ToRunEvery(11).Minutes().SkipHours(startTime, endTime);
			schedule1.RunAfter(DateTime.Now.Date.AddHours(1).AddMinutes(25));
			var scheduledTime1 = schedule1.CalculateNextRun(DateTime.Now.Date.AddHours(2));
			Assert.True(scheduledTime1.Hour >= 9);
			Assert.True(scheduledTime1.Minute < 11, "First run after pause");
			Assert.True(scheduledTime1.Hour <= 21);
			var schedule2 = new Schedule(task.Object);
			schedule2.ToRunAt(DateTime.Now.Date.AddHours(1).AddMinutes(30)).AndEvery(30).Minutes().SkipHours(startTime, endTime);
			schedule2.RunAfter(DateTime.Now.Date.AddHours(1).AddMinutes(30));
			var scheduledTime2 = schedule2.AdditionalSchedules.First().CalculateNextRun(DateTime.Now.Date.AddHours(2));
			Assert.True(scheduledTime2.Hour >= 9);
			Assert.True(scheduledTime2.Minute == 0, "First run after pause in exact time "+ scheduledTime2);
			Assert.True(scheduledTime2.Hour <= 21);

			var schedule = new Schedule(task.Object);
			schedule.ToRunAt(DateTime.Now.Date.AddHours(12).AddMinutes(25)).AndEvery(11).Minutes().SkipHours(startTime, endTime);
			schedule.WithName("Something");
			schedule.RunAfter(DateTime.Now.Date.AddHours(12).AddMinutes(25));
			var testTime=schedule.AdditionalSchedules.First().CalculateNextRun(DateTime.Now.Date.AddHours(1));
			Assert.True(testTime.Hour >= 12);
			for(int i = 0; i < 24; i++) {
				schedule.RunAfter(DateTime.Now.Date.AddHours(12).AddMinutes(25));//Is removed after first call
				var scheduledTime = new DateTime[] { schedule.CalculateNextRun(DateTime.Now.Date.AddHours(i)), schedule.AdditionalSchedules.First().CalculateNextRun(DateTime.Now.Date.AddHours(i)) }.Min();
				Assert.True(scheduledTime.Hour >= 9);
				Assert.True(scheduledTime.Hour <= 21);
			}
		}
	}
}
