using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class BaseController : Controller
    {
        public TodoEntities db { get; set; }
        public BaseController()
        {
             db = new TodoEntities();
        }
       
    }
}