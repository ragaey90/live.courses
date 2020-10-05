using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using live.courses.Models;

namespace live.courses.Controllers.mvc_controllers
{
    public class mvcUsersController : Controller
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        // GET: mvcUsers
        public ActionResult Accept_instructor(string id)
        { 
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            aspNetUser.IsInstructor = true;
            db.Entry(aspNetUser).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Remove_instructor(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            aspNetUser.IsInstructor = false;
            db.Entry(aspNetUser).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Get_All_instructors()
        {
            return View("Index", db.AspNetUsers.Where(x=>x.IsInstructor==true).ToList());
        }
        public ActionResult Get_All_Non_instructors()
        {
            return View("Index", db.AspNetUsers.Where(x => x.IsInstructor != true).ToList());
        }
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        // GET: mvcUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: mvcUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mvcUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Country,State,Gender,Photo,Apout,AnotherAccount,IsInstructor,GuaranteeDecument,IdCard,Residence,Photograph,IdCardBack,facebookAccount,twitterAccount,jobTitle,DeviceToken")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: mvcUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: mvcUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Country,State,Gender,Photo,Apout,AnotherAccount,IsInstructor,GuaranteeDecument,IdCard,Residence,Photograph,IdCardBack,facebookAccount,twitterAccount,jobTitle,DeviceToken")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: mvcUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: mvcUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
