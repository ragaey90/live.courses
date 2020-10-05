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
    public class commentsController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();


        //get comments for a post
        public IEnumerable<comment> Getpost_comments(int id)
        {
            var comments = db.comments.Where(x => x.post_id == id);
            List<comment> comments_with_likes = new List<comment>();
            foreach (var item in comments)
            {
                item.likes_number = db.likes.Where(x => x.liked_id == item.id&&x.liked_type=="comment").Count();
                item.replies_number = db.replies.Where(x => x.comment_id == item.id).Count();
                comments_with_likes.Add(item);
            }

            return comments_with_likes;

        }


        //// GET: api/comments
        //public IQueryable<comment> Getcomments()
        //{
        //    return db.comments;
        //}

        // GET: api/comments/5
        [ResponseType(typeof(comment))]
        public IHttpActionResult Getcomment(int id)
        {
            comment comment = db.comments.Find(id);
            
            if (comment == null)
            {
                return NotFound();
            }

            List<reply> replies = db.replies.Where(x => x.comment_id == id).ToList();
            foreach (var item in replies)
            {
                item.likes_number = db.likes.Where(x => x.liked_id == item.id && x.liked_type == "reply").Count();
                replies.Add(item);
            }
            comment.likes_number = db.likes.Where(x => x.liked_id == id && x.liked_type == "comment").Count();
            comment.comment_replies = replies;

            return Ok(comment);
        }

        // PUT: api/comments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcomment(int id, comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.id)
            {
                return BadRequest();
            }

            db.Entry(comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!commentExists(id))
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

        // POST: api/comments
        [ResponseType(typeof(comment))]
        public IHttpActionResult Postcomment(comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.comments.Add(comment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = comment.id }, comment);
        }

        // DELETE: api/comments/5
        [ResponseType(typeof(comment))]
        public IHttpActionResult Deletecomment(int id)
        {
            comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.comments.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool commentExists(int id)
        {
            return db.comments.Count(e => e.id == id) > 0;
        }
    }
}