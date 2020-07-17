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
        public static string Authorization { get; set; }
        public static string ConnectionUrl { get; set; } = @"http://118.31.16.221:8012/eagle";
       
        //protected internal static Task<ServiceResponse> ApiPost<T>(string controller, string method, T parameter)
        //{
        //    try
        //    {
        //        string result = ""; //返回结果
        //        string url = ConnectionUrl + "/" + controller + "/" + method;
        //        string date = JsonConvert.SerializeObject(parameter);
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "POST";
        //        request.Accept = "text/html, application/xhtml+xml, */*";
        //        request.ContentType = "application/json";
        //        //读数据
        //        request.Timeout = 300000;
        //        request.Headers.Set("Pragma", "no-cache");
        //        byte[] buffer = Encoding.UTF8.GetBytes(date);
        //        request.ContentLength = buffer.Length;
        //        request.GetRequestStream().Write(buffer, 0, buffer.Length);
        //        HttpWebResponse WebResponse = (HttpWebResponse)request.GetResponse();
        //        using (StreamReader reader = new StreamReader(WebResponse.GetResponseStream(), Encoding.UTF8))
        //        {
        //            result = reader.ReadToEnd();
        //        }
        //        ServiceResponse response = JsonConvert.DeserializeObject<ServiceResponse>(result);
        //        return Task.FromResult(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        protected internal static Task<ServiceResponse> ApiPost(string controller, string method, Dictionary<string, object> parameter=null)
        {
            try
            {
                string result = ""; //返回结果
                Encoding encoding = Encoding.UTF8;
                string url = ConnectionUrl + "/" + controller + "/" + method;
                //string date = JsonConvert.SerializeObject(parameter, Formatting.Indented);
                string parStr = "";
                if (parameter!=null)
                {
                    foreach (var item in parameter)
                    {
                        parStr += item.Key + "=" + item.Value + "&";
                    }
                    url += "?" + parStr.TrimEnd(Convert.ToChar("&"));
                }
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Method = "POST";
                wbRequest.Accept = "*/*";
                wbRequest.Host = "118.31.16.221:8012";
                if (!string.IsNullOrEmpty(Authorization))
                {
                    wbRequest.Headers.Add("Authorization", Authorization); //当前请求是否拦截
                }
                wbRequest.ContentType = "application/json";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                Stream responseStream = wbResponse.GetResponseStream();
                using (StreamReader sReader = new StreamReader(responseStream))
                {
                    result = sReader.ReadToEnd();
                }
                responseStream.Dispose();
                ServiceResponse response = JsonConvert.DeserializeObject<ServiceResponse>(result);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        protected internal static Task<ServiceResponse> ApiGet(string controller, string method, Dictionary<string, object> parameter = null)
        {
            try
            {
                string result = ""; //返回结果
                Encoding encoding = Encoding.UTF8;
                string url = ConnectionUrl + "/" + controller + "/" + method;
                //string date = JsonConvert.SerializeObject(parameter, Formatting.Indented);
                string parStr = "";
                if (parameter != null)
                {
                    foreach (var item in parameter)
                    {
                        parStr += item.Key + "=" + item.Value + "&";
                    }
                    url += "?" + parStr.TrimEnd(Convert.ToChar("&"));
                }
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
               
                wbRequest.Method = "GET";
                wbRequest.Accept = "*/*";
                wbRequest.Host = "118.31.16.221:8012";
                wbRequest.Headers.Set("Pragma", "no-cache");
                wbRequest.Headers.Set("Accept-Encoding", "gzip, deflate");
                if (!string.IsNullOrEmpty(Authorization))
                {
                    wbRequest.Headers.Set("Authorization", Authorization); 
                }
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                Stream responseStream = wbResponse.GetResponseStream();
                using (StreamReader sReader = new StreamReader(responseStream))
                {
                    result = sReader.ReadToEnd();
                }
                responseStream.Dispose();
                ServiceResponse response = JsonConvert.DeserializeObject<ServiceResponse>(result);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static void HttpDownload(string url, string strPath,string imgName)
        {
            try
            {
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                else
                {
                    DirectoryInfo root = new DirectoryInfo(strPath);
                    FileInfo[] files = root.GetFiles();
                    foreach (var item in files)
                    {
                        try
                        {
                            item.Delete();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                strPath = strPath + imgName;
                FileStream fs = new FileStream(strPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                byte[] bArr = new byte[1024];
                int iTotalSize = 0;
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    iTotalSize += size;
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                fs.Close();
                responseStream.Close();
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
