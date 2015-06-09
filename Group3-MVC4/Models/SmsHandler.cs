using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Group3_MVC4.Models
{
    public class SmsHandler
    {
        public async void SendMessage(string phone, string message)
        {
            // Connection and authorization information
            string account = ConfigurationManager.AppSettings["ApiAccount"];
            string password = ConfigurationManager.AppSettings["ApiPassword"];
            string sendApiUrl = ConfigurationManager.AppSettings["SendSMSApi"];

            using (var client = new HttpClient())
            {
                //1. Authorize account       
                client.BaseAddress = new Uri(sendApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes
                    (string.Format("{0}:{1}", account, password))));

                //2. Put data to send SMS
                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("to", phone),
                    new KeyValuePair<string, string>("message", message)
                };

                //3. Post data to API
                HttpContent content = new FormUrlEncodedContent(postData);

                //4.Wait for the return result
                HttpResponseMessage response = await client.PostAsync(sendApiUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    // Log here
                }
                else
                {
                    SmsResponse result = await response.Content.ReadAsAsync<SmsResponse>();
                    if (result.IsSuccessful)
                    {
                        // Log here
                    }
                }
            }
        }
    }
}