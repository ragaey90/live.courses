using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using live.courses.classesForReturn;
using live.courses.Models;

namespace live.courses.Controllers
{
    public class SearchController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();
        [HttpGet]
        public IQueryable<cls_searchAll> all(string name)
        {

            var courses = from x in db.courses where x.name.Contains(name) select  new cls_searchAll { id = x.id.ToString(), name = x.name, photo = "noimage", type = "course" };
            var groups = db.work_group.Where(x => x.name.Contains(name)).Select(x => new cls_searchAll { id = x.id.ToString(), name = x.name, photo = "noimage", type = "group" });
            var users = db.AspNetUsers.Where(x => x.UserName.Contains(name)).Select(x => new cls_searchAll { id = x.Id, name = x.UserName, photo = x.Photo, type = "user" });
            var courseGroups = courses.Concat(groups);
            var all = courseGroups.Concat(users);
           // dt.Merge(db.courses.Where(x => x.name.Contains(name)).Select(x => new cls_searchAll {id= x.id.ToString(),name= x.name ,photo="noimage",type="course"}) as DataTable);
            //dt.Merge(db.work_group.Where(x => x.name.Contains(name)).Select(x => new cls_searchAll { id = x.id.ToString(), name = x.name, photo = "noimage", type = "group" }) as DataTable);
            //dt.Merge(db.AspNetUsers.Where(x => x.UserName.Contains(name)).Select(x => new cls_searchAll{ id = x.Id, name = x.UserName, photo = x.Photo, type = "user" }) as DataTable);

            return all;
            //return db.search_all(name).ToList();
        }
        [HttpGet]
        public IEnumerable<search_courses_Result> course(string name)
        {
            return db.search_courses(name);
        }
        [HttpGet]
        public IEnumerable<search_work_groups_Result> workgroup(string name)
        {
            return db.search_work_groups(name).ToList();
        }
        [HttpGet]
        public IEnumerable<search_courses_WGroup_Result> CourseGroup(string name)
        {
            return db.search_courses_WGroup(name).ToList();
        }
        [HttpGet]
        public IEnumerable<search_users_Result> user(string name)
        {
            return db.search_users(name).ToList();
        }
        [HttpGet]
        public IEnumerable<search_tags_Result> tag(string name)
        {
            return db.search_tags(name).ToList();
        }

    }
}