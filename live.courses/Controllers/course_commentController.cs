using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using live.courses.classesForReturn;
using live.courses.Models;

namespace live.courses.Controllers
{
    public class course_commentController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "LVcDfpyKKnIViyvnUCDzVK6pGzNsEVfFxrXghhiS",//"1NvMs4WAnA4a1NdIvEkfek5YBaPZGN0yLlwCeQI9",
            BasePath = "https://startup-livecourse.firebaseio.com/"//"https://livecoursechat.firebaseio.com"
        };
        public async Task<IHttpActionResult> Push(cls_notifi notifi, string path)
        {
            IFirebaseClient client = new FirebaseClient(config);
            var my_notifi = notifi;
            PushResponse response = await client.PushAsync(path, my_notifi);
            var xcv = response.Result.name; //The result will contain the child name of the new data that was added
            return Ok();
        }
        // GET: api/course_comment
        public IHttpActionResult GetCourseComments(int courseId)
        {
            return Ok(db.course_comment.Where(x=>x.course_id==courseId).Include(x=>x.AspNetUser).Select(x => 
            new {x.id, x.body ,x.course_id,x.timestamp,x.user_id,
                x.AspNetUser.UserName,
                x.AspNetUser.Photo }));
        }

        // GET: api/course_comment/5
        [ResponseType(typeof(course_comment))]
        public IHttpActionResult Getcourse_SingleComment(int id)
        {
            course_comment course_comment = db.course_comment.Include(x => new { x.AspNetUser.UserName, x.AspNetUser.Photo }).FirstOrDefault(x=>x.id==id);
            if (course_comment == null)
            {
                return NotFound();
            }

            return Ok(course_comment);
        }

        // PUT: api/course_comment/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcourse_comment(int id, course_comment course_comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course_comment.id)
            {
                return BadRequest();
            }

            db.Entry(course_comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!course_commentExists(id))
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

        // POST: api/course_comment
        [ResponseType(typeof(course_comment))]
        public async Task<IHttpActionResult> Postcourse_comment(course_comment course_comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.course_comment.Add(course_comment);
            db.SaveChanges();
            //send notification to all friends
            var friends = db.friends.Where(x => x.UserId == course_comment.user_id && x.Confirmed == true).Select(x => x.friendId);
            var friends2 = db.friends.Where(x => x.friendId == course_comment.user_id && x.Confirmed == true).Select(x => x.UserId);
            friends.Concat(friends2);

            var new_notifi = new cls_notifi
            {
                source_name = "course",
                source_id = course_comment.course_id.ToString(),
                image = db.AspNetUsers.FirstOrDefault(x => x.Id == course_comment.user_id).Photo,//"no image",
                body_English =  db.AspNetUsers.FirstOrDefault(x => x.Id == course_comment.user_id).UserName + " commented on course " + db.courses.FirstOrDefault(x => x.id == course_comment.course_id).name,
                body_Arabic = "كتب "+  db.AspNetUsers.FirstOrDefault(x => x.Id == course_comment.user_id).UserName + " تعليق على كورس " + db.courses.FirstOrDefault(x => x.id == course_comment.course_id).name ,
                timestamp = DateTime.Now,
                readed = false
            };
            //await Push(new_notifi, "notifications/" + db.courses.FirstOrDefault(x => x.id == course_comment.course_id).instructor + "/" + DateTime.UtcNow.ToString("dd-MM-yyyy"));
            foreach (var item in friends)
            {
                await Push(new_notifi, "notifications/" + item + "/" + DateTime.UtcNow.ToString("dd-MM-yyyy"));
                PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id == item).DeviceToken, "comment", new_notifi.body_English, "course", course_comment.course_id.ToString());

            }

            return CreatedAtRoute("DefaultApi", new { id = course_comment.id }, course_comment);
        }

        // DELETE: api/course_comment/5
        [ResponseType(typeof(course_comment))]
        public IHttpActionResult Deletecourse_comment(int id)
        {
            course_comment course_comment = db.course_comment.Find(id);
            if (course_comment == null)
            {
                return NotFound();
            }

            db.course_comment.Remove(course_comment);
            db.SaveChanges();

            return Ok(course_comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool course_commentExists(int id)
        {
            return db.course_comment.Count(e => e.id == id) > 0;
        }
        private void PushNotifi(string deviceToken, string title, string body, string type, string type_id)
        {

            try
            {
                var applicationID = "AAAAOt4XBc8:APA91bED-1d6lWt60bK2pFMCYg1aFr9gQ-AuYfbgN9ZpzZqSuqEFdVg3FKWyOYZSroCsdoF-ZiOP-bu6ba7zzyLm4UFSnnVfpJCptQ1ns3TOhQINYKTbjjDeWXjti9wK6Z_DKSrMQJNT";

                var senderId = "252834153935";

                string deviceId = deviceToken;

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                tRequest.Method = "post";

                tRequest.ContentType = "application/json";

                var data = new

                {

                    to = deviceId,

                    notification = new

                    {
                        title = title,
                        text = body,
                        source_type = type,
                        source_id = type_id,
                        icon = "myicon"

                    }
                };

                var serializer = new JavaScriptSerializer();

                var json = serializer.Serialize(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                tRequest.ContentLength = byteArray.Length;


                using (Stream dataStream = tRequest.GetRequestStream())
                {

                    dataStream.Write(byteArray, 0, byteArray.Length);


                    using (WebResponse tResponse = tRequest.GetResponse())
                    {

                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {

                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {

                                String sResponseFromServer = tReader.ReadToEnd();

                                string str = sResponseFromServer;

                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                string str = ex.Message;

            }
        }
    }
}