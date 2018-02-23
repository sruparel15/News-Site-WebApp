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
using System.Collections;
using System.Web.Helpers;

namespace CreditTask2.Controllers
{
    public class ArticlesController : Controller
    {
        private NewsdbEntities db = new NewsdbEntities();

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


        // GET: Articles

        public ActionResult Index()
        {
            return View(db.Articles.OrderByDescending(a => a.articleDate).ToList());
        }


        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.journId = new SelectList(db.Journalists, "journId", "journFname");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "articleId,journId,articleDate,articleTopic,articleTitle,articleDesc")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.journId = new SelectList(db.Journalists, "journId", "journFname", article.journId);
            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.journId = new SelectList(db.Journalists, "journId", "journFname", article.journId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "articleId,journId,articleDate,articleTopic,articleTitle,articleDesc")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.journId = new SelectList(db.Journalists, "journId", "journFname", article.journId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }


        public ActionResult Chart()
        {

            return View();
        }

        public ActionResult Graph()
        {
            var context = new NewsdbEntities();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();

            //  var temp = (from m in db.Journalists where (m.Journalist_Id == temper) && (m.Id_System == userid) && (m.Id_System != null) select m.Journalist_Id).FirstOrDefault();
            //   var count = db.Suspenses.GroupBy(s => s.SuspenseDate.Month).Select(g => g.Count());


            KeyValuePair<string, int> list = new KeyValuePair<string, int>();
            var result = context.Articles.GroupBy(m => m.articleDate.Value.Year).Select(x => new
            {
                Date_Published = x.Key,
                Count = x.Count()
            }).ToList();

            // var results = (from c in context.Articles.GroupBy(m => m.Date_Published.Year) select c.Count());
            result.ToList().ForEach(rs => xValue.Add(rs.Date_Published));
            result.ToList().ForEach(rs => yValue.Add(rs.Count));

            new Chart(width: 600, height: 400, theme: ChartTheme.Green)
                .AddTitle("Yearly Increase in Articles")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
                .Write("bmp");
            return null;
        }

        public ActionResult ExportPDF()
        {
            return new Rotativa.MVC.ActionAsPdf("Index")
            {
                FileName = Server.MapPath("Articles.pdf")
            };
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
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
