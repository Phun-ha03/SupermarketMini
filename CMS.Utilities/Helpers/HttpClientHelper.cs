using System;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace CMS.Utilities.Helpers
{
    public class HttpClientHelper
    {
        public static string Craw(string url) {
            string retVal = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = true;

                const SecurityProtocolType tls13 = (SecurityProtocolType)12288;
                ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls
                                                        | SecurityProtocolType.Tls11
                                                        | SecurityProtocolType.Tls12
                                                        | tls13
                                                        | SecurityProtocolType.Ssl3;

                // Skip validation of SSL/TLS certificate
                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };


                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.54 Safari/537.36";

                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;

                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data))
                {
                    retVal = sr.ReadToEnd();
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return retVal;
        }
    }
}
