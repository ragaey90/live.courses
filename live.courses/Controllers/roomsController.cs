using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using live.courses.classesForReturn;
using live.courses.Models;

namespace live.courses.Controllers
{
    public class roomsController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();


        public IEnumerable<cls_room> GetUserRooms(string UserId)
        {
            var rooms_id = db.Room_Members.Where(x => x.Member == UserId).Select(x => x.Room_id);
            List<room> userRooms = new List<room>();
            cls_room cls_Room = new cls_room();
            foreach (var item in rooms_id)
            {
                userRooms.Add(db.rooms.FirstOrDefault(x => x.id == item));

            }
            return userRooms.Select(x => new cls_room()
            {
                id = x.id,
                name = x.name,
                photo = x.photo,
                about = x.about,
                admin = x.admin
            }).ToList();
            
        }

        //public IEnumerable<cls_room> GetUserRooms(string UserId)
        //{
        //    var rooms_id = db.Room_Members.Where(x => x.Member == UserId).Select(x=>x.Room_id);
        //    List<room> userRooms=new List<room>();
        //    List<cls_room> rooms_for_return = new List<cls_room>();
        //    cls_room cls_Room = new cls_room();
        //    foreach (var item in rooms_id)
        //    {
        //        userRooms.Add(db.rooms.FirstOrDefault(x => x.id == item));

        //    }
        //    if (userRooms.Any())
        //    {
        //        foreach (var item in userRooms)
        //        {
        //            cls_Room = new cls_room();
        //            cls_Room.id = item.id;
        //            cls_Room.name = item.name;
        //            cls_Room.photo = item.photo;
        //            cls_Room.about = item.about;
        //            cls_Room.admin = item.admin;
        //            rooms_for_return.Add(cls_Room);
        //        }
        //    }
        //    return rooms_for_return;
        //}
        // GET: api/rooms
        public IQueryable<room> Getrooms()
        {
            return db.rooms;
        }

        // GET: api/rooms/5
        [ResponseType(typeof(room))]
        public IHttpActionResult Getroom(int id)
        {
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // PUT: api/rooms/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putroom(int id, room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.id)
            {
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!roomExists(id))
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

        // POST: api/rooms
        [ResponseType(typeof(room))]
        public IHttpActionResult Postroom(room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Room_Members rm = new Room_Members();
            rm.Room_id = room.id;
            rm.Member = room.admin;
            rm.AddingDate = DateTime.Now;
            db.Room_Members.Add(rm);
            db.rooms.Add(room);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (roomExists(room.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = room.id }, room);
        }

        // DELETE: api/rooms/5
        [ResponseType(typeof(room))]
        public IHttpActionResult Deleteroom(int id)
        {
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            db.rooms.Remove(room);
            db.SaveChanges();

            return Ok(room);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool roomExists(int id)
        {
            return db.rooms.Count(e => e.id == id) > 0;
        }
    }
}