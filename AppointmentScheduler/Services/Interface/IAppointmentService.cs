using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Services.Interface
{
    public interface IAppointmentService
    {
        void AddAppointment(string date, string time);
        void DeleteAppointment(string date, string time);
        void FindFreeTimeSlots(string date);
        void KeepTimeSlot(string time);
    }
}
