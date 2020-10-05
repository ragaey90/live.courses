using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using live.courses.chat_classes;
using live.courses.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace live.courses.Controllers.firebase
{
    public class chatController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "LVcDfpyKKnIViyvnUCDzVK6pGzNsEVfFxrXghhiS",//"1NvMs4WAnA4a1NdIvEkfek5YBaPZGN0yLlwCeQI9",
            BasePath = "https://startup-livecourse.firebaseio.com/"//"https://livecoursechat.firebaseio.com"
        };
        public async Task<IHttpActionResult> Push(object thingToPush, string path)
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

        [HttpGet]
        public async Task<IHttpActionResult> SearchStrangers(string RoomId, string search_text)
        {
            IFirebaseClient client = new FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("chat/members/" + RoomId);
            // mems MembersFromFireBase = response.ResultAs<mems>(); //The response will contain the data being retreived
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<mems>();
            foreach (var itemDynamic in data)
            {
                list.Add(JsonConvert.DeserializeObject<mems>(((JProperty)itemDynamic).Value.ToString()));
            }
            var members = list.Select(x => x.mem_id);

            var strangers = db.AspNetUsers.Where(x => !members.Contains(x.Id)).Select(x => new { x.Id, x.UserName, x.Photo });
            
            var result = strangers.Where(x => x.UserName.Contains(search_text));
            return Ok(result);
        }
        public async Task<IHttpActionResult> GetStrangers( string RoomId)
        {
            IFirebaseClient client = new FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("chat/members/"+RoomId);
           // mems MembersFromFireBase = response.ResultAs<mems>(); //The response will contain the data being retreived
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<mems>();
            foreach (var itemDynamic in data)
            {
                list.Add(JsonConvert.DeserializeObject<mems>(((JProperty)itemDynamic).Value.ToString()));
            }
            var members = list.Select(x => x.mem_id);

            var strangers = db.AspNetUsers.Where(x => !members.Contains(x.Id)).Select(x => new { x.Id, x.UserName, x.Photo });
            return Ok(strangers);
        }

        public async Task<IHttpActionResult> addRoom(cls_chat_room room)
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
                    HttpContext.Current.Server.MapPath("~/uploads/images/"),
                    filename
                );
                room.photo = "~/uploads/images/" + filename;
                file.SaveAs(path);
            }




            IFirebaseClient client = new FirebaseClient(config);
            var new_room = room;

            PushResponse response = await client.PushAsync("chat/rooms/", new_room);
            var xcv = response.Result.name;
            var firstMem = new
            {
                mem_id = db.AspNetUsers.FirstOrDefault(x => x.UserName == room.adminName).Id,
                mem_name = room.adminName
            };
            await Push(firstMem, "chat/members/" +xcv);
            /////////////////////////////
            var new_room2 = new
            {
                RoomId = xcv,
                RoomName = room.name,
                about=room.about,
                AddedOn = DateTime.UtcNow,
                RoomType = "ordinary-room",
                RelatedTypeId = 0
            };
            await Push(new_room2, "chat/memberRooms/" + db.AspNetUsers.FirstOrDefault(x => x.UserName == room.adminName).Id);

            return Ok(room);
        }

        public async Task<IHttpActionResult> addMemperToRoom(string room_id,string room_name, string memper_id)
        {
            IFirebaseClient client = new FirebaseClient(config);

            
            FirebaseResponse response = await client.GetAsync("chat/members/" + room_id);
           // var response_new= JsonConvert.DeserializeObject<cls_room_member[,,]>(response.Body.ToString());
           ////var checkExists= response_new.Where(rd => rd.mem_id == memper_id).ToList();
           // var checkExists = response.ResultAs<List<cls_room_member>>().Where(rd => rd.mem_id== memper_id).ToList();
            // response.ResultAs<cls_chat_room>();


            var new_Mem = new
            {
                mem_id = memper_id,
                mem_name = db.AspNetUsers.FirstOrDefault(x => x.Id == memper_id).UserName
            };
            var new_room = new
            {
                RoomId = room_id,
                RoomName=room_name,
                about = "",
                AddedOn = DateTime.UtcNow,
                RoomType = "ordinary-room",
                RelatedTypeId = 0
            };
            if (!response.Body.Contains(memper_id))
            {
                await Push(new_Mem, "chat/members/" + room_id);
                await Push(new_room, "chat/memberRooms/" + memper_id);
            }
            else
            {
                return StatusCode(HttpStatusCode.Conflict);
            }
            return Ok(new_Mem);
        }

        public async Task<IHttpActionResult> sendMessage(cls_msg _Msg)
        {
            IFirebaseClient client = new FirebaseClient(config);
            if (_Msg.File != null)
            {
                string filename = Guid.NewGuid().ToString();// Path.GetFileNameWithoutExtension(post1.imagefile.FileName);
                string extention = Path.GetExtension(_Msg.File.FileName);
                filename = filename + DateTime.Now.ToString("yymmddssfff") + extention;
                _Msg.message = "~/Content/image/" + filename;
                filename = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/image/"), filename);
                //other way
                // Path.Combine(HttpContext.Current.Server.MapPath("~/Content/image/"), filename);
                _Msg.File.SaveAs(filename);
            }
            var new_msg = new 
            {
                sender = db.AspNetUsers.FirstOrDefault(x => x.Id == _Msg.sender_id).UserName,
                sender_photo = db.AspNetUsers.FirstOrDefault(x => x.Id == _Msg.sender_id).Photo,
                message = _Msg.message,
                timeStamp=DateTime.Now,
                type= _Msg.MsgType
            };
            await Push(new_msg, "chat/messages/" + _Msg.Room_id);
            ////////////////////////////////////////////////////////////////////////////////////////////
            FirebaseResponse response_ids = await client.GetAsync("chat/members/" + _Msg.Room_id);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response_ids.Body);
            var list = new List<mems>();
            foreach (var itemDynamic in data)
            {
                list.Add(JsonConvert.DeserializeObject<mems>(((JProperty)itemDynamic).Value.ToString()));
            }
            var members = list.Where(x=>x.mem_id!=_Msg.sender_id).Select(x => x.mem_id);
            foreach (var item in members)
            {
                PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id == item).DeviceToken, "new message from " + new_msg.sender, _Msg.message, "room", _Msg.Room_id);

            }
            /////////////////////////////////////////////////////////////////////////////////////////////
            FirebaseResponse response = await client.GetAsync("chat/rooms/"+ _Msg.Room_id);
            cls_chat_room todo = response.ResultAs<cls_chat_room>();
            todo.lastMessage = _Msg.message;
            todo.timeStamp = DateTime.Now;

            FirebaseResponse response2 = await client.UpdateAsync("chat/rooms/" + _Msg.Room_id, todo);

            return Ok(new_msg);
        }

    }
}
