using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using live.courses.Models;

namespace live.courses.PL
{
    public class CourseMembersManager:Repository<Course_Members>
    {
        public CourseMembersManager(adv_coursesEntities ctx)
            : base(ctx)
        {

        }
    }
}