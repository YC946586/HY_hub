using HY.Client.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver
{
    public class Network
    {

        public static string ConnectionUrl { get; set; } = @"http://118.31.16.221:8012/eagle";

        //public static  Task<ServiceResponse> ApiPost<T>(string controller, string method, T parameter)
        //{
        //    try
        //    {
        //        string result = ""; //返回结果
        //        Encoding encoding = Encoding.UTF8;
        //        string url = ConnectionUrl +"/" +controller + "/" + method;
        //        //string date = JsonConvert.SerializeObject(parameter, Formatting.Indented);
        //        string parStr = "";
        //        foreach (var item in parameter)
        //        {
        //            parStr += item.Key + "=" + item.Value;
        //        }
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "POST";
        //        request.Accept = "*/*";
        //        request.ContentType = "application/json";
        //        //加入头信息
        //        //request.Headers.Add("Authorization",""); 
        //        //读数据
        //        request.Timeout = 300000;
        //        request.Headers.Set("Pragma", "no-cache");
        //        byte[] buffer = encoding.GetBytes(date);
        //        request.ContentLength = buffer.Length;
        //        request.GetRequestStream().Write(buffer, 0, buffer.Length);
        //        HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
        //        using (StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
        //        {
        //            result = reader.ReadToEnd();
        //        }
        //       ServiceResponse response=JsonConvert.DeserializeObject<ServiceResponse>(result);
        //       return  Task.FromResult(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

        protected internal static Task<ServiceResponse> ApiPost(string controller, string method, Dictionary<string, object> parameter)
        {
            try
            {
                string result = ""; //返回结果
                Encoding encoding = Encoding.UTF8;
                string url = ConnectionUrl + "/" + controller + "/" + method;
                //string date = JsonConvert.SerializeObject(parameter, Formatting.Indented);
                string parStr = "";
                foreach (var item in parameter)
                {
                    parStr += item.Key + "=" + item.Value+"&";
                }
                url += "?" + parStr.TrimEnd(Convert.ToChar("&"));
             
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Method = "POST";
                wbRequest.Accept = "*/*";
                wbRequest.Host = "118.31.16.221:8012";
                wbRequest.ContentType = "application/json";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                Stream responseStream = wbResponse.GetResponseStream();
                using (StreamReader sReader = new StreamReader(responseStream))
                {
                    result = sReader.ReadToEnd();
                }
                ServiceResponse response = JsonConvert.DeserializeObject<ServiceResponse>(result);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


     
    }
}
