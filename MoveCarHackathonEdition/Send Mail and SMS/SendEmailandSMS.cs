using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using MoveCarHackathonEdition.Data_Access_Layer;
using MoveCarHackathonEdition.Events;
using MoveCarHackathonEdition.Logs;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MoveCarHackathonEdition.Send_Mail_and_SMS
{
    internal class SendEmailandSms
    {
        //private readonly MsgStringCreator _msgString = new MsgStringCreator();

        public void OnSendtoEmployee(object source, EmailSmsEventArgs e)
        {
                Email(e.CarUserBlocker.EMAIL, MsgStringCreator.MsgToCarBlocker(e.CarUserBlocked), "MoveCar App - Please Move Your Car Immediately !", e.CarUserBlocker.Manager_EMAIL);
                Email(e.CarUserBlocked.EMAIL, MsgStringCreator.MsgToCarBlocked(e.CarUserBlocker), "MoveCar App - Here's Your Car Blocker Details");

                Sms(e.CarUserBlocker, MsgStringCreator.MsgToCarBlocker(e.CarUserBlocked));
                Sms(e.CarUserBlocked, MsgStringCreator.MsgToCarBlocked(e.CarUserBlocker));
        }

        public void OnSendtoEmployeeNoBlocker(object source, EmailSmsNoBlockerEventArgs e)
        {
                string plateNumberWithSpaces = AddSpacesinPlateText(e.Req.PLATE_NUMBER);

                Email(e.CarUserBlocked?.EMAIL, MsgStringCreator.MsgToCarBlocked(), "Move Car App - Your Blocker Car Plate Number was not found");
                
                ////Send Email to all EMC Empolyees
                //Email("Ramy.Reda@emc.com", $"Dear All, Owner of {plateNumberWithSpaces} <br />  Kindly move your car immediately as it blocks others to leave the office.", $"ATTENTION! Move Car App - {plateNumberWithSpaces} Please Move Your Car", "mohamed.abdallah@emc.com"); // All COE

                Sms(e.CarUserBlocked, MsgStringCreator.MsgToCarBlocked());
        }

        public void OnSendtoEmployeeNoBlocked(object source, EmailSmsNoBlockedEventArgs e)
        {
                Email(e.CarUserBlocker?.EMAIL, MsgStringCreator.MsgToCarBlocker(), "MoveCar App - Please Move Your Car", e.CarUserBlocker.Manager_EMAIL);
                Sms(e.CarUserBlocker, MsgStringCreator.MsgToCarBlocker());
        }

        private bool Sms(CAR_USER_VIEW user,string body)
        {
            // This is a trial twilio Account to test
            string accountSid = "ACa87fbb932f84a58fa90e92ad8a9e0bdf";
            string authToken = "0ed0016760b2631d2f1dc7ff78213a84";

            string testFromPhoneNumber = "+15005550006";

            TwilioClient.Init(accountSid, authToken);

            List<string> personalAndWork = GetUsersToSendsms(user);

            foreach (string personalOrWork in personalAndWork)
            {

                var message = MessageResource.Create(
                    to: new PhoneNumber(personalOrWork),
                    @from: new PhoneNumber(testFromPhoneNumber),
                    body: body
                    );

                if (message.ErrorCode != null || message.ErrorMessage != null)
                {
                    string error = $"Error Sending SMS with Error Code: {message.ErrorCode} & Error Message: {message.ErrorMessage}";
                    return false;
                }
                Console.WriteLine(message.Sid);
            }

            return true;
        }

        private static List<string> GetUsersToSendsms(CAR_USER_VIEW user)
        {
            List<string> personalAndWork = new List<string>();

            if (user.PERSONAL_MOBILE == user.WORK_MOBILE)
            {
                string personalMobileUpdated = user.PERSONAL_MOBILE;
                //Check if the number begins with "00" , and replace with "+"
                if (personalMobileUpdated.Substring(0, 2) == "00")
                    personalMobileUpdated = "+" + user.PERSONAL_MOBILE.Substring(2);

                personalAndWork.Add(personalMobileUpdated);
            }
            else
            {
                string personalMobileUpdated = user.PERSONAL_MOBILE;
                string workMobileUpdated = user.WORK_MOBILE;

                //Check if the number begins with "00" , and replace with "+"
                if (personalMobileUpdated.Substring(0, 2) == "00")
                    personalMobileUpdated = "+" + user.PERSONAL_MOBILE.Substring(2);
                if (workMobileUpdated.Substring(0, 2) == "00")
                    workMobileUpdated = "+" + user.WORK_MOBILE.Substring(2);

                personalAndWork.Add(personalMobileUpdated);
                personalAndWork.Add(workMobileUpdated);
            }

            return personalAndWork;
        }

        internal void Email(string to,
            string body,
            string subject,
            string CC = null,
            string fromAddress = "MoveCarApp@emc.com",
            string fromDisplay = "MoveCarApp",
            params Attachment[] attachments)
        {
            string host = ConfigurationManager.AppSettings["SMTPHost"];
            try
            {
                MailMessage mail = new MailMessage
                {
                    Body = body,
                    IsBodyHtml = true,
                    From = new MailAddress(fromAddress, fromDisplay, Encoding.UTF8),
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8,
                    Priority = MailPriority.Normal
                };

                ////////////////////////////////////
                
                //The Destination email Addresses
                MailAddressCollection toAddressList = new MailAddressCollection();

                //Prepare the Destination email Addresses list
                foreach (string currAddress in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    MailAddress mytoAddress = new MailAddress(currAddress, "MoveCar App");
                    toAddressList.Add(mytoAddress);
                }
                mail.To.Add(toAddressList.ToString());

                if (!String.IsNullOrEmpty(CC))
                {
                    mail.CC.Add(CC);
                }

                ////////////////////////////////////

                SmtpClient client = new SmtpClient();
                client.Send(mail);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder(1024);
                sb.Append("\nTo:" + to);
                sb.Append("\nbody:" + body);
                sb.Append("\nsubject:" + subject);
                sb.Append("\nfromAddress:" + fromAddress);
                sb.Append("\nfromDisplay:" + fromDisplay);
                //sb.Append("\ncredentialUser:" + credentialUser);
                //sb.Append("\ncredentialPasswordto:" + credentialPassword);
                sb.Append("\nHosting:" + host);
                Logger.Error(sb + " " + ex);
            }
        }

        private string AddSpacesinPlateText(string input)
        {
            return string.Join(" ", input.ToCharArray());
        }
    }
}