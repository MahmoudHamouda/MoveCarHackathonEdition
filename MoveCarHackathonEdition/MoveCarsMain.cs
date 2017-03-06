using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MoveCarHackathonEdition.Data_Access_Layer;
using MoveCarHackathonEdition.Events;
using MoveCarHackathonEdition.Logs;
using MoveCarHackathonEdition.MySqlNS;
using MoveCarHackathonEdition.Send_Mail_and_SMS;

namespace MoveCarHackathonEdition
{
    public sealed class MoveCarsMain
    {
        private readonly MySqlDatabaseRepository _mysqlRep = new MySqlDatabaseRepository();
        private readonly OracleDatabaseRepository _oracleRep = new OracleDatabaseRepository();

        public event EventHandler<EmailSmsEventArgs> EmailSmsEvent;
        public event EventHandler<EmailSmsNoBlockerEventArgs> EmailSmsEventNoBlocker;
        public event EventHandler<EmailSmsNoBlockedEventArgs> EmailSmsEventNoBlocked;

        public void DebugServiceExecute()
        {
            try
            {
                MoveCarsMain moveCarsMain = new MoveCarsMain();
                SendEmailandSms sendEmailandSms = new SendEmailandSms();

                moveCarsMain.EmailSmsEvent += sendEmailandSms.OnSendtoEmployee;
                moveCarsMain.EmailSmsEventNoBlocked += sendEmailandSms.OnSendtoEmployeeNoBlocked;
                moveCarsMain.EmailSmsEventNoBlocker += sendEmailandSms.OnSendtoEmployeeNoBlocker;
                moveCarsMain.Execute();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void Execute()
        {
            while (_mysqlRep.TestMySqlConnection() == false || _oracleRep.TestOracleConnection() == false)
            {
                if (_mysqlRep.TestMySqlConnection() == false)
                {
                    Logger.Info("Cannot Access MySQL Database");
                }
                else
                {
                    Logger.Info("Cannot Access Oracle Database");
                }

                Thread.Sleep(5000);
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (car_request req in _mysqlRep.OpenCases())
            {
                try
                {
                    CAR_USER_VIEW carUserBlocked = _oracleRep.CarUserBlocked(req?.CAR_USERNAME);
                    List<CAR_USER_VIEW> carUserBlockers = _oracleRep.CarUserBlockers(req?.PLATE_NUMBER);

                    _mysqlRep.ChangeStatus(req, "INPROGRESS");


                    if (carUserBlockers?.Count >= 1 && carUserBlocked?.EMAIL != null)
                    {
                        // Apply the "ChangeUserWorkExtensionToFullPhone" Method for all users found first
                        carUserBlocked.EXTENSION = ChangeUserWorkExtensionToFullPhone(carUserBlocked.EXTENSION);

                        foreach (CAR_USER_VIEW caruserBlocker in carUserBlockers)
                        {
                            if (caruserBlocker.EMAIL != null)
                            {
                                caruserBlocker.EXTENSION = ChangeUserWorkExtensionToFullPhone(caruserBlocker.EXTENSION);

                                OnSendtoEmployee(caruserBlocker, carUserBlocked);
                                Logger.Info($"Request N {req?.ID} Blocked: {carUserBlocked?.EMAIL} Blocker: {caruserBlocker?.EMAIL} ");

                                //Notification();
                                //add log to Oracle
                                _oracleRep.OracleTransactionLog(carUserBlocked, caruserBlocker, req);
                            }
                        }
                    }

                    else if ((carUserBlockers.Count < 1 || carUserBlockers == null) && carUserBlocked?.EMAIL != null)
                    {
                        carUserBlocked.EXTENSION = ChangeUserWorkExtensionToFullPhone(carUserBlocked?.EXTENSION);

                        OnSendtoEmployeeNoBlocker(carUserBlocked, req);
                        Logger.Info($"Request N {req?.ID} Blocked: {carUserBlocked?.EMAIL} ");
                    }

                    // blocked user not found -- what the hell case !!
                    else if (carUserBlocked?.EMAIL == null && carUserBlockers?.Count >= 1)
                    {
                        SendEmailandSms sendemail = new SendEmailandSms();

                        // this email is sent to admin, to enform him that there is a blocked user with no database record !
                        sendemail.Email("yehia.amer@emc.com", $"Hi yehia, Case ID is :{req?.ID}", "Blocked Car App - Cannot find the Blocked User - Admin Mail");

                        foreach (CAR_USER_VIEW caruserBlocker in carUserBlockers)
                        {
                            caruserBlocker.EXTENSION = ChangeUserWorkExtensionToFullPhone(caruserBlocker?.EXTENSION);


                            OnSendtoEmployeeNoBlocked(caruserBlocker, req);
                            Logger.Info($"Request N {req?.ID} Blocker: {caruserBlocker?.EMAIL} ");

                            //Notification();
                            //add log to Oracle
                            _oracleRep.OracleTransactionLog(null, caruserBlocker, req);
                        }
                    }

                    // Both blocked user and Blockers not found -- what the hell case !!
                    else if ((carUserBlockers.Count < 1 || carUserBlockers == null) && carUserBlocked?.EMAIL == null)
                    {
                        _mysqlRep.ChangeStatus(req, "BlockedandBlockerNF");
                        return;
                    }

                    // Unknown or unpredictable Case !
                    else
                    {
                        _mysqlRep.ChangeStatus(req, "BlockedandBlockerNF");
                        return;
                    }

                    _mysqlRep.ChangeStatus(req, "RESOLVED");
                }
                catch (Exception ex)
                {
                    Logger.Error($"{req?.ID} {ex?.Message} {ex?.InnerException} {ex?.StackTrace}");
                    //_mysqlRep.ChangeStatus(req, "OPEN");

                    SendEmailandSms sendemail = new SendEmailandSms();
                    sendemail.Email("yehia.amer@emc.com", $"{ex.Message} {ex.InnerException} {ex.StackTrace}", "General Exception");

                }
            }

            stopwatch.Stop();
        }

        private void OnSendtoEmployee(CAR_USER_VIEW carUserBlocker, CAR_USER_VIEW carUserBlocked, car_request req = null)
        {
            EmailSmsEvent?.Invoke(this, new EmailSmsEventArgs() { CarUserBlocker = carUserBlocker, CarUserBlocked = carUserBlocked, Req = req });
        }

        private void OnSendtoEmployeeNoBlocker(CAR_USER_VIEW carUserBlocked, car_request req = null)
        {
            EmailSmsEventNoBlocker?.Invoke(this, new EmailSmsNoBlockerEventArgs() { CarUserBlocked = carUserBlocked, Req = req });
        }

        private void OnSendtoEmployeeNoBlocked(CAR_USER_VIEW carUserBlocker, car_request req = null)
        {
            EmailSmsEventNoBlocked?.Invoke(this, new EmailSmsNoBlockedEventArgs() { CarUserBlocker = carUserBlocker, Req = req });
        }

        private static string ChangeUserWorkExtensionToFullPhone(string carUserExtension)
        {
            //o   022249 range: 1200 – 1599
            //o   022503 range: 2400 – 2599
            //o   022506 range: 5800 – 5999
            //o   022322 range: 5600 - 5799

            if (int.Parse(carUserExtension) >= 1200 && int.Parse(carUserExtension) <= 1599)
            {
                return "022249" + carUserExtension;
            }
            if (int.Parse(carUserExtension) >= 2400 && int.Parse(carUserExtension) <= 2599)
            {
                return "022503" + carUserExtension;
            }
            if (int.Parse(carUserExtension) >= 5800 && int.Parse(carUserExtension) <= 5999)
            {
                return "022506" + carUserExtension;
            }
            if (int.Parse(carUserExtension) >= 5600 && int.Parse(carUserExtension) <= 5799)
            {
                return "022322" + carUserExtension;
            }
            //All above cases are not applied, Return the Same number without modification !
            return carUserExtension;
        }
    }

}
