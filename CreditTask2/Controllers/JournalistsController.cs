using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CreditTask2.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.IO;
using System.Drawing;

namespace CreditTask2.Controllers
{
    public class JournalistsController : Controller
    {
        private NewsdbEntities db = new NewsdbEntities();
        private NewsdbEntities db1 = new NewsdbEntities();

        public class CheckDateRangeAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                DateTime dt = (DateTime)value;
                if (dt <= DateTime.UtcNow)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(ErrorMessage ?? "Make sure your date is <= than today");
            }

        }


        // GET: Journalists
        public ActionResult Index()
        {
            return View(db.Journalists.OrderBy(j => j.journDOB).ToList());
        }
      //  private int temper = 0;

        // GET: Journalists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journalist journalist = db.Journalists.Find(id);
            if (journalist == null)
            {
                return HttpNotFound();
            }
            return View(journalist);
        }

        // GET: Journalists/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create1()
        {
            return View();
        }
        public ActionResult Create2()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "JournFname,JournLname,JournDOB,JournContact,JournEmail,JournCity,Journalist_Image")] Journalist journalist, HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (Helpers.Util.IsRecognisedImageFile(postedFile.FileName))
                {
                    // Here I want each image to have a unique path. Doesn't matter if the same image is uploaded. E
                    // Each path is the same length.
                    String filePath = Helpers.Util.GenerateUniqueString();
                    filePath = Helpers.Util.CalculateMD5Hash(filePath);

                    String fileExtension = Path.GetExtension(postedFile.FileName);
                    String full = filePath + fileExtension;
                    postedFile.SaveAs(path + full);
                    journalist.Journalist_Image = full;
                    string abc = journalist.Journalist_Image + journalist.journFname + journalist.JournLname + journalist.journDOB + journalist.journContact + journalist.journEmail+ journalist.journCity;
                    using (StreamWriter file = new StreamWriter(@"D:\WriteLines2.txt"))
                        file.Write(abc);
                }
                else
                {
                    ViewBag.Message = "Please upload a valid image.";
                    return View(journalist);
                }
            }



            db.Journalists.Add(journalist);
           db.SaveChanges();
            procedureExe();
            return RedirectToAction("Register", "Account");



        }





        // POST: Journalists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "journId,journFname,JournLname,journDOB,journContact,journEmail,journCity, Journalist_Password, Journlist_Image")] Journalist journalist)
        {
            if (ModelState.IsValid)
            {
                db.Journalists.Add(journalist);
                db.SaveChanges();
                return RedirectToAction("Register", "Account");
                //return RedirectToAction("Index");
            }

            return View(journalist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1([Bind(Include = "journId,journFname,JournLname,journDOB,journContact,journEmail,journCity")] Journalist journalist)
        {
            if (ModelState.IsValid)
            {
                db.Journalists.Add(journalist);
                db.SaveChanges();
                return RedirectToAction("Register", "Account");
                //return RedirectToAction("Index");
            }

            return View(journalist);
        }

        // GET: Journalists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journalist journalist = db.Journalists.Find(id);
            if (journalist == null)
            {
                return HttpNotFound();
            }
            return View(journalist);
        }

        // POST: Journalists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "journId,journFname,JournLname,journDOB,journContact,journEmail,journCity,Journalist_Image")] Journalist journalist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(journalist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(journalist);
        }

        void procedureExe()
        {
            string constring = ConfigurationManager.ConnectionStrings["sru"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);

            
            var command = new SqlCommand("CopyUser", con);

            command.CommandType = CommandType.StoredProcedure;
            con.Open();

            command.ExecuteNonQuery();
            con.Close();
        }


        // GET: Journalists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journalist journalist = db.Journalists.Find(id);
            if (journalist == null)
            {
                return HttpNotFound();
            }
            return View(journalist);
        }

        // POST: Journalists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Journalist journalist = db.Journalists.Find(id);
            db.Journalists.Remove(journalist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search(string searchBy, string search)
        {
            if (searchBy == "Name")
            {
                return View(db.Journalists.Where(x => x.journFname.StartsWith(search) || search == null).ToList());
            }

            else if (searchBy == "City")
            {
                return View(db.Journalists.Where(x => x.journCity.StartsWith(search) || search == null).ToList());
            }

            else if (searchBy == "Email")
            {
                return View(db.Journalists.Where(x => x.journEmail.StartsWith(search) || search == null).ToList());
            }
            else
            {
                return View(db.Journalists.Where(x => x.journEmail.StartsWith(search) || x.journCity.StartsWith(search) || x.journFname.StartsWith(search) || search == null).ToList());
            }
            
            return View(db.Journalists.ToList());
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
