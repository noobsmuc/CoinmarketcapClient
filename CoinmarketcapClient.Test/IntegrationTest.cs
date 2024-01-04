using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using FluentAssertions.Execution;
using NoobsMuc.Coinmarketcap.Client;
using NUnit.Framework;

namespace CoinCapClient.Test
{
    [TestFixture()]
    public class IntegrationTest
    {
        private static string API_KEY = "YourApiKey";

        [Test]
        public void GetCurrency_Nothing_Return100()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            var retValue = m_Sut.GetCurrencies();
            retValue.Count().Should().Be(100);
        }

        [Test]
        public void GetCurrency_Using300_Return300Entries()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            var retValue = m_Sut.GetCurrencies(300);
            retValue.Count().Should().Be(300);
        }

        [Test]
        public void GetCurrency_UsingGBP_ReturnCurrencyWithConverted()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            var retValue = m_Sut.GetCurrencies("GBP");
            using (new AssertionScope())
            {
                retValue.Count().Should().Be(100);
                retValue.First().Price.Should().NotBe(null); ;
                retValue.First().MarketCapConvert.Should().NotBe(null);
                retValue.First().ConvertCurrency.Should().Be("GBP");

                retValue.Take(155).Last().Price.Should().NotBe(null);
                retValue.Take(155).Last().MarketCapConvert.Should().NotBe(null);
                retValue.Take(155).Last().ConvertCurrency.Should().Be("GBP");
            };
        }

        [Test]
        public void GetCurrency_UsingJPYAnd200_Return200Entries()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            IEnumerable<Currency> retValue = m_Sut.GetCurrencies(200,"JPY");
            using (new AssertionScope())
            {
                retValue.Count().Should().Be(200);
                retValue.First().Price.Should().NotBe(null);
                retValue.First().MarketCapConvert.Should().NotBe(null);
                retValue.First().ConvertCurrency.Should().Be("JPY");

                retValue.Last().Price.Should().NotBe(null);
                retValue.Last().MarketCapConvert.Should().NotBe(null);
                retValue.Last().ConvertCurrency.Should().Be("JPY");
            };
        }

        [Test]
        public void GetCurrencyBySlug_Bitcoin_ReturnBitcoinDetail()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            Currency currency = m_Sut.GetCurrencyBySlug("bitcoin");
            Assert.Multiple(() =>
            {
                currency.Name.Should().Be("Bitcoin");
                currency.Symbol.Should().Be("BTC");
                currency.Price.Should().NotBe(null);
                currency.MarketCapConvert.Should().NotBe(null);
            });
        }

        [Test]
        public void GetCurrencyBySlug_PivxInEur_ReturnPivxDetail()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            Currency currency = m_Sut.GetCurrencyBySlug("pivx","EUR");
            using (new AssertionScope())
            {
                currency.Name.Should().Be("PIVX");
                currency.Symbol.Should().Be("PIVX");
                currency.Price.Should().NotBe(null);
                currency.MarketCapConvert.Should().NotBe(null);
                currency.ConvertCurrency.Should().Be("EUR");
            };
        }

        [Test]
        public void GetCurrencyBySlugList_StratisIrisnet_ReturnDetails()
        {
            //StraTis-> test with upper and lower case
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            List<Currency> currencyList = m_Sut.GetCurrencyBySlugList(new[] { "StraTis", "irisnet" }).ToList();
            using (new AssertionScope())
            {
                currencyList[0].Name.Should().Be("Stratis");
                currencyList[0].Symbol.Should().Be("STRAX");
                currencyList[0].Price.Should().NotBe(null);
                currencyList[0].MarketCapConvert.Should().NotBe(null);

                currencyList[1].Name.Should().Be("IRISnet");
                currencyList[1].Symbol.Should().Be("IRIS");
                currencyList[1].Price.Should().NotBe(null);
                currencyList[1].MarketCapConvert.Should().NotBe(null);
            };
        }

        [Test]
        public void GetCurrencyBySlug_WrongCurrency_ThrowsException()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            Action act = () => m_Sut.GetCurrencyBySlug("AnyWrongCurrency");
            act.Should().Throw<HttpRequestException>();
        }

        [Test]
        public void GetCurrencyBySymbol_USDT_ReturnsCurrency()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            Currency currency = m_Sut.GetCurrencyBySymbol("USDT");

            using (new AssertionScope())
            {
                currency.Name.Should().Be("Tether USDt");
                currency.Symbol.Should().Be("USDT");
                currency.Price.Should().NotBe(null);
                currency.MarketCapConvert.Should().NotBe(null);
            };
        }

        [Test]
        public void GetCurrencyBySymbol_ADAInEuro_ReturnsCurrency()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            Currency currency = m_Sut.GetCurrencyBySymbol("ADA", "EUR");

            using (new AssertionScope())
            {
                currency.Name.Should().Be("Cardano");
                currency.Symbol.Should().Be("ADA");
                currency.Price.Should().NotBe(null);
                currency.MarketCapConvert.Should().NotBe(null);
                currency.ConvertCurrency.Should().Be("EUR");
            };
        }

        [Test]
        public void GetCurrencyBySymbol_DogeLink_ReturnsCurrency()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            var currencyList = m_Sut.GetCurrencyBySymbolList(new[] { "DOGE", "LINK" }, "EUR").ToList();

            using (new AssertionScope())
            {
                currencyList[0].Name.Should().Be("Dogecoin");
                currencyList[0].Symbol.Should().Be("DOGE");
                currencyList[0].Price.Should().NotBe(null);
                currencyList[0].MarketCapConvert.Should().NotBe(null);
                currencyList[0].ConvertCurrency.Should().Be("EUR");

                currencyList.Last().Name.Should().Be("Chainlink");
                currencyList.Last().Symbol.Should().Be("LINK");
                currencyList.Last().Price.Should().NotBe(null);
                currencyList.Last().MarketCapConvert.Should().NotBe(null);
                currencyList.Last().ConvertCurrency.Should().Be("EUR");
            };
        }


        [Test]
        public void GetCurrencyBySymbol_Arb_ReturnsCurrency()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient(API_KEY);
            var currencyList = m_Sut.GetCurrencyBySymbolList(new[] { "ARB" }).ToList();

            using (new AssertionScope())
            {
                currencyList[0].Name.Should().Be("Arbitrum");
                currencyList[0].Symbol.Should().Be("ARB");
                currencyList[0].Price.Should().NotBe(null);
                currencyList[0].MarketCapConvert.Should().NotBe(null);
                currencyList[0].ConvertCurrency.Should().Be("USD");

                currencyList.Last().Name.Should().Be("Arbitrum (IOU)");
                currencyList.Last().Symbol.Should().Be("ARB");
                currencyList.Last().Price.Should().NotBe(null);
                currencyList.Last().MarketCapConvert.Should().NotBe(null);
                currencyList.Last().ConvertCurrency.Should().Be("USD");
            };
        }
    }
}
