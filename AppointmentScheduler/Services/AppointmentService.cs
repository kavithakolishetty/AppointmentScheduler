using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentScheduler.Repository.Interface;
using AppointmentScheduler.Services.Interface;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public void AddAppointment(string date, string time)
        {
            try
            {
                DateTime appointmentDateTime = ParseDateTime(date, time);

                // Check if the appointment date is less than today's date
                if (appointmentDateTime.Date < DateTime.Today)
                {
                    Console.WriteLine("Cannot add appointment for a past date.");
                    return;
                }

                // Check if the slot falls between 4 PM and 5 PM on each second day of the third week
                if (appointmentDateTime.Hour >= 16 && appointmentDateTime.Minute >= 0 && IsSecondDayOfThirdWeek(appointmentDateTime))
                {
                    Console.WriteLine("Cannot add appointment between 4 PM and 5 PM on each second day of the third week.");
                    return;
                }

                // Check if appointment already exists for the same date and time
                if (_appointmentRepository.AppointmentExists(appointmentDateTime))
                {
                    Console.WriteLine("This slot is not available.");
                    return;
                }

                _appointmentRepository.AddAppointment(appointmentDateTime);
                Console.WriteLine("Appointment added successfully.");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }



        public void DeleteAppointment(string date, string time)
        {
            try
            {
                DateTime appointmentDateTime = ParseDateTime(date, time);
                bool deleted = _appointmentRepository.DeleteAppointment(appointmentDateTime);
                if (deleted)
                    Console.WriteLine("Appointment deleted successfully.");
                else
                    Console.WriteLine("Appointment not found. Unable to delete.");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void FindFreeTimeSlots(string date)
        {
            DateTime inputDate = ParseDate(date);
            List<DateTime> appointments = _appointmentRepository.GetAppointmentsForDay(inputDate);
            List<DateTime> freeSlots = FindFreeSlots(inputDate, appointments);
            PrintFreeTimeSlots(freeSlots, inputDate);
        }

        public void KeepTimeSlot(string time)
        {
            try
            {
                TimeSpan slotTime = ParseTime(time);
                DateTime currentDate = DateTime.Today;

                // Check if the slot falls between 4 PM and 5 PM on each second day of the third week
                if (slotTime.Hours >= 16 && slotTime.Minutes >= 0 && IsSecondDayOfThirdWeek(currentDate))
                {
                    Console.WriteLine("Cannot keep time slot between 4 PM and 5 PM on each second day of the third week.");
                    return;
                }

                // Keep searching for available slots including today
                int daysToSearch = 0;
                while (true)
                {
                    DateTime searchDate = currentDate.AddDays(daysToSearch);
                    DateTime slotToKeep = searchDate.Date.Add(slotTime);

                    // Check if the slot already exists
                    List<DateTime> appointments = _appointmentRepository.GetAppointmentsForDay(searchDate);
                    if (!appointments.Contains(slotToKeep))
                    {
                        // Slot is available, add appointment and return
                        _appointmentRepository.AddAppointment(slotToKeep);
                        Console.WriteLine($"Time slot kept successfully on {searchDate.ToShortDateString()}.");
                        return;
                    }

                    daysToSearch++; // Move to the next day
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }


        private bool IsSecondDayOfThirdWeek(DateTime date)
        {
            // Calculate the start date of the third week
            DateTime startDateOfThirdWeek = new DateTime(date.Year, date.Month, 15);

            // Check if the given date falls within the third week and is the second day
            return date >= startDateOfThirdWeek && date < startDateOfThirdWeek.AddDays(7) && date.DayOfWeek == DayOfWeek.Tuesday;
        }
        private DateTime ParseDateTime(string date, string time)
        {
            DateTime appointmentDate = ParseDate(date);
            TimeSpan appointmentTime = ParseTime(time);
            return CombineDateTime(appointmentDate, appointmentTime);
        }

        private DateTime ParseDate(string date)
        {
            return DateTime.ParseExact(date, "dd/MM", CultureInfo.InvariantCulture);
        }


        private TimeSpan ParseTime(string time)
        {
            // Convert to 24-hour format and get the time with AM/PM
            string timeWithAMPM = ConvertTo12hrWithAMPM(time);

            // Parse the time with AM/PM format
            if (!DateTime.TryParseExact(timeWithAMPM, "h\\:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedTime))
            {
                throw new ArgumentException("Invalid time format. Time should be in HH:mm format.");
            }

            // Extract hours and minutes from the parsed time
            int hours = parsedTime.Hour;
            int minutes = parsedTime.Minute;

            // Validate hours
            if (hours < 9 || hours >= 17)
            {
                throw new ArgumentException("Please select appointment time between 9AM to 5PM only.");
            }

            // Validate minutes
            if (minutes != 0 && minutes != 30)
            {
                throw new ArgumentException("Appointment time should have minutes either 00 or 30.");
            }

            return parsedTime.TimeOfDay;
        }

        private string ConvertTo12hrWithAMPM(string time)
        {
            // Parse the time into hours and minutes
            string[] parts = time.Split(':');
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);

            // Determine if it's AM or PM
            string meridian = (hours >= 9 && hours < 12) ? "AM" : "PM";

            // Return the time in 12-hour format with AM/PM
            return $"{hours:D2}:{minutes:D2} {meridian}";
        }


        private DateTime CombineDateTime(DateTime date, TimeSpan time)
        {
            return date.Date.Add(time);
        }


        private List<DateTime> FindFreeSlots(DateTime date, List<DateTime> appointments)
        {
            TimeSpan startTime = new TimeSpan(9, 0, 0);
            TimeSpan endTime = new TimeSpan(17, 0, 0);

            if (IsSecondDayOfThirdWeek(date))
                endTime = new TimeSpan(16, 0, 0);

            List<DateTime> freeSlots = new List<DateTime>();

            for (TimeSpan slotStart = startTime; slotStart < endTime; slotStart = slotStart.Add(TimeSpan.FromMinutes(30)))
            {
                DateTime slot = date.Add(slotStart);
                if (!appointments.Contains(slot))
                {
                    freeSlots.Add(slot);
                }
            }
            return freeSlots;
        }

        private void PrintFreeTimeSlots(List<DateTime> freeSlots, DateTime date)
        {
            if (freeSlots.Count == 0)
            {
                Console.WriteLine("No free time slots available for the specified date.");
            }
            else
            {
                Console.WriteLine("Free time slots for " + date.ToString("dd/MM/yyyy") + ":");
                foreach (var slot in freeSlots)
                {
                    Console.WriteLine(slot.ToString("HH:mm"));
                }
            }
        }
    }
}
