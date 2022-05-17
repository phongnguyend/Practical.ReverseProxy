using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;

namespace Practical.ReverseProxy.ReverseProxy.NetFramework
{
    public static class HttpContentExtensions
    {

        public static StringContent AsStringContent(this object obj, string contentType)
        {
            var text = string.Empty;

            if (contentType == "application/json")
            {
                text = JsonConvert.SerializeObject(obj);
            }
            else if (contentType == "application/xml")
            {
                var xmlSerializer = new XmlSerializer(obj.GetType());

                using (var stringWriter = new StringWriter())
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, obj);
                    text = stringWriter.ToString();
                }
            }
            else
            {
                throw new Exception($"Unsupported Media Type: {contentType}");
            }

            var content = new StringContent(text);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return content;
        }

        public static StringContent AsJsonContent(this object obj)
        {
            return obj.AsStringContent("application/json");
        }

        public static StringContent AsXmlContent(this object obj)
        {
            return obj.AsStringContent("application/xml");
        }
    }
}
