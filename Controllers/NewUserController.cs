using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Configuration;
using System.IO;
using Face_Recognition.Models;
using System.Web.Helpers;
using System;

namespace Face_Recognition.Controllers
{
    public class NewUserController : Controller
    {
        // GET: NewUser
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Capture()
        {
            return View();
        }
        [HttpPost]
        public ContentResult SaveCapture(string data)
        {
            string fileName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");

            //Convert Base64 Encoded string to Byte Array.
            byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);

            //Save the Byte Array as Image File.
            string filePath = Server.MapPath(string.Format("~/Images/{0}.jpg", fileName));
            System.IO.File.WriteAllBytes(filePath, imageBytes);

            return Content("true");
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Reg uc, HttpPostedFileBase file)
        {
            string maincon = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(maincon);
            face_DBEntities db = new face_DBEntities();
            var user = db.Users.Where(x => x.username.Equals(uc.username)).FirstOrDefault();
            if (user != null)
            {
                ViewBag.Msg = "Provided email Already registered- Login Instead";
            }
            else {
                string sqlquery = "INSERT INTO [dbo].[User](username,password,conf_pass,photo,role_id) VALUES(@username,@password,@conf_pass,@photo,@role_id)";
                SqlCommand sqlcom = new SqlCommand(sqlquery, sqlcon);
                sqlcon.Open();
                sqlcom.Parameters.AddWithValue("@username", uc.username);
                sqlcom.Parameters.AddWithValue("@password", uc.password);
                sqlcom.Parameters.AddWithValue("@conf_pass", uc.conf_pass);
                if (file != null && file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string imgpath = Path.Combine(Server.MapPath("~/User-Images/"), filename);
                    file.SaveAs(imgpath);
                }
                sqlcom.Parameters.AddWithValue("@photo", "~/User-Images/" + file.FileName);
                sqlcom.Parameters.AddWithValue("@role_id", uc.role_id);
                sqlcom.ExecuteNonQuery();
                sqlcon.Close();
                ViewBag.Message = "User Record " + uc.username + " Is Successfully Registered";
            }
                return View();
            
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model, string returnUrl)
        {
            face_DBEntities db = new face_DBEntities();
            var dataitem = db.Users.Where(x=>x.username.Equals(model.username) && x.password.Equals(model.password)).FirstOrDefault();
            if (dataitem != null)
            {
                FormsAuthentication.SetAuthCookie(dataitem.username, true);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length >1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    Session["email"] = model.username.ToString();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.Msg = "Wrong Username/Password";
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "NewUser");

        }
        
        public ActionResult ForgotPassword()
        {
            return View();

        }

       [HttpPost]
        public ActionResult ForgotPassword(Reg uc)
        {
            face_DBEntities db = new face_DBEntities();
            var user = db.Users.Where(x => x.username.Equals(uc.username)).FirstOrDefault();
            if (user != null)
            {
                string subject = "Requested Password";
                string body = "Requested Password";

                WebMail.Send(uc.username, subject, body, null, null, null, true, null, null, null, null, null, null);
                ViewBag.Message = "Email sent successfully check your mail box";

            }
            else
            {
                ViewBag.Message = "Email not registered";
            }



            return View();

        }
    }
}