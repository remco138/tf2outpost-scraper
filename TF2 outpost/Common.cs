using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace TF2_outpost
{
    /*
    class Tf2Outpost
    {
    }
     * */
    
    class Item
    {
        static string[] itemTypes = { "normal", "unique", "vintage", "genuine", "strange", "unusual", "haunted", "community", "self-made", "valve" };
        public HtmlNode htmlCode;
        public string id;
        public string type; //unusual, strange, etc
        public string name;
        public string description;
        public string paint;
        public string level;

        public Item(HtmlNode code)
        {
            htmlCode = code;
            type    = code.GetAttributeValue("class", "");
            if (type.Contains("item ")) //"item unusual", "item unique", "item strange", etc are all possibilities
            {
                type = type.Remove(type.IndexOf("item "), "item ".Length); //strip the string substr "item" from type 
            }
            id = code.GetAttributeValue("id", ""); //either a number or "stock"
            name = code.SelectSingleNode("div[@class='details']/h1").InnerText;
            for (int i = 0; i < itemTypes.Count(); i++)
            {
                if(name.ToLower().Contains(itemTypes[i]))
                {
                    name = name.Remove(name.ToLower().IndexOf(itemTypes[i]), itemTypes[i].Count()); //remove first word,
                }
            }

            HtmlNode levelNode = code.SelectSingleNode("div[@class='details']/span[@class='level']");
            level = (levelNode != null) ? levelNode.InnerText : "";

            HtmlNode paintNode = code.SelectSingleNode("span[@class='label']");
            paint = (paintNode != null && paintNode.InnerText == "Painted:") ? paintNode.NextSibling.InnerText : "";

        }
    };


    class Trade
    {
        public HtmlNode htmlCode;
        public string name;
        public string description;
        public string id;
        public List<Item> offer;
        public List<Item> request;

        public Trade(HtmlNode code)
        {
            htmlCode = code;

            //  fetch the trade ID (well, actually its the url)
            id = htmlCode.SelectSingleNode("//a[contains(@href,'/trade/')]").GetAttributeValue("href", "");

            //  fetch the name, donator type is used to find it
            HtmlNode node = htmlCode.SelectSingleNode("//span[@class='regular' or starts-with(@class, 'donator')]");
            name = (node != null) ? node.InnerText : null;

            //  the trade's description
            HtmlNode descriptionNode = htmlCode.SelectSingleNode("//div[@class='notes expandable show']");
            description = (descriptionNode != null) ? descriptionNode.InnerText : "";


            /*
             * here we construct the item objects
             * 
             */
            HtmlNode itemOfferBundleHtml = htmlCode.SelectSingleNode("(div[@class='four-column'])[1]");
            HtmlNode itemRequestBundleHtml = htmlCode.SelectSingleNode("(div[@class='four-column'])[2]");

            HtmlNodeCollection itemOfferHtml = itemOfferBundleHtml.SelectNodes("div[starts-with(@class, 'item')]");
            HtmlNodeCollection itemRequestHtml = itemRequestBundleHtml.SelectNodes("div[starts-with(@class, 'item')]");

            offer = new List<Item>();
            request = new List<Item>();

            for (int i = 0; i < itemOfferHtml.Count; i++) { offer.Add(new Item(itemOfferHtml[i])); }
            for (int i = 0; i < itemRequestHtml.Count; i++) { request.Add(new Item(itemRequestHtml[i])); }

        }
    }
}
