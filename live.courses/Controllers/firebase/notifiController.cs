using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using live.courses.Models;
using live.courses.classesForReturn;
using Newtonsoft.Json;
using System.Text;
using System.Web.Helpers;
using static System.Net.Mime.MediaTypeNames;
using live.courses.chat_classes;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.IO;

namespace live.courses.Controllers.firebase
{
    public class notifiController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "1NvMs4WAnA4a1NdIvEkfek5YBaPZGN0yLlwCeQI9",
            BasePath = "https://livecoursechat.firebaseio.com"
        };
        public async Task set()
        {
            IFirebaseClient client = new FirebaseClient(config);
            var my_notifi = new notification
            {
                id=1,
                user_id = "",//db.AspNetUsers.FirstOrDefault(x => x.UserName == User.Identity.Name).Id,
                source_name = "app",
                source_id = 0,
                image = "no image",
                body = "this is the first notification in the app",
                timestamp = DateTime.Now,
                readed=false
            };
            SetResponse response = await client.SetAsync("my_notifi/set", my_notifi);
            notification result = response.ResultAs<notification>(); //The response will contain the data written
        }
        [Authorize]
        [HttpPost]
        public async Task Push(cls_notifi notifi,string path)
        {
            IFirebaseClient client = new FirebaseClient(config);
            var my_notifi = notifi;
            PushResponse response = await client.PushAsync(path, my_notifi);
            var xcv= response.Result.name; //The result will contain the child name of the new data that was added
        }
        public async Task Get()
        {
            IFirebaseClient client = new FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("chat/rooms");
            //JObject jObj = JObject.Parse(response.Body);
            //accessToken = jObj["access_token"].ToString();
            var todo = response.ResultAs<notification>(); //The response will contain the data being retreived
        }
        public async Task Update()
        {
            IFirebaseClient client = new FirebaseClient(config);
            var my_notifi = new notification
            {
                user_id = db.AspNetUsers.FirstOrDefault(x => x.UserName == User.Identity.Name).Id,
                source_name = "app",
                source_id = 0,
                image = "no image",
                body = "this is the first notification update in the app",
                timestamp = DateTime.Now,
                readed = false
            };
            FirebaseResponse response = await client.UpdateAsync("my_notifi/set", my_notifi);
            notification todo = response.ResultAs<notification>(); //The response will contain the data written
        }
        public async Task Listen()
        {
            IFirebaseClient client = new FirebaseClient(config);

            EventStreamResponse response = await client.OnAsync("chat", (sender, args, context) => {
                System.Console.WriteLine(args.Data);
            });

            //Call dispose to stop listening for events
            //response.Dispose();
        }

        private void PushNotifi(string deviceToken,string title,string body)
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
