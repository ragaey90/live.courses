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
    public class workgrouptagsController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        // GET: api/workgrouptags
        public IQueryable<work_group_tags> Getwork_group_tags()
        {
            return db.work_group_tags;
        }

        // GET: api/workgrouptags/5
        [ResponseType(typeof(work_group_tags))]
        public IHttpActionResult Getwork_group_tags(int id)
        {
            work_group_tags work_group_tags = db.work_group_tags.FirstOrDefault(x=>x.work_group_id==id);
            if (work_group_tags == null)
            {
                return NotFound();
            }

            return Ok(work_group_tags);
        }

        // PUT: api/workgrouptags/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putwork_group_tags(int id, work_group_tags work_group_tags)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != work_group_tags.work_group_id)
            {
                return BadRequest();
            }

            db.Entry(work_group_tags).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!work_group_tagsExists(id))
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

        // POST: api/workgrouptags
        [ResponseType(typeof(work_group_tags))]
        public IHttpActionResult Postwork_group_tags(work_group_tags work_group_tags)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.work_group_tags.Add(work_group_tags);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (work_group_tagsExists(work_group_tags.work_group_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = work_group_tags.work_group_id }, work_group_tags);
        }

        // DELETE: api/workgrouptags/5
        [ResponseType(typeof(work_group_tags))]
        public IHttpActionResult Deletework_group_tags(int id)
        {
            work_group_tags work_group_tags = db.work_group_tags.Find(id);
            if (work_group_tags == null)
            {
                return NotFound();
            }

            db.work_group_tags.Remove(work_group_tags);
            db.SaveChanges();

            return Ok(work_group_tags);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool work_group_tagsExists(int id)
        {
            return db.work_group_tags.Count(e => e.work_group_id == id) > 0;
        }
    }
}