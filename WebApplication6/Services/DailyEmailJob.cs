using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication6.Services
{
    public class DailyEmailJob : IJob
    {
        private readonly UserService _userService;
        private readonly MailerSendService _mailerSendService;

        public DailyEmailJob(UserService userService, MailerSendService mailerSendService)
        {
            _userService = userService;
            _mailerSendService = mailerSendService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var apiKey = "mlsn.b77eb93f0c6df174621b412e9b821fff7af8da955d648da10de7e458bff4a978"; // Securely manage this in a real application
            var mailerSendService = new MailerSendService(apiKey);
            // Retrieve email addresses from the database
            var emailAddresses = await _userService.GetAllUserEmailAddressesAsync();

            // Send emails to all retrieved addresses
            foreach (var email in emailAddresses)
            {
                await _mailerSendService.SendEmailAsync(email, "Daily Update", "Your daily message content here.");
            }
        }
    }
}