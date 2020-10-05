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
    public class likesController : ApiController
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

        //get all likes for post
        public IQueryable<like> GetCourse_likes(int id)
        {
            return db.likes.Where(x=>x.liked_id==id&&x.liked_type=="course");
        }
        public IQueryable<like> GetGroup_likes(int id)
        {
            return db.likes.Where(x => x.liked_id == id && x.liked_type == "group");
        }
        //get all likes for post
        public IQueryable<like> GetCoursecomment_likes(int id)
        {
            return db.likes.Where(x => x.liked_id == id && x.liked_type == "coursecomment");
        }
        public IQueryable<like> GetGroupcomment_likes(int id)
        {
            return db.likes.Where(x => x.liked_id == id && x.liked_type == "groupcomment");
        }

        [HttpPost]
        [ResponseType(typeof(like))]
        public async Task<IHttpActionResult> likeCourse(like like)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            like.liked_type = "course";
            db.likes.Add(like);
            db.SaveChanges();


            var new_notifi = new cls_notifi
            {
                source_name = "course",
                source_id = like.liked_id.ToString(),
                image = db.AspNetUsers.FirstOrDefault(x => x.Id == like.user_id).Photo,//"no image",
                body_English = db.AspNetUsers.FirstOrDefault(x => x.Id == like.user_id).UserName + " likes your course " + db.courses.FirstOrDefault(x => x.id == like.liked_id).name,
                body_Arabic = "اعجب " + db.AspNetUsers.FirstOrDefault(x => x.Id == like.user_id).UserName + " بانشائك كورس " + db.courses.FirstOrDefault(x => x.id == like.liked_id).name ,
                timestamp = DateTime.Now,
                readed = false
            };
            var course_admin = db.courses.FirstOrDefault(x => x.id == like.liked_id).instructor;

            await Push(new_notifi, "notifications/" + course_admin.ToString() + "/" + DateTime.UtcNow.ToString("dd-MM-yyyy"));
                PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id == course_admin).DeviceToken, "More Likes", new_notifi.body_English, "course", like.liked_id.ToString());


                return CreatedAtRoute("DefaultApi", new { id = like.id }, like);
        }
        [HttpPost]
        [ResponseType(typeof(like))]
        public async Task<IHttpActionResult> likeGroup(like like)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            like.liked_type = "group";
            db.likes.Add(like);
            db.SaveChanges();


            var new_notifi = new cls_notifi
            {
                source_name = "group",
                source_id = like.liked_id.ToString(),
                image = db.AspNetUsers.FirstOrDefault(x => x.Id == like.user_id).Photo,//"no image",
                body_English = db.AspNetUsers.FirstOrDefault(x => x.Id == like.user_id).UserName + " likes your work group " + db.work_group.FirstOrDefault(x => x.id == like.liked_id).name,
                body_Arabic = "اعجب " + db.AspNetUsers.FirstOrDefault(x => x.Id == like.user_id).UserName + " بإنشاء مجموعتك " + db.work_group.FirstOrDefault(x => x.id == like.liked_id).name,
                timestamp = DateTime.Now,
                readed = false
            };
            var Group_admin = db.work_group.FirstOrDefault(x => x.id == like.liked_id).admin;
            await Push(new_notifi, "notifications/" + Group_admin.ToString() + "/" + DateTime.UtcNow.ToString("dd-MM-yyyy"));
            PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id == Group_admin).DeviceToken, "More Likes", new_notifi.body_English, "group", like.liked_id.ToString());





            return CreatedAtRoute("DefaultApi", new { id = like.id }, like);
        }
        [HttpPost]
        [ResponseType(typeof(like))]
        public IHttpActionResult likeCourseComment(like like)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            like.liked_type = "coursecomment";
            db.likes.Add(like);
            db.SaveChanges();



            return CreatedAtRoute("DefaultApi", new { id = like.id }, like);
        }
        [HttpPost]
        [ResponseType(typeof(like))]
        public IHttpActionResult likeGroupComment(like like)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            like.liked_type = "groupcomment";
            db.likes.Add(like);
            db.SaveChanges();





            return CreatedAtRoute("DefaultApi", new { id = like.id }, like);
        }

        // DELETE: api/likes/5
        [HttpDelete]
        [ResponseType(typeof(like))]
        public IHttpActionResult Dislike(like like)
        {
            like likeToRemove = db.likes.FirstOrDefault(x=>x.liked_id==like.liked_id&&x.liked_type==like.liked_type&&x.user_id==like.user_id);
            if (likeToRemove == null)
            {
                return NotFound();
            }

            db.likes.Remove(likeToRemove);
            db.SaveChanges();

            return Ok(likeToRemove);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool likeExists(int id)
        {
            return db.likes.Count(e => e.id == id) > 0;
        }
    }
}