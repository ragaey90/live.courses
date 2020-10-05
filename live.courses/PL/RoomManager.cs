using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using live.courses.Models;

namespace live.courses.PL
{
    public class RoomManager : Repository<room>
    {
        public RoomManager(adv_coursesEntities ctx)
            : base(ctx)
        {

        }
    }
}