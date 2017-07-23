using Quartz;
using System;
using System.Net;
using System.Net.Mail;
using ToDoList.Models;

namespace ToDoList.Notifications
{
    public class EmailJob : IJob
    {
       
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                using (var message = new MailMessage("notificationfortodo@gmail.com", dataMap.GetString("EmailAddress")))
                {
                    message.Subject = "You have notification from todo app";
                    message.Body = "Don't forget your task:" + dataMap.GetString("TaskBody");
                    using (SmtpClient client = new SmtpClient
                    {
                        EnableSsl = false,
                        Host = "smtp.gmail.com",
                        Port = 587,
                        Credentials = new NetworkCredential("notificationfortodo@gmail.com", "notificationforto")
                    })
                    {
                        client.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                TodoEntities db = new TodoEntities();
                db.Errors.Add(new Error { MethodName = "Execute", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
          
        }

    }
}