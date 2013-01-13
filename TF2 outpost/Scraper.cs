using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace TF2_outpost
{
    class Scraper
    {
        public List<Trade> tradeList;

        public Scraper()
        {
            tradeList = new List<Trade>();
        }

        public void Merge(Scraper scraper)
        {
            foreach (Trade trade1 in scraper.tradeList)
            {
                foreach (Trade trade2 in tradeList)
                {
                    if (trade1.id == trade2.id)
                    {
                        break;
                    }
                }
                tradeList.Add(trade1); //trade1 is the external Scraper, this code maybe needs clarifying, or should be done in a database class instead..
            }
        }

        public bool Add(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection tradeSections = doc.DocumentNode.SelectNodes("//div[@class='trade']");
            if (tradeSections == null)
            {
                return false;
            }

            foreach (HtmlNode t in tradeSections)
            {
                Trade trade = new Trade(t);

                foreach(Trade tradeIdCheck in tradeList)
                {
                    if (tradeIdCheck.id == trade.id)
                    {
                        break;
                    }
                }
                tradeList.Add(new Trade(t));
            }

            return true;
        }

    }
}
