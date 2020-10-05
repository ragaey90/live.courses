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
    public class tagsController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        [ResponseType(typeof(tag))]
        public IHttpActionResult PostNewtags(cls_tag tags)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<int> ids = new List<int>();
            foreach (string item in tags.tag_names)
            {
                if (!db.tags.Any(x=>x.tag1==item))
                {
                    tag tag_to_add = new tag();
                    tag_to_add.tag1 = item;
                    db.tags.Add(tag_to_add);
                    db.SaveChanges();
                    ids.Add(db.tags.OrderByDescending(x => x.id).FirstOrDefault().id);
                    
                }
                else
                {
                    ids.Add(db.tags.FirstOrDefault(x=>x.tag1==item).id);
                }
            }
            if (tags.Type == "course")
            {
                foreach (int item in ids)
                {
                    course_tags t = new course_tags
                    {
                        course_id = tags.TypeId,
                        tag_id =item
                    };
                    db.course_tags.Add(t);
                    db.SaveChanges();
                }
                
            }
            else if (tags.Type == "group")
            {
                foreach (int item in ids)
                {
                    work_group_tags t = new work_group_tags
                    {
                        work_group_id = tags.TypeId,
                        tag_id = item
                    };
                    db.work_group_tags.Add(t);
                    db.SaveChanges();
                }

            }


            return Ok(tags);
        }

        // GET: api/tags
        public IHttpActionResult Gettags()
        {
            return Ok(db.tags.Select(x=>new { x.id,x.tag1}));
        }

        // GET: api/tags/5
        [ResponseType(typeof(tag))]
        public IHttpActionResult Gettag(int id)
        {
            tag tag = db.tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        // PUT: api/tags/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttag( tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Entry(tag).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tagExists(tag.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/tags
        [ResponseType(typeof(tag))]
        public IHttpActionResult Posttag(string tag_name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            tag tag_to_add = new tag();
            tag_to_add.tag1 = tag_name;
            db.tags.Add(tag_to_add);
            db.SaveChanges();

            return Ok(tag_to_add);
        }

        // DELETE: api/tags/5
        [ResponseType(typeof(tag))]
        public IHttpActionResult Deletetag(int id)
        {
            tag tag = db.tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            db.tags.Remove(tag);
            db.SaveChanges();

            return Ok(tag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tagExists(int id)
        {
            return db.tags.Count(e => e.id == id) > 0;
        }
    }
}