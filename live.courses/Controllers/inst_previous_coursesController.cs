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
    public class inst_previous_coursesController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        // GET: api/inst_previous_courses
        public IHttpActionResult Getinst_previous_courses(string id)
        {
            return Ok(db.inst_previous_courses.Where(x=>x.inst_id==id).Select(x=>new {x.id,x.inst_id,x.title,x.body }));
        }

        // GET: api/inst_previous_courses/5
        [ResponseType(typeof(inst_previous_courses))]
        public IHttpActionResult Get_sindle_inst_previous_courses(int id)
        {
            inst_previous_courses inst_previous_courses = db.inst_previous_courses.Find(id);
            if (inst_previous_courses == null)
            {
                return NotFound();
            }

            return Ok(inst_previous_courses);
        }

        // PUT: api/inst_previous_courses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putinst_previous_courses(int id, inst_previous_courses inst_previous_courses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inst_previous_courses.id)
            {
                return BadRequest();
            }

            db.Entry(inst_previous_courses).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!inst_previous_coursesExists(id))
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

        // POST: api/inst_previous_courses
        [ResponseType(typeof(inst_previous_courses))]
        public IHttpActionResult Postinst_previous_courses(inst_previous_courses inst_previous_courses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.inst_previous_courses.Add(inst_previous_courses);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = inst_previous_courses.id }, inst_previous_courses);
        }

        // DELETE: api/inst_previous_courses/5
        [ResponseType(typeof(inst_previous_courses))]
        public IHttpActionResult Deleteinst_previous_courses(int id)
        {
            inst_previous_courses inst_previous_courses = db.inst_previous_courses.Find(id);
            if (inst_previous_courses == null)
            {
                return NotFound();
            }

            db.inst_previous_courses.Remove(inst_previous_courses);
            db.SaveChanges();

            return Ok(inst_previous_courses);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool inst_previous_coursesExists(int id)
        {
            return db.inst_previous_courses.Count(e => e.id == id) > 0;
        }
    }
}