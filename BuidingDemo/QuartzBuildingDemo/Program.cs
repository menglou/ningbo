using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace QuartzBuildingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            // Logger logger = NLog.LogManager.GetLogger("test");
            try
            {
               

                HostFactory.Run(x =>
                {

                    x.SetDescription("Building服务");
                    x.SetDisplayName("Building Quartz");
                    x.SetServiceName("Building Quartz主要用于调度");

                    x.Service<ServiceRunner>();


                    //x.EnablePauseAndContinue();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
