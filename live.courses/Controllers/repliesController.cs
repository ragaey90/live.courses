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
    public class repliesController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();
        //get all replies for comment
        // GET: api/replies
        public IQueryable<reply> Getcomment_replies(int id)
        {
            return db.replies.Where(x=>x.comment_id==id);
        }

        // GET: api/replies/5
        [ResponseType(typeof(reply))]
        public IHttpActionResult Getreply(int id)
        {
            reply reply = db.replies.Find(id);
            if (reply == null)
            {
                return NotFound();
            }

            return Ok(reply);
        }

        // PUT: api/replies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putreply(int id, reply reply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reply.id)
            {
                return BadRequest();
            }

            db.Entry(reply).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!replyExists(id))
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

        // POST: api/replies
        [ResponseType(typeof(reply))]
        public IHttpActionResult Postreply(reply reply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.replies.Add(reply);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = reply.id }, reply);
        }

        // DELETE: api/replies/5
        [ResponseType(typeof(reply))]
        public IHttpActionResult Deletereply(int id)
        {
            reply reply = db.replies.Find(id);
            if (reply == null)
            {
                return NotFound();
            }

            db.replies.Remove(reply);
            db.SaveChanges();

            return Ok(reply);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool replyExists(int id)
        {
            return db.replies.Count(e => e.id == id) > 0;
        }
    }
}