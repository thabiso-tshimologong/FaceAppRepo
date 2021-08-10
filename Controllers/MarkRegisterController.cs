using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Face_Recognition.Models;

namespace Face_Recognition.Controllers
{
    public class MarkRegisterController : Controller
    {
        face_DBEntities1 db = new face_DBEntities1();
        // GET: MarkRegister
        [Authorize(Roles = "2")]
        public ActionResult Mark()
        {
            return View();
        }
        public ActionResult Capture()
        {
            return View();
        }
        [Authorize(Roles ="2")]
        [HttpPost]
        public ActionResult Mark(attendace at)
        {
            string maincon = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(maincon);
            var user = db.attendaces.Where(x => x.username.Equals(at.username)).FirstOrDefault();
            if (user != null)
            {
                ViewBag.Msg = "Provided email Already registered- Login Instead";
            }
            else
            {
                string sqlquery = "INSERT INTO [dbo].[attendace](att_date,username) VALUES(@att_date,@username)";
                SqlCommand sqlcom = new SqlCommand(sqlquery, sqlcon);
                sqlcon.Open();
                sqlcom.Parameters.AddWithValue("@att_date", at.att_date);
                sqlcom.Parameters.AddWithValue("@username", at.username);
                sqlcom.ExecuteNonQuery();
                sqlcon.Close();
                ViewBag.Message = "User " + at.username + " Has Successfully Mark the Register";
            }
            return View();
        }

    }
}