
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using AppointmentScheduler.Repository;
using AppointmentScheduler.Repository.Interface;
using AppointmentScheduler.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection();
            ConfigureServices(serviceProvider);
            serviceProvider.AddSingleton<Executor, Executor>()
                .BuildServiceProvider()
                .GetService<Executor>()
                .Execute();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DBContext.AppointmentDbContext>(options =>
                    options.UseSqlServer(ConfigurationManager.ConnectionStrings["AppointmentDbContext"].ConnectionString))
                .AddSingleton<IAppointmentRepository, AppointmentRepository>()
                .AddSingleton<IAppointmentService, Services.AppointmentService>();
        }
    }
}