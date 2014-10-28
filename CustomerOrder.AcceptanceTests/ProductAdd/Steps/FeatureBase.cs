namespace CustomerOrder.AcceptanceTests.ProductAdd.Steps
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using System.Xml.Serialization;
    using Helpers;
    using Model;
    using NUnit.Framework;

    public abstract class FeatureBase
    {
        protected CustomerOrderHttpClient Client;
        protected HttpResponseMessage Result;
        protected string OrderNumber;
        protected string OrderNumberToken;

        protected static Money ConvertToMoney(string moneyString)
        {
            var moneyStrings = moneyString.Split(new[] { ' ' });
            var amount = decimal.Parse(moneyStrings[0]);
            var currencyCode = (Currency)Enum.Parse(typeof(Currency), moneyStrings[1]);
            return new Money(currencyCode, amount);
        }

        protected static Currency ToCurrency(string currency)
        {
            Currency result;
            Enum.TryParse(currency, true, out result);
            return result;
        }

        protected static Money ToMoney(string formattedString)
        {
            var parts = formattedString.Split(' ');
            return new Money(ToCurrency(parts[1]), decimal.Parse(parts[0]));
        }

        protected SyndicationItem GetFirstSyndicationItem()
        {
            var feed = ReadSyndicationFeed();
            var syndicationItem = feed.Items.FirstOrDefault();
            Assert.IsNotNull(syndicationItem);
            return syndicationItem;
        }

        protected SyndicationFeed ReadSyndicationFeed()
        {
            var reader = new XmlTextReader(Result.Content.ReadAsStreamAsync().Result);
            var feed = SyndicationFeed.Load(reader);

            Assert.IsNotNull(feed);
            return feed;
        }

        protected string ReplaceTokensInString(string url)
        {
            var result = url.Replace(OrderNumberToken, OrderNumber);
            return result;
        }

        protected T GetEventFromFirstSyndicationItem<T>()
        {
            var syndicationItem = GetFirstSyndicationItem();

            var content = ((XmlSyndicationContent)syndicationItem.Content);
            var serializer = new XmlSerializer(typeof(T));
            var deserializedEvent = content.ReadContent<T>(serializer);

            Assert.IsNotNull(deserializedEvent, "Expected a {0} in the first Syndication entry", typeof(T).Name);
            return deserializedEvent;
        }
    }
}