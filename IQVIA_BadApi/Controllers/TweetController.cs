using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQVIA_BadApi.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace IQVIA_BadApi.Controllers
{
    public class TweetController : Controller
    {
        string Baseurl = "https://badapi.iqvia.io/";

        //Load page
        public ActionResult Index()
        {
            return View();
        }

        //GET: To get tweets between date range mentioned in web config
        [HttpGet]
        public async Task<ActionResult> GetData(DateTime sDate, DateTime eDate, double offSet)
        {
            #region
            try
            {
                //Initialization 
                int tweetCount = 0;
                List<Tweet> TweetInfo = new List<Tweet>();
                //Adding offset
                string startDate = sDate.AddMinutes(offSet).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                //Adding offset and subtracting 1 millisecond 
                string endDate = eDate.AddMilliseconds(-1).AddDays(1).AddMinutes(offSet).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

                //Looping to get all records between dates
                while (true)
                {
                    using (var client = new HttpClient())
                    {
                        //Passing service base url  
                        client.BaseAddress = new Uri(Baseurl);

                        client.DefaultRequestHeaders.Clear();
                        //Define request data format  
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //Sending request to find web api REST service resource GetAllTweets using HttpClient  
                        HttpResponseMessage Res = await client.GetAsync("api/v1/Tweets?startDate=" + startDate + "&endDate=" + endDate);

                        //Checking the response is successful or not which is sent using HttpClient  
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var TweetResponse = Res.Content.ReadAsStringAsync().Result;

                            //Deserializing the response recieved from web api and storing into the Tweet list  
                            //TweetInfo = JsonConvert.DeserializeObject<List<Tweet>>(TweetResponse);
                            TweetInfo.AddRange(JsonConvert.DeserializeObject<List<Tweet>>(TweetResponse));

                            //Compare total tweet count until previous request with current tweet count
                            if ((TweetInfo.Count - tweetCount) < 100) //TweetInfo.Count == tweetCount || 
                            {
                                //If it matchs, exist from loop
                                break;
                            }
                            else
                            {
                                //Assign total tweet count to variable to compare in next loop
                                tweetCount = TweetInfo.Count;

                                //Set last tweet time stamp to startDate for getting next set of data from swagger api
                                startDate = Convert.ToString(Convert.ToDateTime(TweetInfo[TweetInfo.Count - 1].stamp).ToUniversalTime().
                                    ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                            }
                        }
                    }
                }

                //For safer side checking for duplicate and removing if any
                TweetInfo = (from tw in TweetInfo
                             orderby tw.stamp ascending
                             group tw by tw.id into g
                             select g.First()).ToList();

                ViewData["Count"] = "Record Count: " + TweetInfo.Count;

                //returning tweet list to view  
                var jsonResult = Json(TweetInfo, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            #endregion
        }
    }
}