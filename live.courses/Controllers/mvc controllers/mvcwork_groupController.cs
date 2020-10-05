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
    public class mvcwork_groupController : Controller
    {
        private adv_coursesEntities db = new adv_coursesEntities();

        // GET: mvcwork_group
        public ActionResult Index()
        {
            return View(db.work_group.ToList());
        }

        // GET: mvcwork_group/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            work_group work_group = db.work_group.Find(id);
            if (work_group == null)
            {
                return HttpNotFound();
            }
            return View(work_group);
        }

        // GET: mvcwork_group/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mvcwork_group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,about,creating_date,place,trange,availlable,way_to_apply,notes,admin,category,video,Archived")] work_group work_group)
        {
            if (ModelState.IsValid)
            {
                db.work_group.Add(work_group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(work_group);
        }

        // GET: mvcwork_group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            work_group work_group = db.work_group.Find(id);
            if (work_group == null)
            {
                return HttpNotFound();
            }
            return View(work_group);
        }

        // POST: mvcwork_group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,about,creating_date,place,trange,availlable,way_to_apply,notes,admin,category,video,Archived")] work_group work_group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work_group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(work_group);
        }

        // GET: mvcwork_group/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            work_group work_group = db.work_group.Find(id);
            if (work_group == null)
            {
                return HttpNotFound();
            }
            return View(work_group);
        }

        // POST: mvcwork_group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            work_group work_group = db.work_group.Find(id);
            db.work_group.Remove(work_group);
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
