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
using live.courses.Models;

namespace live.courses.Controllers
{
    public class RoomMembersController : ApiController
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        // GET: api/RoomMembers
        //public IQueryable<Room_Members> GetRoom_Members()
        //{
        //    return db.Room_Members;
        //}

        // GET: api/RoomMembers/5
        [ResponseType(typeof(Room_Members))]
        public IHttpActionResult GetRoom_Members(int id)
        {
            Room_Members room_Members = db.Room_Members.Find(id);
            if (room_Members == null)
            {
                return NotFound();
            }

            return Ok(room_Members);
        }

        // PUT: api/RoomMembers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRoom_Members( Room_Members room_Members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(room_Members).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Room_MembersExists(room_Members.Room_id,room_Members.Member))
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

        // POST: api/RoomMembers
        [ResponseType(typeof(Room_Members))]
        public IHttpActionResult PostRoom_Members(Room_Members room_Members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Room_Members.Add(room_Members);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Room_MembersExists(room_Members.Room_id,room_Members.Member))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = room_Members.Room_id }, room_Members);
        }

        // DELETE: api/RoomMembers/5
        [ResponseType(typeof(Room_Members))]
        public IHttpActionResult DeleteRoom_Members(int room_id,string member_id)
        {
            Room_Members room_Members = db.Room_Members.Find(room_id,member_id);
            if (room_Members == null)
            {
                return NotFound();
            }

            db.Room_Members.Remove(room_Members);
            db.SaveChanges();

            return Ok(room_Members);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Room_MembersExists(int id,string mem_id)
        {
            return db.Room_Members.Count(e => e.Room_id == id&&e.Member==mem_id) > 0;
        }
    }
}