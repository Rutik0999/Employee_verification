using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Employee_verification.Models;

namespace Employee_verification.Controllers
{
    public class LoginController : Controller
    {
        private Emp_dbEntities db = new Emp_dbEntities();

        public ActionResult Index()
        {
            if (Session["UserEmail"] != null)
            {
                string useremail = Session["UserEmail"] as string;

                tblemployee loggedemployee = db.tblemployees.FirstOrDefault(x => x.email_address == useremail);

                return View(loggedemployee);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public static void Send_Gmail_Email(string to, string subject, string employeeName, string otp)
        {
            var fromAddress = new MailAddress("rohitkohakade200@gmail.com", "Rohit");
            var toAddress = new MailAddress(to, to);

            string body = $@"
            <p>Dear: {employeeName},</p>
            <p>Your OTP Code: {otp}</p>
            <p>Regards,</p>
            <p>Rohit</p>";
 
            MailMessage message = new MailMessage(fromAddress, toAddress);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 2000000,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("rohitkohakade200@gmail.com", "lzmr pscf wdpr gtqh")
            };

            try
            {
                smtp.Send(message);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }


        [HttpPost]
        public ActionResult Login(tblemployee log)
        {
            var user = db.tblemployees.SingleOrDefault(x => x.employee_code == log.employee_code 
            && x.pass == log.pass);
  
            if (user != null)
            {

                Session["UserEmail"] = user.email_address;
                return RedirectToAction("OTP_Verification");
            }
            else
            {
                ViewBag.msg = "Invalid Login";
                return View();
            }
        }


        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult OTP_Verification()
        {
            return View();
        }

        // OTP Class

        public string GeneratePassword(int size)
        {
            string msg = "0123456789";
            string Password = "";
            Random r = new Random();
            for (int i = 1; i <= size; i++)
            {
                int P = r.Next(0, msg.Length - 1);
                Password += msg[P];
            }
            return Password;
        }

        [HttpPost]
        public ActionResult SendOTP(tblemployee ot)
        {
            var pass = GeneratePassword(4);
            var userEmail = Session["UserEmail"] as string;

          
            Session["otp"] = pass;

            Send_Gmail_Email(userEmail, "OTP Verification", ot.employee_name, pass);

            ViewBag.msg = "OTP Sent Successfully!";
            return View("OTP_Verification");
        }

        [HttpPost]
        public ActionResult VerifyOTP(tblemployee ot)
        {
            var passotp = Session["otp"] as string;

            if (ot.otp == passotp)
            {

                ViewBag.msg = "Login Succesfully";
                return RedirectToAction("Index");
      
            }
            else
            {
                ViewBag.msg = "Invalid OTP";
                return View("OTP_Verification");
            }
        }
    }
}
