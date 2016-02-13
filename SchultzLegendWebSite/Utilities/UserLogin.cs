using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Web.Security;

namespace SchultzLegendWebSite.Utilities
{
    public class UserLogin
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public UserLogin()
        {

        }

        public static void SendConfirmEmail(UserLogin user)
        {
            MailMessage mail = new MailMessage("no-reply@schultzlegend.com", user.Email);
            SmtpClient client = new SmtpClient("email-smtp.us-west-2.amazonaws.com", 587);
            client.Credentials = new NetworkCredential("AKIAJJQHYH6YBXOFJNXQ", "AvQXrFMjY1HJCH8mAyQzxevxNda6eNieRVKJLhiwUUA2");
            client.EnableSsl = true;
            mail.Subject = "Confirm Your Email Address - Schultz Legend";
            mail.Body = String.Format("Thank you for registering at SchultzLegend.com\n\nPlease click the link to confirm your email:\nhttp://www.schultzlegend.com/ConfirmEmail?token={0}", user.Id);
            client.Send(mail);
        }

        public static void SendResetPasswordEmail(string email)
        {
            string pass = Membership.GeneratePassword(20, 4);
            string phash = LoginHelper.CreateHash(pass);
            DBHelper.DBCChangeUserPassword(email, phash);
            MailMessage mail = new MailMessage("no-reply@schultzlegend.com", email);
            SmtpClient client = new SmtpClient("email-smtp.us-west-2.amazonaws.com", 587);
            client.Credentials = new NetworkCredential("AKIAJJQHYH6YBXOFJNXQ", "AvQXrFMjY1HJCH8mAyQzxevxNda6eNieRVKJLhiwUUA2");
            client.EnableSsl = true;
            mail.Subject = "Reset Password - Schultz Legend";
            mail.Body = String.Format("Here is your temporary password:\n{0}\n\nLogin and reset your password:\nhttp://www.schultzlegend.com/Login", pass);
            client.Send(mail);
        }
    }
}