using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using Face_Recognition.Models;
using System.Reflection;

namespace Face_Recognition.Controllers
{
    public class UserController : Controller
    {
        private face_DBEntities db = new face_DBEntities();
        // GET: User
        [Authorize(Roles ="1")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        [Authorize(Roles = "1")]
        public ActionResult ExportUser()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"),"UserReport.rpt"));
            rd.SetDataSource(ListToDataTable(db.Users.ToList()));
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "UserList.pdf");
            }
            catch
            {
                throw;
            }
        }
        [Authorize(Roles = "1")]
        private DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach(T item in items )
            {
                var values = new object[Props.Length];
                for(int i =0; i<Props.Length;i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);

            }
            return dataTable;
        }
    }
}