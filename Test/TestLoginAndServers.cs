using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TesonetMariusBudrauskas.Models;

namespace Test
{
    [TestClass]
    public class TestLoginAndServers
    {
        [TestMethod]
        public void TestMethod1()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://playground.tesonet.lt/v1/tokens");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"username\":\"tesonet\"," +
                                "\"password\":\"partyanimal\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                result = result.Split(':')[1].Remove(0, 1);
                result = result.Remove(result.Length - 2);

                Assert.AreNotEqual(0, result.Length);

                var request = System.Net.HttpWebRequest.Create("http://playground.tesonet.lt/v1/servers");
                request.Method = "GET";
                request.Headers.Add("Authorization", "Bearer " + result);
                using (System.Net.WebResponse response = request.GetResponse())
                {
                    using (StreamReader streamReader2 = new StreamReader(response.GetResponseStream()))
                    {
                        dynamic jsonResponseText = streamReader2.ReadToEnd();

                        var allServers = JsonConvert.DeserializeObject<List<Servers>>(jsonResponseText);

                        Assert.AreNotEqual(0, allServers);
                    }
                }
            }
        }
    }
}
