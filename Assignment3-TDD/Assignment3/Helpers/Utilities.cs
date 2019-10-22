using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RDJTPServer.Helpers
{
    public static class Utilities
    {
        public static void SendResponse(this TcpClient client, string response)
        {
            var msg = Encoding.UTF8.GetBytes(response);
            client.GetStream().Write(msg, 0, msg.Length);
        }

        public static Request ReadRequest(this TcpClient client)
        {
            var stream = client.GetStream();
            var resp = new byte[4096];
            using var memStream = new MemoryStream();
            var bytesRead = 0;
            do
            {
                bytesRead = stream.Read(resp, 0, resp.Length);
                memStream.Write(resp, 0, bytesRead);

            } while (bytesRead == 2048);

            var responseData = Encoding.UTF8.GetString(memStream.ToArray());
            return JsonConvert.DeserializeObject<Request>(responseData);
        }

        public static string ToJson(this object data)
        {
            return JsonConvert.SerializeObject(data,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public static T FromJson<T>(this string element)
        {
            return JsonConvert.DeserializeObject<T>(element);
        }

        public static string UnixTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }

        public static bool IsValidJson(string json)
        {
            try
            {
                var token = JObject.Parse(json);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int IdFromPath(string path)
        {

            return int.Parse(path.Split("/")[3]);
        }
    }

}