using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace live.courses.Classes_for_sp
{
    public class RegisterResponseClass
    {
        public string id { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string country { get; set; }
        public string state { get; set; }
    }
}