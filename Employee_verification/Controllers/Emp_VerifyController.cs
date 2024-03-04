using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Employee_verification.Models;

namespace Employee_verification.Controllers
{
    public class Emp_VerifyController : Controller
    {
        Emp_dbEntities db = new Emp_dbEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        // Email Verification code

        public static void Send_Gmail_Email(string to, string subject, string employeeName, string employeeCode, string password)
        {
            var fromAddress = new MailAddress("rohitkohakade200@gmail.com", "Rohit");
            var toAddress = new MailAddress(to, to);

            string body = $@"
            <p>Dear {employeeName},</p>
            <p>Your Employee Code: {employeeCode}</p>
            <p>Your Password: {password}</p>
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
        public ActionResult Create(tblemployee e)
        {
            Send_Gmail_Email(e.email_address, "Employee Data", e.employee_name, e.employee_code, e.pass);
            ViewBag.msg = "Email Sent Successfully";
            db.tblemployees.Add(e);
            db.SaveChanges();
            return View();
        }

        
    }
}
