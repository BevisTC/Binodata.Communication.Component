using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Communication.Component.Error;

namespace Communication.Component.WebApi
{
    public static class HttpTools
    {
        static HttpTools()
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((p1, p2, p3, p4) => true);
        }

        public static async Task<string> UploadFile(string url, MultipartFormDataContent formData, object token = null)
        {
            var client = new HttpClient();
            var response = await client.PostAsync(url, formData).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpToolsException("File Upload is fail");
            }
            byte[] resp = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            return Encoding.GetEncoding("utf-8").GetString(resp);

        }

        public static HttpWebResponse Post(string url, byte[] body, string ContentType, object token = null, int timeOutSec = 90)
        {
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = ContentType;
            request.Timeout = timeOutSec * 1000;
            request.ContentLength = body.Length;
            request.AddToken(token);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
            }

            return (HttpWebResponse)request.GetResponse();
        }


        public static HttpWebResponse PostNoParam(string url, object token = null)
        {
            byte[] body = new byte[0];

            return Post(url, body, "text/plain ; charset=utf-8", token);
        }

        public static HttpWebResponse PostJson<T>(string url, T model, object token = null)
        {
            string json = JsonConvert.SerializeObject(model);
            byte[] body = Encoding.UTF8.GetBytes(json);

            return Post(url, body, "application/json ; charset=utf-8", token);
        }

        public static HttpWebResponse Get(string url, int timeOutSec = 90)
        {
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = timeOutSec * 1000;
            request.Method = "GET";

            return (HttpWebResponse)request.GetResponse();
        }

        public static byte[] ReadBody(this WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                return stream.ReadToEnd();
            }
        }

        private static void AddToken(this HttpWebRequest request, object token)
        {
            if (token == null)
                return;

            token.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && !string.IsNullOrWhiteSpace(p.GetValue(token) as string)).ToList() // 所有非空字串的property
                .ForEach(p => request.Headers.Add(p.Name, p.GetValue(token) as string)); //全加入Header
        }
    }


    public static class StreamExtension
    {
        public static byte[] ReadToEnd(this Stream stream)
        {
            byte[] buffer = new byte[4096];
            using (MemoryStream ms = new MemoryStream())
            {
                int read = 0;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                return ms.ToArray();
            }
        }
    }
}


