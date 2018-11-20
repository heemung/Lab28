using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Lab28.Models;
using Newtonsoft.Json.Linq;

namespace Lab28.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MakeDeck()
        {
            //for deck ID
            string deckID;

            //make our request
            HttpWebRequest requestData = WebRequest.CreateHttp(
                "https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1");
            //get in habit of using this
            requestData.UserAgent = "Mozilla / 5.0(Windows NT 6.1; WOW64; rv: 64.0) Gecko / 20100101 Firefox / 64.0";

            // make reponse, cast 
            HttpWebResponse responseData = (HttpWebResponse)requestData.GetResponse();

            //got data, do this. code 200
            if(responseData.StatusCode == HttpStatusCode.OK)
            {
                //get response stream
                StreamReader reader = new StreamReader(responseData.GetResponseStream());

                //store it in string
                string readerData = reader.ReadToEnd();

                //abc
                reader.Close();

                //convert to Json Object
                JObject jsonDeck = JObject.Parse(readerData);

                //do we have a deck already?
                if(TempData["deck_id"] == null)
                {
                    //make temp data = jsonDeck
                    //make string = jsonDeck
                    TempData["deck_id"] = jsonDeck["deck_id"];
                    deckID = jsonDeck["deck_id"].ToString();

                }
                else
                {
                    deckID = jsonDeck["deck_id"].ToString();
                }
                ViewBag.DeckID = deckID;

            }
            else
            {

            }

            responseData.Close();
            return View("Index");
        }

        public ActionResult DrawCards(string deckID)
        {
            //make cookies to store temp data
            HttpCookie theCookie;

            if(Request.Cookies["deckID"] == null)
            {
                theCookie = new HttpCookie("deckID");
                theCookie.Value = deckID;
                Request.Cookies.Add(theCookie);
            }
            else
            {
                theCookie = Request.Cookies["deckID"];
                theCookie.Value = deckID;
            }

            //make our request
            HttpWebRequest requestData = WebRequest.CreateHttp(
                "https://deckofcardsapi.com/api/deck/"+deckID+"/draw/?count=5");
            //get in habit of using this
            requestData.UserAgent = "Mozilla / 5.0(Windows NT 6.1; WOW64; rv: 64.0) Gecko / 20100101 Firefox / 64.0";

            // make reponse, cast 
            HttpWebResponse responseData = (HttpWebResponse)requestData.GetResponse();

            //got data, do this. code 200
            if (responseData.StatusCode == HttpStatusCode.OK)
            {
                //get response stream
                StreamReader reader = new StreamReader(responseData.GetResponseStream());

                //store it in string
                string readerData = reader.ReadToEnd();

                //abc
                reader.Close();

                //convert to Json Object
                JObject jsonCards = JObject.Parse(readerData);

                ViewBag.CardBag = jsonCards["cards"];

                //Card model style
                Card[] cards = new Card[jsonCards["cards"].Count()];
                int i = 0;

                foreach(var c in jsonCards["cards"])
                {

                    cards[i] = new Card(c["image"].ToString(), c["value"].ToString(), c["suit"].ToString());
                    i++;

                }

                ViewBag.ModelCard = cards;
            }
            else
            {

            }

            responseData.Close();
            return View("Index");
        }
    }
}