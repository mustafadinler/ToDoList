using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoList.Models;

namespace ToDoList.Notifications
{
    public static class Scheduler
    {
        public static StdSchedulerFactory _SchedulerFactory;
        public static IScheduler _Scheduler;

        public static void Start()
        {
            _SchedulerFactory = new StdSchedulerFactory();
            _Scheduler = _SchedulerFactory.GetScheduler();
            _Scheduler.Start();
        }

        public static void CreateEmailJob(EmailNotificationModel emailModel)
        {
            try
            {
                IJobDetail job = JobBuilder.Create<EmailJob>()
                                                      .UsingJobData(nameof(emailModel.EmailAddress), emailModel.EmailAddress)
                                                      .UsingJobData(nameof(emailModel.TaskBody), emailModel.TaskBody)
                                                      .Build();

                ITrigger trigger = TriggerBuilder.Create()
                                                .StartAt(emailModel.NotificationTime.ToUniversalTime())
                                                .ForJob(job)
                                                .Build();
                _Scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                TodoEntities db = new TodoEntities();
                db.Errors.Add(new Error { MethodName = "CreateEmailJob", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
           
        }
    }
}