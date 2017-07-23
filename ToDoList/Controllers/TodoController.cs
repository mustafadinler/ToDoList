using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TodoController : BaseController
    {

        public ActionResult Index()
        {
            return View(db.Todoes.Where(x => x.IsActive == true).ToList());
        }

        [HttpPost]
        public JsonResult AddList(Todo todo)
        {
            todo.CreateDate = DateTime.Now;
            todo.IsActive = true;
            try
            {
                db.Todoes.Add(todo);
                db.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "AddList", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
        }

        [HttpPost]
        public JsonResult DeleteList(int Id)
        {
            try
            {
                db.Todoes.Single(x => x.Id == Id).IsActive = false;
                db.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "DeleteList", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
        }

        public ActionResult UpdateList(int Id)
        {
            var model = db.Todoes.SingleOrDefault(x => x.Id == Id && x.IsActive == true);
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
                db.Errors.Add(new Error { MethodName = "UpdateListGet", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
        }

        [HttpPost]
        public JsonResult UpdateList(Todo todo)
        {

            try
            {
                db.Todoes.SingleOrDefault(x => x.Id == todo.Id).Name = todo.Name;
                db.SaveChanges();
                return Json("/Todo/Index");
            }
            catch (Exception ex)
            {
                db.Errors.Add(new Error { MethodName = "UpdateListPost", Message = ex.Message, Time = DateTime.Now });
                db.SaveChanges();
                throw;
            }
        }

    }
}