namespace EM.Print.Enqueuer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            System.ServiceProcess.ServiceBase.Run(new System.ServiceProcess.ServiceBase[]
            {
                new PrintOrderEnqueuerService()
            });
        }
    }
}
