using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using Face_Recognition.Models;

namespace Face_Recognition.Controllers
{
    public class AttendanceController : Controller
    {
        private face_DBEntities1 db = new face_DBEntities1();

        // GET: Attendance
        [Authorize(Roles = "1")]
        public ActionResult attend()
        {
            return View(db.attendaces.ToList());
        }
        [Authorize(Roles = "1")]
        public ActionResult ExportRegister()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "attendReport.rpt"));
            rd.SetDataSource(ListToDataTable(db.attendaces.ToList()));
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "AttendanceRegister.pdf");
            }
            catch
            {
                throw;
            }
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        private DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);

            }
            return dataTable;
        }

    }
}