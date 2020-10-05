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
using live.courses.chat_classes;
using live.courses.classesForReturn;
using live.courses.Models;

namespace live.courses.Controllers
{
    public class CourseMembersController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();


        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "LVcDfpyKKnIViyvnUCDzVK6pGzNsEVfFxrXghhiS",//"1NvMs4WAnA4a1NdIvEkfek5YBaPZGN0yLlwCeQI9",
            BasePath = "https://startup-livecourse.firebaseio.com/"//"https://livecoursechat.firebaseio.com"
        };
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

        [HttpGet]
        public IHttpActionResult SearchStrangers(int id,string search_text)
        {
            var members = db.Course_Members.Where(x => x.Course_id == id).Select(x => x.Member_id);

            var strangers = db.AspNetUsers.Where(x => !members.Contains(x.Id)).Select(x => new { x.Id, x.UserName, x.Photo });
            var result = strangers.Where(x => x.UserName.Contains(search_text));
            return Ok(result);
        }
        public IHttpActionResult GetStrangers(int id)
        {
            var members = db.Course_Members.Where(x => x.Course_id == id).Select(x=>x.Member_id);

            var strangers = db.AspNetUsers.Where(x => !members.Contains(x.Id)).Select(x => new { x.Id, x.UserName, x.Photo });

            return Ok(strangers);
        }
        public async Task<IHttpActionResult> Push_for_room(object thingToPush, string path)
        {
            IFirebaseClient client = new FirebaseClient(config);
            var my_thing = thingToPush;
            PushResponse response = await client.PushAsync(path, my_thing);
            var xcv = response.Result.name; //The result will contain the child name of the new data that was added
            return Ok();
        }
        public async Task<IHttpActionResult> Push(cls_notifi notifi, string path)
        {
            IFirebaseClient client = new FirebaseClient(config);
            var my_notifi = notifi;
            PushResponse response = await client.PushAsync(path, my_notifi);
            var xcv = response.Result.name; //The result will contain the child name of the new data that was added
            return Ok();
        }
        // GET: api/CourseMembers
        //public IQueryable<Course_Members> GetCourse_Members()
        //{
        //    return db.Course_Members;
        //}

        // GET: api/CourseMembers/5
        [ResponseType(typeof(Course_Members))]
        public IHttpActionResult GetCourse_Members(int id)
        {
            var course_Members = db.Course_Members.Where(x=>x.Course_id==id).Select(x=>new { x.Member_id,x.Finished,x.Course_id,x.AspNetUser.UserName ,x.AspNetUser.Photo});
            if (course_Members == null)
            {
                return NotFound();
            }

            return Ok(course_Members);
        }

        // PUT: api/CourseMembers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCourse_Members(Course_Members course_Members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Entry(course_Members).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Course_MembersExists(course_Members.Course_id,course_Members.Member_id))
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

        // POST: api/CourseMembers
        [ResponseType(typeof(Course_Members))]
        public async Task<IHttpActionResult> PostCourse_Members(Course_Members course_Members)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Course_Members.Add(course_Members);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Course_MembersExists(course_Members.Course_id,course_Members.Member_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var new_notifi = new cls_notifi
            {
                source_name = "user",
                source_id = course_Members.Member_id,
                image = db.AspNetUsers.FirstOrDefault(x => x.Id == course_Members.Member_id).Photo,//"no image",
                body_English = db.AspNetUsers.FirstOrDefault(x => x.Id == course_Members.Member_id).UserName + " Joined to Your course ("+db.courses.FirstOrDefault(x=>x.id==course_Members.Course_id).name+")",
                body_Arabic ="انضم "+ db.AspNetUsers.FirstOrDefault(x => x.Id == course_Members.Member_id).UserName + " الي كورس ("+db.courses.FirstOrDefault(x=>x.id==course_Members.Course_id).name+")",
                timestamp = DateTime.Now,
                readed = false
            };
            await Push(new_notifi, "notifications/" + db.courses.FirstOrDefault(x => x.id == course_Members.Course_id).instructor + "/" + DateTime.UtcNow.ToString("dd-MM-yyyy"));
            PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id == db.courses.FirstOrDefault(Y => Y.id == course_Members.Course_id).instructor).DeviceToken, "New Member To your Course", new_notifi.body_English, "user", course_Members.Member_id);

            //////////////////////////////////////////////////////////////////////add member to room 
            IFirebaseClient client = new FirebaseClient(config);

            var Course_room_id = db.courses.FirstOrDefault(x => x.id == course_Members.Course_id).room_id;
            var new_Mem = new
            {
                mem_id = course_Members.Member_id,
                mem_name = db.AspNetUsers.FirstOrDefault(x => x.Id == course_Members.Member_id).UserName
            };
            var new_room = new
            {
                RoomId = Course_room_id,
                RoomName = db.courses.FirstOrDefault(x => x.id == course_Members.Course_id).name,
                AddedOn = DateTime.UtcNow,
                about = course_Members.course.about,
                RoomType = "course",
                RelatedTypeId = course_Members.Course_id
            };

                await Push_for_room(new_Mem, "chat/members/" + Course_room_id);
                await Push_for_room(new_room, "chat/memberRooms/" + course_Members.Member_id);

            //////////////////////////////////////////////////////////////////////////////////

            return CreatedAtRoute("DefaultApi", new { id = course_Members.Course_id }, course_Members);
        }

        // DELETE: api/CourseMembers/5
        [HttpPost]
        [ResponseType(typeof(Course_Members))]
        public IHttpActionResult LeaveCourse(int course_id, string member_id)
        {
            Course_Members course_Members = db.Course_Members.Find(course_id,member_id);
            if (course_Members == null)
            {
                return NotFound();
            }

            db.Course_Members.Remove(course_Members);
            db.SaveChanges();

            return Ok(course_Members);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Course_MembersExists(int id,string mem_id)
        {
            return db.Course_Members.Count(e => e.Course_id == id&&e.Member_id==mem_id) > 0;
        }

    }
}