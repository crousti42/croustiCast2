using System;
using System.IO;
using System.Net;
using System.Xml;

namespace croustiCast
{
    class RssFlow
    {
        public string title = "";
        public string desc = "";
        public string link = "";
        public string imageUrl = "";
        public string imageAlt = "";
        public string language = "";
        public string[,] items;

        private void logThis(string textToDisplay, string title="")
        {
            if (title != "")
            {
                Console.WriteLine("[Log] " + title + ": " + textToDisplay);
            }
            else
            {
                Console.WriteLine("[Log] " + textToDisplay);
            }
        }

        public void create(string rssUrl)
        {
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(rssUrl);
            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            Stream rssStream = response.GetResponseStream();
            XmlDocument rssDoc = new XmlDocument();
            rssDoc.Load(rssStream);

            title = getRssAttribute(rssStream, rssDoc, "rss/channel/title");
            desc = getRssAttribute(rssStream, rssDoc, "rss/channel/description");
            link = getRssAttribute(rssStream, rssDoc, "rss/channel/link");
            language = getRssAttribute(rssStream, rssDoc, "rss/channel/language");
            imageUrl = getRssAttribute(rssStream, rssDoc, "rss/channel/image/url");
            imageAlt = getRssAttribute(rssStream, rssDoc, "rss/channel/image/title");
            setRssContent(rssStream, rssDoc);
            
            response.Close();
        }

        private string getRssAttribute(Stream rssStream, XmlDocument rssDoc, string rssAttribute)
        {
            string returnedValue = "";
            XmlNode rssNode;
            rssNode = rssDoc.SelectSingleNode(rssAttribute);

            if (rssNode != null)
            {
                returnedValue = rssNode.InnerText;
            }
            else
            {
                returnedValue = "";
            }

            return returnedValue;
        }

        // Set un épisode d'un podcast
        private string getRssItem(XmlNodeList rssItems, int iCurrentNode, string rssItemAttribute)
        {
            string returnedValue = "";

            XmlNode rssNode = rssItems.Item(iCurrentNode).SelectSingleNode(rssItemAttribute);
            if (rssNode != null)
            {
                returnedValue = rssNode.InnerText;
            }
            else
            {
                returnedValue = "";
            }

            return returnedValue;
        }

        // Set l'ensemble des épisodes d'un podcast
        private void setRssContent(Stream rssStream, XmlDocument rssDoc)
        {   
            XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");

            String[,] tempRssData = new String[1000, 4];
                        
            for (int i = 0; i < rssItems.Count; i++)
            {
                tempRssData[i, 0] = getRssItem(rssItems, i, "title");
                tempRssData[i, 1] = getRssItem(rssItems, i, "description");
                tempRssData[i, 2] = getRssItem(rssItems, i, "guid");
                tempRssData[i, 3] = getRssItem(rssItems, i, "pubDate");
            }

            items = tempRssData;
        }

    }
}