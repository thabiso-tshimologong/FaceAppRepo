using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Face_Recognition.Models;

namespace Face_Recognition.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "1")]
        public ActionResult GetData()
        {
            using (face_DBEntities db = new face_DBEntities())
            {
                List<User> userList = db.Users.ToList<User>();
                return Json(new { data = userList }, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "1")]
        [HttpGet]
        public ActionResult addOrEdit(int id = 0)
        {
            if (id == 0)
            return View(new User());
            else
            {
                using( face_DBEntities db = new face_DBEntities())
                {
                    return View(db.Users.Where(x => x.user_id == id).FirstOrDefault<User>());
                }
            }
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult addOrEdit(User us)
        {
            using (face_DBEntities db = new face_DBEntities())
            {
                if (us.user_id == 0)
                {
                    db.Users.Add(us);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved User Data successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(us).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated User Data successfully" }, JsonRequestBehavior.AllowGet);
                }
            }

        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (face_DBEntities db = new face_DBEntities())
            {
               User user = db.Users.Where(x => x.user_id == id).FirstOrDefault<User>();
                db.Users.Remove(user);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted User Data successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
    
    
