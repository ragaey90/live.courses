using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace live.courses.chat_classes
{
    public class cls_chat_room
    {
        public string name { get; set; }
        public string about { get; set; }
        public string photo { get; set; }
        public string adminName { get; set; }
        public string lastMessage { get; set; }
        public Nullable<DateTime> timeStamp { get; set; }

    }
}