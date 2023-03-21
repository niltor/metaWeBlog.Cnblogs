using System.Net;
using System.Text;

namespace Ater.MetaWeBlog.XmlRPC
{
    public class Service
    {
        public string URL { get; private set; }

        private readonly bool EnableExpect100Continue = false;

        public Service(string url)
        {
            URL = url;
        }

        public CookieContainer Cookies = null;

        public MethodResponse Execute(MethodCall methodcall)
        {
            System.Xml.Linq.XDocument doc = methodcall.CreateDocument();
            WebRequest request = WebRequest.Create(URL);

            if (Cookies != null)
            {
                HttpWebRequest hRequest = request as HttpWebRequest;
                hRequest.CookieContainer = Cookies;

            }

            HttpWebRequest wr = (HttpWebRequest)request;
            wr.ServicePoint.Expect100Continue = EnableExpect100Continue;
            request.Method = "POST";
            string content = doc.ToString();
            byte[] byteArray = Encoding.UTF8.GetBytes(content);
            request.ContentType = "text/xml;charset=utf-8";
            request.ContentLength = byteArray.Length;

            using (System.IO.Stream webpageStream = request.GetRequestStream())
            {
                webpageStream.Write(byteArray, 0, byteArray.Length);
            }

            using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
            {
                using (System.IO.Stream responseStream = webResponse.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        throw new XmlRPCException("Response Stream is unexpectedly null");
                    }

                    using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                    {
                        string webpageContent = reader.ReadToEnd();
                        MethodResponse response = new MethodResponse(webpageContent);
                        return response;
                    }
                }
            }
        }
    }
}