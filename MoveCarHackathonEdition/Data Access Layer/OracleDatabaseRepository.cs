using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MoveCarHackathonEdition.MySqlNS;
using MoveCarHackathonEdition.Send_Mail_and_SMS;
using MySql.Data.MySqlClient;

namespace MoveCarHackathonEdition.Data_Access_Layer
{
    public class OracleDatabaseRepository
    {
        // Get Blocked User
        public CAR_USER_VIEW CarUserBlocked(string carUsername)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("SERVER = localhost ; DATABASE= Oracledb ; UID= root ; PASSWORD=;Charset=utf8;"))
                {
                    //string sql = "SELECT * FROM \"EGYPTCOE\".\"CAR_USER_VIEW\" WHERE \"CAR_USERNAME\" = '" + carUsername + "'";
                    string sql = $"SELECT * FROM CAR_USER_VIEW WHERE UPPER(CAR_USERNAME) = UPPER('{carUsername}')";
                    return connection.Query<CAR_USER_VIEW>(sql).FirstOrDefault();
                }
                //return Oracledb.CAR_USER_VIEW.Single(cu => cu.CAR_USERNAME.Contains(carUsername));
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Get Blockers
        public List<CAR_USER_VIEW> CarUserBlockers(string plateNumber)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("SERVER = localhost ; DATABASE= Oracledb ; UID= root ; PASSWORD=;Charset=utf8;"))
                {
                    string sql = $"SELECT * FROM CAR_USER_VIEW WHERE PLATE_NUMBER = '{plateNumber}'";
                    return connection.Query<CAR_USER_VIEW>(sql).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Log the Transactions
        public bool OracleTransactionLog(CAR_USER_VIEW carUserBlocked, CAR_USER_VIEW carUserBlocker, car_request id)
        {
            try
            {
                //Mail & SMS to Blocker


                using (MySqlConnection connection = new MySqlConnection("SERVER = localhost ; DATABASE= Oracledb ; UID= root ; PASSWORD=;Charset=utf8;"))
                {
                    string sql =
                        "INSERT INTO `CAR_LOG` (`CAR_REQUEST_ID`,`CAR_NOTIFICATION_ID`,`BLOCKED_USER`,`BLOCKER_USER`,`REQUEST_DATE`) " +
                        "VALUES(@CAR_REQUEST_ID , @CAR_NOTIFICATION_ID , @BLOCKED_USER , @BLOCKER_USER , @REQUEST_DATE)";

                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@CAR_REQUEST_ID", 123); // Need To be Changed
                        cmd.Parameters.AddWithValue("@CAR_NOTIFICATION_ID", 7357);
                        cmd.Parameters.AddWithValue("@BLOCKED_USER", carUserBlocked.CAR_USERNAME ?? " ");
                        cmd.Parameters.AddWithValue("@BLOCKER_USER", carUserBlocked.CAR_USERNAME ?? " ");
                        cmd.Parameters.AddWithValue("@REQUEST_DATE", DateTime.Now);
                        //Execute command
                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        
        public bool TestOracleConnection()
        {
            using (MySqlConnection connection = new MySqlConnection("SERVER = localhost ; DATABASE= Oracledb ; UID= root ; PASSWORD=;"))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (MySqlException ex)
                {
                    SendEmailandSms sendemail = new SendEmailandSms();
                    sendemail.Email("yehia.amer@emc.com", $"{ex.Message} {ex.InnerException} {ex.StackTrace}", "Blocked Car App - MySql Database not Accessible ");

                    return false;
                }
            }
        }
    }
}