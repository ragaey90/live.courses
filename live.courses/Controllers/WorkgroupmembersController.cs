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
    public class WorkgroupmembersController : ApiController
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
        public IHttpActionResult SearchStrangers(int id, string search_text)
        {
            var members = db.Work_group_members.Where(x => x.Work_group_id == id).Select(x => x.Member_id);

            var strangers = db.AspNetUsers.Where(x => !members.Contains(x.Id)).Select(x => new { x.Id, x.UserName, x.Photo });
            var result = strangers.Where(x => x.UserName.Contains(search_text));

            return Ok(result);
        }
        public IHttpActionResult GetStrangers(int id)
        {
            var members = db.Work_group_members.Where(x => x.Work_group_id == id).Select(x => x.Member_id);

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
        // GET: api/Workgroupmembers
        //public IQueryable<Work_group_members> GetWork_group_members()
        //{
        //    return db.Work_group_members;
        //}

        // GET: api/Workgroupmembers/5
        [ResponseType(typeof(Work_group_members))]
        public IHttpActionResult GetWork_group_members(int id)
        {
            var work_group_members = db.Work_group_members.Where(x=>x.Work_group_id==id).Select(x=>new {x.Member_id,x.Work_group_id,x.AddingDate,x.AspNetUser.UserName,x.AspNetUser.Photo });
            if (work_group_members == null)
            {
                return NotFound();
            }

            return Ok(work_group_members);
        }

        // PUT: api/Workgroupmembers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWork_group_members( Work_group_members work_group_members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Entry(work_group_members).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Work_group_membersExists(work_group_members.Work_group_id,work_group_members.Member_id))
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

        // POST: api/Workgroupmembers
        [ResponseType(typeof(Work_group_members))]
        public async Task<IHttpActionResult> PostWork_group_members(Work_group_members work_group_members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Work_group_members.Add(work_group_members);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Work_group_membersExists(work_group_members.Work_group_id, work_group_members.Member_id))
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
                source_id = work_group_members.Member_id,
                image = db.AspNetUsers.FirstOrDefault(x => x.Id == work_group_members.Member_id).Photo,//"no image",
                body_English = db.AspNetUsers.FirstOrDefault(x => x.Id == work_group_members.Member_id).UserName + " Joined to Your work group (" + db.work_group.FirstOrDefault(x => x.id == work_group_members.Work_group_id).name + ")",
                body_Arabic ="انضم "+ db.AspNetUsers.FirstOrDefault(x => x.Id == work_group_members.Member_id).UserName + " الي جروب (" + db.work_group.FirstOrDefault(x => x.id == work_group_members.Work_group_id).name + ")",
                timestamp = DateTime.Now,
                readed = false
            };
            await Push(new_notifi, "notifications/" + db.work_group.FirstOrDefault(x => x.id == work_group_members.Work_group_id).admin + "/" + DateTime.UtcNow.ToString("dd-MM-yyyy"));
            PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id == db.courses.FirstOrDefault(Y =>Y.id == work_group_members.Work_group_id).instructor).DeviceToken, "New Member To your Group", new_notifi.body_English, "user", work_group_members.Member_id);
            //////////////////////////////////////////////////////////////////////add member to room 
            IFirebaseClient client = new FirebaseClient(config);

            var Course_room_id = db.work_group.FirstOrDefault(x => x.id == work_group_members.Work_group_id).room_id;
            var new_Mem = new
            {
                mem_id = work_group_members.Member_id,
                mem_name = db.AspNetUsers.FirstOrDefault(x => x.Id == work_group_members.Member_id).UserName
            };
            var new_room = new
            {
                RoomId = Course_room_id,
                RoomName = db.work_group.FirstOrDefault(x => x.id == work_group_members.Work_group_id).name,
                AddedOn = DateTime.UtcNow,
                about = work_group_members.work_group.about,
                RoomType = "group",
                RelatedTypeId = work_group_members.Work_group_id
            };

            await Push_for_room(new_Mem, "chat/members/" + Course_room_id);
            await Push_for_room(new_room, "chat/memberRooms/" + work_group_members.Member_id);

            //////////////////////////////////////////////////////////////////////////////////

            return CreatedAtRoute("DefaultApi", new { id = work_group_members.Work_group_id }, work_group_members);
        }

        // DELETE: api/Workgroupmembers/5
        [ResponseType(typeof(Work_group_members))]
        [HttpPost]
        public IHttpActionResult LeaveWork_group(int wg_id, string member_id)
        {
            Work_group_members work_group_members = db.Work_group_members.Find(wg_id,member_id);
            if (work_group_members == null)
            {
                return NotFound();
            }

            db.Work_group_members.Remove(work_group_members);
            db.SaveChanges();

            return Ok(work_group_members);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Work_group_membersExists(int id,string mem_id)
        {
            return db.Work_group_members.Count(e => e.Work_group_id == id&&e.Member_id==mem_id) > 0;
        }
    }
}