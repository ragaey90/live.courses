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
    public class FriendsController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();
        private void PushNotifi(string deviceToken, string title, string body,string type,string type_id)
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
                        source_type=type,
                        source_id=type_id,
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

        //Remove friend
        [HttpDelete]
        [ResponseType(typeof(friend))]
        public IHttpActionResult Deletefriend(string User1,string User2)
        {
            friend friend1 = db.friends.Find(User1,User2);
            friend friend2 = db.friends.Find(User2, User1);
            if (friend1 == null&&friend2==null)
            {
                return NotFound();
            }

     //       db.friends.Remove(friend);

            db.remove_friend_or_frequest(User1, User2);
            db.SaveChanges();
            if (friend1==null)
	        {
		        return Ok(friend2);
	        }
            else
	        {
                return Ok(friend1);
	        }
        }
       //  POST send friend Request
        [HttpPost]
        [ResponseType(typeof(friend))]
        public async Task<IHttpActionResult> SendRequest(friend friend)
        {
            if (!ModelState.IsValid || friend.friendId==friend.UserId)
            {
                return BadRequest(ModelState);
            }
            db.send_friend_request(friend.UserId, friend.friendId, friend.since);
            //db.friends.Add(friend);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (friendExists(friend.UserId,friend.friendId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            var new_notifi = new cls_notifi {
                    source_name = "user",
                    source_id = friend.friendId,
                    image = db.AspNetUsers.FirstOrDefault(x => x.Id == friend.friendId).Photo,//"no image",
                    body_English = db.AspNetUsers.FirstOrDefault(x=>x.Id== friend.friendId).UserName+ " send you a friend request",
                body_Arabic = "قام "+ db.AspNetUsers.FirstOrDefault(x => x.Id == friend.friendId).UserName + " بارسال طلب صداقة اليك",

                timestamp = DateTime.Now,
                    readed = false
                 };
             await Push(new_notifi, "notifications/" + friend.UserId.ToString()+"/"+DateTime.UtcNow.ToString("dd-MM-yyyy"));
            PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id == friend.UserId).DeviceToken, "New Friend Request", new_notifi.body_English, "user", friend.friendId);
            //return RedirectToRoute("~/rpc/notifi/push", new { notifi= new_notifi, path = "notifications/" + friend.UserId.ToString() + "/friends" });
            return CreatedAtRoute("DefaultApi", new { id = friend.UserId }, friend);
        }
        //Accept friend Request 
        [HttpPut]
        public async Task<IHttpActionResult> AcceptRequest(string Sender,string Reciver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.accept_friend_request(Reciver, Sender);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!friendExists(Sender,Reciver))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var new_notifi = new cls_notifi
            {
                source_name = "user",
                source_id = Reciver,
                image = db.AspNetUsers.FirstOrDefault(x => x.Id == Reciver).Photo,//"no image",
                body_English = db.AspNetUsers.FirstOrDefault(x => x.Id == Reciver).UserName + " accepted your friend request",
                body_Arabic = "تم قبول طلب صداقتك من قبل "+ db.AspNetUsers.FirstOrDefault(x => x.Id == Reciver).UserName ,
                timestamp = DateTime.Now,
                readed = false
            };
            await Push(new_notifi, "notifications/" + Sender.ToString() + "/" + DateTime.UtcNow.ToString("dd-MM-yyyy"));
            PushNotifi(db.AspNetUsers.FirstOrDefault(x => x.Id == Sender.ToString()).DeviceToken, "Friend Request Accepted", new_notifi.body_English,"user", Reciver);

            //RedirectToRoute("/rpc/notifi/push/", new { notifi = new_notifi, path = "notifications/" + Sender.ToString() + "/friends" });
            return StatusCode(HttpStatusCode.NoContent);
        }

        //get All friends for a user
        public IQueryable<cls_friend> GetFriends(string id)
        {
            var frind1 = db.friends.Where(x => x.UserId == id&&x.Confirmed==true).Select(x => new cls_friend
            {
                friendId = x.friendId,
                Photo=db.AspNetUsers.FirstOrDefault(y=>y.Id==x.friendId).Photo,
                UserName= db.AspNetUsers.FirstOrDefault(y => y.Id == x.friendId).UserName
            });
            var frind2 = db.friends.Where(x => x.friendId == id&&x.Confirmed == true).Select(x => new cls_friend
            {
                friendId = x.UserId,
                Photo = db.AspNetUsers.FirstOrDefault(y => y.Id == x.UserId).Photo,
                UserName = db.AspNetUsers.FirstOrDefault(y => y.Id == x.UserId).UserName
            });
            var allFriends = frind1.Concat(frind2);
            //return db.get_all_friends(id).ToList();
            return allFriends;
        }
        //public IEnumerable<friend> GetFriends(string id)
        //{
        // var friends = db.friends.Where(x => x.UserId == id && x.Confirmed == true).Select(x => new { x.friendId, x.since });

        //    foreach (var item in friends)
        //    {

        //    }
        //    return ;
        //}
        //get All friend Request for a user

        public IEnumerable<cls_friend> GetFriendRequests(string id)
        {
            //friend_id هو اللي بيبعت طلب الصداقة

            var frind1 = db.friends.Where(x => x.UserId == id && x.Confirmed == false).Select(x => new cls_friend
            {
                friendId = x.friendId,
                Photo = db.AspNetUsers.FirstOrDefault(y => y.Id == x.friendId).Photo,
                UserName = db.AspNetUsers.FirstOrDefault(y => y.Id == x.friendId).UserName
            });
            //var frind2 = db.friends.Where(x => x.friendId == id && x.Confirmed == false).Select(x => new cls_friend
            //{
            //    friendId = x.UserId,
            //    Photo = db.AspNetUsers.FirstOrDefault(y => y.Id == x.UserId).Photo,
            //    UserName = db.AspNetUsers.FirstOrDefault(y => y.Id == x.UserId).UserName
            //});
            var allFriends = frind1;// frind1.Concat(frind2);
            //return db.get_all_friends(id).ToList();
            return allFriends;
            //  return db.get_all_friend_requests(id).ToList();
        }

        // GET api/Friends/5
        [ResponseType(typeof(friend))]
        public IHttpActionResult Getfriend(string id)
        {
            friend friend = db.friends.Find(id);
            if (friend == null)
            {
                return NotFound();
            }

            return Ok(friend);
        }


        public IHttpActionResult GetStrangers(string id)
        {
            var frind1 = db.friends.Where(x => x.UserId == id).Select(x =>  
            
                 x.friendId
            ).ToList();
            var frind2 = db.friends.Where(x => x.friendId == id ).Select(x =>  
            
                 x.UserId
            );
            List<string> allFriends = frind1.Concat(frind2).ToList();
            //var allUsers = db.AspNetUsers.Select(x=> x.Id).ToList();
            var strangers = db.AspNetUsers.Where(x => !allFriends.Contains(x.Id)).Select(x=>new { x.Id,x.UserName,x.Photo});

            return Ok(strangers);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool friendExists(string id,string fr_id)
        {
            return db.friends.Count(e => e.UserId == id &&e.friendId==fr_id||  e.UserId == fr_id && e.friendId == id) > 0;
        }
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
    }
}