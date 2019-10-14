
namespace SendRecieveMSMQ
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;

    public class EmilService
    {
        public void EmailService(string ToMail,string token)
        { 
            // This address must be verified with User Login registration.
            string FROM = "maheshaurad183@gmail.com";
            string FROMNAME = "FundooApp";

            // smtp_username with SMTP user name.
            string SMTP_USERNAME = "maheshaurad183@gmail.com";

            // Replace smtp_password with your Amazon SES SMTP user name.
            string SMTP_PASSWORD = "7720009867";

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            //string CONFIGSET = "ConfigSet";

            // If you're using Amazon SES in a region other than US West (Oregon), 
            // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
            // endpoint in the appropriate AWS Region.
            string HOST = "smtp.gmail.com";

            // The port you will connect to on the Amazon SES SMTP endpoint. We
            // are choosing port 587 because we will use STARTTLS to encrypt
            // the connection.
            int PORT = 587;

            // The subject line of the email
            string SUBJECT =
                "Forgot Password";

            // The body of the email
            string BODY =
                "<h1>Please Click on the below link to reset your password</h1>" +
                "<p>" +
                "<a href='http://localhost:4200/resetPassword/'+ token>Click here</a>" + 
                "</p>";

            // Create and build a new MailMessage object  
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(new MailAddress(ToMail));
            message.Subject = SUBJECT;
            message.Body = BODY;
            // Comment or delete the next line if you are not using a configuration set
           // message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                try
                {
                    Console.WriteLine("Attempting to send email...");
                    client.Send(message);
                    Console.WriteLine("Email sent!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);
                }
            }

        }
    }
}
