using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace live.courses.classesForReturn
{
    public class cls_notifi
    {
        public string source_name { get; set; }
        public string source_id { get; set; }
        public string body_English { get; set; }
        public string body_Arabic { get; set; }
        public Nullable<System.DateTime> timestamp { get; set; }
        public string image { get; set; }
        public Nullable<bool> readed { get; set; }
    }
    public class Message
    {
        public string[] registration_ids { get; set; }
        public Notification notification { get; set; }
        public object data { get; set; }
    }
    public class Notification
    {
        public string title { get; set; }
        public string text { get; set; }
    }
}