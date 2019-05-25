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
            Assert.True(1 < result.Result.TradeBalance);
        }
    }
}