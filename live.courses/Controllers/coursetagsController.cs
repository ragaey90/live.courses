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
using live.courses.Models;

namespace live.courses.Controllers
{
    public class coursetagsController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        // GET: api/coursetags
        public IQueryable<course_tags> Getcourse_tags()
        {
            return db.course_tags;
        }

        // GET: api/coursetags/5
        [ResponseType(typeof(course_tags))]
        public IHttpActionResult Getcourse_tags(int id)
        {
            course_tags course_tags = db.course_tags.Find(id);
            if (course_tags == null)
            {
                return NotFound();
            }

            return Ok(course_tags);
        }

        // PUT: api/coursetags/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcourse_tags(int id, course_tags course_tags)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course_tags.course_id)
            {
                return BadRequest();
            }

            db.Entry(course_tags).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!course_tagsExists(id))
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

        // POST: api/coursetags
        [ResponseType(typeof(course_tags))]
        public IHttpActionResult Postcourse_tags(course_tags course_tags)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.course_tags.Add(course_tags);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (course_tagsExists(course_tags.course_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = course_tags.course_id }, course_tags);
        }

        // DELETE: api/coursetags/5
        [ResponseType(typeof(course_tags))]
        public IHttpActionResult Deletecourse_tags(int id)
        {
            course_tags course_tags = db.course_tags.Find(id);
            if (course_tags == null)
            {
                return NotFound();
            }

            db.course_tags.Remove(course_tags);
            db.SaveChanges();

            return Ok(course_tags);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool course_tagsExists(int id)
        {
            return db.course_tags.Count(e => e.course_id == id) > 0;
        }
    }
}