using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;

namespace BCS.Core.Kingdee
{
    public delegate void HttpClientDelegate(DateTime sender, string content);

    public class HttpClient
    {
        private CookieContainer cookiecontainer;
        private string url;
        /// <summary>
        /// 方法URL
        /// </summary>
        public string Url { get { return url; } set { url = value; } }
        /// <summary>
        /// CookieContainer，保证登录后，所有访问持有一个CookieContainer；
        /// </summary>
        public static CookieContainer cookieContainer = new CookieContainer();
        public CookieContainer CookieContainer
        {
            get
            {
                if (cookiecontainer != null)
                {
                    return cookiecontainer;
                }
                else
                {
                    return new CookieContainer();
                }
            }
            set { cookiecontainer = value; }
        }

        //第二步
        public void SysncRequest(DateTime sendTime, HttpClientDelegate resultCallback, string contentType, bool isByte)
        {
            
            HttpWebRequest httpWebRequest = WebRequest.Create(Url) as HttpWebRequest;
            httpWebRequest.CookieContainer = this.CookieContainer;
            if (contentType != "")
            {
                httpWebRequest.ContentType = contentType;
                httpWebRequest.Accept = contentType;
            }

            HttpWebResponse httpWebResponse = null;
            Stream responseStream = null;
            StreamReader streamReader = null;

            try
            {
                httpWebRequest.Timeout = 60 * 60 * 1000;
                httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                responseStream = httpWebResponse.GetResponseStream();
                if (isByte)
                {
                    
                }
                else
                {
                    streamReader = new StreamReader(responseStream);
                    string result = streamReader.ReadToEnd();

                    //第三步
                    resultCallback(sendTime, result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("发生错误!" + ex.ToString());
            }
            finally
            {
                if (streamReader != null) streamReader.Close();
                if (responseStream != null) responseStream.Close();
                if (httpWebResponse != null) httpWebResponse.Close();
            }
        }

        public string Get(string url, string contentType, bool isByte)
        {
            string result = string.Empty;
            HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.CookieContainer = this.CookieContainer;
            if (contentType != "")
            {
                httpWebRequest.ContentType = contentType;
                httpWebRequest.Accept = contentType;
            }

            HttpWebResponse httpWebResponse = null;
            Stream responseStream = null;
            StreamReader streamReader = null;

            try
            {
                httpWebRequest.Timeout = 60 * 60 * 1000;
                httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                responseStream = httpWebResponse.GetResponseStream();
                if (isByte)
                {
                    //未调用到，先删除处理。
                    //if (httpWebResponse.ContentLength == 0) return;
                    //System.IO.MemoryStream Ms = new MemoryStream();
                    //Image img = new System.Drawing.Bitmap(httpWebResponse.GetResponseStream());
                    //img.Save(Ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //byte[] imgs = new byte[Ms.Length];
                    //Ms.Position = 0;
                    //Ms.Read(imgs, 0, Convert.ToInt32(Ms.Length));
                    //Ms.Close();

                    //resultCallback(sendTime, Convert.ToBase64String(imgs));
                }
                else
                {
                    streamReader = new StreamReader(responseStream);
                    result = streamReader.ReadToEnd();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("发生错误!" + ex.ToString());
            }
            finally
            {
                if (streamReader != null) streamReader.Close();
                if (responseStream != null) responseStream.Close();
                if (httpWebResponse != null) httpWebResponse.Close();
            }
           
            return result;
        }

        public string Post(string url, string contentType, string postData)
		{
			string result = string.Empty;
			HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
			httpWebRequest.Method = "POST"; // 更改请求方法为 POST
			httpWebRequest.CookieContainer = this.CookieContainer;
            httpWebRequest.ServicePoint.Expect100Continue = false;
			// 设置请求的内容类型
			if (!string.IsNullOrEmpty(contentType))
			{
				httpWebRequest.ContentType = contentType;
				httpWebRequest.Accept = contentType;
			}
		
			// 准备要发送的 POST 数据
			byte[] dataBytes = Encoding.UTF8.GetBytes(postData);
		
			HttpWebResponse httpWebResponse = null;
			Stream requestStream = null;
			Stream responseStream = null;
			StreamReader streamReader = null;
		
			try
			{
				// 设置请求超时时间
				httpWebRequest.Timeout = 60 * 60 * 1000;
		
				// 写入 POST 数据到请求流
				requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(dataBytes, 0, dataBytes.Length);
				requestStream.Close();
		
				// 发送 HTTP POST 请求并获取响应
				httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				responseStream = httpWebResponse.GetResponseStream();
		
				// 将响应流转化为字符串
				streamReader = new StreamReader(responseStream);
				result = streamReader.ReadToEnd();
				return result;
			}
			catch (Exception ex)
			{
				// 捕获异常并抛出自定义异常消息
				throw new Exception("发生错误!" + ex.ToString());
			}
			finally
			{
				// 确保释放资源
				if (streamReader != null) streamReader.Close();
				if (responseStream != null) responseStream.Close();
				if (requestStream != null) requestStream.Close();
				if (httpWebResponse != null) httpWebResponse.Close();
			}
		}

    }
}
