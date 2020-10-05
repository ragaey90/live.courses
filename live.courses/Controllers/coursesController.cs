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
using System.Web;
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
    public class coursesController : ApiController
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
        public async Task<IHttpActionResult> PushChat(object thingToPush, string path)
        {
            IFirebaseClient client = new FirebaseClient(config);
            var my_thing = thingToPush;
            PushResponse response = await client.PushAsync(path, my_thing);
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

        public coursesController()
        {
            // Add the following code
            // problem will be solved
            db.Configuration.ProxyCreationEnabled = false;
        }


        [HttpPost]
        public IHttpActionResult uploadVideo(int course_id)
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ?
       HttpContext.Current.Request.Files[0] : null;

            if (file != null && file.ContentLength > 0)
            {
                //var fileName = Path.GetFileName(file.FileName);
                string filename = Path.GetFileNameWithoutExtension(file.FileName);
                string extention = Path.GetExtension(file.FileName);
                filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
                var path = Path.Combine(
                    HttpContext.Current.Server.MapPath("~/Content/uploads/videos/"),
                    filename
                );
                var courseTo_edit = db.courses.FirstOrDefault(x => x.id == course_id);
                courseTo_edit.video = "~/Content/uploads/videos/" + filename;
                db.Entry(courseTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
                file.SaveAs(path);
            }
            var group_to_return = db.courses.Where(x => x.id == course_id).Select(x => new { x.id, x.name, x.video });
            return Ok(group_to_return);

        }





        //finish course for only particular memper
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult finishCourseForOne(int Course_id,string memper_id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.finish_course_for_one_member(memper_id,Course_id);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!courseExists(Course_id))
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
        //finish course for all this course mempers
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult finishCourseForAll(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.finish_course_for_all_members(id);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!courseExists(id))
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

        //get all Not finished courses for A user
        public IEnumerable<get_all_not_finished_courses_for_user_Result> GetNotFinishedCourses(string id)
        {
            return db.get_all_not_finished_courses_for_user(id).ToList();
        }

        //get all finished courses for A user
        public IEnumerable<get_all_finished_courses_for_user_Result> GetFinishedCourses(string id)
        {
            return db.get_all_finished_courses_for_user(id).ToList();
        }
        //un Archive A particular course
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult UnArchiveCourse(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.un_archive_course(id);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!courseExists(id))
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
        //Archive A particular course
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult ArchiveCourse(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.archive_course(id);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!courseExists(id))
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
        //get all archived courses for a user
        public IEnumerable<get_all_archived_courses_for_user_Result> GetarchivedcoursesForUser(string id)
        {
            return db.get_all_archived_courses_for_user(id).ToList();
        }
        //get all un archived courses for a user
        public IEnumerable<get_all_un_archived_courses_for_user_Result> GetUnarchivedcoursesForUser(string id)
        {
            return db.get_all_un_archived_courses_for_user(id).ToList();
        }
        //get all unArchived courses
        public IEnumerable<Get_all_unarchived_courses_Result> GetAllUnarchivedCourses()
        {
            return  db.Get_all_unarchived_courses().ToList();
        }
        //get all archived courses
        public IEnumerable<Get_all_archived_courses_Result> GetAllArchivedCourses()
        {
            return db.Get_all_archived_courses().ToList();
        }
        //get all tags for course
        public IEnumerable<get_all_course_tags_Result> Gettags(int id)
        {
            return db.get_all_course_tags(id).ToList();
        }
        // GET: api/courses
        public IQueryable<course> Getcourses()
        {
            return db.courses;
        }

        // GET: api/courses/5
        //[Authorize]
        [ResponseType(typeof(course))]
        public IHttpActionResult Getcourse(int id,string user_id)
        {
            course course = db.courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            var current_user = db.AspNetUsers.FirstOrDefault(x => x.Id ==user_id);
            var liked = db.likes.FirstOrDefault(x => x.liked_id == id && x.liked_type == "course" && x.user_id == current_user.Id);
            var is_liked = false;
            if (liked!= null)
            {
                 is_liked = true;
            }
            
            
            var my_course = new {
                id = course.id,
                name = course.name,
                about = course.about,
                TimeRange = course.trange,
                VideoLink = course.video,
                WayToApply = course.way_to_apply,
                WayToPay = course.way_to_pay,
                Notes = course.notes,
                Place=course.place,
                InstructorId=course.instructor,
                InstructorName=db.AspNetUsers.FirstOrDefault(x=>x.Id==course.instructor).UserName,
                CreatingDate =course.creating_date,
                Catogery=course.category,
                AllowBuying=course.allow_buying,
                Cost=course.cost,
                Available=course.availlable,
                course.Archived,
                MembersCount = course.Course_Members.Count,
                CourseTags=course.course_tags.ToList(),
                CourseMembers=course.Course_Members.ToList(),
                is_liked
            };
            return Ok(my_course);
        }

        // PUT: api/courses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcourse(int id, course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.id)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!courseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var my_course = new
            {
                id = course.id,
                name = course.name,
                about = course.about,
                TimeRange = course.trange,
                VideoLink = course.video,
                WayToApply = course.way_to_apply,
                WayToPay = course.way_to_pay,
                Notes = course.notes,
                Place = course.place,
                InstructorId = course.instructor,
                InstructorName = db.AspNetUsers.FirstOrDefault(x => x.Id == course.instructor).UserName,
                CreatingDate = course.creating_date,
                Catogery = course.category,
                AllowBuying = course.allow_buying,
                Cost = course.cost,
                Available = course.availlable,
                course.Archived,
                MembersCount = course.Course_Members.Count,
                CourseTags = course.course_tags.ToList(),
                CourseMembers = course.Course_Members.ToList()
            };
            return Ok(my_course);
        }

        // POST: api/courses
        [ResponseType(typeof(course))]
        public async Task< IHttpActionResult> Postcourse(course course)
        {
           // course.instructor=
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Course_Members cm = new Course_Members();
            course_tags ct = new course_tags();
            ct.course_id = course.id;
            //ct.tag_id=
            cm.Course_id = course.id;
            cm.Member_id = course.instructor;
            cm.AddingDate = course.creating_date;


            //send notification to all friends
            var friends = db.friends.Where(x => x.UserId == course.instructor&&x.Confirmed==true).Select(x => x.friendId);
            var friends2 = db.friends.Where(x => x.friendId == course.instructor&&x.Confirmed==true).Select(x => x.UserId);
            friends.Concat(friends2);
            //
            var new_notifi = new cls_notifi
            {
                source_name = "course",
                source_id = db.courses.OrderByDescending(x=>x.id).FirstOrDefault().id.ToString(),
                image = "no image",
                body_English = "Course " + course.name+ " is available now by your friend " + db.AspNetUsers.FirstOrDefault(x => x.Id == course.instructor).UserName ,
                body_Arabic = "تمت اتاحة كورس " + course.name + " من قبل صديقك  " + db.AspNetUsers.FirstOrDefault(x => x.Id == course.instructor).UserName,
                timestamp = DateTime.UtcNow,
                readed = false
            };
            foreach (var item in friends)
            {
                await Push(new_notifi, "notifications/" + item + "/" + DateTime.UtcNow.ToString("dd-MM-yyyy"));
                PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id== item).DeviceToken, "New Course", new_notifi.body_English, "course", db.courses.OrderByDescending(x => x.id).FirstOrDefault().id.ToString());

            }


            /////// Add Chat Room for the Course ///////////////////////////////////////////////////////////





            IFirebaseClient client = new FirebaseClient(config);
            cls_room new_room =new cls_room { name =course.name,about= "Chat Room for " + course.name + " Course",admin=course.instructor,photo="" };

            PushResponse response = await client.PushAsync("chat/rooms/", new_room);
            var xcv = response.Result.name;
            var firstMem = new
            {
                mem_id = course.instructor,//db.AspNetUsers.FirstOrDefault(x => x.UserName == room.adminName).Id,
                mem_name = db.AspNetUsers.FirstOrDefault(x => x.Id == course.instructor).UserName//room.adminName
            };
            await PushChat(firstMem, "chat/members/" + xcv);
            /////////////////////////////
            var new_room2 = new
            {
                RoomId = xcv,
                RoomName = new_room.name,
                about = "Chat room for "+course.name+" course",
                AddedOn = DateTime.UtcNow,
                RoomType = "course",
                RelatedTypeId= db.courses.OrderByDescending(x => x.id).FirstOrDefault().id
            };
            await PushChat(new_room2, "chat/memberRooms/" + course.instructor);


           
            course.room_id = xcv;
            db.courses.Add(course);
            db.Course_Members.Add(cm);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = course.id }, course);
        }

        // DELETE: api/courses/5
        [ResponseType(typeof(course))]
        public IHttpActionResult Deletecourse(int id)
        {
            course course = db.courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            db.courses.Remove(course);
            db.SaveChanges();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool courseExists(int id)
        {
            return db.courses.Count(e => e.id == id) > 0;
        }
    }
}