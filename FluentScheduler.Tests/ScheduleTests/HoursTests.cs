using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class HoursTests
	{
		[Test]
		public void Should_Add_Specified_Hours_To_Next_Run_Date()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Hours();

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);

			Assert.AreEqual(scheduledTime.Date, input.Date);
			Assert.AreEqual(scheduledTime.Hour, 2);
			Assert.AreEqual(scheduledTime.Minute, 0);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Set_To_Next_Interval_If_Inputted_Time_Is_After_Run_Time_By_A_Millisecond()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(1).Hours().At(30);

			var input = new DateTime(2000, 1, 1, 5, 30, 0).AddMilliseconds(1);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, 6);
			Assert.AreEqual(scheduledTime.Minute, 30);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Set_Specific_Minute_If_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Hours().At(30);

			var input = new DateTime(2000, 1, 1, 2, 0, 0);//Every two hours includes 0:30
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, 2);
			Assert.AreEqual(scheduledTime.Minute, 30);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Hours().At(15);

			var input = new DateTime(2000, 1, 1, 5, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, 7);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Pick_Next_Interval_If_Specified_Time_Is_After_Specified_At_Minutes()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(1).Hours().At(15);

			var input = new DateTime(2000, 1, 1, 5, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, 6);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Pick_Next_Interval_If_Specified_Time_Is_After_Specified_At_Minutes2()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Hours().At(15);

			var input = new DateTime(2000, 1, 1, 5, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, 8);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Pick_Current_Hour_If_Specified_Time_Is_Before_Specified_At_Minutes()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(1).Hours().At(15);

			var input = new DateTime(2000, 1, 1, 5, 14, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, 5);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Pick_Current_Hour_If_Specified_Time_Is_Before_Specified_At_Minutes2()
		{//suppose running of next time calculation is in 15 minutes of get next time call
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Hours().At(15);

			var input = new DateTime(2000, 1, 1, 5, 14, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, 5);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}
		[Test]
		public void Should_Pick_Current_Hour_If_Specified_Time_Interval_Is_Less_Then_Fifteen_Minutes() {//suppose running of next time calculation is in 15 minutes of get next time call
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Hours().At(55);

			var input = new DateTime(2000, 1, 1, 5, 05, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, 7);
			Assert.AreEqual(scheduledTime.Minute, 55);
			Assert.AreEqual(scheduledTime.Second, 0);
		}
		[Test]
		public void Should_Skip_Hours() {
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			var startTime = new TimeSpan(9, 0, 0);
			var endTime = new TimeSpan(21, 0, 0);
			schedule.ToRunEvery(3).Hours().At(15).SkipHours(startTime, endTime);

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input.Add(startTime).AddHours(-4));
			Assert.AreEqual(startTime.Add(new TimeSpan(2, 15, 0)), scheduledTime.TimeOfDay);
		}
		[Test]
		public void Should_All_Skip_Hours() {
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			var startTime = new TimeSpan(9, 0, 0);
			var endTime = new TimeSpan(21, 0, 0);
			schedule.ToRunEvery(3).Hours().At(15).SkipHours(startTime, endTime);
			for(int i = 0;i < 24;i++) {
				var scheduledTime = schedule.CalculateNextRun(DateTime.Now.Date.AddHours(i));
				Assert.True(scheduledTime.Hour >= 9);
				Assert.True(scheduledTime.Hour <= 21);
			}
		}
	}
}
