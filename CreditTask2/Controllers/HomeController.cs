using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Collections;
using Microsoft.Ajax.Utilities;
using System.Net;
using System.Net.Mail;
using System.Web.UI.WebControls;
using CreditTask2.Models;
using System.Web.Helpers;

namespace CreditTask2.Controllers
{
    public class HomeController : Controller
    {
        private Models.NewsdbEntities db = new Models.NewsdbEntities();
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Power News Ltd";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "To reach us:";

            return View();
        }

        public ActionResult Documentation()
        {
            ViewBag.Message = " Details of Author";

            return View();
        }

        public ActionResult Email()
        {
            display();

            return View();
        }

        public ActionResult display()
        {

            var result = (from journEmail in db.Journalists select journEmail).ToList();
            if (result != null)
            {
                ViewBag.myJEmail = result.Select(N => new SelectListItem { Text = N.journEmail, Value = N.journId.ToString() });

            }

            return View();
        }
        
             
        [HttpPost]
        public ActionResult Email(EmailModel model)
        {
            var value = Request.Cookies["name"].Value;
            var a = Convert.ToInt32(model.To);

            // var temp = (from m in db.Journalists where (m.JournId ==a ) select m.JournEmail).FirstOrDefault();


            using (System.IO.StreamWriter file =
              new System.IO.StreamWriter(@"D:\WriteLines2.txt"))
                file.Write(value);

            using (MailMessage mm = new MailMessage(model.Email, value))
            {
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                if (model.Attachment != null && model.Attachment.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(model.Attachment.FileName);
                    mm.Attachments.Add(new Attachment(model.Attachment.InputStream, fileName));
                }
                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(model.Email, model.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);

                }
            }

            return RedirectToAction("Index", "Home");
        }

    }
}