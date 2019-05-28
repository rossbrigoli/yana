using System;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;

namespace com.rossbrigoli.Yana.Tests
{
    public class ClientPrivateTest
    {
        private static string _apiKey;
        private static string _apiSecret;

        public ClientPrivateTest()
        {
            if( _apiKey != null) return;
            string homePath = (Environment.OSVersion.Platform == PlatformID.Unix || 
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
                    ? Environment.GetEnvironmentVariable("HOME")
                    : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            var fs = new FileStream($"{homePath}/apikey.secret",FileMode.Open);
            var sr = new StreamReader(fs);
            _apiKey = sr.ReadLine().Split(' ').Last();
            _apiSecret = sr.ReadLine().Split(' ').Last();
            sr.Close();
            fs.Close();
        }

        [Fact]
        public void TestGetTradeBalance()
        {
            var client = new Client(_apiKey,_apiSecret);

            var result = client.GetTradeBalance().Result;
            Assert.Empty(result.Error);
        }

        [Fact]
        public void TestGetTradeBalanceEUR()
        {
            var client = new Client(_apiKey,_apiSecret);

            var result = client.GetTradeBalance(asset:"EUR").Result;
            Assert.Empty(result.Error);
            Assert.Equal(typeof(TradeBalanceData), result.Result.GetType());
            Assert.True(1 < result.Result.TradeBalance);
        }

        [Fact]
        public void TestGetOpenOrders()
        {
            var client = new Client(_apiKey, _apiSecret);

            var result = client.GetOpenOrders().Result;
            Assert.Empty(result.Error);
            Assert.Equal(typeof(OrderData), result.Result.GetType());
            Assert.NotNull(result.Result.Orders);
            Assert.NotEmpty(result.Result.Orders.Where(o => o.Status == OrderStatus.Open));
        }       

        [Fact]
        public void TestGetClosedOrders()
        {
            var client = new Client(_apiKey, _apiSecret);

            var result = client.GetClosedOrders(100).Result;
            Assert.Empty(result.Error);
            Assert.Equal(typeof(OrderData), result.Result.GetType());
            Assert.NotEmpty(result.Result.Orders.Where(o => o.Status == OrderStatus.Closed));
        }

        public void TestGetClosedOrdersWithTrades()
        {
            var client = new Client(_apiKey, _apiSecret);

            var result = client.GetClosedOrders(100,true).Result;
            Assert.Empty(result.Error);
            Assert.Equal(typeof(OrderData), result.Result.GetType());
            Assert.NotEmpty(result.Result.Orders.Where(o => o.Status == OrderStatus.Closed));
            Assert.True(result.Result.Orders.Any(o => o.Trades.Any()));
        }

        [Fact]
        public void TestGetOrders()
        {
            var client = new Client(_apiKey, _apiSecret);
            var sampleTxns = client.GetClosedOrders(100).Result.Result.Orders;
            var txnArgs = sampleTxns.Select(c => c.TransactionId);

            var result = client.GetOrders(txnArgs).Result;
            Assert.Empty(result.Error);
            Assert.Equal(typeof(OrderData), result.Result.GetType());
            Assert.NotEmpty(result.Result.Orders);
        }

        [Fact]
        public void TestGetOrdersWithTrades()
        {
            var client = new Client(_apiKey, _apiSecret);
            var sampleTxns = client.GetClosedOrders(100).Result.Result.Orders;
            var txnArgs = sampleTxns.Select(c => c.TransactionId);

            var result = client.GetOrders(txnArgs, null, true).Result;
            Assert.Empty(result.Error);
            Assert.Equal(typeof(OrderData), result.Result.GetType());
            Assert.True(result.Result.Orders.Any(o => o.Trades.Any()));
        }
    }
}