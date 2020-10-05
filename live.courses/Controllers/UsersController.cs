using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using live.courses.Classes_for_sp;
using live.courses.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace live.courses.Controllers
{
    public class UsersController : ApiController
    {
        

        private adv_coursesEntities db = new adv_coursesEntities();
        [HttpPost]
        public IHttpActionResult SetDeviceToken(string userId,string DeviceToken)
        {

            var userTo_edit = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            userTo_edit.DeviceToken = DeviceToken;
            db.Entry(userTo_edit).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
            }

            var userToReturn = db.AspNetUsers.Where(x => x.Id == userId).Select(x => new cls_user
            {
                Id = x.Id,
                UserName = x.UserName,
                State = x.State,
                Residence = x.Residence,
                Photograph = x.Photograph,
                Photo = x.Photo,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                PhoneNumber = x.PhoneNumber,
                IsInstructor = x.IsInstructor,
                IdCard = x.IdCard,
                GuaranteeDecument = x.GuaranteeDecument,
                Gender = x.Gender,
                EmailConfirmed = x.EmailConfirmed,
                Email = x.Email,
                Country = x.Country,
                Apout = x.Apout,
                AnotherAccount = x.AnotherAccount,
                IdCardBack = x.IdCardBack,
                facebookAccount = x.facebookAccount,
                twitterAccount = x.twitterAccount,
                jobTitle = x.jobTitle
            });
            return Ok(userToReturn);

        }

        [HttpPost]
        public IHttpActionResult uploadResidence_photo(string userId)
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
                    HttpContext.Current.Server.MapPath("~/Content/uploads/images/"),
                    filename
                );
                var userTo_edit = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
                userTo_edit.Residence = "~/Content/uploads/images/" + filename;
                db.Entry(userTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
                file.SaveAs(path);
            }
            var userToReturn = db.AspNetUsers.Where(x => x.Id == userId).Select(x => new cls_user
            {
                Id = x.Id,
                UserName = x.UserName,
                State = x.State,
                Residence = x.Residence,
                Photograph = x.Photograph,
                Photo = x.Photo,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                PhoneNumber = x.PhoneNumber,
                IsInstructor = x.IsInstructor,
                IdCard = x.IdCard,
                GuaranteeDecument = x.GuaranteeDecument,
                Gender = x.Gender,
                EmailConfirmed = x.EmailConfirmed,
                Email = x.Email,
                Country = x.Country,
                Apout = x.Apout,
                AnotherAccount = x.AnotherAccount,
                IdCardBack = x.IdCardBack,
                facebookAccount = x.facebookAccount,
                twitterAccount = x.twitterAccount,
                jobTitle = x.jobTitle
            });
            return Ok(userToReturn);

        }

        [HttpPost]
        public IHttpActionResult uploadPhotoGraph(string userId)
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
                    HttpContext.Current.Server.MapPath("~/Content/uploads/images/"),
                    filename
                );
                var userTo_edit = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
                userTo_edit.Photograph = "~/Content/uploads/images/" + filename;
                db.Entry(userTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
                file.SaveAs(path);
            }
            var userToReturn = db.AspNetUsers.Where(x => x.Id == userId).Select(x => new cls_user
            {
                Id = x.Id,
                UserName = x.UserName,
                State = x.State,
                Residence = x.Residence,
                Photograph = x.Photograph,
                Photo = x.Photo,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                PhoneNumber = x.PhoneNumber,
                IsInstructor = x.IsInstructor,
                IdCard = x.IdCard,
                GuaranteeDecument = x.GuaranteeDecument,
                Gender = x.Gender,
                EmailConfirmed = x.EmailConfirmed,
                Email = x.Email,
                Country = x.Country,
                Apout = x.Apout,
                AnotherAccount = x.AnotherAccount,
                IdCardBack = x.IdCardBack,
                facebookAccount = x.facebookAccount,
                twitterAccount = x.twitterAccount,
                jobTitle = x.jobTitle
            });
            return Ok(userToReturn);

        }


        [HttpPost]
        public IHttpActionResult uploadIdCardBack(string userId)
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
                    HttpContext.Current.Server.MapPath("~/Content/uploads/images/"),
                    filename
                );
                var userTo_edit = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
                userTo_edit.IdCardBack = "~/Content/uploads/images/" + filename;
                db.Entry(userTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
                file.SaveAs(path);
            }
            var userToReturn = db.AspNetUsers.Where(x => x.Id == userId).Select(x => new cls_user
            {
                Id = x.Id,
                UserName = x.UserName,
                State = x.State,
                Residence = x.Residence,
                Photograph = x.Photograph,
                Photo = x.Photo,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                PhoneNumber = x.PhoneNumber,
                IsInstructor = x.IsInstructor,
                IdCard = x.IdCard,
                GuaranteeDecument = x.GuaranteeDecument,
                Gender = x.Gender,
                EmailConfirmed = x.EmailConfirmed,
                Email = x.Email,
                Country = x.Country,
                Apout = x.Apout,
                AnotherAccount = x.AnotherAccount,
                IdCardBack = x.IdCardBack,
                facebookAccount = x.facebookAccount,
                twitterAccount = x.twitterAccount,
                jobTitle = x.jobTitle
            });
            return Ok(userToReturn);

        }



        [HttpPost]
        public IHttpActionResult uploadIdCard(string userId)
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
                    HttpContext.Current.Server.MapPath("~/Content/uploads/images/"),
                    filename
                );
                var userTo_edit = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
                userTo_edit.IdCard = "~/Content/uploads/images/" + filename;
                db.Entry(userTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
                file.SaveAs(path);
            }
            var userToReturn = db.AspNetUsers.Where(x => x.Id == userId).Select(x => new cls_user
            {
                Id = x.Id,
                UserName = x.UserName,
                State = x.State,
                Residence = x.Residence,
                Photograph = x.Photograph,
                Photo = x.Photo,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                PhoneNumber = x.PhoneNumber,
                IsInstructor = x.IsInstructor,
                IdCard = x.IdCard,
                GuaranteeDecument = x.GuaranteeDecument,
                Gender = x.Gender,
                EmailConfirmed = x.EmailConfirmed,
                Email = x.Email,
                Country = x.Country,
                Apout = x.Apout,
                AnotherAccount = x.AnotherAccount,
                IdCardBack = x.IdCardBack,
                facebookAccount = x.facebookAccount,
                twitterAccount = x.twitterAccount,
                jobTitle = x.jobTitle
            });
            return Ok(userToReturn);

        }




        [HttpPost]
        public IHttpActionResult uploadGuranteeDock(string userId)
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
                    HttpContext.Current.Server.MapPath("~/Content/uploads/images/"),
                    filename
                );
                var userTo_edit = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
                userTo_edit.GuaranteeDecument = "~/Content/uploads/images/" + filename;
                db.Entry(userTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
                file.SaveAs(path);
            }
            var userToReturn = db.AspNetUsers.Where(x => x.Id == userId).Select(x => new cls_user
            {
                Id = x.Id,
                UserName = x.UserName,
                State = x.State,
                Residence = x.Residence,
                Photograph = x.Photograph,
                Photo = x.Photo,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                PhoneNumber = x.PhoneNumber,
                IsInstructor = x.IsInstructor,
                IdCard = x.IdCard,
                GuaranteeDecument = x.GuaranteeDecument,
                Gender = x.Gender,
                EmailConfirmed = x.EmailConfirmed,
                Email = x.Email,
                Country = x.Country,
                Apout = x.Apout,
                AnotherAccount = x.AnotherAccount,
                IdCardBack = x.IdCardBack,
                facebookAccount = x.facebookAccount,
                twitterAccount = x.twitterAccount,
                jobTitle = x.jobTitle
            });
            return Ok(userToReturn);

        }





        [HttpPost]
        public IHttpActionResult uploadPhoto(string userId)
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
                    HttpContext.Current.Server.MapPath("~/Content/uploads/images/"),
                    filename
                );
                var userTo_edit = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
                userTo_edit.Photo = "~/Content/uploads/images/" + filename;
                db.Entry(userTo_edit).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }
                file.SaveAs(path);
            }
            var userToReturn = db.AspNetUsers.Where(x => x.Id == userId).Select(x => new cls_user
            {
                Id = x.Id,
                UserName = x.UserName,
                State = x.State,
                Residence = x.Residence,
                Photograph = x.Photograph,
                Photo = x.Photo,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                PhoneNumber = x.PhoneNumber,
                IsInstructor = x.IsInstructor,
                IdCard = x.IdCard,
                GuaranteeDecument = x.GuaranteeDecument,
                Gender = x.Gender,
                EmailConfirmed = x.EmailConfirmed,
                Email = x.Email,
                Country = x.Country,
                Apout = x.Apout,
                AnotherAccount = x.AnotherAccount,
                IdCardBack = x.IdCardBack,
                facebookAccount = x.facebookAccount,
                twitterAccount = x.twitterAccount,
                jobTitle = x.jobTitle
            });
            return Ok(userToReturn);

    }


        [HttpGet]
        public IHttpActionResult Getid()
        {
           // string ss = System.Web.HttpContext.Current.User.Identity.GetUserId();
           // string id = User.Identity.GetUserId();
            
           //  string iiid = RequestContext.Principal.Identity.GetUserId();
            return Ok(System.Web.HttpContext.Current.User.Identity.GetUserId());
            // Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(iid);
            // return System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();

        }
        public IHttpActionResult GetUserInfoById(string userId)
        {
            AspNetUser aspNetUser = db.AspNetUsers.FirstOrDefault(user => user.Id == userId);
           
            if (aspNetUser == null)
            {
                return NotFound();
            }


            var user_to_return = new
            {
                aspNetUser.Id,
                aspNetUser.Email,
                aspNetUser.EmailConfirmed,
                aspNetUser.PasswordHash,
                aspNetUser.PhoneNumber,
                aspNetUser.PhoneNumberConfirmed,
                aspNetUser.TwoFactorEnabled,
                aspNetUser.LockoutEnabled,
                aspNetUser.LockoutEndDateUtc,
                aspNetUser.UserName,
                aspNetUser.Country,
                aspNetUser.State,
                aspNetUser.Gender,
                aspNetUser.Photo,
                aspNetUser.Apout,
                aspNetUser.AnotherAccount,
                aspNetUser.IsInstructor,
                aspNetUser.GuaranteeDecument,
                aspNetUser.IdCard,
                aspNetUser.Residence,
                aspNetUser.Photograph,
                aspNetUser.IdCardBack,
                aspNetUser.facebookAccount,
                aspNetUser.twitterAccount,
                aspNetUser.jobTitle,
                CoursesCount = aspNetUser.Course_Members.Count(),
                GroupsCount = aspNetUser.Work_group_members.Count,
                education = db.certificates.Where(x => x.inst_id == aspNetUser.Id).Select(y => new { y.id, y.title, y.body, y.image }).ToList()


            };

            return Ok(user_to_return);
        }
        public IHttpActionResult GetUserinfo(string name)
        {
            AspNetUser aspNetUser = db.AspNetUsers.FirstOrDefault(user=>user.UserName==name);
            //cls_user user_data = new cls_user();
            //////

            //user_data.Id = aspNetUser.Id;
            //user_data.Email = aspNetUser.Email;
            //user_data.EmailConfirmed = aspNetUser.EmailConfirmed;
            ////user_data.PasswordHash = aspNetUser.PasswordHash;
            //user_data.PhoneNumber = aspNetUser.PhoneNumber;
            //user_data.PhoneNumberConfirmed = aspNetUser.PhoneNumberConfirmed;
            ////user_data.TwoFactorEnabled = aspNetUser.TwoFactorEnabled;
            //// user_data.LockoutEnabled = aspNetUser.LockoutEnabled;
            ////user_data.LockoutEndDateUtc = aspNetUser.LockoutEndDateUtc;
            //user_data.UserName = aspNetUser.UserName;
            //user_data.Country = aspNetUser.Country;
            //user_data.State = aspNetUser.State;
            //user_data.Gender = aspNetUser.Gender;
            //user_data.Photo = aspNetUser.Photo;
            //user_data.Apout = aspNetUser.Apout;
            //user_data.AnotherAccount = aspNetUser.AnotherAccount;
            //user_data.IsInstructor = aspNetUser.IsInstructor;
            //user_data.GuaranteeDecument = aspNetUser.GuaranteeDecument;
            //user_data.IdCard = aspNetUser.IdCard;
            //user_data.Residence = aspNetUser.Residence;
            //user_data.Photograph = aspNetUser.Photograph;
            //user_data.IdCardBack = aspNetUser.IdCardBack;
            //user_data.facebookAccount = aspNetUser.facebookAccount;
            //user_data.twitterAccount = aspNetUser.twitterAccount;
            //user_data.jobTitle = aspNetUser.jobTitle;

            ////
            if (aspNetUser == null)
            {
                return NotFound();
            }

            var user_to_return = new {
            Id = aspNetUser.Id,
            Email = aspNetUser.Email,
            EmailConfirmed = aspNetUser.EmailConfirmed,
            PasswordHash = aspNetUser.PasswordHash,
            PhoneNumber = aspNetUser.PhoneNumber,
            PhoneNumberConfirmed = aspNetUser.PhoneNumberConfirmed,
            TwoFactorEnabled = aspNetUser.TwoFactorEnabled,
            LockoutEnabled = aspNetUser.LockoutEnabled,
            LockoutEndDateUtc = aspNetUser.LockoutEndDateUtc,
            UserName = aspNetUser.UserName,
            Country = aspNetUser.Country,
            State = aspNetUser.State,
            Gender = aspNetUser.Gender,
            Photo = aspNetUser.Photo,
            Apout = aspNetUser.Apout,
            AnotherAccount = aspNetUser.AnotherAccount,
            IsInstructor = aspNetUser.IsInstructor,
            GuaranteeDecument = aspNetUser.GuaranteeDecument,
            IdCard = aspNetUser.IdCard,
            Residence = aspNetUser.Residence,
            Photograph = aspNetUser.Photograph,
            IdCardBack = aspNetUser.IdCardBack,
            facebookAccount = aspNetUser.facebookAccount,
            twitterAccount = aspNetUser.twitterAccount,
            jobTitle = aspNetUser.jobTitle,
            CoursesCount=aspNetUser.Course_Members.Count(),
            GroupsCount=aspNetUser.Work_group_members.Count,
            education=db.certificates.Where(x=>x.inst_id==aspNetUser.Id).Select(y=>new { y.id,y.title,y.body,y.image}).ToList()

        };




            return Ok(user_to_return);
        }
        //[HttpPost]
        ////[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PostSecurityInfo( security_info info)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    db.add_security_info(info.Id, info.GuaranteeDecument, info.IdCard,info.IdCardBack, info.Residence, info.Photograph);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AspNetUserExists(info.Id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    var returnContent = db.AspNetUsers.Where(x => x.Id == info.Id).Select(x => new
        //    {
        //        x.Id, x.UserName, x.GuaranteeDecument });
        //    return Ok(returnContent);
        //}

      //  private string Userid { get => User.Identity.GetUserId(); set => Userid = value; }
        //[Authorize]
        [HttpPost]
        public IHttpActionResult EditUser(Sp_Edit_User User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //db.edit_user(User.Id,User.Email,User.PhoneNumber,User.UserName,User.Country,User.State,User.Gender
            //    ,User.Photo,User.Apout,User.AnotherAccount,User.facebookAccount,User.twitterAccount,User.jobTitle);
            var user_to_edit = db.AspNetUsers.FirstOrDefault(x => x.Id == User.Id);
            user_to_edit.Email = User.Email;
            user_to_edit.PhoneNumber = User.PhoneNumber;
            user_to_edit.UserName = User.UserName;
            user_to_edit.Country = User.Country;
            user_to_edit.State = User.State;
            user_to_edit.Gender = User.Gender;
            user_to_edit.Apout = User.Apout;
            user_to_edit.AnotherAccount = User.AnotherAccount;
            user_to_edit.facebookAccount = User.facebookAccount;
            user_to_edit.twitterAccount = User.twitterAccount;
            user_to_edit.jobTitle = User.jobTitle;
            db.Entry(user_to_edit).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(User.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //cls_user user_data = new cls_user();
            //////

            //user_data.Id = User.Id;
            //user_data.Email = User.Email;
            //user_data.PhoneNumber = User.PhoneNumber;
            //user_data.UserName = User.UserName;
            //user_data.Country = User.Country;
            //user_data.State = User.State;
            //user_data.Gender = User.Gender;
            //user_data.Photo = User.Photo;
            //user_data.Apout = User.Apout;
            //user_data.AnotherAccount = User.AnotherAccount;
            //user_data.facebookAccount = User.facebookAccount;
            //user_data.twitterAccount = User.twitterAccount;
            //user_data.jobTitle = User.jobTitle;


            return Ok(User); // StatusCode(HttpStatusCode.NoContent);
        }
        // GET: api/Users
        public IEnumerable<cls_user> GetAspNetUsers()
        {
            List<cls_user> alluser=new List<cls_user>();
            cls_user user_to_add = new cls_user();
            var allAspnetUsers= db.AspNetUsers.Where(x => x.UserName != User.Identity.Name);
            foreach (var item in allAspnetUsers)
            {
                user_to_add = new cls_user();

                user_to_add.Id = item.Id;
                user_to_add.UserName = item.UserName;
                user_to_add.State = item.State;
                user_to_add.Residence = item.Residence;
                user_to_add.Photograph = item.Photograph;
                user_to_add.Photo = item.Photo;
                user_to_add.PhoneNumberConfirmed = item.PhoneNumberConfirmed;
                user_to_add.PhoneNumber = item.PhoneNumber;
                user_to_add.IsInstructor = item.IsInstructor;
                user_to_add.IdCard = item.IdCard;
                user_to_add.GuaranteeDecument = item.GuaranteeDecument;
                user_to_add.Gender = item.Gender;
                user_to_add.EmailConfirmed = item.EmailConfirmed;
                user_to_add.Email = item.Email;
                user_to_add.Country = item.Country;
                user_to_add.Apout = item.Apout;
                user_to_add.AnotherAccount = item.AnotherAccount;
                user_to_add.IdCardBack = item.IdCardBack;
                user_to_add.facebookAccount = item.facebookAccount;
                user_to_add.twitterAccount = item.twitterAccount;
                user_to_add.jobTitle = item.jobTitle;

                alluser.Add(user_to_add);
            }
            return alluser;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserExists(string id)
        {
            return db.AspNetUsers.Count(e => e.Id == id) > 0;
        }
    }
}