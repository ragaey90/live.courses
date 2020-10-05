using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace live.courses.chat_classes
{
    public class cls_msg
    {
        public string sender_id { get; set; }
        public string Room_id { get; set; }
        public string MsgType { get; set; }
        public string message { get; set; }
        //   public Nullable<DateTime> timeStamp { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}