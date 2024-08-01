using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication6.Services
{
    public class MailerSendService
    {
        private readonly string _apiKey;

        public MailerSendService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var client = new RestClient("https://api.mailersend.com/v1/email");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                from = new { email = "akashajay339@gmail.com" },
                to = new[] { new { email = toEmail } },
                subject = subject,
                html = body
            });

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to send email: {response.Content}");
            }
        }
    }
}