using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using live.courses.Models;

namespace live.courses.PL
{
    public class WorkGroupManager : Repository<work_group>
    {
        public WorkGroupManager(adv_coursesEntities ctx)
            : base(ctx)
        {

        }
    }
}