using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;
using ToDoList.Notifications;

namespace ToDoList.Controllers
{
    public class TaskController : BaseController
    {

        // GET: Task
        public ActionResult Index(int Id)
        {
            ViewBag.ListId = Id;
            try
            {
                var todo = db.Todoes.SingleOrDefault(x => x.Id == Id);
                if (todo != null)
                {
                    return View(todo);
                }
                return RedirectToAction("Index", "Todo");
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "Task/Index", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                 return RedirectToAction("Index", "Todo");
    
            }
          
        }

        [HttpPost]
        public JsonResult AddTask(Task task)
        {
            task.CreateDate = DateTime.Now;
            task.IsActive = true;
            try
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "AddTask", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
        }

        [HttpPost]
        public JsonResult DeleteTask(int Id)
        {
            try
            {
                db.Tasks.Single(x => x.Id == Id).IsActive = false;
                db.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "DeleteTask", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
        }

        public ActionResult UpdateTask(int Id)
        {
            var model = db.Tasks.SingleOrDefault(x => x.Id == Id && x.IsActive == true);
            try
            {
                if (model != null)
                {
                    return View(model);
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "UpdateTaskGET", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateTask(Task task)
        {
            try
            {
                var Task = db.Tasks.SingleOrDefault(x => x.Id == task.Id);
                Task.Body = task.Body;
                db.SaveChanges();
                return Json("/Task/Index/" + Task.TodoId);
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "UpdateTaskPOST", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
        }

        public ActionResult AddReminder(int Id)
        {
            try
            {
                var model = db.Tasks.Single(x => x.Id == Id && x.IsActive == true);
                if (model != null)
                {
                    return View(model);
                }
                return RedirectToAction("Index", "Todo");
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "AddReminderGET", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
            

        }
        [HttpPost]
        public ActionResult AddReminder(EmailNotificationModel emailModel)
        {

            Notification notification = new Notification();
            notification.IsActive = true;
            notification.NotificationTime = emailModel.NotificationTime;
            notification.TaskId = emailModel.TaskId;
            notification.NotificationTo = emailModel.EmailAddress;
            try
            {
                db.Notifications.Add(notification);
                db.SaveChanges();
                Scheduler.CreateEmailJob(emailModel);
                return Json("/Task/Index/"+db.Tasks.Single(x=>x.Id==emailModel.TaskId).TodoId);
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "AddReminderPOST", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }

        }
    }
}