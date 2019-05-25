using System;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace com.rossbrigoli.Yana.Tests
{
    public class ClientTest
    {
        [Fact]
        public void TestGetServerTime()
        {
            var client = new Client();
            var result = client.GetServerTime().Result;
            Assert.Empty(result.Error);
            Assert.Equal(
                UnixTime.Create(result.Result.UnixTime).ToUniversalTime().ToString("ddd, dd MMM yy HH':'mm':'ss '+0000'"), 
                result.Result.Rfc1123);
        }

        [Fact]
        public void TestGetAssetInfo()
        {
            var client = new Client();
            var result = client.GetAssetInfo().Result;

            Assert.Empty(result.Error);
            Assert.Equal(6, result.Result["ADA"].DisplayDecimals);
            Assert.Equal("currency", result.Result["XTZ"].AssetClass);
        }

        [Fact]
        public void TestGetAssetPairDefault()
        {
            var client = new Client();
            var result = client.GetAssetPairs().Result;

            Assert.Empty(result.Error);
            Assert.Equal("XXBT", result.Result.Where(c => c.Value.WsName == "XBT/USD").First().Value.Base);
        }

        [Fact]
        public void TestGetAssetPairNonDefault()
        {
            var client = new Client();
            var result = client.GetAssetPairs("info", "ETHUSD").Result;

            Assert.Empty(result.Error);
            Assert.Equal(1, result.Result.Count());
            Assert.Equal("XETH", result.Result.Where(c => c.Value.WsName == "ETH/USD").First().Value.Base);
        }

        [Fact]
        public void TestGetTickerOneAsset()
        {
            var client = new Client();
            
            var result = client.GetTicker("XBTUSD").Result;
            Assert.Empty(result.Error);
            Assert.True(0 < result.Result.AssetPairs.First().Value.High.Last24H);
        }

        [Fact]
        public void TestGetTickerMultiAsset()
        {
            var client = new Client();
            
            var result = client.GetTicker("XTZEUR", "XBTUSD").Result;
            Assert.Empty(result.Error);
            Assert.Contains("XXBTZUSD", result.Result.AssetPairs.Keys);
            Assert.Contains("XTZEUR", result.Result.AssetPairs.Keys);
            Assert.True(0 < result.Result.AssetPairs["XXBTZUSD"].High.Today);
        }

        [Fact]
        public void TestGetOHLCDataXBTUSD()
        {
            var client = new Client();
            var result = client.GetOHLCData("XBTUSD").Result;

            Assert.Empty(result.Error);
            Assert.Contains("XXBTZUSD", result.Result.PairName);
            Assert.True(100 < result.Result.Last);
            Assert.NotNull(result.Result.Data);
            Assert.True(result.Result.Data.First().Time < DateTime.Now.AddHours(1));
            Assert.True(1000.2M < result.Result.Data.First().High);
        }

        [Fact]
        public void TestGetOrderBook()
        {
            var client = new Client();
            var result = client.GetOrderBook("XTZEUR").Result;

            Assert.Empty(result.Error);
            Assert.Equal("XTZEUR", result.Result.PairName);
            Assert.NotEmpty(result.Result.Asks);
            Assert.NotEmpty(result.Result.Bids);
        }

        [Fact]
        public void TestGetRecentTrades()
        {
            var client = new Client();
            var result = client.GetRecentTrades("XTZEUR").Result;

            Assert.Empty(result.Error);
            Assert.NotNull(result.Result);
            Assert.NotNull(result.Result.Trades);
            Assert.Equal("XTZEUR", result.Result.PairName);
            Assert.NotEmpty(result.Result.Trades);
            Assert.NotEmpty(result.Result.Trades.Where(t => t.TradeType == TradeType.Sell));
        }

        [Fact]
        public void TestGetRecentSpreadData()
        {
            var client = new Client();
            var result = client.GetRecentSpreadData("XTZEUR").Result;

            Assert.Empty(result.Error);
            Assert.Equal("XTZEUR", result.Result.PairName);
            Assert.True(result.Result.Data.First().Time < DateTime.Now.AddMinutes(5));
            Assert.True(result.Result.Data.Last().Ask > 0M);
            Assert.True(result.Result.Data.First().Bid > 0M);
        }
    }

}
