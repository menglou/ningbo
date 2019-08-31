using Common.Logging;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuartzBuildingDemo.model;
using QuartzBuildingDemo.until;
using System.Configuration;
using System.Data;
using System.Xml;

namespace QuartzBuildingDemo
{
    public class BuildingJob : IJob
    {
         private static readonly  Logger logger = NLog.LogManager.GetLogger("test");


         static string uuid = ConfigurationManager.AppSettings["uuid"].ToString();
         static BuildingClientServiceHelper servicehelper = new until.BuildingClientServiceHelper();
       
         static System.Timers.Timer timers = new System.Timers.Timer();

         static root rootpwd = new root();
         static root rootlogin = new root();
         static root rootheart = new root();
         static root rootfour = new root();


        public void Execute(IJobExecutionContext context)
        {
            // string uuidxml = servicehelper.GetUUID();

            //获取密码
            Console.WriteLine(DateTime.Now + "：" + "开始获取客户端通讯密码");
            logger.Info(DateTime.Now + "：" + "开始获取客户端通讯密码");
            string pwdxml = servicehelper.GetPassword(uuid);
            rootpwd = XmlSerializeHelper.DESerializer<root>(pwdxml);
            if (rootpwd.code == "0000")
            {
                Console.WriteLine(DateTime.Now + "：" + "获取客户端通讯密码成功");
                logger.Info(DateTime.Now + "：" + "获取客户端通讯密码成功");
                //登录
                Console.WriteLine(DateTime.Now + "：" + "开始客户端登录");
                logger.Info(DateTime.Now + "：" + "开始客户端登录");
                string loginxml = servicehelper.Login(uuid, rootpwd.pw);
                rootlogin = XmlSerializeHelper.DESerializer<root>(loginxml);


                if (rootlogin.code == "0000")
                {
                    Console.WriteLine(DateTime.Now + "：" + "客户端登录成功");
                    logger.Info(DateTime.Now + "：" + "客户端登录成功");
                    //ExectueSendHeartBeat();//先执行一次心跳接口

                    timers.Elapsed += Timers_Elapsed;
                    timers.Interval = Convert.ToDouble(rootlogin.heartbeattime) * 1000;
                    timers.AutoReset = true;
                    timers.Enabled = true;
                    timers.Start();


                    //数据列表
                    Console.WriteLine(DateTime.Now + "：" + "开始执行获取要发送的数据列表");
                    logger.Info(DateTime.Now + "：" + "开始执行获取要发送的数据列表");
                    string buildongxml = servicehelper.GetBuildingData(uuid, rootpwd.pw);
                    allnode allmode = XmltoXMLDocument(buildongxml);

                    if (allmode.root.code == "0000")
                    {

                        Console.WriteLine("------------------" + "数据列表" + "----------------");
                        logger.Info("------------------" + "数据列表" + "----------------");
                        if (allmode.buildinglist.Count > 0)
                        {
                            foreach (var item in allmode.buildinglist)
                            {
                                Console.WriteLine("xml如下\n" + "<building id=\"" + item.id + "\"" + " code=\"" + item.code + "\">");
                                logger.Info("xml如下\n" + "<building id=\"" + item.id + "\"" + " code=\"" + item.code + "\">");
                            }
                        }
                        if (allmode.meterlist.Count > 0)
                        {
                            foreach (var itemmeter in allmode.meterlist)
                            {
                                Console.WriteLine("  <meter id=\"" + itemmeter.attibuteid + "\">" + itemmeter.innertext + "</meter>");
                                logger.Info("  <meter id=\"" + itemmeter.attibuteid + "\">" + itemmeter.innertext + "</meter>");
                            }
                            Console.WriteLine("</building>");
                            logger.Info("</building>");
                        }
                        Console.WriteLine("数据列表数量：" + allmode.meterlist.Count);
                        logger.Info(DateTime.Now + "：" + "数据列表数量：" + allmode.meterlist.Count);
                        Console.WriteLine("------------------" + "数据列表" + "----------------");
                        logger.Info("------------------" + "数据列表" + "----------------");
                        //发送数据
                        Console.WriteLine(DateTime.Now + "：" + "执行获取要发送的数据列表成功");
                        logger.Info(DateTime.Now + "：" + "执行获取要发送的数据列表成功");

                        List<datatabledata> datalist = GetLocalDataFormWebService();//获取本地数据
                        if (datalist.Count > 0)
                        {

                            List<string> returndata = JointStringToXml(datalist, allmode);

                            foreach (var item in returndata)
                            {

                                Console.WriteLine(DateTime.Now + "：" + "开始执行发送数据接口");
                                logger.Info(DateTime.Now + "：" + "开始执行发送数据接口");

                                string test = ConfigurationManager.AppSettings["test"].ToString();

                                if (test == "1")
                                {
                                    string senddataxml = servicehelper.SendMeterData(uuid, rootpwd.pw, item);

                                    root rootsenddata = XmlSerializeHelper.DESerializer<root>(senddataxml);

                                    if (rootsenddata.code == "0000")
                                    {
                                        Console.WriteLine(DateTime.Now + "：" + "执行发送数据接口成功");
                                        logger.Info(DateTime.Now + "：" + "执行发送数据接口成功");
                                    }
                                    else
                                    {
                                        Console.WriteLine(DateTime.Now + "：" + "执行发送数据接口失败，原因：" + rootsenddata.msg);
                                        logger.Info(DateTime.Now + "：" + "执行发送数据接口失败，原因：" + rootsenddata.msg);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(DateTime.Now + "：" + "执行发送数据接口成功");
                                    logger.Info(DateTime.Now + "：" + "执行发送数据接口成功");
                                }

                            }
                        }
                    }
                    else
                    {
                        //todo 记日志？？ 
                        Console.WriteLine(DateTime.Now + "：" + "获取要发送的数据列表，原因：" + allmode.root.msg);
                        logger.Info(DateTime.Now + "：" + "获取要发送的数据列表，原因：" + allmode.root.msg);
                    }

                }
                else
                {
                    //todo 记日志？？
                    Console.WriteLine(DateTime.Now + "：" + "客户端登录失败,原因：" + rootlogin.msg);
                    logger.Info(DateTime.Now + "：" + "客户端登录失败,原因：" + rootlogin.msg);
                }
            }
            else
            {
                //todo 记日志？？
                Console.WriteLine(DateTime.Now + "：" + "获取客户端通讯密码失败，原因：" + rootpwd.msg);
                logger.Info(DateTime.Now + "：" + "获取客户端通讯密码失败，原因：" + rootpwd.msg);
            }
           // Console.ReadKey();
        }


        private static void Timers_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ExectueSendHeartBeat();
        }


        private static void ExectueSendHeartBeat()
        {
            Console.WriteLine(DateTime.Now + "：" + "开始执行心跳接口");
            logger.Info(DateTime.Now + "：" + "开始执行心跳接口");
            string heartxml = servicehelper.SendHeartBeat(uuid, rootpwd.pw);
            rootheart = XmlSerializeHelper.DESerializer<root>(heartxml);

            if (rootheart.code == "0000")
            {
                Console.WriteLine(DateTime.Now + "：" + "执行心跳接口成功");
                logger.Info(DateTime.Now + "：" + "执行心跳接口成功");
            }
            else
            {
                //todo 记日志？？ （心跳接口用来检测登录是否过期，如果）
                Console.WriteLine(DateTime.Now + "：" + "执行心跳接口失败，原因：" + rootheart.msg);
                logger.Info(DateTime.Now + "：" + "执行心跳接口失败，原因：" + rootheart.msg);
            }

        }

        /// <summary>
        /// 读xml方法
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        private static allnode XmltoXMLDocument(string xmlstr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);

            //得到根节点
            XmlNode xn = xmlDoc.SelectSingleNode("root");

            //得到所有得子节点
            XmlNodeList xnl = xn.ChildNodes;
            allnode allnode = new allnode();

            root root = new root();
            List<building> buildinglist = new List<building>();
            List<meter> meterlist = new List<meter>();
            foreach (XmlNode item in xnl)
            {
                switch (item.Name)
                {
                    case "code":
                        root.code = item.InnerText;
                        break;
                    case "msg":
                        root.msg = item.InnerText;
                        break;
                    case "building":
                        building buid = new building();
                        buid.id = item.Attributes["id"].Value.ToString();
                        buid.code = item.Attributes["code"].Value.ToString();
                        buildinglist.Add(buid);
                        if (item.ChildNodes.Count > 0)
                        {
                            foreach (XmlNode itemnode in item.ChildNodes)//获取二级子节点
                            {
                                meter meter = new meter();
                                meter.attibuteid = itemnode.Attributes["id"].Value.ToString();
                                meter.innertext = itemnode.InnerText;
                                meter.parentid = item.Attributes["id"].Value.ToString();//存父节点得id是为了找对应得父节点,当有多个父节点时
                                meterlist.Add(meter);
                            }
                        }
                        break;
                }
            }

            allnode.root = root;
            allnode.buildinglist = buildinglist;
            allnode.meterlist = meterlist;
            return allnode;
        }

        /// <summary>
        /// 通过接口获取本地数据
        /// </summary>
        /// <returns></returns>
        private static List<datatabledata> GetLocalDataFormWebService()
        {
            DataTable tabe = new ServiceReference1.Service1Client().GetElectricMeter("Hu19921227");

            DataTable tabewater = new ServiceReference1.Service1Client().GetWaterMeter("Hu19921227");


            DataRowCollection rows = tabe.Rows;

            List<datatabledata> datalist = new List<datatabledata>();

            if (rows.Count > 0)
            {
                foreach (DataRow item in rows)
                {
                    datatabledata data = new datatabledata
                    {
                        meterid = item["MeterId"].ToString().Trim(),
                        result = item["Result"].ToString().Trim(),
                        time = Convert.ToDateTime(item["Time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim()
                    };
                    datalist.Add(data);
                }
            }

            DataRowCollection rowswater = tabewater.Rows;

            if (rows.Count > 0)
            {
                foreach (DataRow item in rowswater)
                {
                    datatabledata data = new datatabledata
                    {
                        meterid = item["MeterId"].ToString().Trim(),
                        result = item["Result"].ToString().Trim(),
                        time = Convert.ToDateTime(item["Time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim()
                    };
                    datalist.Add(data);
                }
            }


            return datalist;
        }

        /// <summary>
        /// 拼接xml
        /// </summary>
        /// <param name="datalist"></param>
        /// <param name="allnode"></param>
        public static List<string> JointStringToXml(List<datatabledata> datalist, allnode allnode)
        {

            StringBuilder newstr = new StringBuilder();
            string msg = "<?xml version=\"1.0\"  encoding=\"UTF-8\" ?>";
            newstr.Append(msg);
            newstr.Append("<root>");
            List<datatabledata> datapram = new List<datatabledata>();
            List<string> datastr = new List<string>();//发送数据时要的参数（data）--有可能有多个

            foreach (var item in allnode.buildinglist)
            {
                newstr.Append("<building  id=\"" + item.id + "\">");

                Console.WriteLine("------------------" + "发送的数据列表" + "----------------");
                logger.Info("------------------" + "发送的数据列表" + "----------------");

                Console.WriteLine("xml如下\n" + "<building  id=\"" + item.id + "\">");
                logger.Info("xml如下\n" + "<building  id=\"" + item.id + "\">");

                foreach (var meterparam in allnode.meterlist)//根据meter的innertext筛选数据
                {
                    datatabledata data = new datatabledata();

                    //meterparam.attibuteid;

                    data = datalist.FirstOrDefault(x => x.meterid == meterparam.innertext);

                    if (data != null)
                    {
                        data.id = meterparam.attibuteid;
                        data.extid = meterparam.innertext;
                        datapram.Add(data);
                    }
                }

                if (datapram.Count > 0)
                {
                    foreach (var dataitem in datapram)
                    {
                        newstr.Append("<meter  id=\"" + dataitem.id + "\"" + "  extid=\"" + dataitem.extid + "\"" + " time=\"" + dataitem.time + "\"" + " >" + dataitem.result + "</meter>");

                        Console.WriteLine("   <meter  id=\"" + dataitem.id + "\"" + "  extid=\"" + dataitem.extid + "\"" + " time=\"" + dataitem.time + "\"" + " >" + dataitem.result + "</meter>");
                        logger.Info("   <meter  id=\"" + dataitem.id + "\"" + "  extid=\"" + dataitem.extid + "\"" + " time=\"" + dataitem.time + "\"" + " >" + dataitem.result + "</meter>");
                    }
                    Console.WriteLine("</building>");
                    logger.Info("</building>");
                    Console.WriteLine("发送的数据列表数量：" + datapram.Count);
                    logger.Info("发送的数据列表数量：" + datapram.Count);

                    Console.WriteLine("------------------" + "发送的数据列表" + "----------------");
                    logger.Info("------------------" + "发送的数据列表" + "----------------");

                    newstr.Append("</building>");
                    newstr.Append("</root>");

                    datastr.Add(newstr.ToString());
                }
            }

            return datastr;
        }
    }
}
