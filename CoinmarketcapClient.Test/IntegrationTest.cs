using System;
using System.Linq;
using FluentAssertions;
using NoobsMuc.Coinmarketcap.Client;
using NUnit.Framework;


namespace CoinCapClient.Test
{
    [TestFixture()]
    public class IntegrationTest
    {
        [Test]
        public void GetCurrency_Nothing_ReturnAll()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient();
            var retValue = m_Sut.GetCurrencies();
            retValue.Count().Should().BeGreaterThan(900);
        }

        [Test]
        public void GetCurrency_Using200_Return200Entries()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient();
            var retValue = m_Sut.GetCurrencies(200);
            retValue.Count().Should().Be(200);
        }

        [Test]
        public void GetCurrency_UsingGBP_ReturnAllCurrencyWithConverted()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient();
            var retValue = m_Sut.GetCurrencies("GBP");
            retValue.Count().Should().BeGreaterThan(900);
            retValue.First().PriceConvert.Should().NotBeNullOrEmpty();
            retValue.First().MarketCapConvert.Should().NotBeNullOrEmpty();
            retValue.First().Volume24Convert.Should().NotBeNullOrEmpty();
            retValue.First().ConvertCurrency.Should().Be("GBP");

            retValue.Take(155).Last().PriceConvert.Should().NotBeNullOrEmpty();
            retValue.Take(155).Last().MarketCapConvert.Should().NotBeNullOrEmpty();
            retValue.Take(155).Last().Volume24Convert.Should().NotBeNullOrEmpty();
            retValue.Take(155).Last().ConvertCurrency.Should().Be("GBP");
        }

        [Test]
        public void GetCurrency_UsingJPYAnd200_Return200Entries()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient();
            var retValue = m_Sut.GetCurrencies(200,"JPY");
            retValue.Count().Should().Be(200);
            retValue.First().PriceConvert.Should().NotBeNullOrEmpty();
            retValue.First().MarketCapConvert.Should().NotBeNullOrEmpty();
            retValue.First().Volume24Convert.Should().NotBeNullOrEmpty();
            retValue.First().ConvertCurrency.Should().Be("JPY");

            retValue.Last().PriceConvert.Should().NotBeNullOrEmpty();
            retValue.Last().MarketCapConvert.Should().NotBeNullOrEmpty();
            retValue.Last().Volume24Convert.Should().NotBeNullOrEmpty();
            retValue.Last().ConvertCurrency.Should().Be("JPY");
        }

        [Test]
        public void GetCurrencyById_Bitcoin_ReturnBitcoinDetail()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient();
            Currency currency = m_Sut.GetCurrencyById("bitcoin");
            currency.Id.Should().Be("bitcoin");
            currency.Symbol.Should().Be("BTC");
            currency.PriceConvert.Should().BeNull();
            currency.MarketCapConvert.Should().BeNull();
            currency.Volume24Convert.Should().BeNull();
        }

        [Test]
        public void GetCurrencyById_PivxInEur_ReturnPivxDetail()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient();
            Currency currency = m_Sut.GetCurrencyById("pivx","EUR");
            currency.Id.Should().Be("pivx");
            currency.Symbol.Should().Be("PIVX");
            currency.PriceConvert.Should().NotBeNullOrEmpty();
            currency.MarketCapConvert.Should().NotBeNullOrEmpty();
            currency.Volume24Convert.Should().NotBeNullOrEmpty();
            currency.ConvertCurrency.Should().Be("EUR");
        }

        [Test]
        public void GetCurrencyById_WrongCurrency_ThrowsException()
        {
            ICoinmarketcapClient m_Sut = new CoinmarketcapClient();
            Action act = () => m_Sut.GetCurrencyById("AnyWrongCurrency");
            act.ShouldThrow<AggregateException>().Where(e => e.InnerExceptions[0].Message.Contains("404"));
        }
    }
}