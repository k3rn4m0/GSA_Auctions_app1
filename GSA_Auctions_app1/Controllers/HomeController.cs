using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GSA_Auctions_app1.Models;
using System.Net.Http;
using Newtonsoft.Json;
using GSA_Auctions_app1.DataAccess;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GSA_Auctions_app1.Controllers
{
    public class HomeController : Controller
    {

        public ApplicationDbContext dbContext;

        //Base URL for the IEXTrading API. Method specific URLs are appended to this base URL.
        string BASE_URL = "https://api.data.gov/gsa/auctions?api_key=HhzjT9mxA1caYgVWpLNWh0AI5r6xpMDEJ5P0wEJX&format=JSON";
        HttpClient httpClient;

        /// <summary>
        /// Initialize the database connection and HttpClient object
        /// </summary>
        /// <param name="context"></param>
        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

       

        public GSA_Rootobject GET_method()
        {
            Regex regex = new Regex(@"\s\s+");
            string Auction_resp_str = "";
            GSA_Rootobject Auction_Obj = null;
            string tst = "";
           
            httpClient.BaseAddress = new Uri(BASE_URL);
            HttpResponseMessage response = httpClient.GetAsync(BASE_URL).GetAwaiter().GetResult();

         
            if (response.IsSuccessStatusCode)
            {
                Auction_resp_str = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                tst = regex.Replace(Auction_resp_str, string.Empty);
            }

           
            if (!Auction_resp_str.Equals(""))
            {
              
                Auction_Obj = JsonConvert.DeserializeObject<GSA_Rootobject>(tst);
             
            }

            return Auction_Obj;
        }

        
        public IActionResult Index()
        {
            ViewBag.dbSucessComp = 0;
            IndexView indexVariable = new IndexView();

            indexVariable.auctionList = GET_method().Results.ToList();
            foreach (Result item in indexVariable.auctionList)
            {

                if (dbContext.AuctionResults.Where(c => c.ImageURL.Equals(item.ImageURL)).Count() == 0)
                {
                    dbContext.AuctionResults.Add(item);
                }
            }
            indexVariable.state = (indexVariable.auctionList.Select(s => s.PropertyState).Distinct()).OrderBy(s => s).ToList();
            //dbContext.SaveChanges();
            return View(indexVariable);
        }
        public IndexView viewFunction()
        {
            IndexView stateAuctionResult = new IndexView();
            stateAuctionResult.auctionList = GET_method().Results.ToList();
            stateAuctionResult.state = (stateAuctionResult.auctionList.Select(s => s.PropertyState).Distinct()).OrderBy(s => s).ToList();
            return (stateAuctionResult);
        }
        public IActionResult stateAuction()
        {
            float biddingAmount1 = 0, biddingAmount2 = 0;
            DateTime? startDate = null, endDate = null;
            if (Request.Form["startDate"] != "")
            {
                startDate = Convert.ToDateTime(Request.Form["startDate"]);
            }
            if (Request.Form["endDate"] != "")
            {
                endDate = Convert.ToDateTime(Request.Form["endDate"]);
            }
           
            if (Request.Form["biddingAmount1"] != "")
            {
                biddingAmount1 = float.Parse(Request.Form["biddingAmount1"]);
            }
            if(Request.Form["biddingAmount2"] != "")
            {
                biddingAmount2 = float.Parse(Request.Form["biddingAmount2"]);
            }
            string state = Request.Form["state"];
            IndexView stateAuctionResult = new IndexView();
            stateAuctionResult = viewFunction();
            CultureInfo MyCultureInfo = new CultureInfo("en-US");
            stateAuctionResult.auctionList = stateAuctionResult.auctionList.Where(s =>
                      (startDate == null || DateTime.Parse(s.AucStartDt, MyCultureInfo) >= startDate)
                      && (endDate == null || DateTime.Parse(s.AucEndDt, MyCultureInfo) <= endDate)
                      && (biddingAmount1 == 0 || s.HighBidAmount >= biddingAmount1)
                      && (biddingAmount2 == 0 || s.HighBidAmount <= biddingAmount2)
                      && (state == null || s.PropertyState.Equals(state))).ToList();


            return View("Index", stateAuctionResult);
        }
            public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Info()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
