using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using live.courses.Models;

namespace live.courses.PL
{
    public class UnitOfWork
    {
        private adv_coursesEntities ctx = new adv_coursesEntities();
        public UserManager UserManager { get { return new UserManager(ctx); } }
        public MessageManager MessageManager { get { return new MessageManager(ctx); } }
        public NotificationManager NotificationManager { get { return new NotificationManager(ctx); } }
        public RoomManager RoomManager { get { return new RoomManager(ctx); } }
        public CourseManager CourseManager { get { return new CourseManager(ctx); } }
        public WorkGroupManager WorkGroupManager { get { return new WorkGroupManager(ctx); } }
        public CertificatesManager CertificatesManager { get { return new CertificatesManager(ctx); } }

        public bool save()
        {
            return ctx.SaveChanges() > 0;
        }
    }
}