using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace live.courses.Classes_for_sp
{
    public class cls_user
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        //public string PasswordHash { get; set; }
        //public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        //public bool TwoFactorEnabled { get; set; }
        //public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        //public bool LockoutEnabled { get; set; }
        //public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public string Apout { get; set; }
        public string AnotherAccount { get; set; }
        public Nullable<bool> IsInstructor { get; set; }
        public string GuaranteeDecument { get; set; }
        public string IdCard { get; set; }
        public string IdCardBack { get; set; }
        public string Residence { get; set; }
        public string Photograph { get; set; }
        public string facebookAccount { get; set; }
        public string twitterAccount { get; set; }
        public string jobTitle { get; set; }

    }
}