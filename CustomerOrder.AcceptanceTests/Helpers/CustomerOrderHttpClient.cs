using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CustomerOrder.AcceptanceTests.Helpers
{
    using Model;

    public class CustomerOrderHttpClient
    {
        internal const string BaseAddress = "http://localhost:3579";

        #region product add

        internal HttpResponseMessage ProductAdd(string relativeUrl, string productId)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(BaseAddress) })
            {
                var uri = BuildProductAddUrl(relativeUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonText);
                var postTask = client.PostAsync(uri, new StringContent(@"{""productId"" : """ + productId + @""" }",
                    Encoding.UTF8, "application/json"));

                return postTask.Result;
            }
        }

        internal HttpResponseMessage ProductAdd(string relativeUrl, string productId, Quantity quantity)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(BaseAddress) })
            {
                var uri = BuildProductAddUrl(relativeUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonText);
                var postTask = client.PostAsync(uri, new StringContent(string.Format(
                    "{{ \"productId\": \"{0}\", \"quantity\": {{ \"amount\": {1:a}, \"uom\": \"{1:u}\"}}}}", productId, quantity),
                    Encoding.UTF8, "application/json"));

                return postTask.Result;
            }
        }

        private Uri BuildProductAddUrl(string relativeUrl)
        {
            return BuildAbsoluteUri(relativeUrl);
        }

        private Uri BuildAbsoluteUri(string relativeUrl)
        {
            return new Uri(String.Format("{0}{1}", BaseAddress, relativeUrl), UriKind.Absolute);
        }

        internal Dictionary<string, string> GetSubscribeResult(HttpResponseMessage issueResult)
        {
            string content = issueResult.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
        }
        #endregion

        #region Status Asserts
        internal void AssertIsSuccess(HttpResponseMessage result)
        {
            Assert.IsTrue(result.IsSuccessStatusCode, String.Format("Status={0}", result.StatusCode));
        }

        public void AssertStatusAreEqual(HttpResponseMessage result, string expectedStatus)
        {
            const string assertMessage =
                "Expected status must be in the format '<number>' '<StatusName>' as defined by HttpStatusCode.  e.g. '407 ProxyAuthenticationRequired'";
            var parts = expectedStatus.Split(new[] { ' ' });
            Assert.AreEqual(2, parts.Count(), assertMessage);
            HttpStatusCode expectedStatusCode;
            Assert.IsTrue(Enum.TryParse(parts[0], true, out expectedStatusCode), assertMessage);
            Assert.AreEqual(expectedStatusCode.ToString(), parts[1], assertMessage);

            Assert.AreEqual(expectedStatusCode, result.StatusCode);
        }
        #endregion


        internal HttpResponseMessage GetProductAddedEvents(string relativeUrl, string acceptHeader)
        {
            return ExecuteGet(BuildAbsoluteUri(relativeUrl), acceptHeader);
        }

        private HttpResponseMessage ExecuteGet(Uri uri, string acceptHeader)
        {
            using (var client = new HttpClient {BaseAddress = new Uri(BaseAddress)})
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeader));
                var getTask = client.GetAsync(uri);
                return getTask.Result;
            }
        }

        public HttpResponseMessage GetOrderPricedEvents(string relativeUrl, string acceptHeader)
        {
            return ExecuteGet(BuildAbsoluteUri(relativeUrl), acceptHeader);
        }

        internal HttpResponseMessage GetAllEvents(string relativeUrl, string acceptHeader)
        {
            return ExecuteGet(BuildAbsoluteUri(relativeUrl), acceptHeader);
        }

        public HttpResponseMessage GetOrder(string relativeUrl, string acceptHeader)
        {
            return ExecuteGet(BuildAbsoluteUri(relativeUrl), acceptHeader);
        }

        internal HttpResponseMessage GetUrl(string absoluteUri, string acceptHeader)
        {
            return ExecuteGet(new Uri(absoluteUri), acceptHeader);
        }
    }
}
