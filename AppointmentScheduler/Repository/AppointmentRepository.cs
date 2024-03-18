using AppointmentScheduler.DBContext;
using AppointmentScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AppointmentScheduler.Repository.Interface;

namespace AppointmentScheduler.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;

        public AppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        public void AddAppointment(DateTime appointmentDateTime)
        {
            _context.Appointments.Add(new Appointment { AppointmentDateTime = appointmentDateTime });
            _context.SaveChanges();
        }

        public bool DeleteAppointment(DateTime appointmentDateTime)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentDateTime == appointmentDateTime);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<DateTime> GetAppointmentsForDay(DateTime date)
        {
            return _context.Appointments
                .Where(a => EF.Functions.DateDiffDay(a.AppointmentDateTime.Date, date.Date) == 0)
                .Select(a => a.AppointmentDateTime)
                .ToList();
        }

        public bool AppointmentExists(DateTime appointmentDateTime)
        {
            return _context.Appointments.Any(a => a.AppointmentDateTime == appointmentDateTime);
        }
    }
}
