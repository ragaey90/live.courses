//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace live.courses.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Course_Members
    {
        public int Course_id { get; set; }
        public string Member_id { get; set; }
        public Nullable<System.DateTime> AddingDate { get; set; }
        public Nullable<bool> Finished { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual course course { get; set; }
    }
}
