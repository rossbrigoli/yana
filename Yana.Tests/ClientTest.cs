using System;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            
            var result = client.GetTicker("XTZEUR").Result;
            Assert.Empty(result.Error);
            Assert.Equal(typeof(long[]), result.Result["XTZEUR"].t.GetType());
        }

        [Fact]
        public void TestGetTickerMultiAsset()
        {
            var client = new Client();
            
            var result = client.GetTicker("XTZEUR", "XBTUSD").Result;
            Assert.Empty(result.Error);
            Assert.Contains("XXBTZUSD", result.Result.Keys);
            Assert.Contains("XTZEUR", result.Result.Keys);
            Assert.True(100 < result.Result["XXBTZUSD"].t[0]);
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
            Assert.True(100 < result.Result.Data.First().Time);
            Assert.True(1000.2M < result.Result.Data.First().High);
        }

        [Fact]
        public void TestGetOrderBook()
        {
            var client = new Client();
            var result = client.GetOrderBook("XTZEUR").Result;

            Assert.Empty(result.Error);
            Assert.Equal("XTZEUR", result.AssetPair);
            Assert.NotEmpty(result.Asks);
            Assert.NotEmpty(result.Bids);
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
    }

}
