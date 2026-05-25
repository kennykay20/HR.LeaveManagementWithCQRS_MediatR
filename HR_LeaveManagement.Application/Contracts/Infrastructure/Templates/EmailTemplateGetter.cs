using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.Contracts.Infrastructure.Templates
{
    public static class EmailTemplateGetter
    {
        public static string LeaveRequestNotification(DateTime startDate, DateTime endDate)
        {
            string template = @"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Leave Request Notification</title>
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
                    <h2>Leave Request Notification</h2>

                    <p>Your leave request for ${startDate} to ${endDate} has been submitted successfully</p>

                </div>
            </body>
            </html>";

            string templateData = template.Replace("${startDate}", startDate.ToString())
                                          .Replace("${endDate}", endDate.ToString());

            return templateData;
        }
    
        public static string RegisterNotification(string FullName, string VerificationLink)
        {
            string template = @"
                html
                <!DOCTYPE html>
                <html lang='en''>
                <head>
                    <meta charset='UTF-8' />
                    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                    <title>Email Verification</title>
                </head>
                <body style='margin:0; padding:0; background-color:#f4f4f4; font-family:Arial, Helvetica, sans-serif;'>
                    <table width='100%' cellspacing='0' cellpadding='0' style='background-color:#f4f4f4; padding:40px 0;'>
                        <tr>
                            <td align='center'>

                                <table width='600' cellspacing='0' cellpadding='0'
                                    style='background-color:#ffffff; border-radius:10px; overflow:hidden;'>

                                    <!-- Header -->
                                    <tr>
                                        <td align='center'
                                            style='background-color:#2563eb; padding:30px;'>
                                            <h1 style='color:#ffffff; margin:0; font-size:28px;'>
                                                Welcome to HR_LeaveManagement 
                                            </h1>
                                        </td>
                                    </tr>

                                    <!-- Body -->
                                    <tr>
                                        <td style='padding:40px 30px; color:#333333;'>

                                            <h2 style='margin-top:0; color:#111827;'>
                                                Verify Your Email Address
                                            </h2>

                                            <p style='font-size:16px; line-height:1.6;'>
                                                Hello <strong>${FullName}</strong>,
                                            </p>

                                            <p style='font-size:16px; line-height:1.6;'>
                                                Thank you for registering with us.
                                                To complete your registration and activate your account,
                                                please verify your email address by clicking the button below.
                                            </p>

                                            <!-- Button -->
                                            <table cellspacing='0' cellpadding='0' align='center' style='margin:30px auto;'>
                                                <tr>
                                                    <td align='center' bgcolor='#2563eb' style='border-radius:6px;'>
                                                        <a href='${VerificationLink}'
                                                           target='_blank'
                                                           style='display:inline-block;
                                                                  padding:14px 28px;
                                                                  font-size:16px;
                                                                  color:#ffffff;
                                                                  text-decoration:none;
                                                                  font-weight:bold;'>
                                                            Verify Email
                                                        </a>
                                                    </td>
                                                </tr>
                                            </table>

                                            <p style='font-size:15px; line-height:1.6;'>
                                                If the button above does not work, copy and paste the link below into your browser:
                                            </p>

                                            <p style='word-break:break-all; font-size:14px; color:#2563eb;'>
                                                ${VerificationLink}
                                            </p>

                                            <p style='font-size:15px; line-height:1.6;'>
                                                This verification link may expire after a certain period for security reasons.
                                            </p>

                                            <p style='font-size:15px; line-height:1.6;'>
                                                If you did not create this account, please ignore this email.
                                            </p>

                                            <p style='margin-top:40px; font-size:16px;'>
                                                Best regards,<br />
                                                <strong>HR_LeaveManagement Team</strong>
                                            </p>

                                        </td>
                                    </tr>

                                    <!-- Footer -->
                                    <tr>
                                        <td align='center'
                                            style='background-color:#f9fafb; padding:20px; font-size:13px; color:#6b7280;'>
                                            © 2026 HR_LeaveManagement. All rights reserved.
                                        </td>
                                    </tr>

                                </table>

                            </td>
                        </tr>
                    </table>

                </body>
                </html>
            ";

            string templateReplace = template.Replace("${FullName}", FullName)
                           .Replace("${VerificationLink}", VerificationLink);

            return templateReplace;
        }
    }
}
