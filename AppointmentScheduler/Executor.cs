using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentScheduler.Services.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentScheduler
{
    public class Executor
    {
        private readonly IAppointmentService _appointmentService;
        public Executor(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public void Execute()
        {
            while (true)
            {
                Console.WriteLine("Enter command (ADD, DELETE, FIND, KEEP, EXIT):");
                string command = Console.ReadLine()?.ToUpper();

                switch (command)
                {
                    case "ADD":
                        Console.WriteLine("Enter date (DD/MM):");
                        string dateToAdd = Console.ReadLine();
                        Console.WriteLine("Enter time in (hh:mm):");
                        string timeToAdd = Console.ReadLine();
                        _appointmentService.AddAppointment(dateToAdd, timeToAdd);
                        break;
                    case "DELETE":
                        Console.WriteLine("Enter date (DD/MM):");
                        string dateToDelete = Console.ReadLine();
                        Console.WriteLine("Enter time in (hh:mm):");
                        string timeToDelete = Console.ReadLine();
                        _appointmentService.DeleteAppointment(dateToDelete, timeToDelete);
                        break;
                    case "FIND":
                        Console.WriteLine("Enter date (DD/MM):");
                        string dateToFind = Console.ReadLine();
                        _appointmentService.FindFreeTimeSlots(dateToFind);
                        break;
                    case "KEEP":
                        Console.WriteLine("Enter time in (hh:mm):");
                        string timeToKeep = Console.ReadLine();
                        _appointmentService.KeepTimeSlot(timeToKeep);
                        break;
                    case "EXIT":
                        Console.WriteLine("Exiting the application...");
                        return;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
