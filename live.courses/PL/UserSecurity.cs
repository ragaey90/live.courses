using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using live.courses.Models;

namespace live.courses.PL
{
    public class UserSecurity
    {
        public static bool login(string email, string password)
        {
            using (adv_coursesEntities ctx = new adv_coursesEntities())
            {
                return ctx.AspNetUsers.Any(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && user.PasswordHash == password);
            }
        }
    }
}