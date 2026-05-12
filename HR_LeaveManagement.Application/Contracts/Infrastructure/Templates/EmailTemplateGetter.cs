using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.Contracts.Infrastructure.Templates
{
    public static class EmailTemplateGetter
    {
        public static string EmailTemplate(DateTime startDate, DateTime endDate)
        {
            string template = @"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Order Notification</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        color: #333;
                        padding: 20px;
                        margin: 0;
                    }
                    .container {
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        max-width: 600px;
                        margin: 0 auto;
                    }
                    h2 {
                        color: #4CAF50;
                    }
                    .otp-code {
                        font-size: 24px;
                        font-weight: bold;
                        color: #333;
                        margin: 20px 0;
                    }
                    p {
                        font-size: 16px;
                    }
                    .footer {
                        font-size: 14px;
                        color: #777;
                        margin-top: 20px;
                    }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Leave Notification</h2>

                    <p>Your leave request for ${startDate} to ${endDate} has been submitted successfully</p>

                </div>
            </body>
            </html>";

            string templateData = template.Replace("${startDate}", startDate.ToString())
                                          .Replace("${endDate}", endDate.ToString());

            return templateData;
        }
    }
}
