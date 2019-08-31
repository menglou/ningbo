
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QuartzBuildingDemo.model;
using QuartzBuildingDemo.until;

namespace QuartzBuildingDemo.until
{
   public class BuildingClientServiceHelper
    {
        private static readonly string url = ConfigurationManager.AppSettings["url"].ToString();
        private static readonly string clientName = ConfigurationManager.AppSettings["clientname"].ToString();
        private static readonly string clientCode = ConfigurationManager.AppSettings["clientcode"].ToString();

        private ByteStringConvert byteconvert = new ByteStringConvert();


        /// <summary>
        /// 获取客户端唯一标识符
        /// </summary>
        /// <returns></returns>
        public string GetUUID()
        {
            // 调用WebService GetUUID接口
            Uri address = new Uri(url + "/GetUUID");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            StringBuilder data = new StringBuilder();
            data.Append("clientName=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientName)));
            data.Append("&clientCode=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientCode)));
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            // WebService反馈
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string xml = AESHelper.AESDecrypt(ByteStringConvert.StringToBytes(reader.ReadToEnd()));
                reader.Close();
                return xml;
            }
        }

        /// <summary>
        /// 获取客户端通讯密码
        /// </summary>
        /// <param name="clientUUID"></param>
        /// <returns></returns>
        public string GetPassword(string clientUUID)
        {
            // 调用WebService GetPassword接口
            Uri address = new Uri(url + "/GetPassword");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            StringBuilder data = new StringBuilder();
            data.Append("clientName=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientName)));
            data.Append("&clientCode=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientCode)));
            data.Append("&clientUUID=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientUUID)));
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            // WebService反馈
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string xml = AESHelper.AESDecrypt(ByteStringConvert.StringToBytes(reader.ReadToEnd()));
                reader.Close();
                return xml;
            }
        }

        /// <summary>
        /// 客户端登录
        /// </summary>
        /// <param name="clientUUID"></param>
        /// <param name="clientPassword"></param>
        /// <returns></returns>
        public string Login(string clientUUID, string clientPassword)
        {
            // 调用WebService Login接口
            Uri address = new Uri(url + "/Login");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            StringBuilder data = new StringBuilder();
            data.Append("clientName=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientName)));
            data.Append("&clientCode=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientCode)));
            data.Append("&clientUUID=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientUUID)));
            data.Append("&clientPassword=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientPassword)));
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            // WebService反馈
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string xml = AESHelper.AESDecrypt(ByteStringConvert.StringToBytes(reader.ReadToEnd()));
                reader.Close();
                return xml;
            }
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <returns></returns>
        public string SendHeartBeat(string clientUUID, string clientPassword)
        {
            // 调用WebService SendHeartBeat接口
            Uri address = new Uri(url + "/SendHeartBeat");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            StringBuilder data = new StringBuilder();
            data.Append("clientName=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientName)));
            data.Append("&clientCode=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientCode)));
            data.Append("&clientUUID=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientUUID)));
            data.Append("&clientPassword=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientPassword)));
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            // WebService反馈
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string xml = AESHelper.AESDecrypt(ByteStringConvert.StringToBytes(reader.ReadToEnd()));
                reader.Close();
                return xml;
            }
        }

        /// <summary>
        /// 获取要发送得数据列表
        /// </summary>
        /// <returns></returns>
        public string GetBuildingData(string clientUUID, string clientPassword)
        {
            // 调用WebService GetBuildingData接口
            Uri address = new Uri(url + "/GetBuildingData");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            StringBuilder data = new StringBuilder();
            data.Append("clientName=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientName)));
            data.Append("&clientCode=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientCode)));
            data.Append("&clientUUID=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientUUID)));
            data.Append("&clientPassword=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientPassword)));
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            // WebService反馈
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string xml = AESHelper.AESDecrypt(ByteStringConvert.StringToBytes(reader.ReadToEnd()));
                reader.Close();
                return xml;
            }
        }


        public string SendMeterData(string clientUUID, string clientPassword, string datas)
        {
            // 调用WebService SendMeterData接口
            Uri address = new Uri(url + "/SendMeterData");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            StringBuilder data = new StringBuilder();
            data.Append("clientName=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientName)));
            data.Append("&clientCode=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientCode)));
            data.Append("&clientUUID=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientUUID)));
            data.Append("&clientPassword=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(clientPassword)));
            data.Append("&data=" + ByteStringConvert.BytesToString(AESHelper.AESEncrypt(datas)));
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            // WebService反馈
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string xml = AESHelper.AESDecrypt(ByteStringConvert.StringToBytes(reader.ReadToEnd()));
                reader.Close();
                return xml;
            }
        }
    }
}
