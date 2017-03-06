using System;
using System.Diagnostics;
using System.ServiceProcess;
using Quartz;
using Quartz.Impl;

namespace MoveCarHackathonEdition
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter { Level = LogLevel.Info };

                // Grab the Scheduler instance from the Factory 
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                // and start it off
                scheduler.Start();

                // define the job and tie it to our MoveCarsMainJob class
                IJobDetail myjob = JobBuilder.Create<QuartzJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger mytrigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(20)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(myjob, mytrigger);
                scheduler.Start();
                // some sleep to show what's happening
                //Thread.Sleep(TimeSpan.FromSeconds(60));

                // and last shut down the scheduler when you are ready to close your program
                //scheduler.Shutdown();
            }
            catch (Exception se)
            {
                Debug.WriteLine(se);
            }

            Debug.WriteLine("Press any key to close the application");
            //console.ReadKey();
        }

        protected override void OnStop()
        {
        }
    }
}
