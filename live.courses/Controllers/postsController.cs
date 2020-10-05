using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using live.courses.Models;

namespace live.courses.Controllers
{
    public class postsController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        ////get timeline posts
        //public IEnumerable<post> Gettimeline(string id)
        //{
        //    List<post> timeline = new List<post>();
        //    var mycourses = db.courses.Where(x=>x.Course_Members.Where())
        //    var mycourses=db.posts.Where(x=>x.group_type=="course")
        //}
        public IHttpActionResult GetCoursesGroupsAsposts()
        {
            var courses = db.courses.Select(x => new { type = "Course", x.id, x.name, x.about, x.availlable, x.category, x.creating_date});
            var groups = db.work_group.Select(x => new { type = "Group", x.id, x.name, x.about, x.availlable, x.category, x.creating_date });


            //var courses = db.courses.Select(x => new {type="Course", x.id, x.name, x.about, x.availlable, x.category ,x.creating_date,comments=x.course_comment.Select(y=> new {y.id,y.body,y.timestamp,y.user_id })});
            //var groups = db.work_group.Select(x => new { type = "Group", x.id, x.name, x.about, x.availlable, x.category, x.creating_date, comments = x.group_comment.Select(y => new { y.id, y.body, y.timestamp, y.user_id }) });
            var psts = courses.Concat(groups).OrderByDescending(x=>x.creating_date).ToList();
            //var posts = new { courses, groups };
            return Ok(psts);
        }

        //get all posts for certain user
        public IEnumerable<post> Getuser_posts(string id)
        {
            var posts = db.posts.Where(x => x.user_id == id);
            List<post> posts_with_likes = new List<post>();
            foreach (var item in posts)
            {
                item.likes_number = db.likes.Where(x => x.liked_id == item.id&&x.liked_type== "post").Count();
                item.comments_number = db.comments.Where(x => x.post_id == item.id).Count() + db.replies.Where(x => x.post_id == item.id).Count();
                posts_with_likes.Add(item);
            }

                return posts_with_likes;
            
        }
        //Get all posts for certain course
        public IEnumerable<post> Getcourse_posts(int id)
        {
            var posts = db.posts.Where(x => x.group_id == id && x.group_type == "course");
            List<post> posts_with_likes = new List<post>();
            foreach (var item in posts)
            {
                item.likes_number = db.likes.Where(x => x.liked_id == item.id && x.liked_type == "post").Count();
                item.comments_number = db.comments.Where(x => x.post_id == item.id).Count() + db.replies.Where(x => x.post_id == item.id).Count();
                posts_with_likes.Add(item);
            }

            return posts_with_likes;
        }
        //Get all posts for certain group
        public IEnumerable<post> Getgroup_posts(int id)
        {
            var posts= db.posts.Where(x => x.group_id == id && x.group_type == "group");
            List<post> posts_with_likes = new List<post>();
            foreach (var item in posts)
            {
                item.likes_number = db.likes.Where(x => x.liked_id == item.id && x.liked_type == "post").Count();
                item.comments_number = db.comments.Where(x => x.post_id == item.id).Count() + db.replies.Where(x => x.post_id == item.id).Count();

                posts_with_likes.Add(item);
            }

            return posts_with_likes;
        }
        // GET: api/posts
        //public IQueryable<post> Getposts()
        //{
        //    return db.posts;
        //}

        // GET: api/posts/5
        [ResponseType(typeof(post))]
        public IHttpActionResult Getpost(int id)
        {
            post post = db.posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            List<comment> comments = db.comments.Where(x => x.post_id == id).ToList();
            List<reply> replies = db.replies.Where(x => x.post_id == id).ToList();
            foreach (var item in comments)
            {
                item.likes_number = db.likes.Where(x => x.liked_id == item.id && x.liked_type == "comment").Count();
                item.replies_number = db.replies.Where(x => x.comment_id == item.id).Count();

                comments.Add(item);
            }
            post.likes_number = db.likes.Where(x => x.liked_id == id && x.liked_type == "post").Count();
            post.post_comments = comments;
            return Ok(post);
        }

        // PUT: api/posts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putpost(int id, post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.id)
            {
                return BadRequest();
            }

            db.Entry(post).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!postExists(id))
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

        // POST: api/posts
        [ResponseType(typeof(post))]
        public IHttpActionResult Postpost(post post1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (post1.imagefile!=null)
            {
                string filename = Path.GetFileNameWithoutExtension(post1.imagefile.FileName);
                string extention = Path.GetExtension(post1.imagefile.FileName);
                filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
                post1.image = "~/Content/image/" + filename;
                filename = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/image/"), filename);
                //other way
                // Path.Combine(HttpContext.Current.Server.MapPath("~/Content/image/"), filename);
                post1.imagefile.SaveAs(filename);
            }
            

            db.posts.Add(post1);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = post1.id }, post1);
        }

        // DELETE: api/posts/5
        [ResponseType(typeof(post))]
        public IHttpActionResult Deletepost(int id)
        {
            post post = db.posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            db.posts.Remove(post);
            db.SaveChanges();

            return Ok(post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool postExists(int id)
        {
            return db.posts.Count(e => e.id == id) > 0;
        }
    }
}