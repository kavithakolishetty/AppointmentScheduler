using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Repository.Interface
{
    public interface IAppointmentRepository
    {
        void AddAppointment(DateTime appointmentDateTime);
        bool DeleteAppointment(DateTime appointmentDateTime);
        List<DateTime> GetAppointmentsForDay(DateTime date);
        bool AppointmentExists(DateTime appointmentDateTime);
    }
}
