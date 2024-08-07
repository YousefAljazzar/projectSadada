﻿using Sadada.ModelViews.Enums;
using System.Collections.Generic;
using System.Net;

namespace Sadada.Models.Static
{
    public class EmailBuilder
    {
        public EmailBuilder(ActionInvocationTypeEnum notificationType, Dictionary<string, string> parameterValues, string url, string assessmentURL = "")
        {
            NotificationType = notificationType;
            Values = parameterValues;
            URL = url;
        }
        
        public ActionInvocationTypeEnum NotificationType { get; set; }

        public Dictionary<string, string> Values { get; set; }

        public string URL { get; set; }

        public string Link { get; set; }

        public string GetTitle()
        {
            return NotificationType switch
            {
                ActionInvocationTypeEnum.EmailConfirmation => $"Email Confirmation!",
                ActionInvocationTypeEnum.ResetPassword => $"Reset Password!",
                _ => ""
            };
        }

        public string GetHeading()
        {
            return NotificationType switch
            {
                ActionInvocationTypeEnum.EmailConfirmation => $"</br>You has been registered successfully just one more step to finish. </br>",
                ActionInvocationTypeEnum.ResetPassword => $"</br> You are going to reset you password. </br>",
                _ => ""
            };
        }

        public string GetBody(string password)
        {
            return NotificationType switch
            {
                ActionInvocationTypeEnum.EmailConfirmation => GetEmailConfirmationEmailBody(password),
                ActionInvocationTypeEnum.ResetPassword => GetResetPasswordEmailBody(),
                _ => ""
            };
        }

        private string GetEmailConfirmationEmailBody(string password)
        {
            var url = URL + $"/confirm?ConfirmaionKey={(Values.ContainsKey("Link") ? Values["Link"] : "")}";

            var emailBody = "<div width = \"100% !important\" style=\"background:#fff; width:100%!important; margin:0; padding:0; font-family:'Roboto',Helvetica,sans-serif; color:rgb(70,72,74,.9); font-size:15px; line-height:1.5em\">" +
                      "<table align = \"center\" bgcolor=\"#e1f1fd\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; max-width:800px; display:table\">" +
                      "<tbody><tr><td align = \"left\" style=\"border-collapse:collapse; text-align:left; font-family:'Muli',Helvetica,sans-serif; font-size:32px; font-weight:900; color:#1B5379; padding-top:20px; letter-spacing:-1px; line-height:1em\">" +
                      "<img data-imagetype=\"External\" src=\"\" width=\"200\" style=\"width:200px\">" +
                      "<br aria-hidden=\"true\"><br aria-hidden=\"true\"><br aria-hidden=\"true\">" +
                      "<span style = \"width:80%; display:block; margin-bottom:40px\">" +
                      "</br>Sadada Confirmation!</br></span> </td></tr></tbody></table>" +
                      "<table align = \"center\" bgcolor= \"#ffffff\" border= \"0\" cellpadding= \"35\" cellspacing= \"0\" width= \"90%\" style= \"margin:0px auto; max-width:800px; display:table\" >" +
                      $"<tbody><tr><td align= \"left\" style= \"border-collapse:collapse; text-align:left\" > Hi {(Values.ContainsKey("AssigneeName") ? Values["AssigneeName"] : "")}," +
                      $"<br>" +
                      $"<br>" +
                      $"This is your Password:{password}. <br><br>Thanks,<br>Sadada App<br></td></tr></tbody></table>" +
                      "<table align=\"center\" bgcolor=\"#3599e8\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; text-align:center; max-width:800px; color:rgba(255,255,255,.5); font-size:11px; display:table\">" +
                      "<tbody>" +
                      "<tr width=\"100%\"><td width=\"100%\" style=\"border - collapse:collapse\"><img data-imagetype=\"External\" src=\"\" alt=\"Sanad\" width=\"25\" style=\"width:25px; opacity:.5\">" +
                      "<br aria-hidden=\"true\"><span style=\"color:#fff; color:rgba(255,255,255,.5)\"></span> | <a href=\"\" target=\"_blank\" rel=\"noopener noreferrer\" data-auth=\"NotApplicable\" style=\"text-decoration:none; color:#fff; color:rgba(255,255,255,.5)\" data-linkindex=\"1\"></a> </td></tr></tbody>";

            return emailBody;
        }

        private string GetResetPasswordEmailBody()
        {
            var url = URL + $"?confirmation={(Values.ContainsKey("Link") ? Values["Link"] : "")}";

            var emailBody = "<div width = \"100% !important\" style=\"background:#fff; width:100%!important; margin:0; padding:0; font-family:'Roboto',Helvetica,sans-serif; color:rgb(70,72,74,.9); font-size:15px; line-height:1.5em\">" +
                        "<table align = \"center\" bgcolor=\"#e1f1fd\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; max-width:800px; display:table\">" +
                        "<tbody><tr><td align = \"left\" style=\"border-collapse:collapse; text-align:left; font-family:'Muli',Helvetica,sans-serif; font-size:32px; font-weight:900; color:#1B5379; padding-top:20px; letter-spacing:-1px; line-height:1em\">" +
                        "<img data-imagetype=\"External\" src=\"\" width=\"200\" style=\"width:200px\">" +
                        "<br aria-hidden=\"true\"><br aria-hidden=\"true\"><br aria-hidden=\"true\">" +
                        "<span style = \"width:80%; display:block; margin-bottom:40px\">" +
                        "</br>You has been registered successfully just one more step to finish.</br></span> </td></tr></tbody></table>" +
                        "<table align = \"center\" bgcolor= \"#ffffff\" border= \"0\" cellpadding= \"35\" cellspacing= \"0\" width= \"90%\" style= \"margin:0px auto; max-width:800px; display:table\" >" +
                        $"<tbody><tr><td align= \"left\" style= \"border-collapse:collapse; text-align:left\" > Hi {(Values.ContainsKey("AssigneeName") ? Values["AssigneeName"] : "")}," +
                        $"<br>" +
                        $"<br>" +
                        $"Please follow the link to reset your account password <a data-click-track-id=\"381\" href=\"{url}\">Press here</a>. <br><br>Thanks,<br> The Sanad Team<br></td></tr></tbody></table>" +
                        "<table align=\"center\" bgcolor=\"#3599e8\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; text-align:center; max-width:800px; color:rgba(255,255,255,.5); font-size:11px; display:table\">" +
                        "<tbody>" +
                        "<tr width=\"100%\"><td width=\"100%\" style=\"border - collapse:collapse\"><img data-imagetype=\"External\" src=\"\" alt=\"Sanad\" width=\"25\" style=\"width:25px; opacity:.5\">" +
                        "<br aria-hidden=\"true\"><span style=\"color:#fff; color:rgba(255,255,255,.5)\">Jenin Main street</span> | " +
                        "<a href=\"\" target=\"_blank\" rel=\"noopener noreferrer\" data-auth=\"NotApplicable\" style=\"text-decoration:none; color:#fff; color:rgba(255,255,255,.5)\" data-linkindex=\"1\">Unsubscripted</a> </td></tr></tbody>";
            
            var x = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <style>\r\n    /* Add your CSS styles here */\r\n    body {\r\n      font-family: sans-serif;\r\n      color: #333;\r\n    }\r\n    .confirmation-message {\r\n      background-color: #eee;\r\n      padding: 20px;\r\n      border: 1px solid #ccc;\r\n      border-radius: 5px;\r\n    }\r\n    .confirmation-message h1 {\r\n      font-size: 24px;\r\n      margin: 0 0 10px 0;\r\n    }\r\n    .confirmation-message p {\r\n      margin: 0;\r\n      padding: 0;\r\n    }\r\n  </style>\r\n</head>\r\n<body>\r\n  <div class=\"confirmation-message\">\r\n    <h1>Confirmation Email</h1>\r\n    <p>Dear User,</p>\r\n    <p>Thank you for signing up for our service. Please click the following link to confirm your email address:</p>" +
                $"\r\n    <p><a href=\"{url}\">Click Here</a></p>\r\n    <p>Best regards,</p>\r\n    <p>The Your-Site Team</p>\r\n  </div>\r\n</body>\r\n</html>";
           
            return x;
        }
    }
}