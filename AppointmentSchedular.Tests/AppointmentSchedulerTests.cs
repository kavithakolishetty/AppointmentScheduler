using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using AppointmentScheduler.Repository.Interface;

namespace AppointmentScheduler.Tests
{
    [TestFixture]
    public class AppointmentSchedulerTests
    {
        [Test]
        public void AddAppointment_PastDate_ReturnsErrorMessage()
        {
            // Arrange
            var appointmentRepository = new Mock<IAppointmentRepository>();
            var scheduler = new Services.AppointmentService(appointmentRepository.Object);
            var pastDate = DateTime.Today.AddDays(-1).ToString("dd/MM");
            var time = "10:00";

            // Act
            scheduler.AddAppointment(pastDate, time);

            // Assert
            appointmentRepository.Verify(repo => repo.AddAppointment(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void AddAppointment_SecondDayOfThirdWeek_ReturnsErrorMessage()
        {
            // Arrange
            var appointmentRepository = new Mock<IAppointmentRepository>();
            var scheduler = new Services.AppointmentService(appointmentRepository.Object);         
            var secondDayOfThirdWeek = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 15).AddDays(1);
            var date = secondDayOfThirdWeek.ToString("dd/MM");
            var time = "16:00";

            // Act
            scheduler.AddAppointment(date, time);

            // Assert
            appointmentRepository.Verify(repo => repo.AddAppointment(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void DeleteAppointment_ExistingAppointment_DeletesAppointment()
        {
            // Arrange
            var appointmentRepository = new Mock<IAppointmentRepository>();
            var scheduler = new Services.AppointmentService(appointmentRepository.Object);
            var date = DateTime.Today.ToString("dd/MM");
            var time = "10:00";
            appointmentRepository.Setup(repo => repo.DeleteAppointment(It.IsAny<DateTime>())).Returns(true);

            // Act
            scheduler.DeleteAppointment(date, time);

            // Assert
            appointmentRepository.Verify(repo => repo.DeleteAppointment(It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void FindFreeTimeSlots_NoAppointments_ReturnsFullDay()
        {
            // Arrange
            var appointmentRepository = new Mock<IAppointmentRepository>();
            var scheduler = new Services.AppointmentService(appointmentRepository.Object);
            var date = DateTime.Today.ToString("dd/MM");
            appointmentRepository.Setup(repo => repo.GetAppointmentsForDay(It.IsAny<DateTime>())).Returns(new List<DateTime>());

            // Act
            scheduler.FindFreeTimeSlots(date);

            // Assert
            appointmentRepository.Verify(repo => repo.GetAppointmentsForDay(It.IsAny<DateTime>()), Times.Once);
        }
       
    }
}
