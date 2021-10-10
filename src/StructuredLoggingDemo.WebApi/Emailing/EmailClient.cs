using System;

namespace StructuredLoggingDemo.WebApi.Emailing
{
    public class EmailClient
    {
        public void SendEmail(string email, string text)
        {
            if (email.Contains("@fakeemail.com")) throw new Exception("Could not send the message. Service denied the request.");
        }
    }
}