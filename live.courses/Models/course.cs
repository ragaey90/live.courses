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
    
    public partial class course
    {
        public course()
        {
            this.Course_Members = new HashSet<Course_Members>();
            this.course_tags = new HashSet<course_tags>();
            this.course_comment = new HashSet<course_comment>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string about { get; set; }
        public Nullable<System.DateTime> creating_date { get; set; }
        public string place { get; set; }
        public string trange { get; set; }
        public Nullable<double> cost { get; set; }
        public string way_to_pay { get; set; }
        public Nullable<bool> allow_buying { get; set; }
        public Nullable<bool> availlable { get; set; }
        public string way_to_apply { get; set; }
        public string notes { get; set; }
        public string instructor { get; set; }
        public string video { get; set; }
        public string category { get; set; }
        public Nullable<bool> Archived { get; set; }
        public string room_id { get; set; }
    
        public virtual ICollection<Course_Members> Course_Members { get; set; }
        public virtual ICollection<course_tags> course_tags { get; set; }
        public virtual ICollection<course_comment> course_comment { get; set; }
    }
}
