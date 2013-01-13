using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace TF2_outpost
{

    class Program
    {
        static Scraper masterScraper;
        static List<Scraper> scraperList;
        static CookieContainer cookie;
        static int pageNum = 1;

        static void scrapeFrontPage()
        {
            scraperList.Add(new Scraper());
            int ourIndex = scraperList.Count - 1;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://tf2outpost.com/recent/" + (ourIndex + 1).ToString());
            request.CookieContainer = cookie;
            request.KeepAlive = false;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());
            string html = stream.ReadToEnd();

            scraperList[ourIndex].Add(html);
        }

        static void Main(string[] args)
        {
            cookie = new CookieContainer();
            cookie.Add(new Cookie("__cfduid", "db3885975dd994db43565d6bd785ef7981357764858", "/", ".tf2outpost.com"));
            cookie.Add(new Cookie("trade", "7613554", "/", "www.tf2outpost.com"));
            cookie.Add(new Cookie("userid", "262200", "/", "www.tf2outpost.com"));
            cookie.Add(new Cookie("token", "b580e2c50a70d120064cfb6d28c983b5", "/", "www.tf2outpost.com"));
            cookie.Add(new Cookie("session", "5cb4978cc373dc0e52f45f09a9768b69", "/", "www.tf2outpost.com"));
            cookie.Add(new Cookie("intergi", "1", "/", "www.tf2outpost.com"));

            masterScraper = new Scraper();
            scraperList = new List<Scraper>();
            List<Thread> threadList = new List<Thread>();
            for (pageNum = 0; pageNum < 20; pageNum++)
            {
                Thread t = new Thread(scrapeFrontPage);
                threadList.Add(t);
                t.Start();
                //Thread.Sleep(10);
            }

            for(int i = 0; i < threadList.Count; i++)
            {
                while (true)
                {
                    if (threadList[i].IsAlive)
                    {
                        Thread.Sleep(10);
                    }
                    else
                    {
                        break;  // out of infinite while loop
                    }
                }
            }
            foreach (Scraper scraper in scraperList)
            {
                masterScraper.Merge(scraper);
            }

            Console.WriteLine(masterScraper.tradeList.Count());
            Console.ReadLine();
        }
    }
}
