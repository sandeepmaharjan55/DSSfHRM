using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace TaskManager.MVC5.Controllers
{
    public class SendMailerController : Controller
    {
        //
        // GET: /SendMailer/  
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(TaskManager.MVC5.Models.MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress("sandeepmaharjan94@gmail.com");
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("sandeepmaharjan94@gmail.com", "Kathmandu1111");// Enter seders User name and password
                smtp.EnableSsl = true;
               
                mail.Headers.Add("Disposition-Notification-To", "sandeepmaharjan94@gmail.com");
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
               
                mail.ReplyTo = mail.From;

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }
    }
}