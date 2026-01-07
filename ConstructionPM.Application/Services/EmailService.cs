using ConstructionPM.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionPM.Application.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendApprovalEmailAsync(string toEmail, string fullName, string tempPassword)
        {
            var subject = "Your ConstructionPM account has been approved";

            var body = $@"
                            Hello {fullName},
                            
                            Your account has been approved by the administrator.
                            
                            Login details:
                            Email: {toEmail}
                            Temporary Password: {tempPassword}
                            
                            Please log in and change your password immediately.
                            
                            Regards,
                            ConstructionPM Team
                            ";

            // SMTP / SendGrid / MailKit logic here
        }
    }

}
