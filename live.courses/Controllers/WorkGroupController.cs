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
    public class WorkGroupController : ApiController
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

        [HttpPost]
        public IHttpActionResult uploadVideo(int group_id)
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
                var groupTo_edit = db.work_group.FirstOrDefault(x => x.id == group_id);
                groupTo_edit.video = "~/Content/uploads/videos/" + filename;
                db.Entry(groupTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
                file.SaveAs(path);
            }
            var group_to_return = db.work_group.Where(x => x.id == group_id).Select(x => new { x.id, x.name, x.video });
            return Ok(group_to_return);

        }




        // Archive A work group
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult ArchiveGroup(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.archive_work_group(id);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!work_groupExists(id))
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

        //Un Archive A work group
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult UnArchiveGroup(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.un_archive_work_group(id);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!work_groupExists(id))
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
        //get All archived work groups for A user
        public IEnumerable<get_all_archived_work_groups_for_user_Result> GetAllArchivedGroupsForUser(string id)
        {
            return db.get_all_archived_work_groups_for_user(id).ToList();
        }
        //get All Un archived work groups for A user
        public IEnumerable<get_all_un_archived_work_groups_for_user_Result> GetAllUnArchivedGroupsForUser(string id)
        {
            return db.get_all_un_archived_work_groups_for_user(id).ToList();
        }
        //get All archived work groups
        public IEnumerable<Get_all_archived_groups_Result> GetAllArchivedGroups()
        {
            return db.Get_all_archived_groups().ToList();
        }
        //get All un archived work groups
        public IEnumerable<Get_all_unarchived_groups_Result> GetAllUnArchivedGroups()
        {
            return db.Get_all_unarchived_groups().ToList();
        }
        //get all tags
        public IEnumerable<get_all_work_group_tags_Result> Gettags(int id)
        {
            return db.get_all_work_group_tags(id).ToList();
        }
        // GET: api/WorkGroup
        public IQueryable<work_group> Getwork_group()
        {
            return db.work_group;
        }

        // GET: api/WorkGroup/5
        //[Authorize]
        [ResponseType(typeof(work_group))]
        public IHttpActionResult Getwork_group(int id,string user_id)
        {
            work_group work_group = db.work_group.Find(id);
            if (work_group == null)
            {
                return NotFound();
            }
            var current_user = db.AspNetUsers.FirstOrDefault(x => x.Id == user_id);
            var liked = db.likes.FirstOrDefault(x => x.liked_id == id && x.liked_type == "group" && x.user_id == current_user.Id);
            var is_liked = false;
            if (liked != null)
            {
                is_liked = true;
            }
            var my_group = new
            {
                id = work_group.id,
                name = work_group.name,
                about = work_group.about,
                TimeRange = work_group.trange,
                VideoLink = work_group.video,
                WayToApply = work_group.way_to_apply,
                Notes = work_group.notes,
                Place = work_group.place,
                AdminId = work_group.admin,
                AdminName = db.AspNetUsers.FirstOrDefault(x => x.Id == work_group.admin).UserName,
                CreatingDate = work_group.creating_date,
                Catogery = work_group.category,
                work_group.Archived,
                Available = work_group.availlable,
                MembersCount = work_group.Work_group_members.Count,
                groupTags = work_group.work_group_tags.Select(x=>x.tag_name).ToList(),
                is_liked
                //groupMembers = work_group.Work_group_members.ToList()
            };
            return Ok(my_group);
        }

        // PUT: api/WorkGroup/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putwork_group(int id, work_group work_group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != work_group.id)
            {
                return BadRequest();
            }

            db.Entry(work_group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!work_groupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var my_group = new
            {
                id = work_group.id,
                name = work_group.name,
                about = work_group.about,
                TimeRange = work_group.trange,
                VideoLink = work_group.video,
                WayToApply = work_group.way_to_apply,
                Notes = work_group.notes,
                Place = work_group.place,
                AdminId = work_group.admin,
                AdminName = db.AspNetUsers.FirstOrDefault(x => x.Id == work_group.admin).UserName,
                CreatingDate = work_group.creating_date,
                Catogery = work_group.category,
                work_group.Archived,
                Available = work_group.availlable,
                MembersCount = work_group.Work_group_members.Count,
                groupTags = work_group.work_group_tags.Select(x => x.tag_name).ToList()
                //groupMembers = work_group.Work_group_members.ToList()
            };
            return Ok(my_group);
        }

        // POST: api/WorkGroup
        [ResponseType(typeof(work_group))]
        public async Task<IHttpActionResult> Postwork_group(work_group work_group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           





            /////// Add Chat Room for the Course ///////////////////////////////////////////////////////////





            IFirebaseClient client = new FirebaseClient(config);
            cls_room new_room = new cls_room { name = work_group.name, about = "Chat Room for " + work_group.name + " Group", admin = work_group.admin, photo = "" };

            PushResponse response = await client.PushAsync("chat/rooms/", new_room);
            var xcv = response.Result.name;
            var firstMem = new
            {
                mem_id = work_group.admin,//db.AspNetUsers.FirstOrDefault(x => x.UserName == room.adminName).Id,
                mem_name = db.AspNetUsers.FirstOrDefault(x => x.Id == work_group.admin).UserName//room.adminName
            };
            await PushChat(firstMem, "chat/members/" + xcv);
            /////////////////////////////
            var new_room2 = new
            {
                RoomId = xcv,
                RoomName = new_room.name,
                about = "Chat room for " + work_group.name + " group",
                AddedOn = DateTime.UtcNow,
                RoomType = "group",
                RelatedTypeId = db.work_group.OrderByDescending(x => x.id).FirstOrDefault().id
            };
            await PushChat(new_room2, "chat/memberRooms/" + work_group.admin);
            work_group.room_id = xcv;
            db.work_group.Add(work_group);
            db.SaveChanges();

            Work_group_members wg = new Work_group_members();
            wg.Member_id = work_group.admin;
            wg.Work_group_id = db.work_group.OrderByDescending(x=>x.id).FirstOrDefault().id;
            wg.AddingDate = System.DateTime.Now;
            db.Work_group_members.Add(wg);
            db.SaveChanges();




            //////////////////////////////////////////////////////////////////////////////////////////////
            //send notification to all friends
            var friends = db.friends.Where(x => x.UserId == work_group.admin && x.Confirmed == true).Select(x => x.friendId);
            var friends2 = db.friends.Where(x => x.friendId == work_group.admin && x.Confirmed == true).Select(x => x.UserId);
            friends.Concat(friends2);
            //
            var new_notifi = new cls_notifi
            {
                source_name = "group",
                source_id = db.work_group.OrderByDescending(x => x.id).FirstOrDefault().id.ToString(),
                image = "no image",
                body_English = "Work group " + work_group.name + " is available now by your friend " + db.AspNetUsers.FirstOrDefault(x => x.Id == work_group.admin).UserName,
                body_Arabic = "تمت اتاحة مجموعة " + work_group.name + " من قبل صديقك " + db.AspNetUsers.FirstOrDefault(x => x.Id == work_group.admin).UserName,
                timestamp = DateTime.UtcNow,
                readed = false
            };
            foreach (var item in friends)
            {
                await Push(new_notifi, "notifications/" + item + "/" + DateTime.UtcNow.ToString("dd-MM-yyyy"));
                PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id == item).DeviceToken, "New Workgroup", new_notifi.body_English, "group", db.work_group.OrderByDescending(x => x.id).FirstOrDefault().id.ToString());

            }




            return CreatedAtRoute("DefaultApi", new { id = work_group.id }, work_group);
        }

        // DELETE: api/WorkGroup/5
        [ResponseType(typeof(work_group))]
        public IHttpActionResult Deletework_group(int id)
        {
            work_group work_group = db.work_group.Find(id);
            if (work_group == null)
            {
                return NotFound();
            }

            db.work_group.Remove(work_group);
            db.SaveChanges();

            return Ok(work_group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool work_groupExists(int id)
        {
            return db.work_group.Count(e => e.id == id) > 0;
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