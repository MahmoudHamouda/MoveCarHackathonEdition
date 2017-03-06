using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using MoveCarHackathonEdition.Logs;
using MoveCarHackathonEdition.MySqlNS;
using MoveCarHackathonEdition.Send_Mail_and_SMS;

namespace MoveCarHackathonEdition.Data_Access_Layer
{
    public class MySqlDatabaseRepository
    {
        private MySQLdb MySqldb { get; set; }

        // Check for open Cases
        public List<car_request> OpenCases()
        {
            try
            {
                return MySqldb.car_request.Where(r => r.STATUS.Contains("OPEN")).ToList();
            }
            catch (Exception)
            {
                return null;
                //throw;
            }
        }

        public bool Notification(CAR_USER_VIEW carUserBlocked, string notificationMsg, car_request req)
        {
            try
            {
                //Mail & SMS to Blocker
                car_notification notification = new car_notification
                {
                    MOBILE_ID = "",
                    NOTIFICATION_MSG = notificationMsg,
                    REQUEST_ID = req.ID
                };
                //notification.car_request = "";
                MySqldb.car_notification.Add(notification);
                MySqldb.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
        }

        
        public bool ChangeStatus(car_request req, String status)
        {
            try
            {
                req.STATUS = status;
                MySqldb.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
                //throw;
            }

        }
        
        public bool TestMySqlConnection()
        {
            MySqldb = new MySQLdb();
            DbConnection mySqlconn = MySqldb.Database.Connection;

            try
            {
                mySqlconn.Open(); // check MySql database connection
                MySqldb.Database.Exists();
                var count = MySqldb.car_request.Count();
                return true;
            }
            catch (Exception ex)
            {
                SendEmailandSms sendemail = new SendEmailandSms();
                sendemail.Email("yehia.amer@emc.com", $"{ex.Message} {ex.InnerException} {ex.StackTrace}", "Blocked Car App - MySql Database not Accessible ");
                return false;
            }
        }
    }
}