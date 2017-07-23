using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
    public class EmailNotificationModel
    {
        public int TaskId { get; set; }

        public string EmailAddress { get; set; }

        public DateTime NotificationTime { get; set; }

        public string TaskBody { get; set; }
    }
}