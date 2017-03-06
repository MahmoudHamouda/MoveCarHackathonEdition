namespace MoveCarHackathonEdition
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //Run As A Service
            //if (Environment.UserInteractive)
            //{
            //    string parameter = string.Concat(args);
            //    switch (parameter)
            //    {
            //        case "--install":
            //            ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
            //            break;
            //        case "--uninstall":
            //            ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
            //            break;
            //    }
            //}
            //else
            //{

            //    ServiceBase[] servicesToRun = new ServiceBase[]
            //                      {
            //                  new Service()
            //                      };
            //    ServiceBase.Run(servicesToRun);
            //}


            // For Testing use this Func

            Service1 myService = new Service1();
            myService.OnDebug();
        }
    }
}
